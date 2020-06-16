namespace EmpresaApp.Domain.Entitys
{
    public abstract class Entity<TDto>
    {
        public int Id { get; protected set; }

        public abstract void Update(TDto dto);
    }
}
