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
    public class Scrapping
    {
        String test_url_1 = "https://www.youtube.com/c/LambdaTest/videos";
        static Int32 vcount = 1;
        public IWebDriver driver;
 
        //[SetUp]
        public void start_Browser()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized"); 
            options.AddArgument("disable-notifications");
            driver = new ChromeDriver("Resources", options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

        }
 
        //[Test(Description = "Web Scraping LambdaTest YouTube Channel"), Order(1)]
        public void YouTubeScraping()
        {
            driver.Url = test_url_1;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
 
            Thread.Sleep(5000);
 
            /* Once the page has loaded, scroll to the end of the page to load all the videos */
            /* Scroll to the end of the page to load all the videos in the channel */
            /* Get scroll height */
            Int64 last_height = (Int64)(((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight"));
            while (true)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.documentElement.scrollHeight);");
                /* Wait to load page */
                Thread.Sleep(2000);
                /* Calculate new scroll height and compare with last scroll height */
                Int64 new_height = (Int64)((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight");
                if (new_height == last_height)
                    /* If heights are the same it will exit the function */
                    break;
                last_height = new_height;
            }
 
            By elem_video_link = By.CssSelector("ytd-grid-video-renderer.style-scope.ytd-grid-renderer");
            ReadOnlyCollection<IWebElement> videos = driver.FindElements(elem_video_link);
            Console.WriteLine("Total number of videos in " + test_url_1 + " are " + videos.Count);
 
            /* Go through the Videos List and scrap the same to get the attributes of the videos in the channel */
            foreach (IWebElement video in videos)
            {
                string str_title, str_views, str_rel;
                IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title"));
                str_title = elem_video_title.Text;
 
                IWebElement elem_video_views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
                str_views = elem_video_views.Text;
 
                IWebElement elem_video_reldate = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[2]"));
                str_rel = elem_video_reldate.Text;
 
                Console.WriteLine("******* Video " + vcount + " *******");
                Console.WriteLine("Video Title: " + str_title);
                Console.WriteLine("Video Views: " + str_views);
                Console.WriteLine("Video Release Date: " + str_rel);
                Console.WriteLine("\n");
                vcount++;
            }
            Console.WriteLine("Scraping Data from LambdaTest YouTube channel Passed");
        }
 
       // [TearDown]
        public void close_Browser()
        {
            driver.Quit();
        }

        
    }
}