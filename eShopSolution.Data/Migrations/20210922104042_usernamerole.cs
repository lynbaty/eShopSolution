using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class usernamerole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserNamee",
                table: "AppRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "058d358d-165d-4751-84c2-3e4f58bb2cb0");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9d845e3c-9f06-48d6-b052-b5ed96e18f04", "AQAAAAEAACcQAAAAEFlWnyhvD3tWDP+DWuC+8blLoyPVIT2QICYbIfCcDQZkTv0D4pyl7aAodnfoGjkwug==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 9, 22, 17, 40, 41, 141, DateTimeKind.Local).AddTicks(3705));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNamee",
                table: "AppRoles");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "2991ea88-fdc5-4201-b57a-445be577505d");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d6653fd5-b042-45fe-962d-02cf3d4e8c02", "AQAAAAEAACcQAAAAEL2c3Ct9V8i4KuTFxexeahJX9Y7EUErCABfLdHz9AAUcrGvJJegVm5Hu7yezvUGyIg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 8, 25, 18, 19, 25, 838, DateTimeKind.Local).AddTicks(5161));
        }
    }
}
