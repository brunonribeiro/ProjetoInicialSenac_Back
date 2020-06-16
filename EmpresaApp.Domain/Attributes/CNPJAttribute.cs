using EmpresaApp.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace EmpresaApp.Domain.Attributes
{
    public class CNPJAttribute : ValidationAttribute
    {
        public CNPJAttribute() { }

        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            bool valido = Validations.ValidarCNPJ(value.ToString());
            return valido;
        }
    }
}
