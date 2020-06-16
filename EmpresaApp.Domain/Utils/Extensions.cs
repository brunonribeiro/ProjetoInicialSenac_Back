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
    }
}
