using System;
using Galaxy.DentalTurn.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxy.DentalTurn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_Recreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ensure enum type not present (avoid error if partially applied previously)
            // drop both possible qualified names
            migrationBuilder.Sql("DROP TYPE IF EXISTS \"AppointmentStatus\" CASCADE;");
            migrationBuilder.Sql("DROP TYPE IF EXISTS \"AppointmentStatus\".\"AppointmentStatus\" CASCADE;");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:AppointmentStatus", "CANCELLED,COMPLETED,CONFIRMED,PENDING")
                .Annotation("Npgsql:Enum:AppointmentStatus.AppointmentStatus", "PENDING,CONFIRMED,CANCELLED,COMPLETED");

            migrationBuilder.CreateTable(
                name: "admin_users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    passwordHash = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "business_hours",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    dayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    isOpen = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    openTime = table.Column<string>(type: "text", nullable: false),
                    closeTime = table.Column<string>(type: "text", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_business_hours", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "practitioners",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    createdAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_practitioners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    durationMin = table.Column<int>(type: "integer", nullable: false, defaultValue: 30),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    createdAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    patientId = table.Column<int>(type: "integer", nullable: false),
                    serviceId = table.Column<int>(type: "integer", nullable: false),
                    practitionerId = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    startTime = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false),
                    endTime = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false),
                    status = table.Column<AppointmentStatus>(type: "\"AppointmentStatus\"", nullable: false, defaultValueSql: "'PENDING'::\"AppointmentStatus\""),
                    notes = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.id);
                    table.ForeignKey(
                        name: "FK_appointments_patients_patientId",
                        column: x => x.patientId,
                        principalTable: "patients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_practitioners_practitionerId",
                        column: x => x.practitionerId,
                        principalTable: "practitioners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "practitioner_services",
                columns: table => new
                {
                    practitionerId = table.Column<int>(type: "integer", nullable: false),
                    serviceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_practitioner_services", x => new { x.practitionerId, x.serviceId });
                    table.ForeignKey(
                        name: "FK_practitioner_services_practitioners_practitionerId",
                        column: x => x.practitionerId,
                        principalTable: "practitioners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_practitioner_services_services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_admin_users_email",
                table: "admin_users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointments_date_status",
                table: "appointments",
                columns: new[] { "date", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_patientId",
                table: "appointments",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_practitionerId_startTime",
                table: "appointments",
                columns: new[] { "practitionerId", "startTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointments_serviceId",
                table: "appointments",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_business_hours_dayOfWeek",
                table: "business_hours",
                column: "dayOfWeek",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_patients_email",
                table: "patients",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_practitioner_services_serviceId",
                table: "practitioner_services",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_services_name",
                table: "services",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_users");

            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "business_hours");

            migrationBuilder.DropTable(
                name: "practitioner_services");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "practitioners");

            migrationBuilder.DropTable(
                name: "services");
        }
    }
}
