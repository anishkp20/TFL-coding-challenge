# C# Console Application with NUnit Tests

This is a simple C# console application to get the road status from TFL using API .

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0)
- [NUnit](https://nunit.org/) testing framework (included as a dependency in the test project) Version 3.13.3
- An API key and App ID from TfL (register at https://api-portal.tfl.gov.uk/)
- - [Visual Studio](https://visualstudio.microsoft.com/) or any IDE for C# development (optional
## Configuration
Unzip the contents of zip folder

Before running the application, navigate to /TFL coding challenge/RoadStatucConsoleApp and open file `appsettings.json`
Update the AppId and AppKey in the `appsettings.json` file.

## How to Run the Console Application
Open command prompt and navigate to folder /TFL coding challenge/RoadStatucConsoleApp
Run the below commands to execute the console application:

dotnet build --configuration Release
In command mode, Navigate to "TFL coding challenge\RoadStatucConsoleApp\bin\Release\net6.0" and run the below commands
RoadStatucConsoleApp.exe run A2
RoadStatucConsoleApp.exe run A222
RoadStatucConsoleApp.exe run
RoadStatucConsoleApp.exe run ""

## How to run the test projects
Navigate to /RoadStatucConsoleApp.Tests folder
Before running the application, navigate to /TFL coding challenge/RoadStatucConsoleApp.Tests and open file `appsettings.json`
Update the AppId and AppKey in the `appsettings.json` file.

Run the below commands to execute the unit tests:
dotnet build --configuration Release
In command mode, Navigate to "TFL coding challenge\RoadStatucConsoleApp.Tests\bin\Release\net6.0" and run the below commands
dotnet test RoadStatucConsoleApp.Tests.dll