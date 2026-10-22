using KeelteKoolV2.ApplicationServices.Services;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KeelteKoolV2.xUnitTesting
{
    public class StudentTests : TestBase
    {
        [Fact]
        public async Task Should_Add_Student_When_Data_Is_Valid()
        {
            // Arrange
            var dto = new StudentDTO
            {
                FirstName = "Nikita",
                LastName = "Fortnite",
                Email = "nikita.fortnite@test.ee",
                CreatedAt = DateTime.Now
            };

            // Act
            var created = await Svc<IStudentsServices>().CreateAsync(dto);

            var context = Svc<KeelteKoolV2Context>();
            var dbItem = await context.Students.FindAsync(created.Id);

            // Assert
            Assert.NotNull(created);
            Assert.NotEqual(Guid.Empty, created.Id);
            Assert.Equal("Nikita", dbItem.FirstName);
            Assert.Equal("Fortnite", dbItem.LastName);
            Assert.Equal("nikita.fortnite@test.ee", dbItem.Email);
        }

        [Fact]
        public async Task Should_Get_Student_By_Id()
        {
            var dto = new StudentDTO
            {
                FirstName = "Mark",
                LastName = "madk1d",
                Email = "mark.madk1d@test.ee",
                CreatedAt = DateTime.Now
            };

            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            var fetched = await Svc<IStudentsServices>().GetByIdAsync(created.Id);

            Assert.NotNull(fetched);
            Assert.Equal(created.Id, fetched.Id);
            Assert.Equal("Mark", fetched.FirstName);
            Assert.Equal("Ots", fetched.LastName);
            Assert.Equal("mark.ots@test.ee", fetched.Email);
        }


        [Fact]
        public async Task Should_Update_Student_Successfully()
        {
            var dto = new StudentDTO
            {
                FirstName = "Liis",
                LastName = "Ilus",
                Email = "liis@test.ee",
                CreatedAt = DateTime.Now
            };

            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            created.FirstName = "Liisbet";
            created.Email = "liisbet@test.ee";

            var updated = await Svc<IStudentsServices>().UpdateAsync(created);
            var dbItem = await Context.Students.FindAsync(created.Id);

            Assert.NotNull(updated);
            Assert.Equal("Liisbet", dbItem.FirstName);
            Assert.Equal("Ilus", dbItem.LastName);
            Assert.Equal("liisbet@test.ee", dbItem.Email);
            Assert.NotEqual("liis@test.ee", dbItem.Email);
        }

        [Fact]
        public async Task Should_Delete_Student_Successfully()
        {
            var dto = new StudentDTO
            {
                FirstName = "Kalle",
                LastName = "Kivi",
                Email = "kalle@test.ee",
                CreatedAt = DateTime.Now
            };

            var created = await Svc<IStudentsServices>().CreateAsync(dto);
            var deleted = await Svc<IStudentsServices>().DeleteAsync(created.Id);
            var dbItem = await Context.Students.FindAsync(created.Id);

            Assert.NotNull(deleted);
            Assert.Equal(created.Id, deleted.Id);
            Assert.Null(dbItem);
            Assert.NotEqual(Guid.Empty, deleted.Id);
            Assert.Equal("Kalle", deleted.FirstName);
        }

        [Fact]
        public async Task Should_Return_All_Students()
        {
            var allBefore = await Svc<IStudentsServices>().GetAllAsync();
            int initialCount = allBefore.Count();

            await Svc<IStudentsServices>().CreateAsync(new StudentDTO { FirstName = "A", LastName = "B", Email = "a@b.ee" });
            await Svc<IStudentsServices>().CreateAsync(new StudentDTO { FirstName = "C", LastName = "D", Email = "c@d.ee" });

            var allAfter = await Svc<IStudentsServices>().GetAllAsync();

            Assert.NotNull(allAfter);
            Assert.True(allAfter.Count() >= initialCount + 2);
            Assert.Contains(allAfter, s => s.Email == "a@b.ee");
            Assert.Contains(allAfter, s => s.Email == "c@d.ee");
            Assert.NotEqual(initialCount, allAfter.Count());
        }

        //Negatiivsed

        [Fact]
        public async Task Should_Not_Get_Student_When_Id_Does_Not_Exist()
        {
            var nonExistentId = Guid.NewGuid();
            var result = await Svc<IStudentsServices>().GetByIdAsync(nonExistentId);
            var allStudents = await Svc<IStudentsServices>().GetAllAsync();

            Assert.Null(result);
            Assert.NotEqual(Guid.Empty, nonExistentId);
            Assert.NotNull(allStudents);
            Assert.DoesNotContain(allStudents, s => s.Id == nonExistentId);
        }

        [Fact]
        public async Task Should_Not_Delete_Student_When_Id_Is_Invalid()
        {
            var invalidId = Guid.NewGuid();
            var countBefore = (await Svc<IStudentsServices>().GetAllAsync()).Count();

            var result = await Svc<IStudentsServices>().DeleteAsync(invalidId);

            var countAfter = (await Svc<IStudentsServices>().GetAllAsync()).Count();
            var checkItem = await Context.Students.FindAsync(invalidId);

            Assert.Null(result);
            Assert.Equal(countBefore, countAfter);
            Assert.NotEqual(Guid.Empty, invalidId);
            Assert.Null(checkItem);
        }


    }
}