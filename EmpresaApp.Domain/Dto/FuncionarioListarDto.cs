using EmpresaApp.Domain.Entitys;

namespace EmpresaApp.Domain.Dto
{
    public class FuncionarioListarDto : DtoBase
    {
        public string Nome { get; set; }

        public string Cpf { get; set; }

        public string DataContratacao { get; set; }

        public string Empresa { get; set; }

        public string Cargo { get; set; }

        public static FuncionarioListarDto ConvertEntityToDto(Funcionario entity)
        {
            if (entity != null)
            {
                return new FuncionarioListarDto
                {
                    Id = entity.Id,
                    Nome = entity.Nome,
                    Cpf = entity.Cpf,
                    DataContratacao = entity.DataContratacao.ToShortDateString(),
                    Empresa = entity.Empresa?.Nome,
                    Cargo = entity.Cargo?.Descricao
                };
            }

            return null;
        }
    }
}
