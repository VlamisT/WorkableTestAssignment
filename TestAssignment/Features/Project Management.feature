Feature: Project_Management

    Background:
        Given a user explores the PM Tool Application
        And a verified user logs in

    @ProjectManagement
    @ClearProjects
    Scenario: Users can Add / Edit / View / Delete projects.
        When the user adds a new project with valid details, testProject and projectDescription
        Then the project should be successfully added
        When the user edits the project with valid changes, testProjectUptd and projectDescriptionUptd
        Then the project details should be updated
        When the user deletes the project
        Then the project should be removed from the project list

    Scenario: Attempt to create a project without providing a required field (name,or description).
        When the user attempts to create a new project without providing a "<required field>"
        Then an appropriate validation message 'This field is required' should be displayed

    Examples:
      | required field     |
      | name               |
      | description        |
      | nameAndDescription |

    @TaskManagement
    Scenario: Users can Add / Edit / View / Delete tasks for a specific project.
        And the user adds a new project with valid details, testProject and projectDescription
        When the user adds a new task to the existing project with valid details, task summary, task descr, IN PROGRESS
        Then the task should be successfully added
        When the user views the task
        Then the correct task details should be displayed
        When the user edits the task with valid changes, updated summary and updated description
        Then the task details should be updated
        When the user deletes the task
        Then the task should be removed from the task list

    Scenario: Attempt to update a task without providing a required field (name,or description).
        And the user adds a new project with valid details, testProject3 and projectDescription3
        And the user adds a new task to the existing project with valid details, task summary, task description, DONE
        And the user views the task
        When the user attempts to edit a task without providing a "<required field>"
        Then an appropriate validation message 'This field is required' should be displayed

    Examples:
      | required field     |
      | name               |
      | description        |
      | nameAndDescription |