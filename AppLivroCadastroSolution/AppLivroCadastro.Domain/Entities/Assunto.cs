namespace AppLivroCadastro.Domain.Entities
{
    public class Assunto
    {
        public int CodAs { get; set; }
        public string Descricao { get; set; }

        public ICollection<LivroAssunto> LivroAssuntos { get; set; }
    }
}
