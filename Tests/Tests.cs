using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
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

        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://www.eaapp.somee.com");

        var loginPage = new LoginPageUpgraded(page);
        await loginPage.ClickLogin();
        await loginPage.Login("admin", "password");
        var isExist = await loginPage.isEmployeeDetailsExists();
        Assert.IsTrue(isExist);
    }
    [Test]
    public async Task TestNetwork()
    {
        //Playwright
        var pw = await Playwright.CreateAsync();
        //Browser
        await using var browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://www.eaapp.somee.com");

        var loginPage = new LoginPageUpgraded(page);
        await loginPage.ClickLogin();
        await loginPage.Login("admin", "password");

        //var getResponse = page.WaitForResponseAsync("**/Employee");
        //await loginPage.ClickEmployeeList();

        var Response = await page.RunAndWaitForResponseAsync(async () =>
        {
            await loginPage.ClickEmployeeList();
        }, x => x.Url.Contains("/Employee") && x.Status == 200);

        var isExist = await loginPage.isEmployeeDetailsExists();
        Assert.IsTrue(isExist);
    }
    [Test]
    public async Task Flipkart()
    {
        //Playwright
        var pw = await Playwright.CreateAsync();
        //Browser
        await using var browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        var context = await browser.NewContextAsync();
        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://www.flipkart.com/", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });

        await page.Locator("text=✕").ClickAsync();
        await page.Locator("a", new PageLocatorOptions
        {
            HasTextString = "Login"
        }).ClickAsync();

        var request = await page.RunAndWaitForRequestAsync(async () =>
        {
            await page.Locator("text=✕").ClickAsync();
        }, x => x.Url.Contains("flipkart.d1.sc.omtrdc.net") && x.Method == "GET");

        var returnData = HttpUtility.UrlDecode(request.Url);
        returnData.Should().Contain("Account Login:Displayed Exit");
    }
    [Test]
    public async Task TestIntercept(){
        var pw = await Playwright.CreateAsync();
        var browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions{Headless = false
        });
        await browser.NewContextAsync();
        var page = await browser.NewPageAsync();
        await page.GotoAsync("");
    }
}