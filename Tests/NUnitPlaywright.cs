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
        await Page.GotoAsync("http://www.eaapp.somee.com", new PageGotoOptions{
            WaitUntil = WaitUntilState.NetworkIdle
        });
    }

    [Test]
    public async Task Test2()
    {
        Page.SetDefaultTimeout(20000);
        var lnkLogin = Page.Locator("text=login");
        await lnkLogin.ClickAsync();
        //await Page.ClickAsync("text=login");
        await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "EAApp.jpg" });

        await Page.FillAsync("#UserName", "admin");
        await Page.FillAsync("#Password", "password");

        var btnLogin = Page.Locator("input", new PageLocatorOptions{HasTextString = "Log in"});
        await btnLogin.ClickAsync();
        //await Page.ClickAsync("text=Log in");

        await Expect(Page.Locator("text='Employee Details'")).ToBeVisibleAsync();
        

    }
}