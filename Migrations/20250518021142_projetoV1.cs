using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cp2Mottu.Migrations
{
    /// <inheritdoc />
    public partial class projetoV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filiais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Endereco = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Placa = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: false),
                    Modelo = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IdFilial = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Motos_Filiais_IdFilial",
                        column: x => x.IdFilial,
                        principalTable: "Filiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Filiais_Nome",
                table: "Filiais",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Motos_IdFilial",
                table: "Motos",
                column: "IdFilial");

            migrationBuilder.CreateIndex(
                name: "IX_Motos_Placa",
                table: "Motos",
                column: "Placa",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Motos");

            migrationBuilder.DropTable(
                name: "Filiais");
        }
    }
}
