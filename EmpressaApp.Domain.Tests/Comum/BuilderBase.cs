using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmpressaApp.Domain.Tests.Comum
{
    public abstract class BuilderBase
    {
        protected void AtribuirId(int id, object entidade)
        {
            if (!TemId(id)) return;

            Atribuir(id, "Id", entidade);
        }

        private bool TemId(long id)
        {
            return id > 0;
        }

        protected void AtribuirId(long id, object entidade)
        {
            if (!TemId(id)) return;

            Atribuir(id, "Id", entidade);
        }

        private void Atribuir(object valor, string propriedade, object entidade)
        {
            var propertyInfo = entidade.GetType().GetProperty(propriedade, BindingFlags.Public | BindingFlags.Instance);
            propertyInfo.SetValue(entidade, Convert.ChangeType(valor, propertyInfo.PropertyType));
        }
    }
}
