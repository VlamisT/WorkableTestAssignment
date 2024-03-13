using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TestAssignment.Infrastructure;

namespace TestAssignment.Pages;

public class TaskDBPage : BasePage
{
    private readonly string _taskDbUrl = "/tasks/db";

    public TaskDBPage(IPage page, ScenarioContext scenarioContext) : base(page, scenarioContext)
    {
    }

    private ILocator SearchBar => _page.Locator("[id='search']");
    private ILocator TaskCard => _page.Locator("[class*=card-content]");
    private ILocator TaskCardTitle => _page.Locator("[id='card_title']");
    private ILocator SortButton => _page.Locator("[id='sort_area']");
    private ILocator TaskDbTab => _page.Locator("[id='task_db']");


    public async Task ClickTaskDbTAbAsync()
    {
        await TaskDbTab.ClickAsync();
        await _page.WaitForURLAsync(new Regex($".*{_taskDbUrl}.*"));
        await Assertions.Expect(SortButton).ToBeVisibleAsync();
    }

    public async Task VerifyTasksAreDisplayedAsync()
    {
        await _page.WaitForSelectorAsync("[class*='card-content']");
        var retrievedTexts = await GetTaskTitlesText();
        var taskSummaries = (List<string>)_scenarioContext[CommonLabels.TasksList];
        foreach (var summary in taskSummaries)
            if (!retrievedTexts.Contains(summary))
                throw new Exception($"Task summary '{summary}' not found in the retrieved texts.");
    }

    private async Task<List<string>> GetTaskTitlesText()
    {
        var cardTitleElements = await _page.QuerySelectorAllAsync("[id='card_title']");

        var retrievedTexts = new List<string>();

        foreach (var element in cardTitleElements)
        {
            var text = (await element.TextContentAsync())!;
            retrievedTexts.Add(text!);
        }

        return retrievedTexts;
    }

    public async Task SearchTaskAsync()
    {
        var taskSummaries = (List<string>)_scenarioContext[CommonLabels.TasksList];
        var random = new Random();
        var randomIndex = random.Next(0, taskSummaries.Count);
        var randomTaskSummary = taskSummaries[randomIndex];
        var searchTask = randomTaskSummary.Split(' ')[0].Trim();
        await FillSearchBarAsync(searchTask);
        await TaskCard.WaitForAllAsync();
        _scenarioContext.Add(CommonLabels.SearchedTask, searchTask);
    }

    public async Task FillSearchBarAsync(string searchTask)
    {
        await SearchBar.FillAsync(searchTask);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await ClickTaskDbTAbAsync();
    }

    public async Task VerifySearchTaskAsync()
    {
        var searchedTask = _scenarioContext[CommonLabels.SearchedTask].ToString();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await TaskCard.WaitForAllAsync();
        var tasksCount = await TaskCard.CountAsync();
        tasksCount.Should().BeGreaterThan(0);

        await Task.Delay(1000); // Add a delay to wait for elements to be fully loaded

        var searchResults = await _page.QuerySelectorAllAsync("[id='card_title']");
        foreach (var searchResult in searchResults)
        {
            var elementText = await searchResult.TextContentAsync();

            if (!elementText!.Contains(searchedTask!))
                throw new Exception(
                    $"Assertion failed: '{searchedTask}' is not found in the element text: '{elementText}'");
        }
    }

    public async Task SortTasksAsync()
    {
        await SearchBar.ClearAsync();
        await _page.ReloadAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await TaskCardTitle.WaitForAllAsync();
        await SortButton.ClickAsync();
    }

    public async Task VerifySort()
    {
        // Get text from the first and last elements with id="card_title"

        var firstCardTitleText = await TaskCardTitle.Nth(0).TextContentAsync();
        var tasksCount = await TaskCardTitle.CountAsync();
        var lastCardTitleText = await TaskCardTitle.Nth(tasksCount - 1).TextContentAsync();
        var circleIcon = await _page.QuerySelectorAsync(".material-icons.right");
        var isDescending = await circleIcon.InnerTextAsync() == "arrow_circle_down";
        // Get the text again after sorting
        var sortedFirstCardTitleText = await TaskCardTitle.Nth(0).TextContentAsync();
        var sortedLastCardTitleText = await TaskCardTitle.Nth(tasksCount - 1).TextContentAsync();

        // Assert that the sort order is correct
        if (isDescending)
        {
            // If descending, the first card title after sorting should be greater than or equal to the original first card title
            if (string.Compare(sortedFirstCardTitleText, firstCardTitleText, StringComparison.Ordinal) < 0)
                throw new Exception("Sort order verification failed: Descending order is incorrect.");
        }
        else
        {
            // If ascending, the first card title after sorting should be less than or equal to the original first card title
            if (string.Compare(sortedFirstCardTitleText, firstCardTitleText, StringComparison.Ordinal) > 0)
                throw new Exception("Sort order verification failed: Ascending order is incorrect.");
        }
    }
}