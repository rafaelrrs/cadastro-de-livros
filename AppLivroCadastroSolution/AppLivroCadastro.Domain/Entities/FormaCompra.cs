namespace AppLivroCadastro.Domain.Entities
{
    public class FormaCompra
    {
        public int FormaCompraID { get; set; }
        public string Nome { get; set; }
        public ICollection<PrecoLivroFormaCompra> PrecosLivroFormaCompra { get; set; }
    }
}
