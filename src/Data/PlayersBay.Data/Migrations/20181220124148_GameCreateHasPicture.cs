namespace PlayersBay.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class GameCreateHasPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Games");
        }
    }
}
