using System;
using Galaxy.DentalTurn.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxy.DentalTurn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "services",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "services",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "practitioners",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "practitioners",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "practitioner_services",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "practitioner_services",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "practitioner_services",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "practitioner_services",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "practitioner_services",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "patients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "patients",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "business_hours",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "business_hours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "business_hours",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            // Appointment status mapping changed: handled in model mapping; no column type change required here

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "appointments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "admin_users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "admin_users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "services");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "services");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "practitioners");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "practitioners");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "practitioner_services");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "practitioner_services");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "practitioner_services");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "practitioner_services");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "practitioner_services");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "business_hours");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "business_hours");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "business_hours");

            migrationBuilder.DropColumn(
                name: "AppointmentStatus",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "admin_users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "admin_users");

            migrationBuilder.AlterColumn<AppointmentStatus>(
                name: "status",
                table: "appointments",
                type: "\"AppointmentStatus\"",
                nullable: false,
                defaultValueSql: "'PENDING'::\"AppointmentStatus\"",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValueSql: "'PENDING'::\"AppointmentStatus\"");
        }
    }
}
