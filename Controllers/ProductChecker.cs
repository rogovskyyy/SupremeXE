using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupremeXE_UI
{
    class ProductChecker
    {
        private IWebDriver _driver = null;
        private readonly Browser browser = null;
        private readonly Config config = null;

        public ProductChecker()
        {
            if(!Directory.Exists("temp"))
            {
                Directory.CreateDirectory("temp");
            }

            config = Config.GetInstance();
            browser = Browser.GetInstance();
        }

        public void Droplist()
        {
            Debugger.Log($"Nawigowanie do {config.Links.Droplist}");
            _driver.Navigate().GoToUrl(config.Links.Droplist);
        }

        public void DownloadImage(string uri, string filename)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(uri);
            Bitmap bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save($@"{Directory.GetCurrentDirectory()}\\temp\\{filename}.png", ImageFormat.Png);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }

        public void UpdateJson(List<Products> items)
        {
            using (StreamWriter file = File.CreateText($@"{Directory.GetCurrentDirectory()}\\products.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, items);
            }
        }

        public async void Process()
        {
            int counter = 0;
            List<Products> products = new List<Products>();

            await browser.OpenBrowser(true);
            _driver = browser.driver;
            Droplist();

            int amount = _driver.FindElements(By.XPath("//div[contains(@class, 'masonry__item col-sm-4 col-xs-6')]")).Count;
            Debugger.Log($"Znaleziono {amount} przedmiotów");

            foreach (var item in _driver.FindElements(By.XPath("//div[contains(@class, 'masonry__item col-sm-4 col-xs-6')]")))
            {
                Debugger.Log($"Trwa pobieranie danych dla przedmiotu {counter + 1} z {amount}");
                System.Threading.Thread.Sleep(1000);

                //// Handle "scrolling" through window ////
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript($"arguments[0].scrollIntoView(true);", item);

                //// New instance of Products ////
                Products product = new Products();
                product.Index = counter;

                //// Create new action to perform scrolling down to element ////
                Actions element = new Actions(_driver);
                element.MoveToElement(item).Perform();

                //// Download image ////
                var uri = item.FindElement(By.XPath(".//img[@class='prefill-img']")).GetAttribute("src");
                DownloadImage(uri, $"{counter}_temp");
                product.Image = $"\\temp\\{counter}_temp.png";

                //// Name ////
                string name = item.FindElement(By.XPath(".//h2[@class='name item-details item-details-title']")).Text;
                product.Name = name;

                //// Open product ////
                Actions actions = new Actions(_driver);
                actions.MoveToElement(item).Click().Perform();

                //// Wait to load window to be visible ////
                System.Threading.Thread.Sleep(500);

                var elements = _driver.FindElement(By.XPath("//ul[@class='sc-tabs']"));
                var items = elements.FindElements(By.TagName("li"));

                foreach(var productInfo in items)
                {
                    if (productInfo.FindElement(By.XPath(".//h2[@class='details-release-small-box']")).Text == "Colors")
                    {
                        productInfo.Click();
                    }
                }

                var el = _driver.FindElement(By.XPath("//div[@class='itemdetails-centered itemdetails-labels']"));
                var colors = el.FindElements(By.XPath("//span[@class='label label--inline price-label']"));

                List<string> col = new List<string>();

                foreach (var color in colors)
                {
                    if(!String.IsNullOrEmpty(color.Text))
                    {
                        col.Add(color.Text);
                    }
                }

                product.Colors = col;

                _driver.FindElement(By.XPath("//button[@class='close']")).Click();
                products.Add(product);
                counter++;
            }

            UpdateJson(products);
            Debugger.Log("Program zakonczyl aktualizacje - zresetuj aplikacje");
            DialogResult status = MessageBox.Show("Program zakonczyl aktualizacje - zresetuj aplikacje", "Komunikat", MessageBoxButtons.OK);
            if(status == DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}
