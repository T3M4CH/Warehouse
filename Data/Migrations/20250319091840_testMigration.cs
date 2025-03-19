using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Data.Migrations
{
    /// <inheritdoc />
    public partial class testMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Warehouses_WarehousesId",
                table: "Containers");

            migrationBuilder.DropIndex(
                name: "IX_Containers_WarehousesId",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "WarehousesId",
                table: "Containers");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_WarehouseId",
                table: "Containers",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Warehouses_WarehouseId",
                table: "Containers",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Warehouses_WarehouseId",
                table: "Containers");

            migrationBuilder.DropIndex(
                name: "IX_Containers_WarehouseId",
                table: "Containers");

            migrationBuilder.AddColumn<int>(
                name: "WarehousesId",
                table: "Containers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Containers_WarehousesId",
                table: "Containers",
                column: "WarehousesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Warehouses_WarehousesId",
                table: "Containers",
                column: "WarehousesId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }
    }
}
