using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetShelter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreLookupData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Breeds",
                columns: new[] { "Id", "Name", "SpeciesId" },
                values: new object[,]
                {
                    { 12, "Български овчарски пес", 1 },
                    { 13, "Йоркширски териер", 1 },
                    { 14, "Чихуахуа", 1 },
                    { 15, "Бордър коли", 1 },
                    { 16, "Американски стафордширски териер", 1 },
                    { 17, "Френски булдог", 1 },
                    { 18, "Сиамска", 2 },
                    { 19, "Мейн кун", 2 },
                    { 20, "Шотландска клепоуха", 2 },
                    { 21, "Руска синя", 2 }
                });

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Спокоен и търпелив в присъствието на деца.");

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Може да живее с друго куче в дома.");

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Не гони и не напада котки.");

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "Предпочита тишината и почивката.");

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Обича игри и играчки.");

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Нуждае се от много движение всеки ден.");

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Радва се на дълги разходки навън.");

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Върви спокойно на каишка.");

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Чувства се добре и в по-малко пространство.");

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Има нужда от последователно възпитание.");

            migrationBuilder.InsertData(
                table: "PersonalityTraits",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 11, "Ходи до тоалетна навън или в котешка тоалетна.", "Приучен на тоалетна" },
                    { 12, "Търси ласка и близост с хората.", "Гальовен" },
                    { 13, "Нужно му е време да свикне с нови хора.", "Плах" },
                    { 14, "Спокойно изчаква, докато стопаните са на работа.", "Може да остава сам" },
                    { 15, "Има здравословен проблем, който изисква внимание.", "Нуждае се от специални грижи" }
                });

            migrationBuilder.InsertData(
                table: "Species",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Заек" },
                    { 4, "Птица" }
                });

            migrationBuilder.InsertData(
                table: "Breeds",
                columns: new[] { "Id", "Name", "SpeciesId" },
                values: new object[,]
                {
                    { 22, "Смесена порода", 3 },
                    { 23, "Холандско джудже", 3 },
                    { 24, "Клепоух заек", 3 },
                    { 25, "Вълнисто папагалче", 4 },
                    { 26, "Корела", 4 },
                    { 27, "Канарче", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Species",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Species",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: null);
        }
    }
}
