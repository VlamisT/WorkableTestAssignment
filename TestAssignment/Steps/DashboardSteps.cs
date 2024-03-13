using Microsoft.Playwright;
using TechTalk.SpecFlow.UnitTestProvider;
using TestAssignment.Infrastructure;
using TestAssignment.Pages;

namespace TestAssignment.Steps;

[Binding]
public sealed class DashboardSteps
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

    public DashboardSteps(IPage page, ScenarioContext scenarioContext, DashboardPage dashboardPage, LoginPage loginPage,
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

    [StepDefinition(@"the user adds a new project with valid details, (.*) and (.*)")]
    public async Task UserAddsNewProject(string projectName, string projectDescription)
    {
        await _dashboardPage.ClickCreateProjectButtonAsync();
        await _dashboardPage.AddProjectDetails(projectName, projectDescription);
    }

    [StepDefinition(@"the project should be successfully added")]
    public async Task VerifyProjectCreation()
    {
        await _dashboardPage.AssertProjectSuccessfullyAdded();
    }

    [StepDefinition(@"the user edits the project with valid changes, (.*) and (.*)")]
    public async Task EditProjectDetailsAsync(string updatedProjectName, string updatedProjectDescription)
    {
        await _dashboardPage.ClickEditProjectButton();
        await _dashboardPage.EditProjectDetails(updatedProjectName, updatedProjectDescription);
        await _dashboardPage.ClickUpdateProjectAsync();
    }

    [StepDefinition(@"the project details should be updated")]
    public async Task AssertProjectDetailsUpdated()
    {
        await _dashboardPage.VerifyProjectDetailsUpdate();
    }

    [StepDefinition(@"the user deletes the project")]
    public async Task DeleteProject()
    {
        await _dashboardPage.DeleteProjectAsync();
    }

    [StepDefinition(@"the project should be removed from the project list")]
    public async Task AssertDeletedProjectRemoved()
    {
        await _dashboardPage.VerifyProjectDeletionAsync();
    }

    [StepDefinition(@"the user adds a new task to the existing project with valid details, (.*), (.*), (.*)")]
    public async Task UserAddsTaskInProject(string? taskSummary, string? taskDescription, string? taskStatus)
    {
        await _dashboardPage.AddProjectTaskAsync(taskSummary, taskDescription, taskStatus);
    }

    [StepDefinition(@"the task should be successfully added")]
    public async Task VerifyTaskAdded()
    {
        await _dashboardPage.AssertTaskAddedAsync();
    }

    [StepDefinition(@"the user views the task")]
    public async Task UserViewsTask()
    {
        await _dashboardPage.ViewTaskAsync();
    }

    [StepDefinition(@"the correct task details should be displayed")]
    public async Task AssertTaskDetails()
    {
        await _dashboardPage.VerifyTaskDetailsAsync();
    }

    [StepDefinition(@"the user edits the task with valid changes, (.*) and (.*)")]
    public async Task EditTaskDetails(string? taskSummary, string? taskDescription)
    {
        await _dashboardPage.EditTaskDetailsAsync(taskSummary, taskDescription);
        await _dashboardPage.ClickUpdateTaskButtonAsync();
    }


    [StepDefinition(@"the task details should be updated")]
    public async Task AssertTaskDetailsUpdated()
    {
        await _dashboardPage.VerifyTaskDetailsUpdate();
    }

    [StepDefinition(@"the user deletes the task")]
    public async Task DeleteTask()
    {
        await _dashboardPage.DeleteTaskAsync();
    }

    [StepDefinition(@"the task should be removed from the task list")]
    public async Task AssertTaskDeletion()
    {
        await _dashboardPage.VerifyTaskDeletionAsync();
    }


    [StepDefinition(@"the user adds (.*) new tasks to the existing project with valid details")]
    public async Task TheUserAddsNewTasksToExistingProject(int numberOfTasks)
    {
        var taskSummaries = new List<string>();
        for (var i = 1; i <= numberOfTasks; i++)
        {
            var taskSummary = $"task{i} summary";
            var taskDescription = $"task{i} descr";
            var taskStatus = "TO DO";
            taskSummaries.Add(taskSummary);
            await _dashboardPage.AddProjectTaskAsync(taskSummary, taskDescription, taskStatus);
            await _dashboardPage.ClickDashboardTabAsync();
        }

        _scenarioContext.Add(CommonLabels.TasksList, taskSummaries);
    }

    [StepDefinition(@"the user attempts to create a new project without providing a ""(.*)""")]
    public async Task TestMissingRequiredFields(string requiredField)
    {
        await _dashboardPage.ClickCreateProjectButtonAsync();
        switch (requiredField)
        {
            case "name":
                await _dashboardPage.TestMissingField(
                    null, "description");
                break;
            case "description":
                await _dashboardPage.TestMissingField(
                    "Test Project", null);
                break;
            case "nameAndDescription":
                await _dashboardPage.TestMissingField(
                    null, null);
                break;

            default:
                throw new ArgumentException("Invalid required field");
        }
    }

    [StepDefinition(@"the user attempts to edit a task without providing a ""(.*)""")]
    public async Task TestMissingTaskRequiredFields(string requiredField)
    {
        switch (requiredField)
        {
            case "name":
                await _dashboardPage.TestMissingTaskField(
                    null, "description");
                break;
            case "description":
                await _dashboardPage.TestMissingTaskField(
                    "Test Project", null);
                break;
            case "nameAndDescription":
                await _dashboardPage.TestMissingTaskField(
                    null, null);
                break;

            default:
                throw new ArgumentException("Invalid required field");
        }
    }
}