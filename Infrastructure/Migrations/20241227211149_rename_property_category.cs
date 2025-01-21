using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rename_property_category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_SubCategoryId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "Categories",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_SubCategoryId",
                table: "Categories",
                newName: "IX_Categories_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentId",
                table: "Categories",
                column: "ParentId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Categories",
                newName: "SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                newName: "IX_Categories_SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_SubCategoryId",
                table: "Categories",
                column: "SubCategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
