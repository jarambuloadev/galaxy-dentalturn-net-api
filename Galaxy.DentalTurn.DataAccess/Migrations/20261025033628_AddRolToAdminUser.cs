using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxy.DentalTurn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRolToAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rol",
                table: "admin_users",
                type: "text",
                nullable: false,
                defaultValue: "admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rol",
                table: "admin_users");
        }
    }
}
