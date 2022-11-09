using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

new DriverManager().SetUpDriver(new ChromeConfig());

var driver = new ChromeDriver();
driver.Navigate().GoToUrl("view-source:https://www.zillow.com/mortgage-rates/");
driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(10000);
var html = driver.PageSource;
File.WriteAllText("output4.html", html);
driver.Quit();