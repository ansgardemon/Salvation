using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salvation.Migrations
{
    /// <inheritdoc />
    public partial class ClassificacaoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Classificacao (DescricaoClassificacao) VALUES ('Livre'),('Kids'),('+18');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
