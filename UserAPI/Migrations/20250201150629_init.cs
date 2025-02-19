using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Passwords",
                columns: table => new
                {
                    PasswordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    PasswordHash = table.Column<string>(type: "nvarchar(150)", maxLength: 30, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(30)", maxLength: 4, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passwords", x => x.PasswordId);
                    table.ForeignKey(
                        name: "FK_Passwords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE() + 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DateOfBirth", "Email", "FirstName", "LastName", "PasswordId", "PhoneNumber", "Role", "SubscriptionId" },
                values: new object[,]
                {
                    { new Guid("695426ac-1a74-4d2f-bae9-f426f693ed63"), new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane.smith@example.com", "Jane", "Smith", new Guid("d52ad829-1605-44e4-9381-605567fdf3e0"), "987-654-3210", "ADMIN", null },
                    { new Guid("6c52c0a6-24e2-4f39-bd69-96049bd9abc3"), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@example.com", "John", "Doe", new Guid("fb2fa649-791f-4022-bad6-edd7bd6259c3"), "123-456-7890", "USER", null }
                });

            migrationBuilder.InsertData(
                table: "Passwords",
                columns: new[] { "PasswordId", "PasswordHash", "PasswordSalt", "UserId" },
                values: new object[,]
                {
                    { new Guid("d52ad829-1605-44e4-9381-605567fdf3e0"), "ixpndSVLRN14+wm2XikLmegNeMYX2TroRYE8iW7iFdY", "UucZLQtt9nI", new Guid("695426ac-1a74-4d2f-bae9-f426f693ed63") },
                    { new Guid("fb2fa649-791f-4022-bad6-edd7bd6259c3"), "knBGOdI98YdYcfWvlRJy8wKzKmCwYtwuv3x1IgscOOg", "xyGJrVaDX6U", new Guid("6c52c0a6-24e2-4f39-bd69-96049bd9abc3") }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "Description", "EndDate", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("2469c5af-92e7-45f7-ac16-f15e14d57531"), "", new DateTime(2026, 2, 1, 16, 6, 27, 88, DateTimeKind.Local).AddTicks(155), "Premium Plan", new Guid("6c52c0a6-24e2-4f39-bd69-96049bd9abc3") },
                    { new Guid("d084f570-fbbb-4c92-8b82-12137cac52c1"), "", new DateTime(2027, 2, 1, 16, 6, 27, 97, DateTimeKind.Local).AddTicks(8123), "Premium Plan", new Guid("695426ac-1a74-4d2f-bae9-f426f693ed63") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_UserId",
                table: "Passwords",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Passwords");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
