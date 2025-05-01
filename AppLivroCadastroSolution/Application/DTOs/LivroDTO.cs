using AppLivroCadastro.Application.DTOs.CreateDTOs;

namespace AppLivroCadastro.Application.DTOs
{
    public class LivroDTO
    {
        public int Codl { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
        public List<AutorDTO> Autores { get; set; }
        public List<AssuntoDTO> Assuntos { get; set; }
        public List<PrecoFormaCompraExibicaoDTO> PrecosFormaCompra { get; set; }
    }
}
