using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmpresaApp.Data.Migrations
{
    public partial class DataContratacaoNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataContratacao",
                table: "Funcionarios",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataContratacao",
                table: "Funcionarios",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
