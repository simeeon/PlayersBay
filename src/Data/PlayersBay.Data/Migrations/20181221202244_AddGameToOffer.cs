using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayersBay.Data.Migrations
{
    public partial class AddGameToOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "Offers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "Offers",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
