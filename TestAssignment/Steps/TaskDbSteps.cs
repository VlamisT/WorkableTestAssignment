using Microsoft.Playwright;
using TechTalk.SpecFlow.UnitTestProvider;
using TestAssignment.Pages;

namespace TestAssignment.Steps;

[Binding]
public sealed class TaskDbSteps
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

    public TaskDbSteps(IPage page, ScenarioContext scenarioContext, DashboardPage dashboardPage, LoginPage loginPage,
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


    [StepDefinition(@"the user checks TaskDB")]
    public async Task UserCheckTaskDb()
    {
        await _taskDBPage.ClickTaskDbTAbAsync();
    }

    [StepDefinition(@"all tasks for the user's projects should be displayed")]
    public async Task AssertTasksAreDisplayed()
    {
        await _taskDBPage.VerifyTasksAreDisplayedAsync();
    }

    [StepDefinition(@"the user performs a search for a specific task")]
    public async Task UserPerformsSearch()
    {
        await _taskDBPage.SearchTaskAsync();
    }

    [StepDefinition(@"the result should match the searched task")]
    public async Task AssertSearchTaskResults()
    {
        await _taskDBPage.VerifySearchTaskAsync();
    }

    [StepDefinition(@"the user sorts tasks based on different criteria")]
    public async Task SortTasks()
    {
        await _taskDBPage.SortTasksAsync();
    }

    [StepDefinition(@"the tasks should be displayed in the correct order")]
    public async Task AssertSortFunctionality()
    {
        await _taskDBPage.VerifySort();
    }
}