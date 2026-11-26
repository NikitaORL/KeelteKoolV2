using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using Xunit;

namespace KeelteKoolV2.SeleniumTesting
{
    [TestCaseOrderer("KeelteKoolV2.SeleniumTesting.PriorityOrderer", "KeelteKoolV2.SeleniumTesting")]
    public class StudentFrontendTests
    {
        public static string thisRunGuid { get; set; }

        /// <summary>
        /// Test loob uue õpilase unikaalse eesnimega ja kontrollib, et õpilane ilmub nimekirja õigete andmetega.
        /// </summary>
        [Fact]
        [Order(1)]
        public void Should_AddStudentWithUniqueData_ReturnToIndex()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();

            /* tegevus */
            IWebElement indexCreateButton = driver.FindElement(By.Id("IndexCreateNewStudent_BTN"));
            indexCreateButton.Click();

            string uniqueId = DataEntry_Student(driver);
            thisRunGuid = uniqueId;

            IWebElement createButton = driver.FindElement(By.Id("CU_Create_BTN"));
            createButton.Click();
            Thread.Sleep(1000);

            /* kontroll */
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string firstNameData = "";
            string lastNameData = "";
            string emailData = "";
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexTableStudentFirstName")).Text == thisRunGuid)
                {
                    firstNameData = row.FindElement(By.Id("IndexTableStudentFirstName")).Text;
                    lastNameData = row.FindElement(By.Id("IndexTableStudentLastName")).Text;
                    emailData = row.FindElement(By.Id("IndexTableStudentEmail")).Text;
                }
            }
            driver.Quit();

