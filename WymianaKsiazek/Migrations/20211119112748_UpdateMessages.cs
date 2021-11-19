using Microsoft.EntityFrameworkCore.Migrations;

namespace WymianaKsiazek.Migrations
{
    public partial class UpdateMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Messages");

            migrationBuilder.AddColumn<long>(
                name: "Conv_Id",
                table: "Messages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    User2_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsAllMessagesRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_AspNetUsers_User1_Id",
                        column: x => x.User1_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversations_AspNetUsers_User2_Id",
                        column: x => x.User2_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Conv_Id",
                table: "Messages",
                column: "Conv_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_User1_Id",
                table: "Conversations",
                column: "User1_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_User2_Id",
                table: "Conversations",
                column: "User2_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_Conv_Id",
                table: "Messages",
                column: "Conv_Id",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_Conv_Id",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Messages_Conv_Id",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Conv_Id",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
