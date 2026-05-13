using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioBackendSprint3_GabrielVinicius.Migrations
{
    /// <inheritdoc />
    public partial class NovoBancoRelacionado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountCorrente",
                table: "ContasBancarias");

            migrationBuilder.DropColumn(
                name: "CountEmpresarial",
                table: "ContasBancarias");

            migrationBuilder.RenameColumn(
                name: "CountPoupanca",
                table: "ContasBancarias",
                newName: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasBancarias_UsuarioId",
                table: "ContasBancarias",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContasBancarias_Usuarios_UsuarioId",
                table: "ContasBancarias",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContasBancarias_Usuarios_UsuarioId",
                table: "ContasBancarias");

            migrationBuilder.DropIndex(
                name: "IX_ContasBancarias_UsuarioId",
                table: "ContasBancarias");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "ContasBancarias",
                newName: "CountPoupanca");

            migrationBuilder.AddColumn<int>(
                name: "CountCorrente",
                table: "ContasBancarias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountEmpresarial",
                table: "ContasBancarias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
