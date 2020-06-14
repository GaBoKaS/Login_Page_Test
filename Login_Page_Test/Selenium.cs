using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.Drawing;

namespace Login_Page_Test
{
    class Selenium
    {
        static IWebDriver driver;
        public static void StartUp(string pagelink)
        { // Sets timeouts for page loading and load starting page
            driver = new ChromeDriver();
            _ = driver.Manage().Timeouts().ImplicitWait;
            driver.Navigate().GoToUrl(pagelink);
        }
        public static void Click(string selector)
        { // Clicks the element specified by selector
            try
            {
                element(selector).Click();
            }
            catch (Exception e)
            {
                WaitFor(selector);
                Click(selector);
            }
        }
        public static void ClickAuntilB(string selector1, string selector2)
        { // keeps clicking element A until element B apears
            WaitFor(selector1);
            while (!(element(selector2).Displayed && element(selector2).Enabled))
            {
                try
                {
                    element(selector1).Click();
                }
                catch (Exception e) { }
            }
        }
        public static void Enter(string selector, string text, string actionAfter)
        { // Types text in a field specified by selector and does desired key press afterwards
            IWebElement elem = element(selector);
            WaitFor(selector);
            while (!element(selector).GetAttribute("value").Equals(text))
            {
                elem.Clear();
                elem.SendKeys(text);
                WaitForText(selector, text);
            }
            elem.SendKeys(actionAfter);
        }
        public static void AssertText(string selector, string text)
        { // checks if element has desired string in it
            WaitForTextElement(selector);
            bool EverythingWentGood = driver.FindElement(By.CssSelector(selector)).Text.Contains(text);
            Debug.Assert(EverythingWentGood);
        }
        public static void WaitForText(string selector, string text)
        { //Waits until certain string is typed into element
            int i = 0;
            while (i < 100 && !element(selector).GetAttribute("value").Contains(text))
            { //when i hits 500 it is a timeout
                i++;
            }
        }
        public static void WaitForTextChange(string selector, string Text)
        { //waits until target element has changed its text
            int i = 0;
            while (i < 300 && !element(selector).Text.Contains(Text))
            { //when i hits 500 it is a timeout
                i++;
            }
        }
        public static void WaitFor(string selector)
        { // Waits until element is displayed 
            int i = 0;
            bool IsVisible = false;
            while (i < 500 && !IsVisible)
            { //when i hits 500 it is a timeout
                try
                {
                    i++;
                    if ((element(selector).Displayed && element(selector).Enabled))
                    { // Element is visible now, while loop ends
                        IsVisible = true;
                    }
                }
                catch (Exception e) { }
            }
        }

        public static void WaitForTextElement(string selector)
        { // Waits until non clickable text element is displayed 
            int i = 0;
            bool IsVisible = false;
            while (i < 500 && !IsVisible)
            { //when i hits 500 it is a timeout
                try
                {
                    i++;
                    if ((driver.FindElement((By.CssSelector(selector))).Displayed))
                    { // Element is visible now, while loop ends
                        IsVisible = true;
                    }
                }
                catch (Exception e) { }
            }
        }

        public static IWebElement element(string selector)
        { // finds and returns element by its Css Selector
            try
            {
                return driver.FindElement(By.CssSelector(selector));
            }
            catch (Exception e)
            {
                WaitFor(selector);
                return element(selector);
            }
        }
        public static void EndTest()
        { // closes browser
            driver.Quit();
        }

        public static void PageBack()
        { // goes page back
            driver.Navigate().Back();
        }
        public static void TabClose()
        {
            driver.Close();
        }
        // Methods for navigation thought out pages/tabs ---------------------------------
        public static void SwitchWindow(string page)
        {
            if (page.Equals("Main"))
            {
                driver.SwitchTo().Window(driver.WindowHandles[0]);
            }
            else if (page.Equals("New"))
            {
                driver.SwitchTo().Window(driver.WindowHandles[1]);
            }
        }
        public static void ChangeResolution(int width, int height)
        {
            driver.Manage().Window.Size = new Size(width, height);
        }
    }
}