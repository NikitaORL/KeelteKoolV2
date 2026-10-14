using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace KeelteKoolV2SeleniumTesting
{
    public class LanguageCourceFrontendTests
    {
        [Fact]
        public void Should_NavigateToIndex_AddSpaceWithCorrectData_ReturnToIndex()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Url = "https://localhost:7191/"; //navigeeritav aadress
            IWebElement navigateToLanguageCourse = driver.FindElement(By.LinkText("Keelekursused")); //otsime testitava osa lingi
            navigateToLanguageCourse.Click(); //simuleeritud tegevus elemendiga, antud juhul klikk



            /* tegevus */
            IWebElement indexcreatebutton = driver.FindElement(By.Id("IndexCreateNewLanguageCourse_BTN"));
            indexcreatebutton.Click();
        }
    }
}