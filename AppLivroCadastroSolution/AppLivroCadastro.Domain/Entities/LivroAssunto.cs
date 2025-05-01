namespace AppLivroCadastro.Domain.Entities
{
    public class LivroAssunto
    {
        public int LivroCodl { get; set; }
        public Livro Livro { get; set; }

        public int AssuntoCodAs { get; set; }
        public Assunto Assunto { get; set; }
    }
}
