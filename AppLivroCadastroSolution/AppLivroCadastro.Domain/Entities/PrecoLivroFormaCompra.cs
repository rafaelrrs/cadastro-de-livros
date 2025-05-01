namespace AppLivroCadastro.Domain.Entities
{
    public class PrecoLivroFormaCompra
    {
        public int LivroCodl { get; set; }
        public Livro Livro { get; set; }

        public int FormaCompraID { get; set; }
        public FormaCompra FormaCompra { get; set; }

        public decimal Preco { get; set; }
    }
}
