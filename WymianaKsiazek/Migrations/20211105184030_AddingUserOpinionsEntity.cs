using Microsoft.EntityFrameworkCore.Migrations;

namespace WymianaKsiazek.Migrations
{
    public partial class AddingUserOpinionsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserOpinions",
                columns: table => new
                {
                    Opinion_Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpinionSumValue = table.Column<long>(type: "bigint", nullable: false),
                    TotalOpinions = table.Column<long>(type: "bigint", nullable: false),
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOpinions", x => x.Opinion_Id);
                    table.ForeignKey(
                        name: "FK_UserOpinions_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOpinions_User_Id",
                table: "UserOpinions",
                column: "User_Id",
                unique: true,
                filter: "[User_Id] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOpinions");
        }
    }
}
