using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Notifications;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Base
{
    public abstract class DomainService 
    {
        protected readonly IDomainNotificationHandlerAsync<DomainNotification> NotificacaoDeDominio;

        protected DomainService(IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio)
        {
            NotificacaoDeDominio = notificacaoDeDominio;
        }

        public async Task NotificarValidacoesDeDominio(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
                await NotificacaoDeDominio.HandleAsync(new DomainNotification(TipoDeNotificacao.ErroDeDominio.ToString(), erro.ErrorMessage));
        }

        public async Task NotificarValidacaoDeServico(string mensagem)
        {
            await NotificacaoDeDominio.HandleAsync(new DomainNotification(TipoDeNotificacao.ErroDeServico.ToString(), mensagem));
        }
    }
}
