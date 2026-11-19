using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KeelteKoolV2.xUnitTesting
{
    public class StudentTests : TestBase
    {
        /// <summary>
        /// Kontrollib, kas uue õpilase lisamine salvestab andmed andmebaasi.
        /// </summary>
        [Fact]
        public async Task Should_Add_Student()
        {
            var dto = new StudentDTO();
            dto.FirstName = "Nikita";
            dto.LastName = "Fortnite";
            dto.Email = "nikita.fortnite@test.ee";

            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            var dbItem = await Context.Students.FindAsync(created.Id);

            Assert.NotNull(created);
            Assert.NotNull(dbItem);
            Assert.Equal("Nikita", dbItem.FirstName);
        }

        /// <summary>
        /// Kontrollib, kas õpilase andmed leitakse ID järgi.
        /// </summary>
        [Fact]
        public async Task Should_Get_Student()
        {
            var dto = new StudentDTO();
            dto.FirstName = "Mark";
            dto.LastName = "madk1d";
            dto.Email = "mark.madk1d@test.ee";

            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            var fetched = await Svc<IStudentsServices>().GetByIdAsync(created.Id);

            Assert.NotNull(fetched);
            Assert.Equal(created.Id, fetched.Id);
            Assert.Equal("Mark", fetched.FirstName);
        }

        /// <summary>
        /// Kontrollib, kas õpilase andmete muutmine salvestub andmebaasi.
        /// </summary>
        [Fact]
        public async Task Should_Update_Student()
        {
            var dto = new StudentDTO();
            dto.FirstName = "Liis";
            dto.LastName = "Ilus";
            dto.Email = "liis@test.ee";

            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            created.FirstName = "Liisbet";

            await Svc<IStudentsServices>().UpdateAsync(created);
            var dbItem = await Context.Students.FindAsync(created.Id);

            Assert.NotNull(dbItem);
            Assert.Equal("Liisbet", dbItem.FirstName);
        }

        /// <summary>
        /// Kontrollib, kas õpilane kustutatakse andmebaasist.
        /// </summary>
        [Fact]
        public async Task Should_Delete_Student()
        {
            var dto = new StudentDTO();
            dto.FirstName = "Kalle";
            dto.LastName = "Kivi";
            dto.Email = "kalle@test.ee";

            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            await Svc<IStudentsServices>().DeleteAsync(created.Id);
            var dbItem = await Context.Students.FindAsync(created.Id);

            Assert.Null(dbItem);
        }

        /// <summary>
        /// Kontrollib, kas õpilaste nimekiri kasvab uue õpilase lisamisel.
        /// </summary>
        [Fact]
        public async Task Should_Get_All_Students()
        {
            var before = await Svc<IStudentsServices>().GetAllAsync();

            var dto = new StudentDTO();
            dto.FirstName = "A";
            dto.LastName = "B";
            dto.Email = "a@b.ee";
            await Svc<IStudentsServices>().CreateAsync(dto);

            var after = await Svc<IStudentsServices>().GetAllAsync();

            Assert.True(after.Count() > before.Count());
        }

        /// <summary>
        /// Negatiivne test: olematu ID-ga õpilast ei leita.
        /// </summary>
        [Fact]
        public async Task Should_Not_Get_Missing_Student()
        {
            var result = await Svc<IStudentsServices>().GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        /// <summary>
        /// Negatiivne test: olematu ID-ga kustutamine ei tagasta objekti.
        /// </summary>
        [Fact]
        public async Task Should_Not_Delete_Missing_Student()
        {
            var result = await Svc<IStudentsServices>().DeleteAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        /// <summary>
        /// Negatiivne test: sama õpilase teine kustutamine ei tagasta objekti.
        /// </summary>
        [Fact]
        public async Task Should_Not_Delete_Same_Student_Twice()
        {
            var dto = new StudentDTO();
            dto.FirstName = "Tiina";
            dto.LastName = "Tamm";
            dto.Email = "tiina@test.ee";

            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            await Svc<IStudentsServices>().DeleteAsync(created.Id);
            var secondDelete = await Svc<IStudentsServices>().DeleteAsync(created.Id);

            Assert.Null(secondDelete);
        }

        /// <summary>
        /// Negatiivne test: kustutatud õpilast ei leita enam ID järgi.
        /// </summary>
        [Fact]
        public async Task Should_Not_Get_Deleted_Student()
        {
            var dto = new StudentDTO();
            dto.FirstName = "Peeter";
            dto.LastName = "Paju";
            dto.Email = "peeter@test.ee";

            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            await Svc<IStudentsServices>().DeleteAsync(created.Id);
            var result = await Svc<IStudentsServices>().GetByIdAsync(created.Id);

            Assert.Null(result);
        }

        /// <summary>
        /// Kontrollib, kas kaks lisatud õpilast saavad erineva ID.
        /// </summary>
        [Fact]
        public async Task Should_Create_Students_With_Different_Id()
        {
            var dto1 = new StudentDTO();
            dto1.FirstName = "Anna";
            dto1.LastName = "Mets";
            dto1.Email = "anna@test.ee";

            var dto2 = new StudentDTO();
            dto2.FirstName = "Jaan";
            dto2.LastName = "Mets";
            dto2.Email = "jaan@test.ee";

            var created1 = await Svc<IStudentsServices>().CreateAsync(dto1);
            var created2 = await Svc<IStudentsServices>().CreateAsync(dto2);

            Assert.NotEqual(created1.Id, created2.Id);
        }
    }
}