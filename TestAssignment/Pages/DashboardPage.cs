using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using NUnit.Framework;
using TestAssignment.Infrastructure;

namespace TestAssignment.Pages;

public class DashboardPage : BasePage
{
    private readonly string _createProjectUrl = "/createProject";
    private readonly string _createTaskUrl = "/createTask";
    private readonly string _dashboardUrl = "/dashboard";
    private readonly string _tasksUrl = "/tasks";
    private readonly string _updateProjectUrl = "/update";
    private readonly string _updateTaskUrl = "/update";

    public DashboardPage(IPage page, ScenarioContext scenarioContext) : base(page, scenarioContext)
    {
    }

    private ILocator LogoutTab => _page.Locator("[href='/logout']");
    private ILocator TaskDbTab => _page.Locator("[href='/task/db']");
    private ILocator SettingsTab => _page.Locator("[href='/settings']");
    private ILocator CreateProjectButton => _page.Locator("[href='/createProject']");
    private ILocator ProjectName => _page.Locator("[id='name']");
    private ILocator ProjectDescription => _page.Locator("[id='description']");
    private ILocator SubmitProject => _page.Locator("button:has-text('Create')");
    private ILocator ProjectCard => _page.Locator("[class*='card-content']");
    private ILocator TaskCard => _page.Locator("[class*='card blue'] [class*='card-content']");
    private ILocator ProjectCardTitle => _page.Locator("[class='card-title']");
    private ILocator TaskCardTitle => _page.Locator("[id='card_title']");
    private ILocator ProjectCardDescription => _page.Locator("[class*='card-content'] p");
    private ILocator AddTaskButton => _page.Locator("[id='btn_add_task']");
    private ILocator ViewTasksButton => _page.Locator("[id='btn_view_tasks']");
    private ILocator EditButton => _page.Locator("[id='btn_update_project']");
    private ILocator DeleteProjectButton => _page.Locator("[id='delete_project']");
    private ILocator DeleteTaskButton => _page.Locator("[id='btn_delete_task']");
    private ILocator UpdateProject => _page.Locator("button:has-text('Update')");
    private ILocator TaskSummary => _page.Locator("[id='summary']");
    private ILocator TaskDescription => _page.Locator("[id='description']");
    private ILocator TaskCardDescription => _page.Locator("[id='card_description']");
    private ILocator TaskStatusDropdown => _page.Locator("[class='select-wrapper']");
    private ILocator TaskLabel => _page.Locator("[id='search_input']");
    private ILocator FileUploadButton => _page.Locator("[id='attachments']");
    private ILocator DropdownListItem => _page.Locator("li[id*='select-options']");
    public ILocator DashboardTab => _page.Locator("[id='dashboard']");
    public ILocator EditTaskButton => _page.Locator("[id='btn_update_task']");
    public ILocator UpdateTaskButton => _page.Locator("button[type ='submit']");


    public string StatusOption(string? taskStatus)
    {
        return $"li[id*='select-options'] span:has-text(\"{taskStatus}\")";
    }


    public async Task VerifyLogin()
    {
        await Assertions.Expect(LogoutTab).ToBeVisibleAsync();
    }

    public async Task ClickCreateProjectButtonAsync()
    {
        await CreateProjectButton.ClickAsync();
        await _page.WaitForURLAsync(new Regex($".*{_createProjectUrl}.*"));
    }

