using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeContatos.Migrations
{
    public partial class CreateCidade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Contatos");

            migrationBuilder.AddColumn<int>(
                name: "CidadeId",
                table: "Contatos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_CidadeId",
                table: "Contatos",
                column: "CidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_Cidades_CidadeId",
                table: "Contatos",
                column: "CidadeId",
                principalTable: "Cidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_Cidades_CidadeId",
                table: "Contatos");

            migrationBuilder.DropIndex(
                name: "IX_Contatos_CidadeId",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "CidadeId",
                table: "Contatos");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Contatos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
