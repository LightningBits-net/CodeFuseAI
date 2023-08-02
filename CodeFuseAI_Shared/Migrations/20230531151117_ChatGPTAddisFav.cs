using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeFuseAI_Shared.Migrations
{
    /// <inheritdoc />
    public partial class ChatGPTAddisFav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFav",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFav",
                table: "Messages");
        }
    }
}
