using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netdexQL.Migrations
{
    /// <inheritdoc />
    public partial class reinitialize_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pokemon",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    base_experience = table.Column<int>(type: "integer", nullable: true),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pokemon", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pokemon_on_types",
                columns: table => new
                {
                    pokemon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    mon_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pokemon_on_types", x => new { x.mon_type_id, x.pokemon_id });
                    table.ForeignKey(
                        name: "fk_pokemon_on_types_pokemon_pokemon_id",
                        column: x => x.pokemon_id,
                        principalTable: "pokemon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pokemon_on_types_types_mon_type_id",
                        column: x => x.mon_type_id,
                        principalTable: "types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pokemon_name",
                table: "pokemon",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_pokemon_on_types_pokemon_id",
                table: "pokemon_on_types",
                column: "pokemon_id");

            migrationBuilder.CreateIndex(
                name: "ix_types_name",
                table: "types",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pokemon_on_types");

            migrationBuilder.DropTable(
                name: "pokemon");

            migrationBuilder.DropTable(
                name: "types");
        }
    }
}
