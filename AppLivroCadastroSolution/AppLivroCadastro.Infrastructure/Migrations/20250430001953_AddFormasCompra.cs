using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppLivroCadastro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFormasCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrecoLivroFormaCompra_FormaCompra_FormaCompraID",
                table: "PrecoLivroFormaCompra");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormaCompra",
                table: "FormaCompra");

            migrationBuilder.RenameTable(
                name: "FormaCompra",
                newName: "FormasCompra");

            migrationBuilder.RenameIndex(
                name: "IX_FormaCompra_Nome",
                table: "FormasCompra",
                newName: "IX_FormasCompra_Nome");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormasCompra",
                table: "FormasCompra",
                column: "FormaCompraID");

            migrationBuilder.AddForeignKey(
                name: "FK_PrecoLivroFormaCompra_FormasCompra_FormaCompraID",
                table: "PrecoLivroFormaCompra",
                column: "FormaCompraID",
                principalTable: "FormasCompra",
                principalColumn: "FormaCompraID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrecoLivroFormaCompra_FormasCompra_FormaCompraID",
                table: "PrecoLivroFormaCompra");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormasCompra",
                table: "FormasCompra");

            migrationBuilder.RenameTable(
                name: "FormasCompra",
                newName: "FormaCompra");

            migrationBuilder.RenameIndex(
                name: "IX_FormasCompra_Nome",
                table: "FormaCompra",
                newName: "IX_FormaCompra_Nome");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormaCompra",
                table: "FormaCompra",
                column: "FormaCompraID");

            migrationBuilder.AddForeignKey(
                name: "FK_PrecoLivroFormaCompra_FormaCompra_FormaCompraID",
                table: "PrecoLivroFormaCompra",
                column: "FormaCompraID",
                principalTable: "FormaCompra",
                principalColumn: "FormaCompraID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
