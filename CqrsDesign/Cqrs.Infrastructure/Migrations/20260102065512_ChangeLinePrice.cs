using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cqrs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLinePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "order_lines",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "order_lines");
        }
    }
}
