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
    }
}