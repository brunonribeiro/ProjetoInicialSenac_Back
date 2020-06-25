using System;
using System.Collections.Generic;
using System.Linq;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly IDomainNotificationHandlerAsync<DomainNotification> NotificacaoDeDominio;

        protected BaseController(IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio)
        {
            NotificacaoDeDominio = notificacaoDeDominio;
        }

        protected bool OperacaoValida() => !NotificacaoDeDominio.HasNotifications();

        protected BadRequestObjectResult BadRequestResponse()
        {
            return BadRequest(NotificacaoDeDominio.GetNotifications().Select(n => n.Value));
        }
    }
}
