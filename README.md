# Finance Case API

A .NET 8 Web API showcasing a realistic enterprise finance workflow
implemented with Clean Architecture, explicit business rules and
structured validation.

This project demonstrates how to build a production-style backend
focused on maintainability, separation of concerns and controlled domain
complexity.

------------------------------------------------------------------------

## 🏗 Architecture Overview

The solution follows a layered Clean Architecture approach:

FinanceCase.Domain\
FinanceCase.Application\
FinanceCase.Infrastructure\
FinanceCase.Api\
FinanceCase.Tests

### Domain

-   Core business entities (`FinanceCase`, `Customer`, `CaseNote`)
-   Explicit state machine
    (`Draft → Submitted → InReview → Approved / Rejected`)
-   Business rules and domain exceptions
-   Risk scoring abstraction (`IRiskCalculator`)
-   Audit trail via case notes

### Application

-   Command handlers
-   DTOs and mapping
-   FluentValidation validators
-   Dependency Injection wiring
-   Repository abstractions

### Infrastructure

-   EF Core (SQL Server)
-   DbContext configuration
-   Repository implementation
-   Persistence mapping

### API

-   REST endpoints
-   Swagger documentation
-   ProblemDetails middleware for consistent error responses
-   FluentValidation integration

### Tests

-   MSTest unit tests
-   Domain workflow coverage
-   Validation rule verification

------------------------------------------------------------------------

## 🔄 Finance Case Workflow

A finance case follows a controlled lifecycle:

``` mermaid
stateDiagram-v2
    [*] --> Draft
    Draft --> Submitted : Submit()
    Submitted --> InReview : MarkInReview()
    InReview --> Approved : Approve()
    InReview --> Rejected : Reject(reason)
    Approved --> [*]
    Rejected --> [*]
```

### Business Rules

-   Draft cases can be edited\
-   Only Draft cases can be submitted\
-   Only Submitted cases can enter review\
-   Approval depends on RiskScore threshold\
-   Each state transition generates an audit note\
-   Rejection requires a reason

This enforces explicit and traceable state transitions in the Domain
layer.

------------------------------------------------------------------------

## 📊 Risk Evaluation Flow

``` mermaid
flowchart TD
    A[Submit Case] --> B[Risk Calculator]
    B --> C{Risk Score <= Threshold?}
    C -- Yes --> D[Approve]
    C -- No --> E[Reject or Manual Review]
```

------------------------------------------------------------------------

## 🧠 What This Project Demonstrates

-   Clean Architecture structuring
-   Explicit domain modeling and invariants
-   Business rule enforcement within entities
-   Controlled state transitions
-   EF Core integration without leaking persistence concerns into Domain
-   FluentValidation for input validation
-   Standardized API error handling via ProblemDetails
-   Unit testing with MSTest

------------------------------------------------------------------------

## 🔌 Example Endpoints

  Method   Endpoint                Description
  -------- ----------------------- -------------------------
  POST     `/cases`                Create a new draft case
  GET      `/cases/{id}`           Retrieve case details
  POST     `/cases/{id}/submit`    Submit a draft case
  POST     `/cases/{id}/review`    Move case to InReview
  POST     `/cases/{id}/approve`   Approve case
  POST     `/cases/{id}/reject`    Reject case

------------------------------------------------------------------------

## 🛠 Technology Stack

-   .NET 8
-   ASP.NET Core Web API
-   Entity Framework Core
-   SQL Server
-   FluentValidation
-   MSTest
-   Swagger (OpenAPI)
-   ProblemDetails middleware

------------------------------------------------------------------------

## 🚀 Getting Started

### 1️⃣ Restore dependencies

``` bash
dotnet restore
```

### 2️⃣ Apply database migrations

``` bash
dotnet ef database update -p FinanceCase.Infrastructure -s FinanceCase.Api
```

### 3️⃣ Run the API

``` bash
dotnet run --project FinanceCase.Api
```

Swagger available at:

    /swagger

------------------------------------------------------------------------

## 🧪 Run Tests

``` bash
dotnet test
```

Unit tests cover:

-   Domain state transitions
-   Business constraint violations
-   Validation scenarios

------------------------------------------------------------------------

## 🏛 Architectural Principles

This sample emphasizes:

-   Explicit domain modeling
-   Separation of concerns
-   Infrastructure isolation
-   Controlled complexity
-   Testable business logic
-   Predictable state transitions

------------------------------------------------------------------------

## 📌 Purpose

This repository is designed as a structured backend reference
implementation for enterprise-oriented financial workflows.

The focus is on clarity, consistency and long-term maintainability
rather than rapid prototyping.
