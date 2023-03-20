using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class Update_Identity_Fix_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "62b7cfd9-d0b8-4300-baf9-0540a0f817c7",
                column: "ConcurrencyStamp",
                value: "cbcfa51b-3c5b-4c0d-9b4d-a1374bad0ff7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d25993c3-bbbc-440f-b7f0-6811e83a9f48",
                column: "ConcurrencyStamp",
                value: "a3de0252-8d5c-481a-ad38-f03ad16b310a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6f5768da-b384-4a79-abab-bc82cec127d9",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c5ccc3b1-63f4-4536-8bea-443bd765abbf", "SOMEMODERATOR", "AQAAAAEAACcQAAAAEPk26Ba5A0a70Xd7WWKg9tZYcuBKXnCipCqo42NaqB1Ysj7/t9hJZsuryiGZ1bxTRA==", "20938145-f4ab-4b53-a076-514805f42c75" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "82584756-126d-4382-b8a2-e8b439b630ad",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "829662bb-2dde-4708-b733-e186484f7917", "SOMEUSER", "AQAAAAEAACcQAAAAENon36aOE0pzAYhnMl030NHnkH9Ov8lJ9lVlLRKVzIgfMoh70Ap8IAiITcAkgRzu9Q==", "30a0de87-3386-4f8c-8c1c-6f9efdb6f23c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "62b7cfd9-d0b8-4300-baf9-0540a0f817c7",
                column: "ConcurrencyStamp",
                value: "d39fc156-ee80-41f9-8fac-d9928ccaaa1a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d25993c3-bbbc-440f-b7f0-6811e83a9f48",
                column: "ConcurrencyStamp",
                value: "f72d4367-17c8-44e9-829d-34321e68322b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6f5768da-b384-4a79-abab-bc82cec127d9",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "79b7be73-b416-4a8c-9f77-1046ce97bb3d", null, "AQAAAAEAACcQAAAAECegAVcdcH1xogEVqNsfn5ogbuERatSuHQQCttIta6lUZvrFzFgTVWUqRd4ldXGBnQ==", "240b7bf1-2c28-47b0-b974-ea2ca7776a84" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "82584756-126d-4382-b8a2-e8b439b630ad",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02e578c5-44cf-492b-ae5a-6a1bf2e18110", null, "AQAAAAEAACcQAAAAEDUtWvmq1ly4hjVEZBM4JYuHfvWgb6PqBvoYQMUqTzSUn4ee90FfNVtFCrfeG/DBTQ==", "bb32170a-1557-443a-87e7-b8893ed7f47e" });
        }
    }
}
