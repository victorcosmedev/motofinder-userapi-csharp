using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoFindrUserAPI.Migrations
{
    /// <inheritdoc />
    public partial class adicionandoforeignmotoid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MotoId",
                table: "tb_mf_motoqueiro",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotoId",
                table: "tb_mf_motoqueiro");
        }
    }
}
