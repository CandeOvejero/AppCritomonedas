using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcialfinalprogApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransaccionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Monedas_MonedaId",
                table: "Transacciones");

            migrationBuilder.RenameColumn(
                name: "Monto",
                table: "Transacciones",
                newName: "Money");

            migrationBuilder.RenameColumn(
                name: "FechaHora",
                table: "Transacciones",
                newName: "DateTime");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "Transacciones",
                newName: "CryptoCode");

            migrationBuilder.AlterColumn<int>(
                name: "MonedaId",
                table: "Transacciones",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Transacciones",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CryptoAmount",
                table: "Transacciones",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Monedas_MonedaId",
                table: "Transacciones",
                column: "MonedaId",
                principalTable: "Monedas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Monedas_MonedaId",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "CryptoAmount",
                table: "Transacciones");

            migrationBuilder.RenameColumn(
                name: "Money",
                table: "Transacciones",
                newName: "Monto");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Transacciones",
                newName: "FechaHora");

            migrationBuilder.RenameColumn(
                name: "CryptoCode",
                table: "Transacciones",
                newName: "Cantidad");

            migrationBuilder.AlterColumn<int>(
                name: "MonedaId",
                table: "Transacciones",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Monedas_MonedaId",
                table: "Transacciones",
                column: "MonedaId",
                principalTable: "Monedas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
