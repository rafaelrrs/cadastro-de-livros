namespace AppLivroCadastro.Application.DTOs.CreateDTOs
{
    public class CreateLivroDTO
    {
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
        public List<int> AutorIds { get; set; } = new List<int>();
        public List<int> AssuntoIds { get; set; } = new List<int>();
        public List<PrecoFormaCompraDTO> PrecosFormaCompra { get; set; } = new List<PrecoFormaCompraDTO>();
    }
}
