using Microsoft.EntityFrameworkCore.Migrations;

namespace RentACar.Migrations
{
    public partial class nulableUposlenik : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_User_UposlenikID",
                table: "Rezervacija");

            migrationBuilder.AlterColumn<int>(
                name: "UposlenikID",
                table: "Rezervacija",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_User_UposlenikID",
                table: "Rezervacija",
                column: "UposlenikID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_User_UposlenikID",
                table: "Rezervacija");

            migrationBuilder.AlterColumn<int>(
                name: "UposlenikID",
                table: "Rezervacija",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_User_UposlenikID",
                table: "Rezervacija",
                column: "UposlenikID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
