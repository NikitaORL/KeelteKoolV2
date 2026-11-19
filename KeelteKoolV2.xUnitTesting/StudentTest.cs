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
        /// Kontrollib, kas uue õpilase lisamine õnnestub ja andmed salvestatakse andmebaasi.
        /// </summary>
        [Fact]
        public async Task Should_Add_Student()
        {
            //ülesseade
            var dto = new StudentDTO();
            dto.FirstName = "Nikita";
            dto.LastName = "Fortnite";
            dto.Email = "nikita.fortnite@test.ee";
            dto.CreatedAt = DateTime.Now;

            //tegevus
            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            var dbItem = await Context.Students.FindAsync(created.Id);

            //kontroll
            Assert.NotNull(created);
            Assert.NotEqual(Guid.Empty, created.Id);
            Assert.NotNull(dbItem);
            Assert.Equal("Nikita", dbItem.FirstName);
            Assert.Equal("Fortnite", dbItem.LastName);
            Assert.Equal("nikita.fortnite@test.ee", dbItem.Email);
        }

        /// <summary>
        /// Kontrollib, kas õpilase andmed leitakse ID järgi.
        /// </summary>
        [Fact]
        public async Task Should_Get_Student()
        {
            //ülesseade
            var dto = new StudentDTO();
            dto.FirstName = "Mark";
            dto.LastName = "madk1d";
            dto.Email = "mark.madk1d@test.ee";
            dto.CreatedAt = DateTime.Now;

            var created = await Svc<IStudentsServices>().CreateAsync(dto);

            //tegevus
            var fetched = await Svc<IStudentsServices>().GetByIdAsync(created.Id);

            //kontroll
            Assert.NotNull(created);
            Assert.NotNull(fetched);
            Assert.Equal(created.Id, fetched.Id);
            Assert.Equal("Mark", fetched.FirstName);
            Assert.Equal("madk1d", fetched.LastName);
            Assert.Equal("mark.madk1d@test.ee", fetched.Email);
        }

        /// <summary>
        /// Kontrollib, kas õpilase andmete muutmine salvestub andmebaasi.
        /// </summary>
        [Fact]
        public async Task Should_Update_Student()
        {
            //ülesseade
            var dto = new StudentDTO();
            dto.FirstName = "Liis";
            dto.LastName = "Ilus";
            dto.Email = "liis@test.ee";
            dto.CreatedAt = DateTime.Now;

            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            created.FirstName = "Liisbet";
            created.Email = "liisbet@test.ee";

            //tegevus
            var updated = await Svc<IStudentsServices>().UpdateAsync(created);
            var dbItem = await Context.Students.FindAsync(created.Id);

            //kontroll
            Assert.NotNull(updated);
            Assert.NotNull(dbItem);
            Assert.Equal(created.Id, dbItem.Id);
            Assert.Equal("Liisbet", dbItem.FirstName);
            Assert.Equal("Ilus", dbItem.LastName);
            Assert.Equal("liisbet@test.ee", dbItem.Email);
        }

        /// <summary>
        /// Kontrollib, kas õpilane kustutatakse andmebaasist ja tagastatakse kustutatud objekt.
        /// </summary>
        [Fact]
        public async Task Should_Delete_Student()
        {
            //ülesseade
            var dto = new StudentDTO();
            dto.FirstName = "Kalle";
            dto.LastName = "Kivi";
            dto.Email = "kalle@test.ee";
            dto.CreatedAt = DateTime.Now;

            var created = await Svc<IStudentsServices>().CreateAsync(dto);

            //tegevus
            var deleted = await Svc<IStudentsServices>().DeleteAsync(created.Id);
            var dbItem = await Context.Students.FindAsync(created.Id);

            //kontroll
            Assert.NotNull(created);
            Assert.NotNull(deleted);
            Assert.Equal(created.Id, deleted.Id);
            Assert.Equal("Kalle", deleted.FirstName);
            Assert.Null(dbItem);
        }

        /// <summary>
        /// Kontrollib, kas kõikide õpilaste pärimine ja lisamine töötab.
        /// </summary>
        [Fact]
        public async Task Should_Get_All_Students()
        {
            //ülesseade
            var before = await Svc<IStudentsServices>().GetAllAsync();
            int initialCount = before.Count();

            var first = new StudentDTO();
            first.FirstName = "A";
            first.LastName = "B";
            first.Email = "a@b.ee";

            var second = new StudentDTO();
            second.FirstName = "C";
            second.LastName = "D";
            second.Email = "c@d.ee";

            var createdFirst = await Svc<IStudentsServices>().CreateAsync(first);
            var createdSecond = await Svc<IStudentsServices>().CreateAsync(second);

            //tegevus
            var after = await Svc<IStudentsServices>().GetAllAsync();
            int finalCount = after.Count();

            //kontroll
            Assert.NotNull(before);
            Assert.NotNull(after);
            Assert.NotNull(createdFirst);
            Assert.NotNull(createdSecond);
            Assert.True(finalCount >= initialCount + 2);
        }

        /// <summary>
        /// Negatiivne test: olematu ID-ga õpilast ei leita ja nimekiri ei muutu.
        /// </summary>
        [Fact]
        public async Task Should_Not_Get_Missing_Student()
        {
            //ülesseade
            var nonExistentId = Guid.NewGuid();
            var before = await Svc<IStudentsServices>().GetAllAsync();

            //tegevus
            var result = await Svc<IStudentsServices>().GetByIdAsync(nonExistentId);
            var dbCheck = await Context.Students.FindAsync(nonExistentId);
            var after = await Svc<IStudentsServices>().GetAllAsync();

            //kontroll
            Assert.NotNull(before);
            Assert.Null(result);
            Assert.Null(dbCheck);
            Assert.NotNull(after);
            Assert.Equal(before.Count(), after.Count());
        }

        /// <summary>
        /// Negatiivne test: olematu ID-ga kustutamine ei tagasta objekti ega muuda andmebaasi.
        /// </summary>
        [Fact]
        public async Task Should_Not_Delete_Missing_Student()
        {
            //ülesseade
            var invalidId = Guid.NewGuid();
            var before = await Svc<IStudentsServices>().GetAllAsync();

            //tegevus
            var result = await Svc<IStudentsServices>().DeleteAsync(invalidId);
            var checkItem = await Context.Students.FindAsync(invalidId);
            var after = await Svc<IStudentsServices>().GetAllAsync();

            //kontroll
            Assert.NotNull(before);
            Assert.Null(result);
            Assert.Null(checkItem);
            Assert.NotNull(after);
            Assert.Equal(before.Count(), after.Count());
        }

        /// <summary>
        /// Negatiivne test: sama õpilase teine kustutamine ei tagasta objekti.
        /// </summary>
        [Fact]
        public async Task Should_Not_Delete_Same_Student_Twice()
        {
            //ülesseade
            var dto = new StudentDTO();
            dto.FirstName = "Tiina";
            dto.LastName = "Tamm";
            dto.Email = "tiina@test.ee";
            dto.CreatedAt = DateTime.Now;

            var created = await Svc<IStudentsServices>().CreateAsync(dto);

            //tegevus
            var firstDelete = await Svc<IStudentsServices>().DeleteAsync(created.Id);
            var secondDelete = await Svc<IStudentsServices>().DeleteAsync(created.Id);
            var dbItem = await Context.Students.FindAsync(created.Id);

            //kontroll
            Assert.NotNull(created);
            Assert.NotNull(firstDelete);
            Assert.Equal(created.Id, firstDelete.Id);
            Assert.Null(secondDelete);
            Assert.Null(dbItem);
        }

        /// <summary>
        /// Negatiivne test: null DTO lisamisel viskab teenus ArgumentNullException erindi.
        /// </summary>
        [Fact]
        public async Task Should_Throw_On_Null_Create()
        {
            //ülesseade
            StudentDTO nullDto = null;
            var before = await Svc<IStudentsServices>().GetAllAsync();

            //tegevus
            ArgumentNullException exception = null;
            try
            {
                await Svc<IStudentsServices>().CreateAsync(nullDto);
            }
            catch (ArgumentNullException ex)
            {
                exception = ex;
            }

            var after = await Svc<IStudentsServices>().GetAllAsync();

            //kontroll
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.NotNull(before);
            Assert.NotNull(after);
            Assert.Equal(before.Count(), after.Count());
        }

        /// <summary>
        /// Negatiivne test: null DTO uuendamisel viskab teenus ArgumentNullException erindi.
        /// </summary>
        [Fact]
        public async Task Should_Throw_On_Null_Update()
        {
            //ülesseade
            StudentDTO nullDto = null;
            var before = await Svc<IStudentsServices>().GetAllAsync();

            //tegevus
            ArgumentNullException exception = null;
            try
            {
                await Svc<IStudentsServices>().UpdateAsync(nullDto);
            }
            catch (ArgumentNullException ex)
            {
                exception = ex;
            }

            var after = await Svc<IStudentsServices>().GetAllAsync();

            //kontroll
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
            Assert.NotNull(before);
            Assert.NotNull(after);
            Assert.Equal(before.Count(), after.Count());
        }
    }
}