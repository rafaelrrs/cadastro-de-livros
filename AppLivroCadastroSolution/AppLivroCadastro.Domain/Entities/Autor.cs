﻿namespace AppLivroCadastro.Domain.Entities
{
    public class Autor
    {
        public int CodAu { get; set; }
        public string Nome { get; set; }

        public ICollection<LivroAutor> LivroAutores { get; set; }

    }
}
