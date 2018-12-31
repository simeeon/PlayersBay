using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayersBay.Data.Migrations
{
    public partial class TransactionPropNameChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_ReceiverId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_SenderId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Transactions",
                newName: "SellerId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Transactions",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SenderId",
                table: "Transactions",
                newName: "IX_Transactions_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ReceiverId",
                table: "Transactions",
                newName: "IX_Transactions_BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_BuyerId",
                table: "Transactions",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_SellerId",
                table: "Transactions",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_BuyerId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_SellerId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Transactions",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Transactions",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SellerId",
                table: "Transactions",
                newName: "IX_Transactions_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_BuyerId",
                table: "Transactions",
                newName: "IX_Transactions_ReceiverId");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_ReceiverId",
                table: "Transactions",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_SenderId",
                table: "Transactions",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
