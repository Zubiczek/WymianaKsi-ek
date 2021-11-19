using Microsoft.EntityFrameworkCore.Migrations;

namespace WymianaKsiazek.Migrations
{
    public partial class AddingUserLikedOffersEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLikedOffers",
                columns: table => new
                {
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Offer_Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikedOffers", x => new { x.User_Id, x.Offer_Id });
                    table.ForeignKey(
                        name: "FK_UserLikedOffers_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikedOffers_Offer_Offer_Id",
                        column: x => x.Offer_Id,
                        principalTable: "Offer",
                        principalColumn: "Offer_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikedOffers_Offer_Id",
                table: "UserLikedOffers",
                column: "Offer_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikedOffers");
        }
    }
}
