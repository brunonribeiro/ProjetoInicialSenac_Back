using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Armazenadores;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Utils;
using System.Threading.Tasks;

namespace FuncionarioApp.Domain.Services.Armazenadores
{
    public class ArmazenadorDeFuncionario : DomainService, IArmazenadorDeFuncionario
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly ICargoRepositorio _cargoRepositorio;

        public ArmazenadorDeFuncionario(
            IFuncionarioRepositorio funcionarioRepositorio,
            IEmpresaRepositorio empresaRepositorio,
            ICargoRepositorio cargoRepositorio,
            IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio) :
            base(notificacaoDeDominio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
            _empresaRepositorio = empresaRepositorio;
            _cargoRepositorio = cargoRepositorio;
        }

        public async Task Armazenar(FuncionarioDto dto)
        {
            await ValidarFuncionarioComMesmoNome(dto);

            if (!NotificacaoDeDominio.HasNotifications())
            {
                var funcionario = new Funcionario(dto.Nome, dto.Cpf);
                if (dto.Id > 0)
                {
                    funcionario = await _funcionarioRepositorio.ObterPorIdAsync(dto.Id);
                    funcionario.AlterarNome(dto.Nome);
                    funcionario.AlterarCpf(dto.Cpf);
                }

                funcionario.AlterarDataContratacao(dto.DataContratacao);

                if (funcionario.Validar() && funcionario.Id == 0)
                {
                    await _funcionarioRepositorio.AdicionarAsync(funcionario);
                }
                else
                {
                    await NotificarValidacoesDeDominio(funcionario.ValidationResult);
                }
            }
        }

        public async Task AdicionarEmpresa(int funcionarioId, int empresaId)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(funcionarioId);

            await ValidarFuncionaroNaoCadastrado(funcionario);
            await ValidarEmpresaNaoCadastrada(empresaId);

            if (!NotificacaoDeDominio.HasNotifications())
            {
                funcionario.AlterarEmpresa(empresaId);
            }
        }

        public async Task AdicionarCargo(int funcionarioId, int cargoId)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(funcionarioId);

            await ValidarFuncionaroNaoCadastrado(funcionario);
            await ValidarFuncionaroComEmpresaCadastrada(funcionario);
            await ValidarCargoNaoCadastrado(cargoId);

            if (!NotificacaoDeDominio.HasNotifications())
            {
                funcionario.AlterarCargo(cargoId);
            }
        }

        private async Task ValidarFuncionarioComMesmoNome(FuncionarioDto dto)
        {
            var funcionarioComMesmaNome = await _funcionarioRepositorio.ObterPorNomeAsync(dto.Nome);

            if (funcionarioComMesmaNome != null && funcionarioComMesmaNome.Id != dto.Id)
                await NotificarValidacaoDeServico(CommonResources.MsgDominioComMesmoNomeNoMasculino);
        }

        private async Task ValidarFuncionaroNaoCadastrado(Funcionario funcionario)
        {
            if (funcionario == null)
                await NotificarValidacaoDeServico("O funcionário informado não está cadastrado.");
        }

        private async Task ValidarEmpresaNaoCadastrada(int empresaId)
        {
            var empresa = await _empresaRepositorio.ObterPorIdAsync(empresaId);
            if (empresa == null)
                await NotificarValidacaoDeServico("A empresa informada não está cadastrada.");
        }

        private async Task ValidarCargoNaoCadastrado(int cargoId)
        {
            var cargo = await _cargoRepositorio.ObterPorIdAsync(cargoId);
            if (cargo == null)
                await NotificarValidacaoDeServico("O cargo informado não está cadastrado.");
        }

        private async Task ValidarFuncionaroComEmpresaCadastrada(Funcionario funcionario)
        {
            if (!funcionario.EmpresaId.HasValue)
                await NotificarValidacaoDeServico("O funcionário precisa estar vinculado a uma empresa para atribuir um cargo.");
        }

    }
}