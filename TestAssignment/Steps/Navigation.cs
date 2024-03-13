using FluentAssertions;
using Microsoft.Playwright;
using TechTalk.SpecFlow.UnitTestProvider;
using TestAssignment.Infrastructure;
using TestAssignment.Pages;

namespace TestAssignment.Steps;

[Binding]
public sealed class Navigation
{
    private readonly DashboardPage _dashboardPage;
    private readonly LandingPage _landingPage;
    private readonly LoginPage _loginPage;
    private readonly IPage _page;
    private readonly RegistrationPage _registrationPage;
    private readonly ScenarioContext _scenarioContext;
    private readonly SettingsPage _settingsPage;
    private readonly TaskDBPage _taskDBPage;
    private readonly IUnitTestRuntimeProvider _unitTestRuntimeProvider;

    public Navigation(IPage page, ScenarioContext scenarioContext, DashboardPage dashboardPage, LoginPage loginPage,
        RegistrationPage registrationPage, SettingsPage settingsPage, TaskDBPage taskDBPage, LandingPage landingPage,
        IUnitTestRuntimeProvider unitTestRuntimeProvider)
    {
        _page = page;
        _scenarioContext = scenarioContext;
        _dashboardPage = dashboardPage;
        _loginPage = loginPage;
        _registrationPage = registrationPage;
        _settingsPage = settingsPage;
        _taskDBPage = taskDBPage;
        _landingPage = landingPage;
        _unitTestRuntimeProvider = unitTestRuntimeProvider;
    }


    [StepDefinition("a user explores the PM Tool Application")]
    public async Task NavigateToApplicationAsync()
    {
        var site = TestConfiguration.ApplicationConfig.Url;
        await _page.GotoAsync(site);
        await _page.WaitForLoadStateAsync();
        var actualUrl = _page.Url;
        actualUrl.Should().Contain("pm-tool");
        await _landingPage.AssertLandingPageOpens();
    }


    [StepDefinition("the user is on the registration page")]
    public async Task GivenTheUserIsOnTheRegistrationPage()
    {
        await ClickOnRegistrationAsync();
        _registrationPage.AssertRegistrationFormOpens();
    }

    private async Task ClickOnRegistrationAsync()
    {
        await _landingPage.SignUpTab.ClickAsync();
    }
}