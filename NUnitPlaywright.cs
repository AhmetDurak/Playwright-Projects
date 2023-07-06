using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;


namespace MySecondProject;

public class NUnitPlaywright : PageTest
{
    [SetUp]
    public async Task Setup()
    {
        await Page.GotoAsync("http://www.eaapp.somee.com");
    }

    [Test]
    public async Task Test2()
    {
        await Page.ClickAsync("text=login");
        await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "EAApp.jpg" });

        await Page.FillAsync("#UserName", "admin");
        await Page.FillAsync("#Password", "password");
        await Page.ClickAsync("text=Log in");

        await Expect(Page.Locator("text='Employee Details'")).ToBeVisibleAsync();
        

    }
}