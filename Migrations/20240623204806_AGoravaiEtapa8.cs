using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja.Migrations
{
    /// <inheritdoc />
    public partial class AGoravaiEtapa8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataVenda",
                table: "Vendas",
                newName: "Data");

            migrationBuilder.RenameColumn(
                name: "Endereço",
                table: "Fornecedores",
                newName: "Endereco");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Vendas",
                newName: "DataVenda");

            migrationBuilder.RenameColumn(
                name: "Endereco",
                table: "Fornecedores",
                newName: "Endereço");
        }
    }
}
