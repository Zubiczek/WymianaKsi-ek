using Microsoft.EntityFrameworkCore.Migrations;

namespace WymianaKsiazek.Migrations
{
    public partial class AddForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Category_Category_Id1",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_BookComments_AspNetUsers_UserId",
                table: "BookComments");

            migrationBuilder.DropForeignKey(
                name: "FK_BookComments_Book_Book_Id1",
                table: "BookComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Address_Address_Id1",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_AspNetUsers_UserId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Book_Book_Id1",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferComments_AspNetUsers_UserId",
                table: "OfferComments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferComments_Offer_Offer_Id1",
                table: "OfferComments");

            migrationBuilder.DropIndex(
                name: "IX_OfferComments_Offer_Id1",
                table: "OfferComments");

            migrationBuilder.DropIndex(
                name: "IX_OfferComments_UserId",
                table: "OfferComments");

            migrationBuilder.DropIndex(
                name: "IX_Offer_Address_Id1",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_Book_Id1",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_UserId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_BookComments_Book_Id1",
                table: "BookComments");

            migrationBuilder.DropIndex(
                name: "IX_BookComments_UserId",
                table: "BookComments");

            migrationBuilder.DropIndex(
                name: "IX_Book_Category_Id1",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Offer_Id1",
                table: "OfferComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OfferComments");

            migrationBuilder.DropColumn(
                name: "Address_Id1",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "Book_Id1",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "Book_Id1",
                table: "BookComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookComments");

            migrationBuilder.DropColumn(
                name: "Category_Id1",
                table: "Book");

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "OfferComments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "Offer",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "BookComments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfferComments_Offer_Id",
                table: "OfferComments",
                column: "Offer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OfferComments_User_Id",
                table: "OfferComments",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_Address_Id",
                table: "Offer",
                column: "Address_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_Book_Id",
                table: "Offer",
                column: "Book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_User_Id",
                table: "Offer",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookComments_Book_Id",
                table: "BookComments",
                column: "Book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookComments_User_Id",
                table: "BookComments",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Book_Category_Id",
                table: "Book",
                column: "Category_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Category_Category_Id",
                table: "Book",
                column: "Category_Id",
                principalTable: "Category",
                principalColumn: "Category_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookComments_AspNetUsers_User_Id",
                table: "BookComments",
                column: "User_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookComments_Book_Book_Id",
                table: "BookComments",
                column: "Book_Id",
                principalTable: "Book",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Address_Address_Id",
                table: "Offer",
                column: "Address_Id",
                principalTable: "Address",
                principalColumn: "Address_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_AspNetUsers_User_Id",
                table: "Offer",
                column: "User_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Book_Book_Id",
                table: "Offer",
                column: "Book_Id",
                principalTable: "Book",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComments_AspNetUsers_User_Id",
                table: "OfferComments",
                column: "User_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComments_Offer_Offer_Id",
                table: "OfferComments",
                column: "Offer_Id",
                principalTable: "Offer",
                principalColumn: "Offer_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Category_Category_Id",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_BookComments_AspNetUsers_User_Id",
                table: "BookComments");

            migrationBuilder.DropForeignKey(
                name: "FK_BookComments_Book_Book_Id",
                table: "BookComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Address_Address_Id",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_AspNetUsers_User_Id",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Book_Book_Id",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferComments_AspNetUsers_User_Id",
                table: "OfferComments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferComments_Offer_Offer_Id",
                table: "OfferComments");

            migrationBuilder.DropIndex(
                name: "IX_OfferComments_Offer_Id",
                table: "OfferComments");

            migrationBuilder.DropIndex(
                name: "IX_OfferComments_User_Id",
                table: "OfferComments");

            migrationBuilder.DropIndex(
                name: "IX_Offer_Address_Id",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_Book_Id",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_User_Id",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_BookComments_Book_Id",
                table: "BookComments");

            migrationBuilder.DropIndex(
                name: "IX_BookComments_User_Id",
                table: "BookComments");

            migrationBuilder.DropIndex(
                name: "IX_Book_Category_Id",
                table: "Book");

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "OfferComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Offer_Id1",
                table: "OfferComments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OfferComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "Offer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Address_Id1",
                table: "Offer",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Book_Id1",
                table: "Offer",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Offer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "BookComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Book_Id1",
                table: "BookComments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Category_Id1",
                table: "Book",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfferComments_Offer_Id1",
                table: "OfferComments",
                column: "Offer_Id1");

            migrationBuilder.CreateIndex(
                name: "IX_OfferComments_UserId",
                table: "OfferComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_Address_Id1",
                table: "Offer",
                column: "Address_Id1");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_Book_Id1",
                table: "Offer",
                column: "Book_Id1");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_UserId",
                table: "Offer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookComments_Book_Id1",
                table: "BookComments",
                column: "Book_Id1");

            migrationBuilder.CreateIndex(
                name: "IX_BookComments_UserId",
                table: "BookComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_Category_Id1",
                table: "Book",
                column: "Category_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Category_Category_Id1",
                table: "Book",
                column: "Category_Id1",
                principalTable: "Category",
                principalColumn: "Category_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookComments_AspNetUsers_UserId",
                table: "BookComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookComments_Book_Book_Id1",
                table: "BookComments",
                column: "Book_Id1",
                principalTable: "Book",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Address_Address_Id1",
                table: "Offer",
                column: "Address_Id1",
                principalTable: "Address",
                principalColumn: "Address_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_AspNetUsers_UserId",
                table: "Offer",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Book_Book_Id1",
                table: "Offer",
                column: "Book_Id1",
                principalTable: "Book",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComments_AspNetUsers_UserId",
                table: "OfferComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComments_Offer_Offer_Id1",
                table: "OfferComments",
                column: "Offer_Id1",
                principalTable: "Offer",
                principalColumn: "Offer_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
