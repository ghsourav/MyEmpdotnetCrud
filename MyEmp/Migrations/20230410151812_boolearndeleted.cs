using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEmp.Migrations
{
    /// <inheritdoc />
    public partial class boolearndeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_active",
                table: "EmpTech");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_active",
                table: "EmpTech",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
