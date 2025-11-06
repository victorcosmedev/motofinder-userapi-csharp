using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoFindrUserAPI.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class precificacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_mf_precificacao_moto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MotoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Preco = table.Column<double>(type: "BINARY_DOUBLE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_mf_precificacao_moto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_mf_precificacao_moto_tb_mf_moto_MotoId",
                        column: x => x.MotoId,
                        principalTable: "tb_mf_moto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_mf_precificacao_moto_MotoId",
                table: "tb_mf_precificacao_moto",
                column: "MotoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_mf_precificacao_moto");
        }
    }
}
