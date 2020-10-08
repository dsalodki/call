using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web.Http;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Web.Security.AntiForgery;
using Call.Calls;
using Call.MultiTenancy;
using Call.Requests;
using Call.Requests.Dto;
using Call.Users;
using Call.Web.Helper;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using ZaprosBy.Feedback.Api.Models;

namespace ZaprosBy.Feedback.Api.Controllers
{

    [AllowAnonymous]
    public class SubmitFormController : ApiController
    {
        private readonly IRequestAppService _requestAppService;
        private readonly TenantManager _tenantManager;
        private readonly IRepository<Request, long> _requestRepository;
        private readonly IRepository<User, long> _userRepository;

        public SubmitFormController(IRequestAppService requestAppService, TenantManager tenantManager, IRepository<Request, long> requestRepository, IRepository<User, long> userRepository)
        {
            _requestAppService = requestAppService;
            _tenantManager = tenantManager;
            _requestRepository = requestRepository;
            _userRepository = userRepository;
        }

        [DisableAbpAntiForgeryTokenValidation]
        [HttpPost]
        public IHttpActionResult Post([FromBody]SubmitFormModel model)
        {
            if (ModelState.IsValid)
            {
                int tenantId;
                if (!int.TryParse(model.TenantId, out tenantId))
                {
                    return BadRequest("Обратитесь в поддержку, не верный провайдер");
                }
                if (_tenantManager.Tenants.ToList().All(x => x.Id != tenantId))
                {
                    return BadRequest("Обратитесь в поддержку, не верный провайдер");
                }

                var isLast15min =
                    _requestRepository.GetAllList()
                        .Any(
                            x =>
                                x.TenantId == tenantId && x.Phone == model.Phone &&
                                x.Created.AddMinutes(15) > DateTime.UtcNow);

                //if (isLast15min)
                //    return BadRequest("нельзя отправить более одного запроса в 15 минут");

                var requestInput = new CreateRequestInput
                {
                    Phone = model.Phone,
                    Created = DateTime.UtcNow,
                    Email = model.Email,
                    Name = model.Name,
                    State = State.NotAnswered,
                    Question = model.Question,
                    TenantId = tenantId
                };
                var requestId = _requestAppService.CreateRequest(requestInput);

                ThreadPool.QueueUserWorkItem((object state) =>
                {
                    var request = requestInput.MapTo<Request>();
                    request.Id = requestId;

                    foreach (var client in RequestHub.Users[request.TenantId].Values)
                    {
                        client.notifyAboutRequest(request);
                    }
                });

                if (!string.IsNullOrEmpty(model.Email))
                {
                    MailMessage mail = new MailMessage("admin@zapros.by", model.Email);
                    SmtpClient client = new SmtpClient("smtp-relay.gmail.com", 587)
                    {
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("admin@zapros.by", "Today123!")
                    };
                    mail.Subject = "запрос поступил";
                    mail.Body = string.Format($"Ваш запрос сохранен в базе данных и будет обработан оператором через некоторое время. Телефон № {model.Phone}. Комментарий к вопросу: {model.Question}");
                    client.Send(mail);
                }

                return Ok("Запрос успешно принят");
            }
            return BadRequest("Не верные данные");
        }
    }
}
