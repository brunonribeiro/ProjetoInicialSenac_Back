namespace EmpresaApp.API.Models
{
    public class Filtro
    {
        public int Pagina { get; set; }
        public int TamanhoDaPagina { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string OrdenarPor { get; set; }
        public string Ordem { get; set; }

        public Filtro()
        {
            TamanhoDaPagina = 10;
        }
    }
}
