namespace PlayersBay.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class HasFeedbackBoolean : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasFeedback",
                table: "Feedbacks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFeedback",
                table: "Feedbacks");
        }
    }
}
