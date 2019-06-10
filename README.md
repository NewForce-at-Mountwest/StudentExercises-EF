# Student Exercises: Entity Framework Edition

#### Instructions
1. Clone this repo onto your local machine by running `git clone <url>`
1. Create a new SQL Server database and name it `StudentExercises-EF`
1. In Visual Studio, open the Package Manager Console and add a migration to build your database with the command `Add-Migration build-db`
1. In the Package Manager Console, run the command `Update-Database`.
1. To run the app without the debugger, use the shortcut `Ctrl` + `F5`.

#### Change Log

*Monday, June 10, 2019*
1. Refactored `Controllers/StudentsController` to use view models instead of view data
    - Files affected:
    - `Controllers/StudentsController.cs`
    - `Models/ViewModels/StudentCreateViewModel.cs`
    - `Models/ViewModels/StudentEditViewModel.cs`
    - `Views/Students/Create.cshtml`
    - `Views/Students/Edit.cshtml`
1. In index view for students, ordered students alphabetically by last name
    - Files affected:
    - `Controllers/StudentsController.cs`, Index method
1. In detail view for students, listed exercises that have been assigned to that student
    - Files affected:
    - `Controllers/StudentController.cs`, Details method
1. Restricted delete behavior on cohorts so that if a cohort cannot be deleted if it has students in it
    - Files affected:
    - `Data/ApplicationDbContext.cs` to restrict delete behavior
    - `Controllers/CohortsController.cs` to handle SQL exception when the user tries to do a restricted delete

