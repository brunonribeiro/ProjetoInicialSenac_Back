using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Utils;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Services.Exclusoes
{
    public class ExclusaoDeEmpresa : DomainService
    {
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public ExclusaoDeEmpresa(IEmpresaRepositorio empresaRepositorio, 
            IFuncionarioRepositorio funcionarioRepositorio,
            IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio) :
            base(notificacaoDeDominio)
        {
            _empresaRepositorio = empresaRepositorio;
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public async Task Excluir(int id)
        {
            var empresa = await _empresaRepositorio.ObterPorIdAsync(id);

            if (await VerificarEmpresaInvalida(empresa))
                return;

            await VerificarEmpresaAtribuidoFuncionario(empresa);

            if (!NotificacaoDeDominio.HasNotifications())
            {
                _empresaRepositorio.Remover(empresa);
            }
        }

        private async Task<bool> VerificarEmpresaInvalida(Empresa empresa)
        {
            if (empresa == null)
            {
                await NotificarValidacaoDeServico(string.Format(CommonResources.MsgDominioNaoCadastradoNoFeminino, CommonResources.EmpresaDominio));
                return true;
            }
            return false;
        }

        private async Task VerificarEmpresaAtribuidoFuncionario(Empresa empresa)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorEmpresaIdAsync(empresa.Id);

            if (funcionario != null)
                await NotificarValidacaoDeServico(CommonResources.MsgEmpresaEstaVinculadoComFuncionario);
        }
    }
}
