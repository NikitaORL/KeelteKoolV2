using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using System;
using System.Threading.Tasks;
using Xunit;

namespace KeelteKoolV2.xUnitTesting
{
    public class LecturersServicesTests : TestBase
    {
        [Fact]
        public async Task Should_AddNewLecturer_WhenResultISReturned()
        {
            //ülesseade
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";
            dto.Qualifications = "Testicles";

            //tegevus
            var result = await Svc<ILecturersServices>().Create(dto);

            //kontroll (5 asserti)
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Test", result.LastName);
            Assert.Equal("Testicles", result.Qualifications);
        }

        [Fact]
        public async Task Should_GetLecturerDetails_WhenGuidISNotNull()
        {
            //ülesseade
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";
            dto.Qualifications = "Testicles";
            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            //tegevus
            var result = await Svc<ILecturersServices>().DetailAsync(createdLecturer.Id);

            //kontroll (5 asserti)
            Assert.NotNull(result);
            Assert.Equal(createdLecturer.Id, result.Id);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Test", result.LastName);
            Assert.Equal("Testicles", result.Qualifications);
        }

        [Fact]
        public async Task Should_UpdateLecturerWithNewData_WhenDataIsDifferentFromDB()
        {
            //ülesseade
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";
            dto.Qualifications = "Testicles";
            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            LecturerDTO updatedinfo = new LecturerDTO();
            updatedinfo.FirstName = "TestUusinfo";
            updatedinfo.LastName = "Test2222222222";
            updatedinfo.Qualifications = "Maximum OwO";
            updatedinfo.Id = createdLecturer.Id;

            //tegevus
            var result = await Svc<ILecturersServices>().Update(updatedinfo);

            //kontroll (5 asserti)
            Assert.NotNull(result);
            Assert.Equal(createdLecturer.Id, result.Id);
            Assert.Equal("TestUusinfo", result.FirstName);
            Assert.Equal("Test2222222222", result.LastName);
            Assert.Equal("Maximum OwO", result.Qualifications);
        }

        [Fact]
        public async Task Should_DeleteLecturer_WhenValidIDIsGiven()
        {
            //ülesseade
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";
            dto.Qualifications = "Testicles";
            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            //tegevus
            var result = await Svc<ILecturersServices>().Delete(createdLecturer.Id);

            //kontroll (5 asserti)
            Assert.NotNull(result);
            Assert.Equal(createdLecturer.Id, result.Id);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Test", result.LastName);
            Assert.Equal("Testicles", result.Qualifications);
        }

        [Fact]
        public async Task Should_ReturnNull_WhenLecturerDoesNotExist()
        {
            //tegevus
            var result = await Svc<ILecturersServices>().DetailAsync(Guid.NewGuid());

            //kontroll (5 asserti)
            Assert.Null(result);
            Assert.True(result == null);
            Assert.False(result != null);
            Assert.IsType<LecturerDTO>(result);
            Assert.Equal(null, result);
        }
    }
}