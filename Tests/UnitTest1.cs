using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;


namespace MySecondProject;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestBasic()
    {
        //Playwright
        var pw = await Playwright.CreateAsync();
        //Browser
        await using var browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        //Page
        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://www.eaapp.somee.com");
        await page.ClickAsync("text=login");
        await page.ScreenshotAsync(new PageScreenshotOptions { Path = "EAApp.jpg" });

        await page.FillAsync("#UserName", "admin");
        await page.FillAsync("#Password", "password");
        await page.ClickAsync("text=Log in");
        var isExist = await page.Locator("text='Employee Details'").IsVisibleAsync();
        Assert.IsTrue(isExist);
    }

        [Test]
    public async Task TestWithPOM()
    {
        //Playwright
        var pw = await Playwright.CreateAsync();
        //Browser
        await using var browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        //Page
        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://www.eaapp.somee.com");
        
        var loginPage = new LoginPageUpgraded(page);
        await loginPage.ClickLogin();
        await loginPage.Login("admin", "password");
        var isExist = await loginPage.isEmployeeDetailsExists();
        
        Assert.IsTrue(isExist);
    }
}