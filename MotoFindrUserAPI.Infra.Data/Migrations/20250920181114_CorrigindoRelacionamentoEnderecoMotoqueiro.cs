using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoFindrUserAPI.Migrations
{
    /// <inheritdoc />
    public partial class CorrigindoRelacionamentoEnderecoMotoqueiro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnderecoId",
                table: "tb_mf_motoqueiro",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Municipio",
                table: "tb_mf_endereco",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "tb_mf_motoqueiro");

            migrationBuilder.DropColumn(
                name: "Municipio",
                table: "tb_mf_endereco");
        }
    }
}
