using System;

namespace EmpresaApp.Domain.Utils
{
    public static class Extensions
    {
        public static DateTime ToDate(this string texto)
        {
            DateTime.TryParse(texto, out var valor);
            return valor;
        }

        public static string RemoverMascaraCpfCnpj(this string cpfCnpj)
        {
            var result = cpfCnpj?.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            return result;
        }
    }
}
