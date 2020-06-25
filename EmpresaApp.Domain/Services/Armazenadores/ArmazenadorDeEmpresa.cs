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
    public class ArmazenadorDeEmpresa : DomainService, IArmazenadorDeEmpresa
    {
        private readonly IEmpresaRepositorio _empresaRepositorio;

        public ArmazenadorDeEmpresa(
            IEmpresaRepositorio empresaRepositorio,
            IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio) :
            base(notificacaoDeDominio)
        {
            _empresaRepositorio = empresaRepositorio;
        }

        public async Task Armazenar(EmpresaDto dto)
        {
            await ValidarEmpresaComMesmoNome(dto);

            var empresa = new Empresa(dto.Nome, dto.Cnpj);

            if (!NotificacaoDeDominio.HasNotifications())
            {
                if (dto.Id > 0)
                {
                    empresa = await _empresaRepositorio.ObterPorIdAsync(dto.Id);
                    empresa.AlterarNome(dto.Nome);
                    empresa.AlterarCnpj(dto.Cnpj);
                }

                empresa.AlterarDataFundacao(dto.DataFundacao);

                if (empresa.Validar() && empresa.Id == 0)
                {
                    await _empresaRepositorio.AdicionarAsync(empresa);
                }
                else
                {
                    await NotificarValidacoesDeDominio(empresa.ValidationResult);
                }
            }
        }

        private async Task ValidarEmpresaComMesmoNome(EmpresaDto dto)
        {
            var empresaComMesmaNome = await _empresaRepositorio.ObterPorNomeAsync(dto.Nome);

            if (empresaComMesmaNome != null && empresaComMesmaNome.Id != dto.Id)
                await NotificarValidacaoDeServico(CommonResources.MsgDominioComMesmoNomeNoFeminino);
        }
    }
}
