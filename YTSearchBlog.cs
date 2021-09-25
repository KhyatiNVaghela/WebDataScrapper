using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium.Safari;
using System.Collections.Generic;
using System.Web;

namespace WebDataScrapper
{
    public class ScrapingTest1
    {
        String test_url_2 = "https://www.lambdatest.com/blog/";
        static Int32 vcount = 1;
        public IWebDriver driver;

        [SetUp]
        public void start_Browser()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            options.AddArgument("disable-notifications");
            driver = new ChromeDriver("..\\..\\..\\Resources", options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        [Test(Description = "Web Scraping LambdaTest Blog Page"), Order(2)]
        public void LTBlogScraping()
        {
            driver.Url = test_url_2;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(5000);

            /* Find total number of blogs on the page */
            By elem_blog_list = By.CssSelector("div.col-xs-12.col-md-12.blog-list");
            ReadOnlyCollection<IWebElement> blog_list = driver.FindElements(elem_blog_list);
            Console.WriteLine("Total number of videos in " + test_url_2 + " are " + blog_list.Count);

            /* Reset the variable from the previous test */
            vcount = 1;

            /* Go through the Blogs List and scrap the same to get the attributes of the blogs on the page*/
            foreach (IWebElement blog in blog_list)
            {
                string str_blog_title, str_blog_author, str_blog_views, str_blog_link;

                IWebElement elem_blog_title = blog.FindElement(By.ClassName("blog-titel"));
                str_blog_title = elem_blog_title.Text;

                IWebElement elem_blog_link = blog.FindElement(By.ClassName("blog-titel"));
                IWebElement elem_blog_alink = elem_blog_link.FindElement(By.TagName("a"));
                str_blog_link = elem_blog_alink.GetAttribute("href");

                IWebElement elem_blog_author = blog.FindElement(By.ClassName("user-name"));
                str_blog_author = elem_blog_author.Text;

                IWebElement elem_blog_views = blog.FindElement(By.ClassName("comm-count"));
                str_blog_views = elem_blog_views.Text;
                vcount++;
            }
        }

        [TearDown]
        public void close_Browser()
        {
            driver.Quit();
        }
    }
}