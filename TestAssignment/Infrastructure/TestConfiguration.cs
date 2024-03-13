namespace TestAssignment.Infrastructure;

public static class TestConfiguration
{
    public static BrowserConfig BrowserConfig { get; set; } = new();
    public static ApplicationConfig ApplicationConfig { get; set; }
}

public class BrowserConfig
{
    public BrowserTypeEnum BrowserType { get; set; }
    public bool HeadlessMode { get; set; }
}

public class ApplicationConfig
{
    public string Url { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}