using Microsoft.Playwright;

namespace TestAssignment.Pages;

public class BasePage
{
    protected readonly IPage _page;
    protected readonly ScenarioContext _scenarioContext;

    public BasePage(IPage page, ScenarioContext scenarioContext)
    {
        _page = page;
        _scenarioContext = scenarioContext;
    }   
}