using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class catalogeproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "205c11b9-76a2-4fdd-8010-89ed797610d1");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "164e6a89-c178-4419-8410-8548075c1cf5", "AQAAAAEAACcQAAAAEFukw/Vm42XbpICD9Huh80jKJvwC0Kt7NHbw7IsC123iMEqJDEC0E4z50F9c1wQfGw==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 8, 25, 2, 47, 11, 727, DateTimeKind.Local).AddTicks(8970));
        }
    }
}
