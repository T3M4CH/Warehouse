using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContainersUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Warehouses_WarehousesId",
                table: "Containers");

            migrationBuilder.AlterColumn<int>(
                name: "WarehousesId",
                table: "Containers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "Containers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Warehouses_WarehousesId",
                table: "Containers",
                column: "WarehousesId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Warehouses_WarehousesId",
                table: "Containers");

            migrationBuilder.AlterColumn<int>(
                name: "WarehousesId",
                table: "Containers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "Containers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Warehouses_WarehousesId",
                table: "Containers",
                column: "WarehousesId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
