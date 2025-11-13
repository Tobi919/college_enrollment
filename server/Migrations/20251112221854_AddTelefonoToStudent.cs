using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSqliteApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTelefonoToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroTelefono",
                table: "Students",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroTelefono",
                table: "Students");
        }
    }
}
