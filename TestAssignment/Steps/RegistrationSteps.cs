using System.Net.Mail;
using Microsoft.Playwright;
using TechTalk.SpecFlow.UnitTestProvider;
using TestAssignment.Pages;

namespace TestAssignment.Steps;

[Binding]
public sealed class RegistrationSteps
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

    public RegistrationSteps(IPage page, ScenarioContext scenarioContext, DashboardPage dashboardPage,
        LoginPage loginPage,
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


    [StepDefinition(
        "the user enters valid name \'(.+)\', email \'(.+)\' and password \'(.+)\' with address '(.+)?' and company '(.+)?'")]
    public async Task CreateAccount(string userName, string userEmail, string password, string? address,
        string? company)
    {
        var isEmailValid = IsValidEmail(userEmail);
        if (!isEmailValid) _unitTestRuntimeProvider.TestIgnore("The email provided was invalid.Skipping");
        await _registrationPage.RegisterUser(userName, userEmail, password, address, company);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    [StepDefinition(@"the account is successfully created")]
    public async Task AssertAccountCreation()
    {
        await _registrationPage.VerifyAccountCreation();
    }

    [StepDefinition(@"the user attempts to create an account without providing a ""(.*)""")]
    public async Task TestMissingRequiredFields(string requiredField)
    {
        switch (requiredField)
        {
            case "name":
                await _registrationPage.TestMissingField(
                    null, "testemail@example.com", "testpassword", null, null);
                break;
            case "email":
                await _registrationPage.TestMissingField(
                    "Test User", null, "testpassword", null, null);
                break;
            case "password":
                await _registrationPage.TestMissingField(
                    "Test User", "testemail@example.com", null, null, null);
                break;

            case "nameAndEmail":
                await _registrationPage.TestMissingField(
                    null, null, "testpassword", null, null);
                break;
            case "nameEmailPassword":
                await _registrationPage.TestMissingField(
                    null, null, null, null, null);
                break;
            default:
                throw new ArgumentException("Invalid required field");
        }
    }

    [StepDefinition(@"an appropriate validation message \'(.+)\' should be displayed")]
    public async Task AssertAppropriateValidationMessage(string expectedMessage)
    {
        await _registrationPage.AssertValidationMessage(expectedMessage);
    }
}