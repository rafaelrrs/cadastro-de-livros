using AppLivroCadastro.Domain.Entities;
using AppLivroCadastro.Reports.Models;
using Microsoft.EntityFrameworkCore;

namespace AppLivroCadastro.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Assunto> Assuntos { get; set; }
        public DbSet<FormaCompra> FormasCompra { get; set; }
        public DbSet<LivroAutor> LivroAutores { get; set; }
        public DbSet<LivroAssunto> LivroAssuntos { get; set; }
        public DbSet<LivroAutorAssuntoReport> LivroAutorAssuntoReport { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Livro>().HasKey(l => l.Codl);
            modelBuilder.Entity<Autor>().HasKey(a => a.CodAu);
            modelBuilder.Entity<Assunto>().HasKey(a => a.CodAs);
            modelBuilder.Entity<FormaCompra>().HasKey(fc => fc.FormaCompraID);

            modelBuilder.Entity<Livro>().Property(l => l.Titulo).HasMaxLength(40).IsRequired();
            modelBuilder.Entity<Livro>().Property(l => l.Editora).HasMaxLength(40);
            modelBuilder.Entity<Livro>().Property(l => l.AnoPublicacao).HasMaxLength(4);
            modelBuilder.Entity<Autor>().Property(a => a.Nome).HasMaxLength(40).IsRequired();
            modelBuilder.Entity<Assunto>().Property(a => a.Descricao).HasMaxLength(20).IsRequired();
            modelBuilder.Entity<FormaCompra>().Property(fc => fc.Nome).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<FormaCompra>().HasIndex(fc => fc.Nome).IsUnique();

            modelBuilder.Entity<LivroAutor>()
                .HasKey(la => new { la.LivroCodl, la.AutorCodAu });

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.LivroAutores)
                .HasForeignKey(la => la.LivroCodl);

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Autor)
                .WithMany(a => a.LivroAutores)
                .HasForeignKey(la => la.AutorCodAu);

            modelBuilder.Entity<LivroAssunto>()
                .HasKey(la => new { la.LivroCodl, la.AssuntoCodAs });

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.LivroAssuntos)
                .HasForeignKey(la => la.LivroCodl);

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Assunto)
                .WithMany(a => a.LivroAssuntos)
                .HasForeignKey(la => la.AssuntoCodAs);

            modelBuilder.Entity<PrecoLivroFormaCompra>()
               .HasKey(plfc => new { plfc.LivroCodl, plfc.FormaCompraID });

            modelBuilder.Entity<PrecoLivroFormaCompra>()
                .HasOne(plfc => plfc.Livro)
                .WithMany(l => l.PrecosLivroFormaCompra)
                .HasForeignKey(plfc => plfc.LivroCodl);

            modelBuilder.Entity<PrecoLivroFormaCompra>()
                .HasOne(plfc => plfc.FormaCompra)
                .WithMany(fc => fc.PrecosLivroFormaCompra)
                .HasForeignKey(plfc => plfc.FormaCompraID);

            modelBuilder.Entity<PrecoLivroFormaCompra>()
                .Property(plfc => plfc.Preco).HasColumnType("decimal(10, 2)").IsRequired();

            modelBuilder.Entity<LivroAutorAssuntoReport>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}
