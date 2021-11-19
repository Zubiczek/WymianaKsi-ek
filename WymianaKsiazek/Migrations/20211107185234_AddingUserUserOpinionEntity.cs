using Microsoft.EntityFrameworkCore.Migrations;

namespace WymianaKsiazek.Migrations
{
    public partial class AddingUserUserOpinionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserOpinionInfo",
                columns: table => new
                {
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Opinion_Id = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOpinionInfo", x => new { x.User_Id, x.Opinion_Id });
                    table.ForeignKey(
                        name: "FK_UserOpinionInfo_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOpinionInfo_UserOpinions_Opinion_Id",
                        column: x => x.Opinion_Id,
                        principalTable: "UserOpinions",
                        principalColumn: "Opinion_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOpinionInfo_Opinion_Id",
                table: "UserOpinionInfo",
                column: "Opinion_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOpinionInfo");
        }
    }
}
