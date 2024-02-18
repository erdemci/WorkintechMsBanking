using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MsBanking.Core.Account.Migrations
{
    /// <inheritdoc />
    public partial class AddRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccountTransaction_AccountId",
                table: "AccountTransaction",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTransaction_Account_AccountId",
                table: "AccountTransaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountTransaction_Account_AccountId",
                table: "AccountTransaction");

            migrationBuilder.DropIndex(
                name: "IX_AccountTransaction_AccountId",
                table: "AccountTransaction");
        }
    }
}
