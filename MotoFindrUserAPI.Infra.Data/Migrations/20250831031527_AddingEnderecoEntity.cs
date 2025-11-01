using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoFindrUserAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingEnderecoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "tb_mf_motoqueiro");

            migrationBuilder.CreateTable(
                name: "tb_mf_endereco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Logradouro = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Complemento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Uf = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Numero = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Cep = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: true),
                    MotoqueiroId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_mf_endereco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_mf_endereco_tb_mf_motoqueiro_MotoqueiroId",
                        column: x => x.MotoqueiroId,
                        principalTable: "tb_mf_motoqueiro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_mf_endereco_MotoqueiroId",
                table: "tb_mf_endereco",
                column: "MotoqueiroId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_mf_endereco");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "tb_mf_motoqueiro",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }
    }
}
