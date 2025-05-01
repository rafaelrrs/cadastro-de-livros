using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppLivroCadastro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assuntos",
                columns: table => new
                {
                    CodAs = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assuntos", x => x.CodAs);
                });

            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    CodAu = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.CodAu);
                });

            migrationBuilder.CreateTable(
                name: "FormaCompra",
                columns: table => new
                {
                    FormaCompraID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormaCompra", x => x.FormaCompraID);
                });

            migrationBuilder.CreateTable(
                name: "Livros",
                columns: table => new
                {
                    Codl = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Editora = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Edicao = table.Column<int>(type: "INTEGER", nullable: false),
                    AnoPublicacao = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livros", x => x.Codl);
                });

            migrationBuilder.CreateTable(
                name: "LivroAssuntos",
                columns: table => new
                {
                    LivroCodl = table.Column<int>(type: "INTEGER", nullable: false),
                    AssuntoCodAs = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivroAssuntos", x => new { x.LivroCodl, x.AssuntoCodAs });
                    table.ForeignKey(
                        name: "FK_LivroAssuntos_Assuntos_AssuntoCodAs",
                        column: x => x.AssuntoCodAs,
                        principalTable: "Assuntos",
                        principalColumn: "CodAs",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivroAssuntos_Livros_LivroCodl",
                        column: x => x.LivroCodl,
                        principalTable: "Livros",
                        principalColumn: "Codl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivroAutores",
                columns: table => new
                {
                    LivroCodl = table.Column<int>(type: "INTEGER", nullable: false),
                    AutorCodAu = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivroAutores", x => new { x.LivroCodl, x.AutorCodAu });
                    table.ForeignKey(
                        name: "FK_LivroAutores_Autores_AutorCodAu",
                        column: x => x.AutorCodAu,
                        principalTable: "Autores",
                        principalColumn: "CodAu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivroAutores_Livros_LivroCodl",
                        column: x => x.LivroCodl,
                        principalTable: "Livros",
                        principalColumn: "Codl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrecoLivroFormaCompra",
                columns: table => new
                {
                    LivroCodl = table.Column<int>(type: "INTEGER", nullable: false),
                    FormaCompraID = table.Column<int>(type: "INTEGER", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrecoLivroFormaCompra", x => new { x.LivroCodl, x.FormaCompraID });
                    table.ForeignKey(
                        name: "FK_PrecoLivroFormaCompra_FormaCompra_FormaCompraID",
                        column: x => x.FormaCompraID,
                        principalTable: "FormaCompra",
                        principalColumn: "FormaCompraID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrecoLivroFormaCompra_Livros_LivroCodl",
                        column: x => x.LivroCodl,
                        principalTable: "Livros",
                        principalColumn: "Codl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormaCompra_Nome",
                table: "FormaCompra",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LivroAssuntos_AssuntoCodAs",
                table: "LivroAssuntos",
                column: "AssuntoCodAs");

            migrationBuilder.CreateIndex(
                name: "IX_LivroAutores_AutorCodAu",
                table: "LivroAutores",
                column: "AutorCodAu");

            migrationBuilder.CreateIndex(
                name: "IX_PrecoLivroFormaCompra_FormaCompraID",
                table: "PrecoLivroFormaCompra",
                column: "FormaCompraID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivroAssuntos");

            migrationBuilder.DropTable(
                name: "LivroAutores");

            migrationBuilder.DropTable(
                name: "PrecoLivroFormaCompra");

            migrationBuilder.DropTable(
                name: "Assuntos");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropTable(
                name: "FormaCompra");

            migrationBuilder.DropTable(
                name: "Livros");
        }
    }
}
