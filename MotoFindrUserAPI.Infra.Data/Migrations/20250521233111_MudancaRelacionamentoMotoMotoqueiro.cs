using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoFindrUserAPI.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class MudancaRelacionamentoMotoMotoqueiro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_mf_moto_tb_mf_motoqueiro_MotoqueiroId",
                table: "tb_mf_moto");

            migrationBuilder.DropIndex(
                name: "IX_tb_mf_moto_MotoqueiroId",
                table: "tb_mf_moto");

            migrationBuilder.AlterColumn<int>(
                name: "MotoId",
                table: "tb_mf_motoqueiro",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.CreateIndex(
                name: "IX_tb_mf_motoqueiro_MotoId",
                table: "tb_mf_motoqueiro",
                column: "MotoId",
                unique: true,
                filter: "\"MotoId\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_mf_motoqueiro_tb_mf_moto_MotoId",
                table: "tb_mf_motoqueiro",
                column: "MotoId",
                principalTable: "tb_mf_moto",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_mf_motoqueiro_tb_mf_moto_MotoId",
                table: "tb_mf_motoqueiro");

            migrationBuilder.DropIndex(
                name: "IX_tb_mf_motoqueiro_MotoId",
                table: "tb_mf_motoqueiro");

            migrationBuilder.AlterColumn<int>(
                name: "MotoId",
                table: "tb_mf_motoqueiro",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_mf_moto_MotoqueiroId",
                table: "tb_mf_moto",
                column: "MotoqueiroId",
                unique: true,
                filter: "\"MotoqueiroId\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_mf_moto_tb_mf_motoqueiro_MotoqueiroId",
                table: "tb_mf_moto",
                column: "MotoqueiroId",
                principalTable: "tb_mf_motoqueiro",
                principalColumn: "Id");
        }
    }
}
