using Microsoft.EntityFrameworkCore.Migrations;

namespace RentACar.Migrations
{
    public partial class nest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PoslovnicaID",
                table: "Rezervacija",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_PoslovnicaID",
                table: "Rezervacija",
                column: "PoslovnicaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Poslovnica_PoslovnicaID",
                table: "Rezervacija",
                column: "PoslovnicaID",
                principalTable: "Poslovnica",
                principalColumn: "PoslovnicaID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Poslovnica_PoslovnicaID",
                table: "Rezervacija");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacija_PoslovnicaID",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "PoslovnicaID",
                table: "Rezervacija");
        }
    }
}
