using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Utils;
using System;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Services.Exclusoes
{
    public class ExclusaoDeFuncionario : DomainService
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public ExclusaoDeFuncionario(IFuncionarioRepositorio funcionarioRepositorio,
            IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio) : 
            base(notificacaoDeDominio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public async Task Excluir(int id)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(id);

            if (await VerificarFuncionarioInvalido(funcionario))
                return;

            if (!NotificacaoDeDominio.HasNotifications())
            {
                _funcionarioRepositorio.Remover(funcionario);
            }
        }

        private async Task<bool> VerificarFuncionarioInvalido(Funcionario funcionario)
        {
            if (funcionario == null)
            {
                await NotificarValidacaoDeServico(string.Format(CommonResources.MsgDominioNaoCadastradoNoMasculino, CommonResources.FuncionarioDominio));
                return true;
            }
            return false;
        }
    }
}
