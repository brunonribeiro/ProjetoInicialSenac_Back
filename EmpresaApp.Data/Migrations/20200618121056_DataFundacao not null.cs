using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmpresaApp.Data.Migrations
{
    public partial class DataFundacaonotnull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFundacao",
                table: "Empresas",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFundacao",
                table: "Empresas",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
