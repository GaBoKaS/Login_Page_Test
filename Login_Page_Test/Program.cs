﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Login_Page_Test
{
    class Program : Selenium
    {
        static string login;
        static string pass;
        static string pageUrl;
        static void Main(string[] args)
        {
            ReadData();
            LoginTest();
            LanguageTest();
            LinkTest();
            ResponsiveTest();

        }
        public static void ReadData()
        {
            string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "TestData.txt");
            string[] allData = File.ReadAllLines(path);
            List<string> lines = new List<string>();
            lines = File.ReadAllLines(path).ToList();
            pageUrl = lines[0];
            login = lines[1];
            pass = lines[2];
        }

        public static void LoginTest()
        {
            string badEmailWarning = "#react-app > div.Loader > div > div > div > div > div.panel-body > div > div";
            string loggedInElement = "#page-wrapper > div > h1";
            string loginByPasswordBtn = "#login-methods-heading-user_credentials > strong";
            StartUp(pageUrl);
            Enter("#userIdentifier", "warning please", Keys.Enter);
            AssertText(badEmailWarning, "The specified user could not be found");

            Enter("#userIdentifier", login, Keys.Enter);
            ClickAuntilB(loginByPasswordBtn, "#password");

            Enter("#password", pass, Keys.Enter);
            AssertText(loggedInElement, "Account Overview");
            Console.WriteLine("Login test is completed");
            EndTest();
        }
        public static void LanguageTest()
        {
            string loginTextElement = "#react-app > div.Loader > div > div > div > div > div.panel-heading.panel-heading-narrow > div";
            string AllLanguagesButton = "#react-language-list-container > ul > li:nth-child(6) > span > i";
            string LanguageCssBeginning = "body > div:nth-child(15) > div.fade.in.modal > div > div > div.modal-body > ul > li:nth-child(";
            string[] MultiLanguageText = new string[10]
            {"LOG IN","PRISIJUNKITE","ВОЙТИ В СИСТЕМУ","PIESLĒGTIES","ZALOGUJ SIĘ","ВХОД","INICIA","LOGI SISSE","MELDEN SIE SICH AN","CONECTARE" };
            string[] LanguageToTest = new string[10]
            {"English","LietuviŲ","Русский","Latviešu","Polski","Български","Español","Eesti","Deutsch","Română"};
            StartUp(pageUrl);
            AssertText(loginTextElement, MultiLanguageText[0]);
            for (int i = 1; i <= 9; i++)
            {
                Click(AllLanguagesButton);
                for (int u = 1; u <= 9; u++)
                {
                    if (element(LanguageCssBeginning + u + ") > a").Text.Equals(LanguageToTest[i], StringComparison.InvariantCultureIgnoreCase))
                    {
                        Click(LanguageCssBeginning + u + ") > a");
                        WaitForTextChange(loginTextElement, MultiLanguageText[i]);
                        AssertText(loginTextElement, MultiLanguageText[i]);
                        break;
                    }
                }
            }
            Console.WriteLine("Language test is completed");
            EndTest();
        }
        public static void LinkTest()
        {
            StartUp(pageUrl);
            string[] elementsToTest = new string[4] {
                "body > footer > div.footer-info-holder > div:nth-child(3) > ul > li:nth-child(1) > a",
                "body > footer > div.footer-info-holder > div:nth-child(3) > ul > li:nth-child(2) > a",
                "body > footer > div.footer-info-holder > div:nth-child(3) > ul > li:nth-child(3) > a",
                "#react-app > div.text-center > span.text-capitalize > a"
            };
            string[] elementForConfirmation = new string[4] {
                "body > main > article > section:nth-child(1) > div > h1",
                "body > main > article > section:nth-child(1) > div > h1",
                "body > main > article > section:nth-child(2) > div > div > h1",
                "#registration-h1"
            };
            string[] desiredText = new string[4]{
                "Privacy Policy",
                "Services Agreement",
                "How to use",
                "ACCOUNT REGISTRATION"
            };
            for (int i = 0; i < elementsToTest.Length; i++)
            {
                Click(elementsToTest[i]);
                AssertText(elementForConfirmation[i], desiredText[i]);
                PageBack();
            }
            LinkTest_NewWindowLinks();
            Console.WriteLine("Link test is completed");
            EndTest();
        }
        public static void LinkTest_NewWindowLinks()
        {
            string[] elementsToTest = new string[2] {
                "body > footer > div.footer-info-holder > div:nth-child(2) > p > a",
                "body > footer > div.kayako-wrapper > p > a > span.img-container > img"
            };
            string[] elementForConfirmation = new string[2] {
                "#content > div > div:nth-child(3) > div > div:nth-child(1) > div > div:nth-child(2) > p:nth-child(1) > strong",
                "#chatform > table:nth-child(3) > tbody > tr:nth-child(1) > td.fieldtitle"
            };
            string[] desiredText = new string[2]{
                "Authorization code",
                "Choose a question"
            };
            for (int i = 0; i < elementsToTest.Length; i++)
            {
                Click(elementsToTest[i]);
                SwitchWindow("New");
                AssertText(elementForConfirmation[i], desiredText[i]);
                TabClose();
                SwitchWindow("Main");
            }
        }
        public static void ResponsiveTest()
        {
            string textElement = "body > aside > div.authentication-sidebar-content > div.login-banner-container > h1";
            StartUp(pageUrl);
            ChangeResolution(1366, 768);
            AssertText(textElement, "for more convenient us");
            ChangeResolution(400, 800);
            Debug.Assert(!element(textElement).Displayed);
            Console.WriteLine("Responsive elements test is completed");
            EndTest();
        }
    }
}