using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeContatos.Migrations
{
    public partial class UpdateTableCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_Cidades_CidadeId",
                table: "Contatos");

            migrationBuilder.DropIndex(
                name: "IX_Contatos_CidadeId",
                table: "Contatos");

            migrationBuilder.AlterColumn<int>(
                name: "CidadeId",
                table: "Contatos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Contatos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EnderecoId",
                table: "Contatos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_EnderecoId",
                table: "Contatos",
                column: "EnderecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_Cidades_EnderecoId",
                table: "Contatos",
                column: "EnderecoId",
                principalTable: "Cidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_Cidades_EnderecoId",
                table: "Contatos");

            migrationBuilder.DropIndex(
                name: "IX_Contatos_EnderecoId",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Contatos");

            migrationBuilder.AlterColumn<int>(
                name: "CidadeId",
                table: "Contatos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
