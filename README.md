## What's  this .NET Web API?
this is the backend api that has service and db and all functions that's requested
## Goals

The goal of this repository is to provide a functional APIs  using .NET 6 Web API. This also serves the purpose of learning advanced concepts and implementations such as `Multitenancy, CQRS, Onion Architecture, Clean Coding standards` and so on.

## Features

- [x] Built on .NET 6.0
- [x] Follows Clean Architecture Principles
- [x] Domain Driven Design
- [x] Self register & login with permissions
- [x] CRUD for tickets 
- [x] Supports  MSSQL

<details>
- [x] Uses Entity Framework Core as DB Abstraction
- [x] Flexible Repository Pattern
- [x] Dapper Integration for Optimal Performance
- [x] Serilog Integration with various Sinks - File, SEQ, Kibana
- [x] OpenAPI - Supports Client Service Generation
- [x] Mapster Integration for Quicker Mapping
- [x] API Versioning
- [x] Response Caching - Distributed Caching + REDIS
- [x] Fluent Validations
- [x] Audit Logging
- [x] Advanced User & Role Based Permission Management
- [x] Code Analysis & StyleCop Integration with Rulesets
- [x] JSON Based Localization with Caching
- [x] Hangfire Support - Secured Dashboard
- [x] File Storage Service
- [x] Test Projects
- [x] JWT & Azure AD Authentication
- [x] MediatR - CQRS
- [x] SignalR Notifications
- [x] & Much More
</details>

## Getting Started

To get started db is code first so you can migrate db but it needs to update the connection string to match your server 
- You can find the connection string in "TalabeyahTaskApi\TalabeyahTaskApi\src\Host\Configurations\database.json"
- Application will run on port http://localhost:5010/swagger/index.html and https://localhost:5011/swagger/index.html#/ 
- Admin Credential   EmailAddress = "admin@root.com" and Password = "123Pa$$word!"
- Default TenantIdName is root , it's handled from the Blazor UI 
