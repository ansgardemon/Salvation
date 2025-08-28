using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salvation.Migrations
{
    /// <inheritdoc />
    public partial class GeneroMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Genero (DescricaoGenero) VALUES ('Gore'),('Zumbi'),('Terror Psicológico'),('Found Footage'),('Body Horror');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
