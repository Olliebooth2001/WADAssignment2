using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WAD.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gigs",
                columns: table => new
                {
                    GigID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GigTitle = table.Column<string>(type: "varchar(100)", nullable: false),
                    GigDescription = table.Column<string>(type: "text", nullable: true),
                    GigImage = table.Column<string>(type: "varchar(100)", nullable: true),
                    GigPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gigs", x => x.GigID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gigs");
        }
    }
}
