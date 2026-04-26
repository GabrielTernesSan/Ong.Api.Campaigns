using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ong.Infra.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCurrentAmountFromCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAmount",
                table: "Campaigns");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentAmount",
                table: "Campaigns",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
