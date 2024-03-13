using Microsoft.Playwright;

namespace TestAssignment.Infrastructure;

public static class LocatorExtensions
{
    public static async Task<IReadOnlyList<ILocator>> WaitForAllAsync(this ILocator locator)
    {
        await locator.First.WaitForAsync();
        return await locator.AllAsync();
    }

    public static ILocator GetParentElement(this ILocator locator)
    {
        return locator.Locator("..");
    }
}