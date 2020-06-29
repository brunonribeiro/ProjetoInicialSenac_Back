using System.Collections.Generic;

namespace EmpresaApp.Domain.Base
{
    public class ResultadoDaConsultaBase
    {
        public int Total { get; set; }
        public object InformacoesGerais { get; set; }
        public IEnumerable<object> Lista { get; set; }
    }

    public class ResultadoDaConsultaBase<T>
    {
        public int Total { get; set; }
        public T InformacoesGerais { get; set; }
        public IEnumerable<T> Lista { get; set; }
    }
}
