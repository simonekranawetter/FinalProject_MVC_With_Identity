using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject_MVC_With_Identity.Migrations
{
    public partial class addprofilepic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Profiles");
        }
    }
}
