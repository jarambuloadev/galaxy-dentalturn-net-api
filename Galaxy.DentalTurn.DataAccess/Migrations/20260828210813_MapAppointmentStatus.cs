using Galaxy.DentalTurn.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxy.DentalTurn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MapAppointmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // No-op: model mapping updated to use AppointmentStatus property mapped to existing "status" enum column.
            // Database already has the correct enum column type; no alteration required.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "appointments",
                type: "boolean",
                nullable: false,
                defaultValueSql: "'PENDING'::\"AppointmentStatus\"",
                oldClrType: typeof(AppointmentStatus),
                oldType: "\"AppointmentStatus\"",
                oldDefaultValueSql: "'PENDING'::\"AppointmentStatus\"");

            migrationBuilder.AddColumn<AppointmentStatus>(
                name: "AppointmentStatus",
                table: "appointments",
                type: "\"AppointmentStatus\"",
                nullable: false,
                defaultValue: AppointmentStatus.PENDING);
        }
    }
}
