using System;

namespace WebDataScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Scrapping scrapping = new Scrapping();
            scrapping.start_Browser();
            scrapping.YouTubeScraping();
            scrapping.close_Browser(); 


            ScrapingTest1 test1 = new ScrapingTest1();
            test1.start_Browser();
            test1.LTBlogScraping();
            test1.close_Browser();
        }
    }
}
