using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastracture.Catalogue.EF.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class MediatypeDbSetAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedia_MediaType_MediaTypeId",
                table: "ProductMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedia_products_ProductId",
                table: "ProductMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductMedia",
                table: "ProductMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaType",
                table: "MediaType");

            migrationBuilder.RenameTable(
                name: "ProductMedia",
                newName: "productMedias");

            migrationBuilder.RenameTable(
                name: "MediaType",
                newName: "mediaTypes");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMedia_ProductId",
                table: "productMedias",
                newName: "IX_productMedias_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMedia_MediaTypeId",
                table: "productMedias",
                newName: "IX_productMedias_MediaTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productMedias",
                table: "productMedias",
                column: "ProductMediaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mediaTypes",
                table: "mediaTypes",
                column: "MediaTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_productMedias_mediaTypes_MediaTypeId",
                table: "productMedias",
                column: "MediaTypeId",
                principalTable: "mediaTypes",
                principalColumn: "MediaTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productMedias_products_ProductId",
                table: "productMedias",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productMedias_mediaTypes_MediaTypeId",
                table: "productMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_productMedias_products_ProductId",
                table: "productMedias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productMedias",
                table: "productMedias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mediaTypes",
                table: "mediaTypes");

            migrationBuilder.RenameTable(
                name: "productMedias",
                newName: "ProductMedia");

            migrationBuilder.RenameTable(
                name: "mediaTypes",
                newName: "MediaType");

            migrationBuilder.RenameIndex(
                name: "IX_productMedias_ProductId",
                table: "ProductMedia",
                newName: "IX_ProductMedia_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_productMedias_MediaTypeId",
                table: "ProductMedia",
                newName: "IX_ProductMedia_MediaTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMedia",
                table: "ProductMedia",
                column: "ProductMediaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaType",
                table: "MediaType",
                column: "MediaTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedia_MediaType_MediaTypeId",
                table: "ProductMedia",
                column: "MediaTypeId",
                principalTable: "MediaType",
                principalColumn: "MediaTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedia_products_ProductId",
                table: "ProductMedia",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
