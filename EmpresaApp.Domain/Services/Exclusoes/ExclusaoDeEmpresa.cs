using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Services.Exclusoes
{
    public class ExclusaoDeEmpresa : DomainService
    {
        private readonly IEmpresaRepositorio _empresaRepositorio;

        public ExclusaoDeEmpresa(IEmpresaRepositorio empresaRepositorio,
            IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio) : 
            base(notificacaoDeDominio)
        {
            _empresaRepositorio = empresaRepositorio;
        }

        public async Task Excluir(int id)
        {
            var empresa = await _empresaRepositorio.ObterPorIdAsync(id);
            _empresaRepositorio.Remover(empresa);
        }
    }
}
