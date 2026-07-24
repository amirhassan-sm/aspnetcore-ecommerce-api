using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastracture.Catalogue.EF.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class fixingProductMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "products");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductMedia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_features_FeatureName",
                table: "features",
                column: "FeatureName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_features_FeatureName",
                table: "features");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductMedia");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "products",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
