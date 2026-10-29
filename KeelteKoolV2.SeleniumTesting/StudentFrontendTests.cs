using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
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
            IWebElement navigateToStudents = driver.FindElement(By.LinkText("Students")); //otsime testitava osa lingi
            navigateToStudents.Click();
            return driver;
        }
    }
}