namespace AppLivroCadastro.Domain.Entities
{
    public class Livro
    {
        public int Codl { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }

        public ICollection<LivroAutor> LivroAutores { get; set; }

        public ICollection<LivroAssunto> LivroAssuntos { get; set; }

        public ICollection<PrecoLivroFormaCompra> PrecosLivroFormaCompra { get; set; }
    }

}
