# Finance Case API

A .NET 8 Web API showcasing a realistic enterprise workflow for a finance case (submission → review → approval), with clean architecture and business rules.

## What this project demonstrates
- Clean Architecture (Domain / Application / Infrastructure / API)
- Explicit business rules and state transitions
- EF Core persistence + migrations
- Validation (FluentValidation)
- Problem Details for consistent API errors
- Unit tests for core domain logic

## Domain overview
A FinanceCase belongs to a Customer and goes through a controlled lifecycle:
Draft → Submitted → InReview → Approved / Rejected

Every state transition is validated and recorded as an audit note.

## Tech stack
- .NET 8
- EF Core
- FluentValidation
- ProblemDetails middleware
- xUnit

## Getting started
1. `dotnet restore`
2. `dotnet ef database update` (after configuring connection string)
3. `dotnet run --project FinanceCase.Api`
