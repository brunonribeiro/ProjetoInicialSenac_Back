using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EmpresaApp.Domain.Base
{
    public class Specification<T>
    {
        public Expression<Func<T, bool>> Predicate { get; protected set; }
        public Func<T, bool> Predicate2 { get; set; }
        public Expression<Func<T, object>> Order { get; set; }
        public Expression<Func<T, object>> Then { get; set; }
        public Expression<Func<T, object>> Group { get; set; }
        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public string Sort { get; set; }

        public Specification(Expression<Func<T, bool>> predicate)
        {
            Predicate = predicate;
            Size = 10;
        }

        public Specification<T> OrderBy(string fieldName)
        {
            var specification = Build();

            if (string.IsNullOrEmpty(fieldName))
                return specification;

            var param = Expression.Parameter(typeof(T), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, fieldName), typeof(object));
            var order = Expression.Lambda<Func<T, object>>(conversion, param);
            specification.Order = order;
            return specification;
        }

        public Specification<T> Sorting(string sort)
        {
            var specification = Build();
            specification.Sort = sort;
            return specification;
        }

        public Specification<T> Paging(int page)
        {
            var specification = Build();
            if (page > 0)
                specification.Page = page;
            return specification;
        }

        public Specification<T> PageSize(int size)
        {
            var specification = Build();
            if (size > 0)
                specification.Size = size;
            return specification;
        }

        private Specification<T> Build()
        {
            return new Specification<T>(Predicate)
            {
                Predicate2 = Predicate2,
                Order = Order,
                Then = Then,
                Group = Group,
                Includes = Includes,
                Page = Page,
                Size = Size,
                Sort = Sort,
            };
        }
    }
}
