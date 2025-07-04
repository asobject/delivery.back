﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "CustomPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Coordinates = table.Column<Point>(type: "geography(Point, 4326)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Method = table.Column<int>(type: "integer", nullable: false),
                    CustomPointId = table.Column<int>(type: "integer", nullable: true),
                    CompanyPointId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryPoints_CustomPoints_CustomPointId",
                        column: x => x.CustomPointId,
                        principalTable: "CustomPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tracker = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    OrderStatus = table.Column<int>(type: "integer", nullable: false),
                    CurrentPointId = table.Column<int>(type: "integer", nullable: true),
                    PackageSize = table.Column<int>(type: "integer", nullable: false),
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    ReceiverId = table.Column<string>(type: "text", nullable: true),
                    ReceiverEmail = table.Column<string>(type: "text", nullable: true),
                    SenderDeliveryPointId = table.Column<int>(type: "integer", nullable: false),
                    ReceiverDeliveryPointId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_DeliveryPoints_ReceiverDeliveryPointId",
                        column: x => x.ReceiverDeliveryPointId,
                        principalTable: "DeliveryPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_DeliveryPoints_SenderDeliveryPointId",
                        column: x => x.SenderDeliveryPointId,
                        principalTable: "DeliveryPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomPoints_Coordinates",
                table: "CustomPoints",
                column: "Coordinates")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPoints_CustomPointId",
                table: "DeliveryPoints",
                column: "CustomPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReceiverDeliveryPointId",
                table: "Orders",
                column: "ReceiverDeliveryPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SenderDeliveryPointId",
                table: "Orders",
                column: "SenderDeliveryPointId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "DeliveryPoints");

            migrationBuilder.DropTable(
                name: "CustomPoints");
        }
    }
}
