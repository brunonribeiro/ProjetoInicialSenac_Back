using EmpresaApp.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace EmpresaApp.Domain.Attributes
{
    public class CPFAttribute : ValidationAttribute
    {
        public CPFAttribute() { }

        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            bool valido = Validations.ValidarCPF(value.ToString());
            return valido;
        }
    }
}
