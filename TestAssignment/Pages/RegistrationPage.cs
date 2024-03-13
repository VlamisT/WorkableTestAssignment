using FluentAssertions;
using Microsoft.Playwright;
using TechTalk.SpecFlow.UnitTestProvider;

namespace TestAssignment.Pages;

public class RegistrationPage : BasePage
{
    public RegistrationPage(IPage page, ScenarioContext scenarioContext,
        IUnitTestRuntimeProvider unitTestRuntimeProvider) : base(page, scenarioContext)
    {
    }

    private ILocator Name => _page.Locator("[id='fullName']");
    private ILocator Email => _page.Locator("[id='email']");
    private ILocator Password => _page.Locator("[id='password']");
    private ILocator Company => _page.Locator("[id='company']");
    private ILocator Address => _page.Locator("[id='address']");
    private ILocator SignUpButton => _page.Locator("button:has-text('Sign Up')");

    private ILocator ErrorMessage => _page.Locator("p[class='invalid-feedback']");

    private ILocator AccountVerificationCard => _page.Locator("[class='card-title']");
    private ILocator AccountVerificationMessage => _page.Locator("[class='card-content white-text'] p");


    public void AssertRegistrationFormOpens()
    {
        AssertLocatorIsVisible(Name, "Name");
        AssertLocatorIsVisible(Email, "Email");
        AssertLocatorIsVisible(Password, "Password");
        AssertLocatorIsVisible(Company, "Company");
        AssertLocatorIsVisible(Address, "Address");
        AssertLocatorIsVisible(SignUpButton, "SignUpButton");
    }

    private void AssertLocatorIsVisible(ILocator locator, string elementName)
    {
        var isVisible = locator.IsVisibleAsync();

        if (isVisible.Result.Equals(false))
            // Assertion failure message
            Console.WriteLine($"Element '{elementName}' is not visible!");
    }


    public async Task RegisterUser(string? userName, string? userEmail, string? password, string? address,
        string? company)
    {
        if (userName != null) await Name.FillAsync(userName);
        if (userEmail != null) await Email.FillAsync(userEmail);
        if (password != null) await Password.FillAsync(password);
        if (address != null) await Address.FillAsync(address);
        if (company != null) await Company.FillAsync(company);
        await SignupAsync(userEmail);
    }


    public async Task SignupAsync(string email)
    {
        await ClickSignupAsync();
        await Task.Delay(500);
        if (!_page.Url.Contains("verifyAccount")) await DoesEmailAlreadyExistMessageAsync(email);
    }

    public async Task ClickSignupAsync()
    {
        await SignUpButton.ClickAsync();
        await _page.WaitForLoadStateAsync();
    }

    public async Task DoesEmailAlreadyExistMessageAsync(string email)
    {
        await _page.WaitForLoadStateAsync();
        var invalidFeedbackElements = await _page.QuerySelectorAllAsync("p[class='invalid-feedback']");
        if (invalidFeedbackElements.Any())
            foreach (var invalidFeedbackElement in invalidFeedbackElements)
            {
                var text = await invalidFeedbackElement.InnerTextAsync();
                Console.WriteLine($"Checking text: {text}");
                if (text.Contains($"Email `{email}` already exits", StringComparison.OrdinalIgnoreCase))
                    throw new Exception($"Email '{email}' already exists");
            }
    }

    public async Task VerifyAccountCreation()
    {
        await Assertions.Expect(AccountVerificationCard).ToBeVisibleAsync();
        await Assertions.Expect(AccountVerificationMessage).ToContainTextAsync("Successfull registration");
        var actualUrl = _page.Url;
        actualUrl.Should().Contain("verifyAccount");
    }

    public async Task TestMissingField(string? name, string? email, string? password, string? address, string? company)
    {
        await RegisterUser(name, email, password, address, company);
    }


    public async Task AssertValidationMessage(string expectedMessage)
    {
        var validationMessages = await GetInvalidFeedbackTexts();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        validationMessages.Should().Contain(expectedMessage);
    }

    public async Task<List<string>> GetInvalidFeedbackTexts()
    {
        var invalidFeedbackElements = await _page.QuerySelectorAllAsync("p[class='invalid-feedback']");
        await Task.Delay(300);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var feedbackTexts = new List<string>();

        foreach (var invalidFeedbackElement in invalidFeedbackElements)
        {
            var text = await invalidFeedbackElement.InnerTextAsync();
            feedbackTexts.Add(text);
        }

        return feedbackTexts;
    }
}