using Microsoft.Playwright;
using TestAssignment.Pages;

namespace TestAssignment.Hooks;

[Binding]
public class Hooks
{
    private readonly DashboardPage _dashboardPage;
    private readonly IPage _page;
    private readonly ScenarioContext _scenarioContext;


    public Hooks(ScenarioContext scenarioContext, IPage page, DashboardPage dashboardPage)
    {
        _page = page;

        _dashboardPage = dashboardPage;
        _scenarioContext = scenarioContext;
    }

    [AfterScenario("@ClearProjects", Order = 100)]
    public async Task ClearProjects()
    {
        await _dashboardPage.ClearProjectsAsync();
    }

    [BeforeScenario("@ExpandBrowserPage", Order = 11)]
    public async Task ExpandBrowserPageAsync()
    {
        await _page.SetViewportSizeAsync(1500, 800);
    }
}