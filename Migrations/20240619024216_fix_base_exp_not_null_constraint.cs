using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netdexQL.Migrations
{
    /// <inheritdoc />
    public partial class fix_base_exp_not_null_constraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BaseExperience",
                table: "Pokemon",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BaseExperience",
                table: "Pokemon",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
