# Task Scheduler & Reminder System

## Overview

As a software engineer focused on expanding my knowledge of statically-typed, object-oriented languages, I wanted to build a non-trivial console application that pushes beyond simple scripts and into real software design. This project gave me the opportunity to explore C#'s type system, language features, and ecosystem in depth.

The software is a console-based Task Scheduler and Reminder system. Users can create tasks with titles, descriptions, due dates, priority levels, and reminder windows. Tasks are persisted to a local JSON file, so they survive between sessions. The application can also export the task list to a formatted CSV file and send a confirmation email via SMTP whenever a new task is created.

My purpose in writing this was to put C#-specific constructs — classes, structs, enums, LINQ, async/await, generics, and dependency injection — to practical use in a cohesive project, rather than in isolated toy examples. I wanted to come away understanding not just the syntax, but how C# projects are structured in the real world.

[Software Demo Video](http://youtube.link.goes.here)

## Development Environment

- **Editor:** Visual Studio Code with the C# Dev Kit extension
- **SDK:** .NET 8 (LTS)
- **Build tool:** `dotnet CLI`
- **Language:** C# 12
- **Libraries & packages:**
  - `Microsoft.Extensions.DependencyInjection` — constructor-based dependency injection
  - `Microsoft.Extensions.Configuration` + `Microsoft.Extensions.Configuration.Json` — loading SMTP settings from `appsettings.json`
  - `System.Text.Json` — JSON serialization for task persistence
  - `System.Net.Mail` — SMTP email (part of the .NET standard library)

## Useful Websites

- [Microsoft C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [Microsoft .NET API Browser](https://learn.microsoft.com/en-us/dotnet/api/)
- [C# — Wikipedia](https://en.wikipedia.org/wiki/C_Sharp_(programming_language))
- [Microsoft.Extensions.DependencyInjection on NuGet](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection)
- [System.Text.Json overview](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview)

## Future Work

- Add a background service that polls every minute and surfaces reminder alerts automatically, rather than only checking at startup.
- Implement task categories and filtering so users can view tasks by tag or project.
- Replace the flat JSON file with a SQLite database using Entity Framework Core for better querying and scalability.
