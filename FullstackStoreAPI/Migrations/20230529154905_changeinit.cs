using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullstackStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Polishers", "https://nailsstorage.blob.core.windows.net/fullstack-store/bottle2.png", "Orange Polish" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Polishers", "https://nailsstorage.blob.core.windows.net/fullstack-store/bottle1.png", "Yellow Polish" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Polishers", "https://nailsstorage.blob.core.windows.net/fullstack-store/bottle3.png", "Blue Polish" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Polishers", "https://nailsstorage.blob.core.windows.net/fullstack-store/bottle4.png", "Pink Polish" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Image", "Name" },
                values: new object[] { "Equipment", "Bundle of most common manicure tools", "https://nailsstorage.blob.core.windows.net/fullstack-store/bundle1.png", "Bundle" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Cosmetics", "https://nailsstorage.blob.core.windows.net/fullstack-store/cosmetic2.png", "Nail Acetone" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Image", "Name", "SpecialTag" },
                values: new object[] { "Files", "https://nailsstorage.blob.core.windows.net/fullstack-store/File1.png", "Basic File", "Most popular" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Files", "https://nailsstorage.blob.core.windows.net/fullstack-store/SquareFile.png", "Square File" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "Image", "Name", "SpecialTag" },
                values: new object[] { "Cosmetics", "https://nailsstorage.blob.core.windows.net/fullstack-store/serum1.png", "Nail Serum", "Top Rated" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Equipment", "https://nailsstorage.blob.core.windows.net/fullstack-store/lamp.png", "UV Lamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Appetizer", "https://res.cloudinary.com/hyxooazrt/image/upload/v1679309366/spring_roll.jpg", "Spring Roll" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Appetizer", "https://res.cloudinary.com/hyxooazrt/image/upload/v1679309365/idli_tqmdii.jpg", "Idli" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Appetizer", "https://res.cloudinary.com/hyxooazrt/image/upload/v1679309366/pani_puri.jpg", "Panu Puri" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Entrée", "https://res.cloudinary.com/hyxooazrt/image/upload/v1679309365/hakka_noodles_i4gedv.jpg", "Hakka Noodles" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Image", "Name" },
                values: new object[] { "Entrée", "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.", "https://res.cloudinary.com/hyxooazrt/image/upload/v1679309365/malai_kofta_qa9xpi.jpg", "Malai Kofta" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Entrée", "https://res.cloudinary.com/hyxooazrt/image/upload/v1679309365/paneer_pizza_dc2e8a.jpg", "Paneer Pizza" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Image", "Name", "SpecialTag" },
                values: new object[] { "Entrée", "https://res.cloudinary.com/hyxooazrt/image/upload/v1679309365/paneer_tikka_vvijie.jpg", "Paneer Tikka", "Chef's Special" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Dessert", "https://res.cloudinary.com/hyxooazrt/image/upload/v1679309365/carrot_love_hkwgri.jpg", "Carrot Love" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "Image", "Name", "SpecialTag" },
                values: new object[] { "Dessert", "https://res.cloudinary.com/hyxooazrt/image/upload/v1679309366/rasmalai.jpg", "Rasmalai", "Chef's Special" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "Image", "Name" },
                values: new object[] { "Dessert", "https://res.cloudinary.com/hyxooazrt/image/upload/v1679309365/sweet_rolls_hteqpd.jpg", "Sweet Rolls" });
        }
    }
}