    public async Task AddProjectDetails(string? projectName, string? projectDescription)
    {
        _scenarioContext.Add(CommonLabels.ProjectName, projectName);
        _scenarioContext.Add(CommonLabels.ProjectDescription, projectDescription);
        if (projectName != null) await ProjectName.FillAsync(projectName);
        if (projectDescription != null) await ProjectDescription.FillAsync(projectDescription);
        await SubmitProject.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task AssertProjectSuccessfullyAdded()
    {
        await Task.Delay(300);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await ProjectCard.WaitForAllAsync();
        var projectCardCount = await ProjectCard.CountAsync();
        projectCardCount.Should().BeGreaterThan(0);

        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var projectTitle = await ProjectCardTitle.Nth(0).InnerTextAsync();

        Assert.AreEqual(projectTitle, _scenarioContext[CommonLabels.ProjectName].ToString());
    }

    public async Task ClickEditProjectButton()
    {
        await EditButton.Nth(0).ClickAsync();
        await _page.WaitForURLAsync(new Regex($".*{_updateProjectUrl}.*"));
    }

    public async Task EditProjectDetails(string? updatedProjectName, string? updatedProjectDescription)
    {
        _scenarioContext.Add(CommonLabels.UpdatedProjectName, updatedProjectName);
        _scenarioContext.Add(CommonLabels.UpdatedProjectDescription, updatedProjectDescription);
        await ProjectName.ClearAsync();
        if (updatedProjectName != null) await ProjectName.FillAsync(updatedProjectName);
        await ProjectDescription.ClearAsync();
        if (updatedProjectDescription != null) await ProjectDescription.FillAsync(updatedProjectDescription);
    }

    public async Task ClickUpdateProjectAsync()
    {
        await _page.RunAndWaitForResponseAsync(
            async () => { await UpdateProject.Nth(0).ClickAsync(); },
            res => res.Ok);
    }

    public async Task VerifyProjectDetailsUpdate()
    {
        await Task.Delay(300);
        var projectCardCount = await ProjectCard.CountAsync();
        _scenarioContext.Add(CommonLabels.ProjectsCount, projectCardCount);
        projectCardCount.Should().BeGreaterThan(0);
        var actualProjectTitle = await ProjectCardTitle.Nth(0).InnerTextAsync();
        actualProjectTitle.Should().Be(_scenarioContext[CommonLabels.UpdatedProjectName].ToString(),
            "Project title should be updated.");


        var actualProjectDescription = await ProjectCardDescription.Nth(0).InnerTextAsync();
        actualProjectDescription.Should().Be(_scenarioContext[CommonLabels.UpdatedProjectDescription].ToString(),
            "Project description should be updated.");
    }

    public async Task ClearProjectsAsync()
    {
        var deleteButtons = await _page.QuerySelectorAllAsync("[id='delete_project']");

        // Start the recursive function
        await ClickDeleteButtonsRecursively(deleteButtons);
    }

    private async Task ClickDeleteButtonsRecursively(IReadOnlyList<IElementHandle> deleteButtons, int index = 0)
    {
        if (index >= deleteButtons.Count)
            // All delete buttons have been clicked
            return;

        _page.Dialog += (_, dialog) => dialog.AcceptAsync();
        await deleteButtons[index].ClickAsync();

        // Wait for the next delete button to appear in the DOM
        var nextIndex = index + 1;
        if (nextIndex < deleteButtons.Count) await _page.WaitForSelectorAsync("[id='delete_project']");

        await ClickDeleteButtonsRecursively(deleteButtons, nextIndex);
    }

    public async Task DeleteProjectAsync()
    {
        _page.Dialog += (_, dialog) => dialog.AcceptAsync();
        await _page.RunAndWaitForResponseAsync(
            async () => { await DeleteProjectButton.Nth(0).ClickAsync(); },
            res => res.Ok);
    }

    public async Task VerifyProjectDeletionAsync()
    {
        var projectCardCount = await CountProjectCardsWithoutWelcomeAsync();
        var totalProjects = (int)_scenarioContext[CommonLabels.ProjectsCount];
        Assert.AreEqual(projectCardCount, totalProjects - 1);
    }

    public async Task<int> CountProjectCardsWithoutWelcomeAsync()
    {
        var projectCards = await _page.QuerySelectorAllAsync("[class*='card-content']");

        var count = 0;

        foreach (var projectCard in projectCards)
        {
            var innerText = await projectCard.InnerTextAsync();

            if (!innerText.Contains("Welcome")) count++;
        }

        return count;
    }

    public async Task AddProjectTaskAsync(string? taskSummary, string? taskDescription, string? taskStatus)
    {
        await ClickAddTaskButton();
        await AddTaskDetails(taskSummary, taskDescription, taskStatus);
    }

    private async Task ClickAddTaskButton()
    {
        await AddTaskButton.Nth(0).ClickAsync();
        await _page.WaitForURLAsync(new Regex($".*{_createTaskUrl}.*"));
    }

    public async Task AddTaskDetails(string? taskSummary, string? taskDescription, string? taskStatus)
    {
        SetOrUpdateContextValue(CommonLabels.TaskSummary, taskSummary);
        SetOrUpdateContextValue(CommonLabels.TaskDescription, taskDescription);
        SetOrUpdateContextValue(CommonLabels.TaskStatus, taskStatus);
        await TaskSummary.FillAsync(taskSummary);
        await TaskDescription.FillAsync(taskDescription);
        await SetTaskStatusAsync(taskStatus);

        await _page.RunAndWaitForResponseAsync(
            async () => { await SubmitProject.ClickAsync(); },
            res => res.Ok);
    }

    public void SetOrUpdateContextValue(string key, string value)
    {
        if (!_scenarioContext.ContainsKey(key))
            _scenarioContext.Add(key, value);
        else
            _scenarioContext.Set(key, value);
    }

    private async Task SetTaskStatusAsync(string? status)
    {
        await TaskStatusDropdown.ClickAsync();
        await VerifyDropdownItemsAsync();
        var statusOption = _page.Locator(StatusOption(status));
        await statusOption.ClickAsync();
    }

    private async Task VerifyDropdownItemsAsync()
    {
        var dropdownItems = await DropdownListItem.WaitForAllAsync();
        var expectedValues = new List<string> { "TO DO", "IN PROGRESS", "IN REVIEW", "DONE" };

        foreach (var expectedValue in expectedValues)
        {
            var containsValue = false;

            foreach (var option in dropdownItems)
            {
                var optionText = await option.InnerTextAsync();
                if (optionText.Trim().Equals(expectedValue, StringComparison.OrdinalIgnoreCase))
                {
                    containsValue = true;
                    break;
                }
            }

            if (!containsValue)
                throw new Exception($"Dropdown item '{expectedValue}' not found.");
        }
    }

    public async Task AssertTaskAddedAsync()
    {
        await Assertions.Expect(TaskCard.Nth(0)).ToBeVisibleAsync();
    }

    public async Task CheckIfTaskDetailsExistAsync(string detail)
    {
        var taskSelector = detail switch
        {
            "summary" => "[id='card_title']",
            "description" => "[id='card_description']",
            _ => throw new ArgumentException("Invalid detail type", nameof(detail))
        };

        var taskElements = await _page.QuerySelectorAllAsync(taskSelector);
        var expectedText =
            _scenarioContext[detail == "summary" ? CommonLabels.TaskSummary : CommonLabels.TaskDescription]?.ToString();

        if (string.IsNullOrWhiteSpace(expectedText))
            throw new InvalidOperationException($"Expected {detail} not provided in the scenario context.");

        foreach (var taskElement in taskElements)
        {
            var elementText = await taskElement.InnerTextAsync();

            if (elementText.Contains(expectedText, StringComparison.OrdinalIgnoreCase)) return;
        }

        // If none of the elements contain the expected text, throw an exception
        throw new Exception($"No element with text '{expectedText}' found for {detail}");
    }


    public async Task ClickViewTaskButton()
    {
        await ViewTasksButton.Nth(0).ClickAsync();
        await _page.WaitForURLAsync(new Regex($".*{_tasksUrl}.*"));
    }

    private async Task AssertStatusColumn()
    {
        var taskCard = _page.Locator("div[class*='card-content']").First;
        var subParentElement = taskCard.GetParentElement();
        var parentElement = subParentElement.GetParentElement();
        var statusColumn = await parentElement.GetAttributeAsync("status");
        statusColumn.Should().Contain(_scenarioContext[CommonLabels.TaskStatus].ToString());
    }

    public async Task ViewTaskAsync()
    {
        if (!_page.Url.Contains(_dashboardUrl))
        {
            await ClickDashboardTabAsync();
            await ClickViewTaskButton();
        }
    }

    public async Task ClickDashboardTabAsync()
    {
        await DashboardTab.ClickAsync();
        await _page.WaitForURLAsync(new Regex($".*{_dashboardUrl}.*"));
    }

    public async Task VerifyTaskDetailsAsync()
    {
        var detailTypes = new[] { "summary", "description" };

        foreach (var detailType in detailTypes) await CheckIfTaskDetailsExistAsync(detailType);

        await AssertStatusColumn();
    }


    private async Task ClickEditTaskButtonAsync()
    {
        await EditTaskButton.Nth(0).ClickAsync();
        await _page.WaitForURLAsync(new Regex($".*{_updateTaskUrl}.*"));
    }

    public async Task ClickUpdateTaskButtonAsync()
    {
        await _page.WaitForSelectorAsync("button[type ='submit']");
        await Task.Delay(300);
        await UpdateTaskButton.ScrollIntoViewIfNeededAsync();

        await _page.RunAndWaitForResponseAsync(
            async () => { await UpdateTaskButton.ClickAsync(); },
            res => res.Ok && res.Url.Contains($"{_tasksUrl}"));
        await _page.WaitForURLAsync(new Regex($".*{_tasksUrl}.*"));
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task EditTaskDetailsAsync(string? updatedTaskSummary, string? updatedTaskDescription)
    {
        await ClickEditTaskButtonAsync();
        _scenarioContext.Add(CommonLabels.UpdatedTaskSummary, updatedTaskSummary);
        _scenarioContext.Add(CommonLabels.UpdatedTaskDescription, updatedTaskDescription);
        await Task.Delay(300);
        await TaskSummary.ClearAsync();
        if (updatedTaskSummary != null) await TaskSummary.FillAsync(updatedTaskSummary);
        await TaskDescription.ClearAsync();
        if (updatedTaskDescription != null) await TaskDescription.FillAsync(updatedTaskDescription);
        await Task.Delay(300);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task VerifyTaskDetailsUpdate()
    {
        await _page.WaitForURLAsync("**");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await TaskCard.WaitForAllAsync();
        var taskCardCount = await TaskCard.CountAsync();

        _scenarioContext.Add(CommonLabels.TasksCount, taskCardCount);

        taskCardCount.Should().BeGreaterThan(0, "There should be at least one task card.");

        var actualTaskTitle = await TaskCardTitle.Nth(0).InnerTextAsync();
        actualTaskTitle.Should().Be(
            _scenarioContext[CommonLabels.UpdatedTaskSummary].ToString(),
            "Task title should be updated.");

        var actualTaskDescription = await TaskCardDescription.Nth(0).InnerTextAsync();
        actualTaskDescription.Should().Be(
            _scenarioContext[CommonLabels.UpdatedTaskDescription].ToString(),
            "Task description should be updated.");
    }

    public async Task DeleteTaskAsync()
    {
        _page.Dialog += (_, dialog) => dialog.AcceptAsync();
        await _page.RunAndWaitForResponseAsync(
            async () => { await DeleteTaskButton.Nth(0).ClickAsync(); },
            res => res.Ok);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }


    public async Task VerifyTaskDeletionAsync()
    {
        await _page.WaitForURLAsync("**");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await TaskCard.WaitForAllAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var tasksCount = await TaskCard.CountAsync();
        var totalTasks = (int)_scenarioContext[CommonLabels.TasksCount];
        Assert.AreEqual(tasksCount, totalTasks - 1);
    }

    public async Task TestMissingField(string? projectName, string? projectDescription)
    {
        await AddProjectDetails(projectName, projectDescription);
    }

    public async Task TestMissingTaskField(string? taskSummary, string? taskDescription)
    {
        await EditTaskDetailsAsync(taskSummary, taskDescription);
        await Task.Delay(300);
        await UpdateTaskButton.WaitForAsync();
        await UpdateTaskButton.ClickAsync();
    }
}