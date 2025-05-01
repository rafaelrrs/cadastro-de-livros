using AppLivroCadastro.Infrastructure.Persistence;
using AppLivroCadastro.Reports.Models;
using Microsoft.EntityFrameworkCore;

namespace AppLivroCadastro.Reports.Services
{
    public class LivroAutorAssuntoReportService
    {
        private readonly AppDbContext _dbContext;

        public LivroAutorAssuntoReportService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LivroAutorAssuntoReport>> GerarRelatorioAsync()
        {
            var query = @"
                SELECT
                    a.CodAu AS AutorId,
                    a.Nome AS NomeAutor,
                    l.CodL AS LivroId,
                    l.Titulo AS TituloLivro,
                    l.Editora AS EditoraLivro,
                    l.AnoPublicacao AS AnoPublicacaoLivro,
                    ass.CodAs AS AssuntoId,
                    ass.Descricao AS DescricaoAssunto
                FROM
                    Autor a
                INNER JOIN
                    LivroAutor la ON a.CodAu = la.AutorCodAu
                INNER JOIN
                    Livro l ON la.LivroCodL = l.CodL
                INNER JOIN
                    LivroAssunto las ON l.CodL = las.LivroCodL
                INNER JOIN
                    Assunto ass ON las.AssuntoCodAs = ass.CodAs";

            return await _dbContext.Set<LivroAutorAssuntoReport>()
                .FromSqlRaw(query)
                .ToListAsync();
        }
    }
}