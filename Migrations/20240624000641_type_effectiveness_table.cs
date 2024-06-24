using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netdexQL.Migrations
{
    /// <inheritdoc />
    public partial class type_effectiveness_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "type_effectivenesses",
                columns: table => new
                {
                    attacking_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    defending_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    multiplier = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_type_effectivenesses", x => new { x.attacking_type_id, x.defending_type_id });
                    table.ForeignKey(
                        name: "fk_type_effectivenesses_types_attacking_type_id",
                        column: x => x.attacking_type_id,
                        principalTable: "types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_type_effectivenesses_types_defending_type_id",
                        column: x => x.defending_type_id,
                        principalTable: "types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_type_effectivenesses_defending_type_id",
                table: "type_effectivenesses",
                column: "defending_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "type_effectivenesses");
        }
    }
}