            Assert.Equal(uniqueId, firstNameData);
            Assert.Equal("Selenium", lastNameData);
            Assert.Equal("selenium@test.ee", emailData);
        }

        /// <summary>
        /// Test avab eelnevalt loodud õpilase Details lehe ja kontrollib, et andmed on samad mis loomisel.
        /// </summary>
        [Fact]
        [Order(2)]
        public void Should_NavigateToDetailsOfStudent_WithPreviouslyUniqueData()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexTableStudentFirstName")).Text == thisRunGuid)
                {
                    isObjectFound = true;
                    row.FindElement(By.Id("IndexTableStudentDetails_BTN")).Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            string details_firstName = driver.FindElement(By.Id("DD_StudentFirstName")).Text;
            string details_lastName = driver.FindElement(By.Id("DD_StudentLastName")).Text;
            string details_email = driver.FindElement(By.Id("DD_StudentEmail")).Text;
            driver.Quit();

            /* kontroll */
            Assert.True(isObjectFound);
            Assert.Equal(thisRunGuid, details_firstName);
            Assert.Equal("Selenium", details_lastName);
            Assert.Equal("selenium@test.ee", details_email);
        }

        /// <summary>
        /// Test muudab eelnevalt loodud õpilase perenime ja e-posti ning kontrollib, et nimekirjas on uued andmed.
        /// </summary>
        [Fact]
        [Order(3)]
        public void Should_UpdateStudent_WithNewData()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexTableStudentFirstName")).Text == thisRunGuid)
                {
                    row.FindElement(By.Id("IndexTableStudentUpdate_BTN")).Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            IWebElement lastName = driver.FindElement(By.Id("CU_Student_LastName"));
            lastName.Clear();
            lastName.SendKeys("Uuendatud");
            IWebElement email = driver.FindElement(By.Id("CU_Student_Email"));
            email.Clear();
            email.SendKeys("uuendatud@test.ee");
            driver.FindElement(By.Id("CU_Update_BTN")).Click();
            Thread.Sleep(1000);

            /* kontroll */
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string lastNameData = "";
            string emailData = "";
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexTableStudentFirstName")).Text == thisRunGuid)
                {
                    lastNameData = row.FindElement(By.Id("IndexTableStudentLastName")).Text;
                    emailData = row.FindElement(By.Id("IndexTableStudentEmail")).Text;
                }
            }
            driver.Quit();

            Assert.Equal("Uuendatud", lastNameData);
            Assert.Equal("uuendatud@test.ee", emailData);
            Assert.NotEqual("Selenium", lastNameData);
        }

        /// <summary>
        /// Test kustutab eelnevalt loodud õpilase ja kontrollib, et teda ei ole enam nimekirjas.
        /// </summary>
        [Fact]
        [Order(4)]
        public void Should_DeleteStudent_FromDeleteView()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexTableStudentFirstName")).Text == thisRunGuid)
                {
                    row.FindElement(By.Id("IndexTableStudentDelete_BTN")).Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            driver.FindElement(By.Id("DeleteDelete_BTN")).Click();
            Thread.Sleep(1000);

            /* kontroll */
            bool isObjectFound = false;
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexTableStudentFirstName")).Text == thisRunGuid)
                {
                    isObjectFound = true;
                    break;
                }
            }
            driver.Quit();

            Assert.False(isObjectFound);
        }

        /// <summary>
        /// Test vajutab loomisvormil "Create" ilma andmeid sisestamata ja kontrollib, et iga välja all on veateade.
        /// </summary>
        [Fact]
        [Order(5)]
        public void Should_ShowErrors_WhenCreateFormIsEmpty()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            driver.FindElement(By.Id("IndexCreateNewStudent_BTN")).Click();

            /* tegevus */
            driver.FindElement(By.Id("CU_Create_BTN")).Click();
            Thread.Sleep(1000);

            /* kontroll */
            string firstNameError = driver.FindElement(By.Id("CU_Student_FirstName_Error")).Text;
            string lastNameError = driver.FindElement(By.Id("CU_Student_LastName_Error")).Text;
            string emailError = driver.FindElement(By.Id("CU_Student_Email_Error")).Text;
            driver.Quit();

            Assert.Equal("Eesnimi on kohustuslik", firstNameError);
            Assert.Equal("Perenimi on kohustuslik", lastNameError);
            Assert.Equal("E-post on kohustuslik", emailError);
        }

        /// <summary>
        /// Test sisestab e-posti väljale vigase aadressi (ilma @ märgita) ja kontrollib, et kuvatakse veateade.
        /// </summary>
        [Fact]
        [Order(6)]
        public void Should_ShowError_WhenEmailIsInvalid()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            driver.FindElement(By.Id("IndexCreateNewStudent_BTN")).Click();
            driver.FindElement(By.Id("CU_Student_FirstName")).SendKeys("Test");
            driver.FindElement(By.Id("CU_Student_LastName")).SendKeys("Test");
            driver.FindElement(By.Id("CU_Student_Email")).SendKeys("ilmaMargita");

            /* tegevus */
            driver.FindElement(By.Id("CU_Create_BTN")).Click();
            Thread.Sleep(1000);

            /* kontroll */
            string emailError = driver.FindElement(By.Id("CU_Student_Email_Error")).Text;
            string firstNameError = driver.FindElement(By.Id("CU_Student_FirstName_Error")).Text;
            driver.Quit();

            Assert.Equal("Vigane e-posti addres", emailError);
            Assert.Equal("", firstNameError);
        }

        /// <summary>
        /// Test täidab loomisvormi, kuid vajutab "Cancel" ilma salvestamata ja kontrollib, et õpilast ei loodud.
        /// </summary>
        [Fact]
        [Order(7)]
        public void Should_NotCreateStudent_WhenCancelIsClicked()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            driver.FindElement(By.Id("IndexCreateNewStudent_BTN")).Click();
            string cancelGuid = DataEntry_Student(driver);

            /* tegevus */
            driver.FindElement(By.Id("CU_Cancel_BTN")).Click();
            Thread.Sleep(1000);

            /* kontroll */
            bool isObjectFound = false;
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexTableStudentFirstName")).Text == cancelGuid)
                {
                    isObjectFound = true;
                    break;
                }
            }
            driver.Quit();

            Assert.False(isObjectFound);
        }

        /// <summary>
        /// Test kontrollib, et menüü lingilt "Students" avaneb õpilaste nimekiri õige aadressi ja pealkirjaga.
        /// </summary>
        [Fact]
        [Order(8)]
        public void Should_OpenStudentsList_FromNavbar()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();

            /* tegevus */
            string url = driver.Url;
            string heading = driver.FindElement(By.TagName("h1")).Text;
            driver.Quit();

            /* kontroll */
            Assert.EndsWith("/Students", url);
            Assert.Equal("Students", heading);
        }

        /// <summary>
        /// Sisestab loomisvormi andmed, eesnimeks pannakse unikaalne Guid.
        /// </summary>
        /// <param name="driver">Hetkel käsitletav draiver</param>
        /// <returns>Tagastab Guidi stringi kujul, mis pandi eesnime väljale</returns>
        private static string DataEntry_Student(IWebDriver driver)
        {
            Guid generatedId = Guid.NewGuid();
            driver.FindElement(By.Id("CU_Student_FirstName")).SendKeys(generatedId.ToString());
            driver.FindElement(By.Id("CU_Student_LastName")).SendKeys("Selenium");
            driver.FindElement(By.Id("CU_Student_Email")).SendKeys("selenium@test.ee");
            return generatedId.ToString();
        }

        /// <summary>
        /// Ülesseade meetod mis tekitab draiveri, navigeerib aadressile ja vajutab Students lingile.
        /// </summary>
        /// <returns>Tagastab testisiseselt kasutatava draiveri</returns>
        private static IWebDriver TestSetup_NavigateToIndex()
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Url = "https://localhost:7191/";
            Thread.Sleep(3000);
            IWebElement navigateToStudents = driver.FindElement(By.LinkText("Students"));
            navigateToStudents.Click();
            return driver;
        }
    }
}