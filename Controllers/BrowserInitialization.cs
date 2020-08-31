namespace SupremeXE_UI
{
    public partial class Browser
    {
        public OpenQA.Selenium.IWebDriver driver = null;
        public static Config config = null;
        private static Browser instance;
        private OpenQA.Selenium.IWebElement element;
        private OpenQA.Selenium.Support.UI.SelectElement selectedElement;
        private System.Collections.Generic.List<string> pathway = new System.Collections.Generic.List<string>()
        {
            "https://www.supremenewyork.com/shop/all/jackets",
            "https://www.supremenewyork.com/shop/all/shirts",
            "https://www.supremenewyork.com/shop/all/tops_sweaters",
            "https://www.supremenewyork.com/shop/all/sweatshirts",
            "https://www.supremenewyork.com/shop/all/pants",
            "https://www.supremenewyork.com/shop/all/shorts",
            "https://www.supremenewyork.com/shop/all/t-shirts",
            "https://www.supremenewyork.com/shop/all/hats",
            "https://www.supremenewyork.com/shop/all/bags",
            "https://www.supremenewyork.com/shop/all/accessories",
            "https://www.supremenewyork.com/shop/all/skate",
        };
    }
}
