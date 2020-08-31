using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using OpenQA.Selenium.Interactions;

namespace SupremeXE_UI
{
    public partial class Browser
    {
        public async void Automatic(List<ToBuy> products)
        {
            await Task.Run(() => {
            foreach (var path in pathway)
            {
                List<string> urls = new List<string>();
                driver.Navigate().GoToUrl(path);

                var articles = driver.FindElements(By.XPath("//div[@id='container']/article"));
                foreach (var article in articles)
                {
                    //// Handle "scrolling" through window ////
                    if(driver.Url == path)
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript($"arguments[0].scrollIntoView(true);", article);
                    }

                    Actions action = new Actions(driver);
                    action.MoveToElement(article).Perform();

                    string articleName = article.FindElement(By.XPath(".//h1/a[@class='name-link']")).Text;
                    string articleColor = article.FindElement(By.XPath(".//p/a[@class='name-link']")).Text.ToLower();
                    bool item = products.Any(x => x.SelectedName == articleName && x.SelectedColor.ToLower() == articleColor);
                    Debugger.Log($"{articleName} {articleColor}");

                    if (item)
                    {
                        string url = article.FindElement(By.XPath(".//h1/a[@class='name-link']")).GetAttribute("href");
                        urls.Add(url);
                    }
                }

                foreach(var uri in urls)
                {
                    driver.Navigate().GoToUrl(uri);

                    int findElement = driver.FindElements(By.XPath("//select[@id='size']")).Count;
                    if (findElement > 0)
                    {
                        var isSold = driver.FindElements(By.XPath("//b[@class='button sold-out']")).Count;

                        if(isSold == 0)
                        {
                            string articleName = driver.FindElement(By.XPath("//h1[@class='protect']")).Text;
                            string articleColor = driver.FindElement(By.XPath("//p[@class='style protect']")).Text.ToLower();

                            var product = products.Find(x => x.SelectedName == articleName && x.SelectedColor.ToLower() == articleColor);


                            if (product.SelectedSize == "Any")
                            {
                                AddRemoveItem();
                            }
                            else if (product.SelectedSize != "Any")
                            {
                                var values = driver.FindElement(By.XPath("//select[@id='size']"));
                                var itemsUnder = new SelectElement(values);
                                itemsUnder.SelectByText(product.SelectedSize);
                                AddRemoveItem();
                            }
                        }
                    }
                }
            }
                Cart();
                Checkout();
            });
        }
    }
}