using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salvation.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Usuario (Nome, Email, Senha, DataNascimento, TipoUsuarioId, Ativo) VALUES('uil', 'uil.admin@saphiraterror.com', 'uil123', '1985-01-10', 1,1),('robsu', 'robsu.admin@saphiraterror.com', 'rob123', '1980-05-20', 1,1),('billy', 'billy.admin@saphiraterror.com', 'bil123', '1988-03-14', 1,1),('ariel', 'ariel.gerente@saphiraterror.com', 'ari123', '1990-09-05', 2,1),('lana', 'lana.gerente@saphiraterror.com', 'lan123', '1987-11-23', 2,1),('cylvia', 'cylvia.gerente@saphiraterror.com', 'cyl123', '1992-04-18', 2,1),('administrador', 'admin@saphiraterror.com', 'adm123', '1999-08-22', 1,1),('gerente', 'gerente@saphiraterror.com', 'ger123', '1995-12-30', 3,1),('outros', 'outros@saphiraterror.com', 'out123', '1998-07-11', 3,1);");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
