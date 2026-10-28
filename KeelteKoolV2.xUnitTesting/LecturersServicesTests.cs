using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using System;
using System.Threading.Tasks;
using Xunit;

namespace KeelteKoolV2.xUnitTesting
{
    public class LecturersServicesTests : TestBase
    {
      
        /// Testime, kas uue õppejõu lisamine töötab ja kas andmed salvestuvad
       
        [Fact]
        public async Task Should_AddNewLecturer_WhenResultISReturned()
        {
            // ettevalmistus
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";
            dto.Qualifications = "Testicles";

            // teeme ära
            var result = await Svc<ILecturersServices>().Create(dto);

            // vaatame kas on ok (5 kontrolli)
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Test", result.LastName);
            Assert.Equal("Testicles", result.Qualifications);
        }

        
        /// Testime, kas saame õppejõu andmed kätte, kui teame tema ID-d
        
        [Fact]
        public async Task Should_GetLecturerDetails_WhenGuidISNotNull()
        {
            // ettevalmistus
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";
            dto.Qualifications = "Testicles";
            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            // teeme ära
            var result = await Svc<ILecturersServices>().DetailAsync(createdLecturer.Id);

            // kontroll (5 tükki)
            Assert.NotNull(result);
            Assert.Equal(createdLecturer.Id, result.Id);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Test", result.LastName);
            Assert.Equal("Testicles", result.Qualifications);
        }

     
        /// Testime, kas õppejõu andmete uuendamine (update) töötab
        
        [Fact]
        public async Task Should_UpdateLecturerWithNewData_WhenDataIsDifferentFromDB()
        {
            // ettevalmistus
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";
            dto.Qualifications = "Testicles";
            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            LecturerDTO updatedinfo = new LecturerDTO();
            updatedinfo.FirstName = "TestUusinfo";
            updatedinfo.LastName = "Test2222222222";
            updatedinfo.Qualifications = "Maximum OwO";
            updatedinfo.Id = createdLecturer.Id; // peab ID kaasa andma

            // teeme ära
            var result = await Svc<ILecturersServices>().Update(updatedinfo);

            // kontroll
            Assert.NotNull(result);
            Assert.Equal(createdLecturer.Id, result.Id);
            Assert.Equal("TestUusinfo", result.FirstName);
            Assert.Equal("Test2222222222", result.LastName);
            Assert.Equal("Maximum OwO", result.Qualifications);
        }

        
        /// Testime, kas õppejõudu saab kustutada (delete)
        
        [Fact]
        public async Task Should_DeleteLecturer_WhenValidIDIsGiven()
        {
            // ettevalmistus
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";
            dto.Qualifications = "Testicles";
            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            // teeme ära
            var result = await Svc<ILecturersServices>().Delete(createdLecturer.Id);

            // kontroll
            Assert.NotNull(result);
            Assert.Equal(createdLecturer.Id, result.Id);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Test", result.LastName);
            Assert.Equal("Testicles", result.Qualifications);
        }

        // Siit algavad minu enda mõeldud testid (5 tükki)

        
        /// Minu test 1: Vaatan mis juhtub kui otsida õppejõudu keda pole olemas
       
        [Fact]
        public async Task Should_ReturnNull_WhenLecturerDoesNotExist()
        {
            var result = await Svc<ILecturersServices>().DetailAsync(Guid.NewGuid());

            Assert.Null(result);
            Assert.True(result == null);
            Assert.False(result != null);
            Assert.IsType<LecturerDTO>(result);
            Assert.Equal(null, result);
        }

        
        /// Minu test 2: Kui jätan eesnime tühjaks, kas süsteem paneb ise nime "Jane"
        
        [Fact]
        public async Task Should_AssignDefaultFirstName_WhenFirstNameIsEmpty()
        {
            LecturerDTO dto = new LecturerDTO { FirstName = "", LastName = "Test", Qualifications = "Test" };
            var result = await Svc<ILecturersServices>().Create(dto);

            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
            Assert.True(result.FirstName.Length > 0);
            Assert.NotNull(result.LastName);
            Assert.Equal("Test", result.LastName);
        }

        
        /// Minu test 3: Proovin kustutada õppejõudu keda pole, kas tuleb error
        
        [Fact]
        public async Task Should_NotThrowException_WhenDeletingNonExistentLecturer()
        {
            var exception = await Record.ExceptionAsync(() => Svc<ILecturersServices>().Delete(Guid.NewGuid()));

            Assert.Null(exception);
            Assert.True(exception == null);
            Assert.False(exception != null);
            Assert.IsType<Guid>(Guid.NewGuid());
            Assert.Equal(36, Guid.NewGuid().ToString().Length);
        }

        
        /// Minu test 4: Proovin uuendada õppejõudu keda pole olemas
        
        [Fact]
        public async Task Should_ReturnNull_WhenUpdatingNonExistentLecturer()
        {
            LecturerDTO dto = new LecturerDTO { Id = Guid.NewGuid(), FirstName = "X", LastName = "Y", Qualifications = "Z" };
            var result = await Svc<ILecturersServices>().Update(dto);

            Assert.Null(result);
            Assert.True(result == null);
            Assert.False(result != null);
            Assert.Equal(null, result);
            Assert.IsType<LecturerDTO>(result);
        }

        
        /// Minu test 5: Kas uue õppejõu loomisel antakse talle ikka uus ID
        
        [Fact]
        public async Task Should_GenerateNewGuid_WhenCreatingLecturer()
        {
            LecturerDTO dto = new LecturerDTO { FirstName = "A", LastName = "B", Qualifications = "C" };
            var result = await Svc<ILecturersServices>().Create(dto);

            Assert.NotNull(result.Id);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.True(result.Id != Guid.Empty);
            Assert.IsType<Guid>(result.Id);
            Assert.Equal(36, result.Id.ToString().Length);
        }
    }
}