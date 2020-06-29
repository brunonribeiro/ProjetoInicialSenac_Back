using EmpresaApp.Domain.AutoMapper;
using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Utils;
using System.Collections.Generic;
using System.Linq;

namespace EmpresaApp.Data
{
    public class ConsultaBase<TEntity, TDto> : IConsultaBase<TEntity, TDto>
        where TEntity : class
    {

        private readonly DataContext _context;
        private readonly ResultadoDaConsultaBase _resultado;
        protected IQueryable<TEntity> Query { get; set; }

        public ConsultaBase(DataContext context)
        {
            _context = context;
            _resultado = new ResultadoDaConsultaBase();
        }

        public virtual void PrepararQuery(Specification<TEntity> specification)
        {
            Query = _context.Set<TEntity>().Where(specification.Predicate);
        }

        public ResultadoDaConsultaBase Consultar(Specification<TEntity> specification)
        {
            PrepararQuery(specification);

            if (specification.Includes != null)
                Query = specification.Includes(Query);

            if (specification.Order != null)
            {
                Query = !string.IsNullOrEmpty(specification.Sort) &&
                        specification.Sort.ToLower().Equals(CommonResources.OrdemDesc)
                    ? Query.OrderByDescending(specification.Order)
                    : Query.OrderBy(specification.Order);
            }

            if (!specification.Page.HasValue)
            {
                var entities = Query.ToList();
                _resultado.Lista = specification.Group == null
                    ? (IEnumerable<object>)entities.EnumerableTo<TDto>()
                    : (IEnumerable<object>)entities.AsQueryable().GroupBy(specification.Group).Select(t => t.First()).ToList().EnumerableTo<TDto>();
                _resultado.Total = Query.Count();
            }
            else
            {
                var page = (specification.Page.Value - 1) >= 0 ? (specification.Page.Value - 1) : 0;
                var total = page * specification.Size;

                var entities = Query.Skip(total).Take(specification.Size).ToList();
                _resultado.Lista = specification.Group == null
                    ? (IEnumerable<object>)entities.EnumerableTo<TDto>()
                    : (IEnumerable<object>)entities.AsQueryable().GroupBy(specification.Group).Select(t => t.First()).ToList().EnumerableTo<TDto>();

                _resultado.Total = Query.Count();
            }

            return _resultado;
        }

        public IEnumerable<TDto> Consultar()
        {
            Query = _context.Set<TEntity>();
            var entities = Query.ToList();
            var lista = entities.EnumerableTo<TDto>();

            return lista;
        }
    }
}
