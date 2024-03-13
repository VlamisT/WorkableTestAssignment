@TaskdbOperations
Feature: TaskDB_Operations

    Background:
        Given a user explores the PM Tool Application
        And a verified user logs in
        And the user adds a new project with valid details, testProject2 and projectDescription2

    @TaskdbOperations
    Scenario: Users can view all tasks belonging to their projects in TaskDB and perform searching
        Given the user adds <numberOfTasks> new tasks to the existing project with valid details
        When the user checks TaskDB
        Then all tasks for the user's projects should be displayed
        When the user performs a search for a specific task
        Then the result should match the searched task
        When the user sorts tasks based on different criteria
        Then the tasks should be displayed in the correct order

    Examples:
      | numberOfTasks |
      | 3             |