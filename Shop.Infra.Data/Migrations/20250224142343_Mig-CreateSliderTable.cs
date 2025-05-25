﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigCreateSliderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SliderImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderPrice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderHref = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SliderTextButton = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sliders");
        }
    }
}
