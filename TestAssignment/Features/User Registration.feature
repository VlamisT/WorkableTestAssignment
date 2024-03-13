Feature: User_Registration

    @UserRegistration
    Scenario: New users can create an account with optional company and address.
        Given a user explores the PM Tool Application
        And the user is on the registration page
        When the user enters valid name 'Thodoris Vlamis', email 'YOUR_NEW_EMAIL@example.com'' and password 'qwerty123' with address ' ' and company ' '
        Then the account is successfully created

    Scenario: Attempt to create an account without providing a required field (name, email, or password).
        Given a user explores the PM Tool Application
        And the user is on the registration page
        When the user attempts to create an account without providing a "<required field>"
        Then an appropriate validation message 'This field is required' should be displayed

    Examples:
      | required field    |
      | name              |
      | email             |
      | password          |
      | nameAndEmail      |
      | nameEmailPassword |