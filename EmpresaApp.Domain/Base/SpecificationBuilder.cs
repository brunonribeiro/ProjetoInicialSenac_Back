using System;
using System.Collections.Generic;
using System.Text;

namespace EmpresaApp.Domain.Base
{
    public abstract class SpecificationBuilder<TEntity>
    {
        protected int Pagina;
        protected int TamanhoDaPagina;
        protected string OrdenarPor;
        protected string Nome;
        protected string Descricao;
        protected string Ordem;


        public SpecificationBuilder<TEntity> ComNome(string nome)
        {
            Nome = nome;
            return this;
        }

        public SpecificationBuilder<TEntity> ComDescricao(string descricao)
        {
            Descricao = descricao;
            return this;
        }

        public SpecificationBuilder<TEntity> ComOrdem(string ordem)
        {
            Ordem = ordem;
            return this;
        }

        public SpecificationBuilder<TEntity> ComPagina(int pagina)
        {
            Pagina = pagina;
            return this;
        }

        public SpecificationBuilder<TEntity> ComTamanhoDaPagina(int tamanhoDaPagina)
        {
            TamanhoDaPagina = tamanhoDaPagina;
            return this;
        }

        public SpecificationBuilder<TEntity> ComOrdemPor(string ordenarPor)
        {
            OrdenarPor = ordenarPor;
            return this;
        }
        public abstract Specification<TEntity> Build();
    }
}
