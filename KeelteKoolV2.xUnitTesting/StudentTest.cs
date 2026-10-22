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

        /// <summary>
        /// Kontrollib, et olemasoleva õpilase andmete pärimine ID järgi tagastab õiged andmed.
        /// </summary>
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

        /// <summary>
        /// Kontrollib, et õpilase andmete uuendamine muudab andmebaasis kirjet korrektselt.
        /// </summary>
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

    }
}