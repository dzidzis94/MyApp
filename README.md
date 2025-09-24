# MyApp - Simple ASP.NET Core Web Application

This is a simple ASP.NET Core web application that allows for basic project and client management.

## Features

- **User Roles**: Admin and Client.
- **Admin**: Manage clients, admins, and projects.
- **Client**: View assigned projects and fill in data.
- **Data Storage**: All data is stored in a local JSON file (`MyApp.Web/Data/database.json`).

## How to Run Locally

1.  **Prerequisites**:
    *   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later.

2.  **Clone the repository**:
    ```bash
    git clone <repository-url>
    cd <repository-directory>
    ```

3.  **Run the application**:
    *   Navigate to the `MyApp.Web` directory:
        ```bash
        cd MyApp.Web
        ```
    *   Run the application using the `dotnet` CLI:
        ```bash
        dotnet run
        ```

4.  **Access the application**:
    *   Open your web browser and navigate to `http://localhost:5000` (or the port specified in the console output).

## Project Structure

- `MyApp.sln`: Visual Studio solution file.
- `MyApp.Web/`: The main ASP.NET Core Web project.
  - `Controllers/`: Contains the MVC controllers.
  - `Models/`: Contains the data models.
  - `Services/`: Contains the data service for JSON storage.
  - `Data/`: Contains the `database.json` file.
  - `Views/`: Contains the Razor views.
  - `wwwroot/`: Contains static assets (CSS, JS, etc.).
- `.gitignore`: Standard .NET gitignore file.
- `README.md`: This file.
