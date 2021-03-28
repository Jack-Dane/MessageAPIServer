using Microsoft.EntityFrameworkCore.Migrations;

namespace MessageAppServer.Migrations
{
    public partial class LiveUsersV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiveUsers",
                columns: table => new
                {
                    LiveUserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    connectionString = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveUsers", x => x.LiveUserId);
                    table.ForeignKey(
                        name: "FK_LiveUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveUsers_UserId",
                table: "LiveUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiveUsers");
        }
    }
}
