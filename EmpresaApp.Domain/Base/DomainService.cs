using FluentValidation.Results;
using System;
using System.Text;

namespace EmpresaApp.Domain.Base
{
    public abstract class DomainService
    {
        protected DomainService()
        {
        }

        public void NotificarValidacoesDeDominio(ValidationResult validationResult)
        {
            if (validationResult.Errors.Count > 0)
            {
                var erroFinal = new StringBuilder();

                foreach (var erro in validationResult.Errors)
                    erroFinal.AppendLine(erro.ErrorMessage);

                throw new ArgumentException(erroFinal.ToString());
            }
        }

        public void NotificarValidacoesDoArmazenador(string mensagem)
        {
            throw new ArgumentException(mensagem);
        }

    }
}
