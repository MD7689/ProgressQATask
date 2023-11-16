using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ProgressStoreTests.Common
{
    [TestFixture]
    public class WebTestSuite
    {
        WebDriver driver;
        
        [SetUp]
        public void DriverSetup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TestCase(TestName = "Initial Order Screen - Add product to the cart and check if properly updated")]
        public void AddItems()
        {
            //Go to item list and get price of item to select
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement Listedprice = (WebElement)driver.FindElement(By.XPath("//*[@id=\"ContentPlaceholder1_C691_Col00\"]/div/div/div[1]/table/thead/tr[4]/th[2]/div/h3/span[2]"));
            string listed = Listedprice.GetAttribute("innerHTML").Replace(",", "");
            //Click to buy
            WebElement BuyProduct = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=6127']"));
            BuyProduct.Click();
            //Wait for order page and parse strings to numbers
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class=\"u-fr e2e-total-price\"]")));
            WebElement TotalPrice = (WebElement)driver.FindElement(By.XPath("//*[@class=\"u-fr e2e-total-price\"]"));
            string price = TotalPrice.GetAttribute("innerHTML").Substring(4).Replace(",", "");
            double d1 = double.Parse(price.Replace(".", ","));
            double d2 = double.Parse(listed);
            // Verify if prices match
            Console.WriteLine("Listed Price = " + d2 + "\n Cart Price = " + d1);
            Assert.IsTrue(d1==d2, "Prices do not match");
        }

        [TestCase(TestName = "Initial Order Screen - Remove product from the cart and check if price is updated")]
        public void RemoveItems()
        {
            //Go to item list and get price of first added item
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement Listedprice = (WebElement)driver.FindElement(By.XPath("//*[@id=\"ContentPlaceholder1_C691_Col00\"]/div/div/div[1]/table/thead/tr[4]/th[2]/div/h3/span[2]"));
            string listed1 = Listedprice.GetAttribute("innerHTML").Replace(",", "");
            WebElement BuyProduct = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=6127']"));
            BuyProduct.Click();
            Thread.Sleep(3000);
            //Accept cookies
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("onetrust-accept-btn-handler"))).Click();
            //Go back to get price for second added item
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement Listedprice2 = (WebElement)driver.FindElement(By.XPath("//*[@id=\"ContentPlaceholder1_C691_Col00\"]/div/div/div[1]/table/thead/tr[4]/th[1]/div/h3/span[2]"));
            string listed2 = Listedprice2.GetAttribute("innerHTML").Replace(",", "");
            WebElement BuyProduct2 = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=801']"));
            BuyProduct2.Click();
            Thread.Sleep(3000);
            //Get total price of all items
            WebElement TotalPrice1 = (WebElement)driver.FindElement(By.XPath("//*[@class=\"u-fr e2e-total-price\"]"));
            string price1 = TotalPrice1.GetAttribute("innerHTML").Substring(4).Replace(",", "");
            Console.WriteLine("Total price pre-removal: " + price1);
            //Remove second product from cart and re-calculate total price
            WebElement RemoveLink = (WebElement)driver.FindElement(By.XPath("//*[@id=\"801\"]/td[1]/div[2]/a[2]"));
            RemoveLink.Click();
            Thread.Sleep(3000);
            WebElement TotalPrice2 = (WebElement)driver.FindElement(By.XPath("//*[@class=\"u-fr e2e-total-price\"]"));
            string price2 = TotalPrice2.GetAttribute("innerHTML").Substring(4).Replace(",", "");
            Console.WriteLine("Total price post-removal: " + price2);
            Assert.IsTrue(double.Parse(price2.Replace(".", ",")) == double.Parse(listed1.Replace(".", ",")), "Prices not matching after removal");
        }

        [TestCase(TestName = "Initial Order Screen - Change item quantity and check price and discount update ")]
        public void ItemQuantityChange()
        {
            //Go to item list and get price of item to select
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement Listedprice = (WebElement)driver.FindElement(By.XPath("//*[@id=\"ContentPlaceholder1_C691_Col00\"]/div/div/div[1]/table/thead/tr[4]/th[2]/div/h3/span[2]"));
            string listed = Listedprice.GetAttribute("innerHTML").Replace(",", "");
             //Click to buy
            WebElement BuyProduct = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=6127']"));
            BuyProduct.Click();
            Thread.Sleep(3000);
            //Accept cookies
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("onetrust-accept-btn-handler"))).Click();
            //Select option 3 from the dropdown
            WebElement QtyDD = (WebElement)driver.FindElement(By.XPath("//*[@id=\"6127\"]/td[2]/div/div[2]/div[2]/quantity-select/div/kendo-dropdownlist"));
            QtyDD.Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//kendo-list/div/ul/li[3]")).Click();
            Thread.Sleep(5000);
            //Calculate 3*times listed price minus discount and compare prices
            double priceQTY = ((double.Parse(listed)*3)*95)/100;
            Console.WriteLine("Price for selected quantity:" + priceQTY.ToString());
            //Get total price after update and compare the two values
            WebElement TotalPrice = (WebElement)driver.FindElement(By.XPath("//*[@class=\"u-fr e2e-total-price\"]"));
            string totalprice = TotalPrice.GetAttribute("innerHTML").Substring(4).Replace(",", "");
            Console.WriteLine("Total price:" + totalprice);
            Assert.IsTrue(double.Parse(totalprice.Replace(".", ",")) == priceQTY, "Prices not matcing" );
        }

        [TestCase(TestName = "Initial Order Screen - Change the support duration and check price and discount update")]
        public void ItemSupportChange()
        {
            //Go to item list and get price of item to select
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement Listedprice = (WebElement)driver.FindElement(By.XPath("//*[@id=\"ContentPlaceholder1_C691_Col00\"]/div/div/div[1]/table/thead/tr[4]/th[2]/div/h3/span[2]"));
            string listed = Listedprice.GetAttribute("innerHTML").Replace(",", "");
            //Click to buy
            WebElement BuyProduct = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=6127']"));
            BuyProduct.Click();            
            Thread.Sleep(3000);
            //Accept cookies
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("onetrust-accept-btn-handler"))).Click();
            //Select 2 extra support years from the dropdown and get the added value from screen
            WebElement SuppDD = (WebElement)driver.FindElement(By.XPath("//*[@id=\"6127\"]/td[4]/div/div[2]/period-select/div/kendo-dropdownlist"));
            SuppDD.Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//kendo-list/div/ul/li[3]")).Click();
            Thread.Sleep(3000);
            WebElement SuppPrice = (WebElement)driver.FindElement(By.XPath("//*[@id=\"6127\"]/td[4]/div/div[1]/div[2]/span/span"));
            string supportprice = SuppPrice.GetAttribute("innerHTML").Substring(1).Replace(".", ",");
            Console.WriteLine("Support price per year - discount:" + supportprice);
            //Check total vs price + support - discount
            WebElement TotalPrice = (WebElement)driver.FindElement(By.XPath("//*[@class=\"u-fr e2e-total-price\"]"));
            string totalprice = TotalPrice.GetAttribute("innerHTML").Substring(4).Replace(",", "");
            Console.WriteLine("Total price:" + totalprice);
            Assert.IsTrue(double.Parse(totalprice.Replace(".", ",")) == ((2*double.Parse(supportprice)) + double.Parse(listed)), "Price + support not matching total");
        }
        
        [TestCase(TestName = "Contact info page - Missing mandatory field warning")]
        public void CIMandatoryFieldsCheck()
        {
            //Add item to cart and navigate to contact page
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement BuyProduct = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=6127']"));
            BuyProduct.Click();
            WebElement ContOrder = (WebElement)driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/your-order/div/div/div/div[5]/button/span[2]"));
            ContOrder.Click();
            //Accept cookies
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("onetrust-accept-btn-handler"))).Click();
            //Populate fields, delete last name
            PopulateContactData();
            driver.FindElement(By.XPath("//li[27]")).Click();
            WebElement LName = (WebElement)driver.FindElement(By.Id("biLastName"));
            LName.Clear();
            LName.SendKeys(" ");
            driver.FindElement(By.Id("biFirstName")).Click();
            //Check if warning message is displayed
            Assert.That(driver.FindElement(By.XPath("//span[contains(.,'Last name is required')]")).Displayed);
        }

        [TestCase(TestName = "Contact info page - Attempt to use invalid/valid VAT")]
        public void CheckVAT()
        {
            //Add item to cart and navigate to contact page
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement BuyProduct = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=6127']"));
            BuyProduct.Click();
            WebElement ContOrder = (WebElement)driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/your-order/div/div/div/div[5]/button/span[2]"));
            ContOrder.Click();
            //Accept cookies
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("onetrust-accept-btn-handler"))).Click();
            //Populate Fields, enter incorrect VAT
            PopulateContactData();
            driver.FindElement(By.XPath("//li[32]")).Click();
            Thread.Sleep(3000);
            WebElement VATID = (WebElement)driver.FindElement(By.Id("biCountryTaxIdentificationNumber"));
            VATID.SendKeys("ZZ160026043");
            //Check if error is returned, fix VAT and continue order
            Thread.Sleep(3000);
            Assert.That(driver.FindElement(By.XPath("//span[contains(.,'Invalid VAT ID')]")).Displayed);
            VATID.Clear();
            VATID.SendKeys("BG160026043");
            Thread.Sleep(5000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div[3]/button")));
            driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div[3]/button")).Click();
            Thread.Sleep(5000);
        }

        [TestCase(TestName = "Contact info page - Edit contact info and check if updated properly in Review Order page ")]
        public void ContactInfoEdit()
        {
            //Add item to cart and navigate to contact page
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement BuyProduct = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=6127']"));
            BuyProduct.Click();
            WebElement ContOrder = (WebElement)driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/your-order/div/div/div/div[5]/button/span[2]"));
            ContOrder.Click();
            //Accept cookies
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("onetrust-accept-btn-handler"))).Click();
            //Populate fields and go to review order
            PopulateContactData();
            driver.FindElement(By.XPath("//li[32]")).Click();
            WebElement VATID = (WebElement)driver.FindElement(By.Id("biCountryTaxIdentificationNumber"));
            VATID.SendKeys("BG160026043");
            Thread.Sleep(3000);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div[3]/button")));
            driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div[3]/button")).Click();
            Thread.Sleep(5000);
            //Click edit button and change  value in the Company textbox
            driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div/div[5]/div/div[2]/a")).Click();
            Thread.Sleep(5000);
            WebElement Company = (WebElement)driver.FindElement(By.Id("biCompany"));
            Company.Clear();
            Company.SendKeys("TodorCorp LTD");
            var textboxname = Company.GetAttribute("value");
            //Go to review order and verify if data is changed
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div[3]/button")));
            driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div[3]/button")).Click();
            string reviewedname = driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div/div[5]/div/div[1]/payment-information/div/div[1]/div[3]/div[2]")).GetAttribute("innerHTML");
            Console.WriteLine(textboxname.Trim() + " " + reviewedname.Trim());
            Assert.IsTrue(textboxname.Trim() == reviewedname.Trim());
        }

        [TestCase(TestName = "Contact info page - Add alternative License Holder Information and verify if displayed")]
        public void LicenseHolderInfoAdd()
        {
            //Add item to cart and navigate to contact page
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement BuyProduct = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=6127']"));
            BuyProduct.Click();
            WebElement ContOrder = (WebElement)driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/your-order/div/div/div/div[5]/button/span[2]"));
            ContOrder.Click();
            //Accept cookies
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("onetrust-accept-btn-handler"))).Click();
            //Populate contact fields
            PopulateContactData();
            driver.FindElement(By.XPath("//li[27]")).Click();
            Thread.Sleep(3000);
            //Change license holder information and continue
            WebElement LicenseHolderBox = (WebElement)driver.FindElement(By.Id("siSameAsBilling"));
            LicenseHolderBox.Click();
            WebElement LicenseHolderComp = (WebElement)driver.FindElement(By.Id("siCompany"));
            LicenseHolderComp.Clear();
            LicenseHolderComp.SendKeys("Dimo Corp");
            WebElement ContButton = (WebElement)driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div[3]/button"));
            ContButton.Click(); 
            //Check displayed value on review page
            Thread.Sleep(3000);
            string displayedLHName = driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div/div[5]/div/div[1]/payment-information/div/div[2]/div[3]/div[2]")).GetAttribute("innerHTML");
            Console.WriteLine(displayedLHName.Trim());
            Assert.AreEqual("Dimo Corp", displayedLHName.Trim());
        }

        [TestCase(TestName = "Review Order page - Attempt to continue to payment without accepting Terms an Conditions")]
        public void TermCondCheck()
        {
            //Add item to cart and navigate to contact page
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement BuyProduct = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=6127']"));
            BuyProduct.Click();
            WebElement ContOrder = (WebElement)driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/your-order/div/div/div/div[5]/button/span[2]"));
            ContOrder.Click();
            //Accept cookies
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("onetrust-accept-btn-handler"))).Click();
            //Populate fields, delete last name
            PopulateContactData();
            driver.FindElement(By.XPath("//li[27]")).Click();
            driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div[3]/button")).Click();
            Thread.Sleep(3000);
            WebElement CompleteButton = (WebElement)driver.FindElement(By.XPath("//*[@id=\"shopping-cart-app-content\"]/ng-component/div/div/div/div[8]/div/div/div/form/button"));
            CompleteButton.Click();
            Thread.Sleep(3000);
            Assert.IsFalse(CompleteButton.Enabled);
            driver.FindElement(By.Id("licenseAgreementCheck")).Click();
            Assert.IsTrue(CompleteButton.Enabled);
            CompleteButton.Click();
            //CAPTCHA solution required for confirming next screen
        }

        [TestCase(TestName = "Apply incorrect coupon code and check error message")]
        public void CouponCodeErrCheck()
        {
            driver.Url = "https://www.telerik.com/purchase.aspx";
            WebElement BuyProduct = (WebElement)driver.FindElement(By.CssSelector("[href*='https://store.progress.com/configure-purchase?skuId=6127']"));
            BuyProduct.Click();
            Thread.Sleep(3000);
            //Accept cookies
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("onetrust-accept-btn-handler"))).Click();
            //Display coupon field and enter invalid coupon number
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".e2e-toogle-coupon-input"))).Click();
            WebElement CouponField = (WebElement)driver.FindElement(By.CssSelector(".coupon-input"));
            CouponField.Click();
            CouponField.SendKeys("Test");
            driver.FindElement(By.CssSelector(".coupon-button")).Click();
            Thread.Sleep(5000);
            //Verify error message is displayed properly
            string errormsg = driver.FindElement(By.XPath("//section[@id='shopping-cart-app-content']/your-order/div/div/div/div[3]/div/div[3]/div/div/div")).GetAttribute("innerHTML");
            Console.WriteLine(errormsg);
            Assert.AreEqual("Invalid Coupon Code", errormsg);
        }

        [TearDown]
        public void DriverShutDown()
        {
            driver.Quit();
        }
        public void PopulateContactData()
        {
            WebElement FName = (WebElement)driver.FindElement(By.Id("biFirstName"));
            WebElement LName = (WebElement)driver.FindElement(By.Id("biLastName"));
            WebElement Email = (WebElement)driver.FindElement(By.Id("biEmail"));
            WebElement Company = (WebElement)driver.FindElement(By.Id("biCompany"));
            WebElement Phone = (WebElement)driver.FindElement(By.Id("biPhone"));
            WebElement Address = (WebElement)driver.FindElement(By.Id("biAddress"));
            WebElement City = (WebElement)driver.FindElement(By.Id("biCity"));
            WebElement ZipCode = (WebElement)driver.FindElement(By.Id("biZipCode"));
            WebElement Country = (WebElement)driver.FindElement(By.CssSelector(".k-i-arrow-s"));

            FName.SendKeys("Toshko");
            LName.SendKeys("Todorov");
            Email.SendKeys("ttodorov@todorov.todor");
            Company.SendKeys("TodorCorp");
            Phone.SendKeys("333444555");
            Address.SendKeys("Pozitano 20");
            City.SendKeys("Sofia");
            ZipCode.SendKeys("1000");
            Country.Click();
            //Note: country selection from the list depends on the test case, as VAT is optional
        }

    }
}