using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V153.Security;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace KeelteKoolV2.SeleniumTesting
{
    [TestCaseOrderer("KeelteKoolV2.SeleniumTesting.PriorityOrderer", "KeelteKoolV2.SeleniumTesting")]
    public class StudentFrontendTests
    {
        public static string thisRunGuid { get; set; } = "";

        /// <summary>
        /// Test loob uue õpilase unikaalse eesnimega. Kontrollib, et pärast salvestamist
        /// ollakse tagasi nimekirjas ja õpilane on tabelis õige eesnime, perenime ja e-postiga.
        /// </summary>
        [Fact]
        [Order(1)]
        public void Should_CreateStudent_WithUniqueData()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();

            /* tegevus */
            driver.FindElement(By.LinkText("Create New")).Click();
            thisRunGuid = DataEntry_Student(driver);
            driver.FindElement(By.CssSelector("input[value='Create']")).Click();
            Thread.Sleep(1000);

            /* kontroll */
            string url = driver.Url;
            IWebElement? row = FindStudentRow(driver, thisRunGuid);
            bool isObjectFound = row != null;
            string firstName = "";
            string lastName = "";
            string email = "";
            if (row != null)
            {
                List<IWebElement> cells = row.FindElements(By.TagName("td")).ToList();
                firstName = cells[0].Text;
                lastName = cells[1].Text;
                email = cells[2].Text;
            }
            driver.Quit();

            Assert.EndsWith("/Students", url);
            Assert.True(isObjectFound);
            Assert.Equal(thisRunGuid, firstName);
            Assert.Equal("Selenium", lastName);
            Assert.Equal("selenium@test.ee", email);
        }


        /// <summary>
        /// Test avab eelnevalt loodud õpilase Details lehe ja kontrollib, et lehel on
        /// samad andmed mis loomisel sisestati ning loomise kuupäev on täidetud.
        /// </summary>
        [Fact]
        [Order(2)]
        public void Should_ShowDetailsOfStudent_WithPreviouslyUniqueData()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            IWebElement? row = FindStudentRow(driver, thisRunGuid);

            /* tegevus */
            bool isObjectFound = row != null;
            if (row != null)
            {
                row.FindElement(By.LinkText("Details")).Click();
            }
            Thread.Sleep(1000);
            string url = driver.Url;
            string heading = driver.FindElement(By.TagName("h1")).Text;
            List<IWebElement> values = driver.FindElements(By.TagName("dd")).ToList();
            string details_firstName = values[0].Text;
            string details_lastName = values[1].Text;
            string details_email = values[2].Text;
            string details_createdAt = values[3].Text;
            driver.Quit();

            /* kontroll */
            Assert.True(isObjectFound);
            Assert.Contains("/Students/Details", url);
            Assert.Equal("Student Details", heading);
            Assert.Equal(thisRunGuid, details_firstName);
            Assert.Equal("Selenium", details_lastName);
            Assert.Equal("selenium@test.ee", details_email);
            Assert.NotEmpty(details_createdAt);
            Assert.Contains(DateTime.Now.Year.ToString(), details_createdAt);
        }

        /// <summary>
        /// Test muudab eelnevalt loodud õpilase perenime ja e-posti (eesnimi jääb samaks, et õpilast leida).
        /// Kontrollib, et nimekirjas on uued väärtused ja vanad väärtused on kadunud.
        /// </summary>
        [Fact]
        [Order(3)]
        public void Should_UpdateStudent_WithNewData()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            IWebElement? row = FindStudentRow(driver, thisRunGuid);

            /* tegevus */
            bool isObjectFound = row != null;
            if (row != null)
            {
                row.FindElement(By.LinkText("Edit")).Click();
            }
            Thread.Sleep(1000);
            IWebElement lastNameInput = driver.FindElement(By.Id("LastName"));
            lastNameInput.Clear();
            lastNameInput.SendKeys("Uuendatud");
            IWebElement emailInput = driver.FindElement(By.Id("Email"));
            emailInput.Clear();
            emailInput.SendKeys("uuendatud@test.ee");
            driver.FindElement(By.CssSelector("input[value='Save']")).Click();
            Thread.Sleep(1000);

            /* kontroll */
            string url = driver.Url;
            IWebElement? updatedRow = FindStudentRow(driver, thisRunGuid);
            string firstName = "";
            string lastName = "";
            string email = "";
            if (updatedRow != null)
            {
                List<IWebElement> cells = updatedRow.FindElements(By.TagName("td")).ToList();
                firstName = cells[0].Text;
                lastName = cells[1].Text;
                email = cells[2].Text;
            }
            driver.Quit();

            Assert.True(isObjectFound);
            Assert.EndsWith("/Students", url);
            Assert.Equal(thisRunGuid, firstName);
            Assert.Equal("Uuendatud", lastName);
            Assert.Equal("uuendatud@test.ee", email);
            Assert.NotEqual("Selenium", lastName);
            Assert.NotEqual("selenium@test.ee", email);
        }

        /// <summary>
        /// Test kustutab eelnevalt loodud õpilase. Kontrollib, et kustutamise lehel on õige õpilane
        /// ja pärast kinnitamist ei ole õpilast enam nimekirjas.
        /// </summary>
        [Fact]
        [Order(4)]
        public void Should_DeleteStudent_FromDeleteView()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            IWebElement? row = FindStudentRow(driver, thisRunGuid);

            /* tegevus */
            bool isObjectFoundBefore = row != null;
            if (row != null)
            {
                row.FindElement(By.LinkText("Delete")).Click();
            }
            Thread.Sleep(1000);
            string deleteUrl = driver.Url;
            string deleteViewFirstName = driver.FindElements(By.TagName("dd"))[0].Text;
            driver.FindElement(By.CssSelector("input[value='Delete']")).Click();
            Thread.Sleep(1000);

            /* kontroll */
            string url = driver.Url;
            IWebElement? deletedRow = FindStudentRow(driver, thisRunGuid);
            bool isObjectFoundAfter = deletedRow != null;
            driver.Quit();

            Assert.True(isObjectFoundBefore);
            Assert.Contains("/Students/Delete", deleteUrl);
            Assert.Equal(thisRunGuid, deleteViewFirstName);
            Assert.EndsWith("/Students", url);
            Assert.False(isObjectFoundAfter);
        }

        /// <summary>
        /// Test vajutab loomisvormil "Create" ilma andmeid sisestamata. Kontrollib, et kasutaja jääb
        /// loomislehele ja iga välja all kuvatakse õige veateade.
        /// </summary>
        [Fact]
        [Order(5)]
        public void Should_ShowErrors_WhenCreateFormIsEmpty()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            driver.FindElement(By.LinkText("Create New")).Click();

            /* tegevus */
            driver.FindElement(By.CssSelector("input[value='Create']")).Click();
            Thread.Sleep(1000);

            /* kontroll */
            string url = driver.Url;
            string firstNameError = driver.FindElement(By.CssSelector("span[data-valmsg-for='FirstName']")).Text;
            string lastNameError = driver.FindElement(By.CssSelector("span[data-valmsg-for='LastName']")).Text;
            string emailError = driver.FindElement(By.CssSelector("span[data-valmsg-for='Email']")).Text;
            driver.Quit();

            Assert.Contains("/Students/Create", url);
            Assert.Equal("Eesnimi on kohustuslik", firstNameError);
            Assert.Equal("Perenimi on kohustuslik", lastNameError);
            Assert.Equal("E-post on kohustuslik", emailError);
        }

        [Fact]
        [Order(6)]
        public void Should_ShowError_WhenEmailIsValid()
        {
            //Ulesseade
            IWebDriver driver = TestSetup_NavigateToIndex();
            driver.FindElement(By.LinkText("Create New")).Click();
            string invalidGuid = Guid.NewGuid().ToString();
            driver.FindElement(By.Id("FirstName")).SendKeys(invalidGuid);
            driver.FindElement(By.Id("LastName")).SendKeys(invalidGuid);
            driver.FindElement(By.Id("Email")).SendKeys(invalidGuid);

            // tegevus
            driver.FindElement(By.CssSelector("input[value='Create']")).Click();
            Thread.Sleep(1000);

            //kontroll
            string url = driver.Url;
            string mailError = driver.FindElement(By.CssSelector("span[data-valmsg-for='Email']")).Text;
            string firstNameError = driver.FindElement(By.CssSelector("span[data-valmsg-for='FirstName']")).Text;
            driver.FindElement(By.LinkText("Back to List")).Click();
            Thread.Sleep(1000);
            IWebElement? row = FindStudentRow(driver, invalidGuid);
            bool isObjectFound = row != null;
            driver.Quit();

            Assert.Contains("/Students/Create", url);
            Assert.Equal("Vigane e-posti addres", mailError);
            Assert.Equal("", firstNameError);
            Assert.False(isObjectFound);

        }

        //oluline funktsioonid 
        private static IWebElement? FindStudentRow(IWebDriver driver, string firstName)
        {
            IWebElement table = driver.FindElement(By.TagName("table"));
            List<IWebElement> rowsOfTable = table.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0); //esimene rida on päis
            foreach (var row in rowsOfTable)
            {
                List<IWebElement> cells = row.FindElements(By.TagName("td")).ToList();
                if (cells[0].Text == firstName)
                {
                    return row;
                }
            }
            return null;
        }
        private static string DataEntry_Student(IWebDriver driver)
        {
            Guid generatedId = Guid.NewGuid();
            driver.FindElement(By.Id("FirstName")).SendKeys(generatedId.ToString());
            driver.FindElement(By.Id("LastName")).SendKeys("Selenium");
            driver.FindElement(By.Id("Email")).SendKeys("selenium@test.ee");
            return generatedId.ToString();
        }
        private static IWebDriver TestSetup_NavigateToIndex()
        {
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Url = "https://localhost:7191/"; //navigeeritav aadress
            Thread.Sleep(3000);
            IWebElement navigateToLanguageCourse = driver.FindElement(By.LinkText("Keelekursused"));//otsime tesitava osa lingi
            navigateToLanguageCourse.Click(); //simuleeritud tegevus elemendiga, antud juhul klikk
            return driver;
        }

    }
}