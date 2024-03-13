using Microsoft.Playwright;
using TechTalk.SpecFlow.UnitTestProvider;
using TestAssignment.Infrastructure;
using TestAssignment.Pages;

namespace TestAssignment.Steps;

[Binding]
public sealed class LoginSteps
{
    private readonly IBrowser _browser;
    private readonly DashboardPage _dashboardPage;
    private readonly LandingPage _landingPage;
    private readonly LoginPage _loginPage;
    private readonly IPage _page;
    private readonly ScenarioContext _scenarioContext;
    private readonly IUnitTestRuntimeProvider _unitTestRuntimeProvider;

    public LoginSteps(IPage page, IUnitTestRuntimeProvider unitTestRuntimeProvider,
        ScenarioContext scenarioContext, IBrowser iBrowser, LoginPage loginPage, LandingPage landingPage,
        DashboardPage dashboardPage)
    {
        _page = page;
        _unitTestRuntimeProvider = unitTestRuntimeProvider;
        _scenarioContext = scenarioContext;
        _browser = iBrowser;
        _loginPage = loginPage;
        _landingPage = landingPage;
        _dashboardPage = dashboardPage;
    }

    [StepDefinition("a verified user logs in")]
    public async Task VerifiedUserLogsIn()
    {
        await ClickOnLoginTabAsync();
        await _loginPage.AssertLoginFormOpens();
        await FillCredentials();
        await ClickOnLoginButtonAsync();
        await _dashboardPage.VerifyLogin();
    }

    private async Task ClickOnLoginTabAsync()
    {
        await _landingPage.LoginTab.ClickAsync();
    }

    public async Task FillCredentials()
    {
        var fillEmail = TestConfiguration.ApplicationConfig.Email;
        var fillPassword = TestConfiguration.ApplicationConfig.Password;
        await _loginPage.LoginEmail.FillAsync(fillEmail);
        await _loginPage.LoginPassword.FillAsync(fillPassword);
    }

    private async Task ClickOnLoginButtonAsync()
    {
        await _page.RunAndWaitForResponseAsync(
            async () => { await _loginPage.LoginButton.ClickAsync(); },
            res => res.Ok);
        await _page.WaitForLoadStateAsync();
    }
}