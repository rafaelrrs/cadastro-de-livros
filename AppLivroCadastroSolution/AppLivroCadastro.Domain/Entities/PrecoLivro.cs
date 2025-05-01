namespace AppLivroCadastro.Domain.Entities
{
    public class PrecoLivro
    {
        public int Id { get; set; }
        public int LivroId { get; set; }
        public string FormaCompra { get; set; }
        public decimal Valor { get; set; }

        public Livro Livro { get; set; }
    }
}
