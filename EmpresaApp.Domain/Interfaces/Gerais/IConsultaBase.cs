using EmpresaApp.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpresaApp.Domain.Interfaces.Gerais
{
    public interface IConsultaBase<TEntity, TDto>
    {
        ResultadoDaConsultaBase Consultar(Specification<TEntity> specification);
        IEnumerable<TDto> Consultar();
    }
}
