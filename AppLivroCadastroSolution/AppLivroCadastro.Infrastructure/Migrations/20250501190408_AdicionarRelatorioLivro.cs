using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppLivroCadastro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarRelatorioLivro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LivroAutorAssuntoReport",
                columns: table => new
                {
                    AutorId = table.Column<int>(type: "INTEGER", nullable: false),
                    NomeAutor = table.Column<string>(type: "TEXT", nullable: false),
                    LivroId = table.Column<int>(type: "INTEGER", nullable: false),
                    TituloLivro = table.Column<string>(type: "TEXT", nullable: false),
                    EditoraLivro = table.Column<string>(type: "TEXT", nullable: false),
                    AnoPublicacaoLivro = table.Column<string>(type: "TEXT", nullable: false),
                    AssuntoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DescricaoAssunto = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivroAutorAssuntoReport");
        }
    }
}
