using EmpresaApp.Domain.Utils;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaApp.Domain.Entitys
{
    public class Funcionario : Entity<int, Funcionario>
    {
        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public DateTime? DataContratacao { get; private set; }
        public int? EmpresaId { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public int? CargoId { get; private set; }
        public virtual Cargo Cargo { get; private set; }

        public Funcionario(string nome, string cpf)
        {
            Nome = nome;
            Cpf = cpf;
        }

        public override bool Validar()
        {
            RuleFor(p => p.Nome)
               .NotEmpty()
               .NotNull();

            RuleFor(p => p.Cpf)
                .NotEmpty()
                .NotNull()
               .MinimumLength(Constantes.QuantidadeDeCaracteres11)
               .MaximumLength(Constantes.QuantidadeDeCaracteres11);

            ValidationResult = Validate(this);
            return ValidationResult.IsValid;
        }

        public void AlterarNome(string nome)
        {
            Nome = nome?.Trim();
        }

        public void AlterarCpf(string cpf)
        {
            Cpf = cpf.Trim().Replace(".", "").Replace("-", "");
        }

        public void AlterarDataContratacao(string dataContratacao)
        {
            DataContratacao = dataContratacao.ToDate();
        }

        public void AlterarCargo(int cargoId)
        {
            CargoId = cargoId;
        }

        public void AlterarEmpresa(int empresaId)
        {
            EmpresaId = empresaId;
        }
    }
}
