using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salvation.Migrations
{
    /// <inheritdoc />
    public partial class TipoUsuarioMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            // TipoUsuario
            migrationBuilder.Sql(@"
                INSERT INTO TipoUsuario (DescricaoTipoUsuario) 
                VALUES ('Administrador'),('Gerente'),('Outros');
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
