using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeFuseAI_Shared.Migrations
{
    /// <inheritdoc />
    public partial class ConversationSystemMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SystemMessage",
                table: "Conversations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemMessage",
                table: "Conversations");
        }
    }
}
