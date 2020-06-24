using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Armazenadores;
using EmpresaApp.Domain.Interfaces.Repositorios;
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
            ICargoRepositorio cargoRepositorio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
            _empresaRepositorio = empresaRepositorio;
            _cargoRepositorio = cargoRepositorio;
        }
      
        public async Task Armazenar(FuncionarioDto dto)
        {
            await ValidarFuncionarioComMesmoNome(dto);

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
                NotificarValidacoesDeDominio(funcionario.ValidationResult);
            }
        }

        public async Task AdicionarEmpresa(int funcionarioId, int empresaId)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(funcionarioId);

            ValidarFuncionaroNaoCadastrado(funcionario);
            await ValidarEmpresaNaoCadastrada(empresaId);

            funcionario.AlterarEmpresa(empresaId);
        }

        public async Task AdicionarCargo(int funcionarioId, int cargoId)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(funcionarioId);

            ValidarFuncionaroNaoCadastrado(funcionario);
            ValidarFuncionaroComEmpresaCadastrada(funcionario);
            await ValidarCargoNaoCadastrado(cargoId);

            funcionario.AlterarCargo(funcionarioId);
        }      

        private async Task ValidarFuncionarioComMesmoNome(FuncionarioDto dto)
        {
            var funcionarioComMesmaNome = await _funcionarioRepositorio.ObterPorNomeAsync(dto.Nome);

            if (funcionarioComMesmaNome != null && funcionarioComMesmaNome.Id != dto.Id)
                NotificarValidacoesDoArmazenador(CommonResources.MsgDominioComMesmoNomeNoMasculino);
        }

        private void ValidarFuncionaroNaoCadastrado(Funcionario funcionario)
        {
            if (funcionario == null)
                NotificarValidacoesDoArmazenador("O funcionário informado não está cadastrado.");
        }

        private async Task ValidarEmpresaNaoCadastrada(int empresaId)
        {
            var empresa = await _empresaRepositorio.ObterPorIdAsync(empresaId);
            if (empresa == null)
                NotificarValidacoesDoArmazenador("A empresa informada não está cadastrada.");
        }

        private async Task ValidarCargoNaoCadastrado(int cargoId)
        {
            var cargo = await _cargoRepositorio.ObterPorIdAsync(cargoId);
            if (cargo == null)
                NotificarValidacoesDoArmazenador("O cargo informado não está cadastrado.");
        }
        private void ValidarFuncionaroComEmpresaCadastrada(Funcionario funcionario)
        {
            if (!funcionario.EmpresaId.HasValue)
                NotificarValidacoesDoArmazenador("O funcionário precisa estar vinculado a uma empresa para atribuir um cargo.");
        }

    }
}