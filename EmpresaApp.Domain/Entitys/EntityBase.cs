using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaApp.Domain.Entitys
{
    public abstract class EntityBase<TId, TEntity> : AbstractValidator<TEntity>
        where TId : struct
        where TEntity : EntityBase<TId, TEntity>
    {
        public TId Id { get; private set; }

        [NotMapped]
        public ValidationResult ValidationResult { get; protected set; }

        public abstract bool Validar();
    }
}
