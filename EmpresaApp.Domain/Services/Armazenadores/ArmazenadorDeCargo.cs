using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Armazenadores;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Utils;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Services.Armazenadores
{
    public class ArmazenadorDeCargo : DomainService, IArmazenadorDeCargo
    {
        private readonly ICargoRepositorio _cargoRepositorio;

        public ArmazenadorDeCargo(
            ICargoRepositorio cargoRepositorio, 
            IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio) :
            base(notificacaoDeDominio)
        {
            _cargoRepositorio = cargoRepositorio;
        }

        public async Task Armazenar(CargoDto dto)
        {
            await ValidarCargoComMesmaDescricao(dto);

            if (!NotificacaoDeDominio.HasNotifications())
            {
                var cargo = new Cargo(dto.Descricao);

                if (dto.Id > 0)
                {
                    cargo = await _cargoRepositorio.ObterPorIdAsync(dto.Id);
                    cargo.AlterarDescricao(dto.Descricao);
                }

                if (cargo.Validar() && cargo.Id == 0)
                {
                    await _cargoRepositorio.AdicionarAsync(cargo);
                }
                else
                {
                    await NotificarValidacoesDeDominio(cargo.ValidationResult);
                }
            }
        }

        private async Task ValidarCargoComMesmaDescricao(CargoDto dto)
        {
            var cargoComMesmaDescricao = await _cargoRepositorio.ObterPorDescricaoAsync(dto.Descricao);

            if (cargoComMesmaDescricao != null && cargoComMesmaDescricao.Id != dto.Id)
                await NotificarValidacaoDeServico(string.Format(CommonResources.MsgDominioComMesmoNomeNoMasculino, CommonResources.CargoDominio));
        }
    }
}
