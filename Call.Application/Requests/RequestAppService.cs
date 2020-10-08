using System;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Call.Calls;
using Call.Requests.Dto;

namespace Call.Requests
{
    public class RequestAppService : CallAppServiceBase, IRequestAppService
    {
        private readonly IRepository<Request, long> _requestRepository;

        public RequestAppService(IRepository<Request, long> requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public long CreateRequest(CreateRequestInput input)
        {
            var request = input.MapTo<Request>();
            return _requestRepository.InsertAndGetId(request);
        }
    }

}
