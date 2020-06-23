using FluentValidation;

namespace EmpresaApp.Domain.Entitys
{
    public class Cargo : EntityBase<int, Cargo>
    {
        public string Descricao { get; private set; }

        public Cargo(string descricao)
        {
            Descricao = descricao?.Trim();
        }

        public override bool Validar()
        {
            RuleFor(x => x.Descricao).NotEmpty().NotNull();

            ValidationResult = Validate(this);
            return ValidationResult.IsValid;
        }

        public void AlterarDescricao(string descricao)
        {
            Descricao = descricao?.Trim();
        }
    }
}
