using BoDi;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using TestAssignment.Infrastructure;

namespace TestAssignment.Hooks;

[Binding]
public static class TestSuiteSetup
{
    private static IObjectContainer _objectContainer;
    private static IBrowser _browser;
    private static IPlaywright _playwright;

    [BeforeTestRun(Order = 1)]
    public static void BeforeTestRun(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
        StartBrowserAsync().Wait();
    }

    private static async Task StartBrowserAsync()
    {
        _playwright = await Playwright.CreateAsync();

        var browserType = _playwright.SelectBrowserType();


        var testConfig = new ConfigurationBuilder()
            .AddJsonFile("test_settings.json", false, true)
            .Build();


        _browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = TestConfiguration.BrowserConfig.HeadlessMode,
            Timeout = 10000,
            Args = new[] { "--start-fullscreen" }
        });

        _objectContainer.RegisterInstanceAs(_playwright);
        _objectContainer.RegisterInstanceAs(_browser);
        _objectContainer.RegisterInstanceAs((IConfiguration)testConfig);
        ConfigurationMapper.MapTestConfig(testConfig);
    }

    private static IBrowserType SelectBrowserType(this IPlaywright playwright)
    {
        return TestConfiguration.BrowserConfig.BrowserType switch
        {
            BrowserTypeEnum.Firefox => playwright.Firefox,
            BrowserTypeEnum.Webkit => playwright.Webkit,
            _ => playwright.Chromium
        };
    }

    [AfterTestRun]
    public static async Task DisposePlaywright()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }
}