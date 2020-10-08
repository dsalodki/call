using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.EntityFramework.Repositories;
using Abp.Runtime.Session;
using Call.Calls;
using Call.Users;
using Castle.Core.Logging;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Call.Web.Helper
{
    public class RequestHub : Hub, ITransientDependency
    {
        private readonly IRepository<Request, long> _requestRepository;
        private readonly IRepository<User, long> _userRepository;
        public IAbpSession AbpSession { get; set; }
        public ILogger Logger { get; set; }
        public static Dictionary<int, Dictionary<string, dynamic>> Users = new Dictionary<int, Dictionary<string, dynamic>>();

        public RequestHub(IRepository<Request, long> requestRepository, IRepository<User, long> userRepository)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            AbpSession = NullAbpSession.Instance;
            Logger = NullLogger.Instance;
        }

        public void NotifyAboutRequest(Request request)
        {
            foreach (var user in Users[request.TenantId])
            {
                user.Value.notifyAboutRequest(request);
            }
        }

        public void Mark(long requestId, State state)
        {
            var request = _requestRepository.Get(requestId);
            request.State = state;
            request.UserId = AbpSession.UserId.Value;
            if (state == State.Treated)
            {
                request.TreatedTime = DateTime.UtcNow - request.Created;
                Clients.Caller.setTreatedTime(request.Id, request.TreatedTime);
            }
            if (state == State.Answered)
            {
                request.AnsweredTime = DateTime.UtcNow - request.Created - request.TreatedTime;
            }
            _requestRepository.Update(request);
            foreach (var user in Users[request.TenantId].Where(x => x.Key != Context.ConnectionId))
            {
                user.Value.updateState(requestId, state);
            }
        }

        public void GetRequests()
        {
            var userId = AbpSession.UserId.Value;
            var tenantId = _userRepository.Get(userId).TenantId;
            //var notAsweredRequests = _requestRepository.GetAll().Where(x => x.TenantId == tenantId && x.State == State.NotAnswered).ToList();
            IEnumerable<Request> notAsweredRequests;
            if (Context.User.Identity.Name == "admin")
            {
                notAsweredRequests = _requestRepository.GetAllList().Where(x => x.TenantId == tenantId && x.Created.Date == DateTime.Today);
            }
            else
            {
                notAsweredRequests = _requestRepository.GetAllList().Where(x => x.TenantId == tenantId && (x.State == State.NotAnswered || (x.UserId == userId && x.State == State.Treated)));
            }
            var result = notAsweredRequests.Select(x => new Request
            {
                Phone = x.Phone,
                State = x.State,
                Name = x.Name,
                Email = x.Email,
                Created = x.Created,
                TenantId = x.TenantId,
                TreatedTime = x.TreatedTime,
                AnsweredTime = x.AnsweredTime,
                Question = x.Question,
                Id = x.Id
            });

            if (!Users.Keys.Contains(tenantId.Value))
            {
                var item = new Dictionary<string, dynamic>();
                item.Add(Context.ConnectionId, Clients.Caller);
                Users.Add(tenantId.Value, item);
            }
            else
            {
                if (!Users[tenantId.Value].Keys.Contains(Context.ConnectionId))
                {
                    Users[tenantId.Value].Add(Context.ConnectionId, Clients.Caller);
                }
            }
            Clients.Caller.addNotAsweredRequests(result/*, tenantId*/);
        }

        public async override Task OnConnected()
        {
            await base.OnConnected();
            GetRequests();
            Logger.Debug("A client connected to RequestHub: " + Context.ConnectionId);
        }

        public async override Task OnDisconnected(bool stopCalled)
        {
            foreach (var user in Users)
            {
                if (user.Value.ContainsKey(Context.ConnectionId))
                {
                    user.Value.Remove(Context.ConnectionId);
                }
            }

            await base.OnDisconnected(stopCalled);
            Logger.Debug("A client disconnected from RequestHub: " + Context.ConnectionId);
        }
    }
}
