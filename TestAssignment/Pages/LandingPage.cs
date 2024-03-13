using Microsoft.Playwright;

namespace TestAssignment.Pages;

public class LandingPage : BasePage
{
    public LandingPage(IPage page, ScenarioContext scenarioContext) : base(page, scenarioContext)
    {
    }

    public ILocator WelcomeCard => _page.Locator("[id='welcome_card']");
    public ILocator LoginTab => _page.Locator("[id='login']");
    public ILocator SignUpTab => _page.Locator("[id='signup']");


    public async Task AssertLandingPageOpens()
    {
        await Assertions.Expect(WelcomeCard).ToBeVisibleAsync();
    }
}