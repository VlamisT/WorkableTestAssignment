using Microsoft.Playwright;

namespace TestAssignment.Pages;

public class LoginPage : BasePage
{
    public LoginPage(IPage page, ScenarioContext scenarioContext) : base(page, scenarioContext)
    {
    }

    public ILocator LoginEmail => _page.Locator("[id='email']");
    public ILocator LoginPassword => _page.Locator("[id='password']");
    public ILocator LoginButton => _page.Locator("button:has-text('Login')");

    public async Task AssertLoginFormOpens()
    {
        await Assertions.Expect(LoginEmail).ToBeVisibleAsync();
        await Assertions.Expect(LoginPassword).ToBeVisibleAsync();
        await Assertions.Expect(LoginButton).ToBeVisibleAsync();
    }
}