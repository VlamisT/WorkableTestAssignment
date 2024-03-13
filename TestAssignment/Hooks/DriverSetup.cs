using BoDi;
using Microsoft.Playwright;
using TechTalk.SpecFlow.Infrastructure;

namespace TestAssignment.Hooks;

[Binding]
public class DriverSetup
{
    private readonly IBrowser _browser;
    private readonly IObjectContainer _objectContainer;
    private readonly IPlaywright _playwright;
    private readonly ScenarioContext _scenarioContext;
    private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
    private IBrowserContext _browserContext = null!;
    private IPage _page = null!;

    public DriverSetup(IPlaywright playwright, IBrowser browser, ScenarioContext scenarioContext,
        IObjectContainer objectContainer, ISpecFlowOutputHelper specFlowOutputHelper)
    {
        _playwright = playwright;
        _browser = browser;
        _scenarioContext = scenarioContext;
        _objectContainer = objectContainer;
        _specFlowOutputHelper = specFlowOutputHelper;
    }

    [BeforeScenario(Order = 2)]
    public async Task CreateDefaultBrowserContextAndPagePerScenario()
    {
        _browserContext = await _browser.NewContextAsync();
        _page = await _browserContext.NewPageAsync();

        _page.SetDefaultTimeout(32 * 1000);
        _objectContainer.RegisterInstanceAs(_browserContext);
        _objectContainer.RegisterInstanceAs(_page);
    }


    [AfterScenario(Order = 100000)]
    public async Task AfterScenario()
    {
        await _page.CloseAsync();
        await _browserContext.CloseAsync();
    }
}