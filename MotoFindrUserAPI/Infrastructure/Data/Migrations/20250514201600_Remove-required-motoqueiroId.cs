using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoFindrUserAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoverequiredmotoqueiroId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_mf_moto_MotoqueiroId",
                table: "tb_mf_moto");

            migrationBuilder.AlterColumn<int>(
                name: "MotoqueiroId",
                table: "tb_mf_moto",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.CreateIndex(
                name: "IX_tb_mf_moto_MotoqueiroId",
                table: "tb_mf_moto",
                column: "MotoqueiroId",
                unique: true,
                filter: "\"MotoqueiroId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_mf_moto_MotoqueiroId",
                table: "tb_mf_moto");

            migrationBuilder.AlterColumn<int>(
                name: "MotoqueiroId",
                table: "tb_mf_moto",
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
                unique: true);
        }
    }
}
