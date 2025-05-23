using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_Repository_Data_Layer.Migrations
{
    /// <inheritdoc />
    public partial class AddingImagePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductImagePath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImagePath",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "AspNetUsers");
        }
    }
}
