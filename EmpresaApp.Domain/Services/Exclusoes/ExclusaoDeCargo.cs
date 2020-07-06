using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Utils;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Services.Exclusoes
{
    public class ExclusaoDeCargo : DomainService
    {
        private readonly ICargoRepositorio _cargoRepositorio;
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public ExclusaoDeCargo(ICargoRepositorio cargoRepositorio,
            IFuncionarioRepositorio funcionarioRepositorio,
            IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio)
            : base(notificacaoDeDominio)
        {
            _cargoRepositorio = cargoRepositorio;
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public async Task Excluir(int id)
        {
            var cargo = await _cargoRepositorio.ObterPorIdAsync(id);

            if (await VerificarCargoInvalido(cargo))
                return;

            await VerificarCargoAtribuidoFuncionario(cargo);

            if (!NotificacaoDeDominio.HasNotifications())
            {
                _cargoRepositorio.Remover(cargo);
            }
        }

        private async Task<bool> VerificarCargoInvalido(Cargo cargo)
        {
            if (cargo == null)
            {
                await NotificarValidacaoDeServico(string.Format(CommonResources.MsgDominioNaoCadastradoNoMasculino, CommonResources.CargoDominio));
                return true;
            }
            return false;
        }

        private async Task VerificarCargoAtribuidoFuncionario(Cargo cargo)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorCargoIdAsync(cargo.Id);

            if(funcionario != null)
                await NotificarValidacaoDeServico(CommonResources.MsgCargoEstaVinculadoComFuncionario);
        }
    }
}
