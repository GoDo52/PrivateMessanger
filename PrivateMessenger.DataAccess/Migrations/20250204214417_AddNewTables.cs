using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PrivateMessenger.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdministrationRoleId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "UserChats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AdministrationRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrationRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserChatRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChatRoles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AdministrationRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Manager" },
                    { 3, "Administrator" }
                });

            migrationBuilder.InsertData(
                table: "UserChatRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Member" },
                    { 2, "Admin" },
                    { 3, "SuperAdmin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AdministrationRoleId",
                table: "Users",
                column: "AdministrationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChats_RoleId",
                table: "UserChats",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChats_UserChatRoles_RoleId",
                table: "UserChats",
                column: "RoleId",
                principalTable: "UserChatRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AdministrationRoles_AdministrationRoleId",
                table: "Users",
                column: "AdministrationRoleId",
                principalTable: "AdministrationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChats_UserChatRoles_RoleId",
                table: "UserChats");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AdministrationRoles_AdministrationRoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AdministrationRoles");

            migrationBuilder.DropTable(
                name: "UserChatRoles");

            migrationBuilder.DropIndex(
                name: "IX_Users_AdministrationRoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserChats_RoleId",
                table: "UserChats");

            migrationBuilder.DropColumn(
                name: "AdministrationRoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserChats");
        }
    }
}
