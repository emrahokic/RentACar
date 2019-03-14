using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RentACar.Migrations
{
    public partial class opcinaizbacena : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grad_Opcina_OpcinaID",
                table: "Grad");

            migrationBuilder.DropTable(
                name: "Opcina");

            migrationBuilder.RenameColumn(
                name: "OpcinaID",
                table: "Grad",
                newName: "RegijaID");

            migrationBuilder.RenameIndex(
                name: "IX_Grad_OpcinaID",
                table: "Grad",
                newName: "IX_Grad_RegijaID");

            migrationBuilder.AddColumn<int>(
                name: "PostanskiBroj",
                table: "Grad",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Grad_Regija_RegijaID",
                table: "Grad",
                column: "RegijaID",
                principalTable: "Regija",
                principalColumn: "RegijaID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grad_Regija_RegijaID",
                table: "Grad");

            migrationBuilder.DropColumn(
                name: "PostanskiBroj",
                table: "Grad");

            migrationBuilder.RenameColumn(
                name: "RegijaID",
                table: "Grad",
                newName: "OpcinaID");

            migrationBuilder.RenameIndex(
                name: "IX_Grad_RegijaID",
                table: "Grad",
                newName: "IX_Grad_OpcinaID");

            migrationBuilder.CreateTable(
                name: "Opcina",
                columns: table => new
                {
                    OpcinaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    PostanskiBroj = table.Column<int>(nullable: false),
                    RegijaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opcina", x => x.OpcinaID);
                    table.ForeignKey(
                        name: "FK_Opcina_Regija_RegijaID",
                        column: x => x.RegijaID,
                        principalTable: "Regija",
                        principalColumn: "RegijaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Opcina_RegijaID",
                table: "Opcina",
                column: "RegijaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Grad_Opcina_OpcinaID",
                table: "Grad",
                column: "OpcinaID",
                principalTable: "Opcina",
                principalColumn: "OpcinaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
