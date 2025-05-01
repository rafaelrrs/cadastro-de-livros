namespace AppLivroCadastro.Reports.Models
{
    public class LivroAutorAssuntoReport
    {
        public int AutorId { get; set; }
        public string NomeAutor { get; set; }
        public int LivroId { get; set; }
        public string TituloLivro { get; set; }
        public string EditoraLivro { get; set; }
        public string AnoPublicacaoLivro { get; set; }
        public int AssuntoId { get; set; }
        public string DescricaoAssunto { get; set; }
    }
}
