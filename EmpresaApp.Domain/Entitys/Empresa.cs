using EmpresaApp.Domain.Utils;
using FluentValidation;
using System;

namespace EmpresaApp.Domain.Entitys
{
    public class Empresa : EntityBase<int, Empresa>
    {
        public string Nome { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime? DataFundacao { get; private set; }

        public Empresa(string nome, string cnpj)
        {
            Nome = nome;
            Cnpj = cnpj;
        }
        public override bool Validar()
        {
            RuleFor(p => p.Nome)
             .NotEmpty()
             .NotNull();

            RuleFor(p => p.Cnpj)
                .NotEmpty()
                .NotNull()
               .MinimumLength(Constantes.QuantidadeDeCaracteres14)
               .MaximumLength(Constantes.QuantidadeDeCaracteres14);

            ValidationResult = Validate(this);
            return ValidationResult.IsValid;
        }

        public void AlterarNome(string nome)
        {
            Nome = nome.Trim();
        }
        public void AlterarCnpj(string cnpj)
        {
            Cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
        }

        public void AlterarDataFundacao(string dataFundacao)
        {
            DataFundacao = dataFundacao.ToDate();
        }

    }
}
