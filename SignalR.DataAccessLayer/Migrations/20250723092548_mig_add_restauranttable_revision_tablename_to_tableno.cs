using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignalR.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_add_restauranttable_revision_tablename_to_tableno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableName",
                table: "RestaurantTables");

            migrationBuilder.AddColumn<int>(
                name: "TableNo",
                table: "RestaurantTables",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableNo",
                table: "RestaurantTables");

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "RestaurantTables",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
