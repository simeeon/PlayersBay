using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayersBay.Data.Migrations
{
    public partial class ChangedDurationNameToExpiryDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Offers",
                newName: "ExpiryDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "Offers",
                newName: "Duration");
        }
    }
}
