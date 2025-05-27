using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja.Migrations
{
    public partial class Etapa8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Clientes` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
                    `Cpf` longtext CHARACTER SET utf8mb4 NOT NULL,
                    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
                    CONSTRAINT `PK_Clientes` PRIMARY KEY (`Id`)
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Depositos` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
                    CONSTRAINT `PK_Depositos` PRIMARY KEY (`Id`)
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Fornecedores` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Cnpj` longtext CHARACTER SET utf8mb4 NOT NULL,
                    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
                    `Endereço` longtext CHARACTER SET utf8mb4 NOT NULL,
                    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
                    `Telefone` longtext CHARACTER SET utf8mb4 NOT NULL,
                    CONSTRAINT `PK_Fornecedores` PRIMARY KEY (`Id`)
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Produtos` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
                    `Preco` double NOT NULL,
                    `Fornecedor` longtext CHARACTER SET utf8mb4 NOT NULL,
                    CONSTRAINT `PK_Produtos` PRIMARY KEY (`Id`)
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Estoques` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `DepositoId` int NOT NULL,
                    `ProdutoId` int NOT NULL,
                    `Quantidade` int NOT NULL,
                    CONSTRAINT `PK_Estoques` PRIMARY KEY (`Id`),
                    CONSTRAINT `FK_Estoques_Depositos_DepositoId` FOREIGN KEY (`DepositoId`) REFERENCES `Depositos` (`Id`) ON DELETE CASCADE,
                    CONSTRAINT `FK_Estoques_Produtos_ProdutoId` FOREIGN KEY (`ProdutoId`) REFERENCES `Produtos` (`Id`) ON DELETE CASCADE
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Vendas` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `DataVenda` datetime(6) NOT NULL,
                    `NumeroNotaFiscal` longtext CHARACTER SET utf8mb4 NOT NULL,
                    `ClienteId` int NOT NULL,
                    `ProdutoId` int NOT NULL,
                    `Quantidade` int NOT NULL,
                    `PrecoUnitario` double NOT NULL,
                    CONSTRAINT `PK_Vendas` PRIMARY KEY (`Id`),
                    CONSTRAINT `FK_Vendas_Clientes_ClienteId` FOREIGN KEY (`ClienteId`) REFERENCES `Clientes` (`Id`) ON DELETE CASCADE,
                    CONSTRAINT `FK_Vendas_Produtos_ProdutoId` FOREIGN KEY (`ProdutoId`) REFERENCES `Produtos` (`Id`) ON DELETE CASCADE
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Estoques_DepositoId",
                table: "Estoques",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_Estoques_ProdutoId",
                table: "Estoques",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_ClienteId",
                table: "Vendas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_ProdutoId",
                table: "Vendas",
                column: "ProdutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estoques");

            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Depositos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Produtos");
        }
    }
}
