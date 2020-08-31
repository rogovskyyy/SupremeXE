using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading.Tasks;

namespace SupremeXE_UI
{
    public partial class Browser
    {
        private Browser() { }

        public static Browser GetInstance()
        {
            if(instance == null)
            {
                instance = new Browser();
                config = Config.GetInstance();
            }

            return instance;
        }

        public async Task OpenBrowser(bool isHidden)
        {
            await Task.Run(() =>
            {
                if (driver == null)
                {
                    Debugger.Log("Otwieranie przegladarki...");
                    try
                    {
                        if (config.BrowserPath != null)
                        {
                            var service = FirefoxDriverService.CreateDefaultService(config.BrowserPath);
                            service.HideCommandPromptWindow = true;
                            if (isHidden)
                            {
                                var option = new FirefoxOptions();
                                option.AddArgument("-headless");
                                driver = new FirefoxDriver(service, option);

                            }
                            else
                            {
                                driver = new FirefoxDriver(service);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debugger.Log($"{e}");
                        throw (e);
                    }
                }
                else
                {
                    Debugger.Log("Przegladarka jest juz otwarta.");
                }
            });
        }

        public async Task DestroyBrowser()
        {
            if(driver != null)
            {
                await Task.Run(() => driver.Close());
                driver = null;
            }
            else
            {
                Debugger.Log("Przegladarka nie jest wlaczona");
            }
        }

        public void AddRemoveItem()
        {
            if (driver.FindElements(By.XPath("//input[@class='button']")).Count > 0)
            {
                driver.FindElement(By.XPath("//input[@class='button']")).Click();
            }

            else if (driver.FindElements(By.XPath("//input[@class='button remove']")).Count > 0)
            {
                driver.FindElement(By.XPath("//input[@class='button remove']")).Click();
            }
        }

        public void Homepage()
        {
            driver.Navigate().GoToUrl("https://www.supremenewyork.com/shop/all");
        }

        public void Cart()
        {
            driver.Navigate().GoToUrl("https://www.supremenewyork.com/shop/cart");
        }

        public void Checkout()
        {
            driver.Navigate().GoToUrl("https://www.supremenewyork.com/checkout");

            // If there is no elements called like below, skip process
            var data = driver.FindElements(By.XPath("//*[@id='order_billing_name']")).Count;
            if (data == 0) { return; }

            if (!String.IsNullOrEmpty(config.FullName))
            {
                driver.FindElement(By.XPath("//*[@id='order_billing_name']")).SendKeys(config.FullName);
            }

            if (!String.IsNullOrEmpty(config.Email))
            {
                driver.FindElement(By.XPath("//*[@id='order_email']")).SendKeys(config.Email);
            }

            if (!String.IsNullOrEmpty(config.Tel))
            {
                driver.FindElement(By.XPath("//*[@id='order_tel']")).SendKeys(config.Tel);
            }

            if (!String.IsNullOrEmpty(config.Address))
            {
                driver.FindElement(By.XPath("//*[@id='bo']")).SendKeys(config.Address);
            }

            if (!String.IsNullOrEmpty(config.Address2))
            {
                driver.FindElement(By.XPath("//*[@id='oba3']")).SendKeys(config.Address2);
            }

            if (!String.IsNullOrEmpty(config.Address3))
            {
                driver.FindElement(By.XPath("//*[@id='order_billing_address_3']")).SendKeys(config.Address3);
            }

            if (!String.IsNullOrEmpty(config.City))
            {
                driver.FindElement(By.XPath("//*[@id='order_billing_city']")).SendKeys(config.City);
            }

            if (!String.IsNullOrEmpty(config.Postcode))
            {
                driver.FindElement(By.XPath("//*[@id='order_billing_zip']")).SendKeys(config.Postcode);
            }

            if(!String.IsNullOrEmpty(config.Country))
            {
                element = driver.FindElement(By.Id("order_billing_country"));
                selectedElement = new SelectElement(element);
                selectedElement.SelectByText(config.Country);
            } 
            else
            {
                element = driver.FindElement(By.Id("order_billing_country"));
                selectedElement = new SelectElement(element);
                selectedElement.SelectByText("POLAND");
            }

            element = driver.FindElement(By.Id("credit_card_type"));
            selectedElement = new SelectElement(element);
            selectedElement.SelectByText("PayPal");

            // Removes ins attirubte which prevents from clicking on checkbox
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(@"var elements = document.getElementsByTagName('ins');
                                while (elements[0]) elements[0].parentNode.removeChild(elements[0]);");

            driver.FindElement(By.XPath("//label[@class='has-checkbox terms']")).Click();
            driver.FindElement(By.XPath("//input[@class='button checkout']")).Click();
        }
    }
}