A sample test suite that can be set up and run in VS by installing the following packages:  
-- .NET 6.0 
-- Microsoft.NET.Test.Sdk 17.8.0 
-- NUnit Version="3.14.0 
-- NUnit.ConsoleRunner 3.16.3 
-- NUnit3TestAdapter 4.5.0 
-- Selenium.Support 4.15.0 
-- Selenium.WebDriver 4.15.0 
-- SeleniumExtras.WaitHelpers 1.0.2 

You can find the test cases and steps below: 

Test Suite: Progress web store sample tests

Test Case 1. Initial Order Screen - Add product to the cart and check if properly updated 
	1.1. Open https://www.telerik.com/purchase.aspx
	1.2. Select an item from the list and save the price value
	1.3. Open cart and verify the item is present and the total price is correct
	
Test Case 2. Initial Order Screen - Remove product from the cart and check if properly updated
	2.1. Open https://www.telerik.com/purchase.aspx
	2.2. Select an item from the list and save the price value 
	2.3. Back in the item list, select a sedcond item and save its price value 
	2.4. Go to the cart find and click on the "Remove Item" link 
	2.5. Verify if the item is feleted and whether the price value has changed accordingly
	
Test Case 3. Initial Order Screen - Change item quantity and check price and discount update 
	3.1. Open https://www.telerik.com/purchase.aspx
	3.2. Select an item from the list and save the price value
	3.3. From the item quantity dropdown, select an option (note the discount percentage)
	3.4. Check the total price for the product after change; Verify if it matches calculations;
	
Test Case 4. Initial Order Screen - Change the support duration and check price and discount update 
	4.1. Open https://www.telerik.com/purchase.aspx 
	4.2. Select an item from the list and save the price value
	4.3. From the support duration dropdown -> select an option 
	4.4. Locate and save the support price (check if discount is pre-calculated)
	4.5. Check the total price for the product after change; Verify if it matches calculations;
	
Test Case 5. Contact info page - Missing mandatory field warning 
	5.1. Open https://www.telerik.com/purchase.aspx
	5.2. Select an item from the list
	5.3. Continue to the contact information screen
	5.4. Fill the contact information with valid data
	5.5. Remove a mandatory field (e.g. last name) 
	5.6. Veriy whether the error message is displayed properly
	
Test Case 6. Contact info page - Attempt to use invalid/valid VAT
	6.1. Open https://www.telerik.com/purchase.aspx
	6.2. Select an item from the list
	6.3. Continue to the contact information screen
	6.4. Fill the contact information with valid data
	6.5. Select Bulgaria from the country dropdown 
	6.6. When the VAT field is present: 
	6.6.1.1. Enter invalid BG VAT ID
	6.6.1.2. Check whether the error message is displayed properly 
	6.6.1.3. Check whether the continue button is active 
	6.6.2.1. Enter valid BG VAT ID
	6.6.2.2. Check whether the continue button is active
	6.6.2.3.  Click to continue 
	
Test Case 7. Contact info page - Edit contact info and check if updated properly in Review Order page 
	7.1. Open https://www.telerik.com/purchase.aspx
	7.2. Select an item from the list
	7.3. Continue to the contact information screen
	7.4. Fill the contact information with valid data
	7.5. Continue to the review order screen and note the displayed values
	7.6. Find and click on the edit button
	7.7. Back on the contact info screen change aa field (e.g. company)
	7.8. Continue to the review order screen and note the displayed values
	7.9. Check if the updated value is displayed properly 
	
Test Case 8. Contact info page - Add License Holder Information
	8.1. Open https://www.telerik.com/purchase.aspx
	8.2. Select an item from the list
	8.3. Continue to the contact information screen
	8.4. Fill the contact information with valid data
	8.5. Click on the license holder checkbox 
	8.6. Once displayed, the values should be the same as the contact information 
	8.7. Change some of the matching values 
	8.8. Go to the review order screen and see if the changes are displayed properly 
	
Test Case 9. Review Order page - Attempt to continue to payment without accepting TaC
	9.1. Open https://www.telerik.com/purchase.aspx
	9.2. Select an item from the list
	9.3. Continue to the contact information screen
	9.4. Fill the contact information with valid data
	9.5. On the review order screen attempt to continue to payment - Button should be inactive and T&C highlighted in error
	9.6. Click the T&C checkbox to accept 
	9.7. Verify if the button is active and click to continue if clickable
	
Test Case 10.Apply incorrect coupon code and check error message
	10.1. Open https://www.telerik.com/purchase.aspx
	10.2. Select an item from the list
	10.3. Locate and click on the coupon link 
	10.4. Enter invalid code in the text field (now visible)
	10.5. Click on the Apply button 
	10.6. Check whether the error message is displayed properly and the button is active