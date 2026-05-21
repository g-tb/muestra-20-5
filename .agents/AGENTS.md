# Copilot Instructions for MovieManager

## Build, Test, and Lint Commands

### Build
```bash
dotnet build
```

### Run All Tests
```bash
dotnet test
```

### Run Single Test Project
```bash
dotnet test BusinessLogicTest/BusinessLogicTest.csproj
```

### Run and Debug UI
```bash
dotnet run --project UserInterface/UserInterface.csproj
```
The Blazor UI will be available at `https://localhost:7019` (or `https://localhost:5001` depending on configuration).

## Architecture

MovieManager is a **.NET 6 Blazor Server** application with a layered architecture.
The project currently contains in-memory repositories, but it is being migrated to
**Azure SQL Edge running in Docker**. New code should treat the in-memory layer as a
temporary implementation detail, not as the architectural source of truth.

The solution has five projects:

1. **Domain** - Core business entities and validation
   - `Movie`, `Actor`, `Category` classes
   - Domain validation logic (e.g., `Movie.Title` must not be empty, `Budget` must be non-negative)
   - Custom `DomainException` for validation violations

2. **Repository** - Persistence layer
   - `MovieMemoryRepository`, `ActorMemoryRepository`, `CategoryMemoryRepository`
   - Current implementation stores data in memory
   - Future implementation should target Azure SQL Edge through repository
     abstractions
   - Do not add new behavior that depends on process-local state, object identity,
     LINQ-to-objects semantics, or in-memory auto-increment logic

3. **BusinessLogic** - Application services and business rules
   - `MovieService`, `ActorService`, `CategoryService` classes
   - DTOs (`MovieDTO`, `ActorDTO`, `CategoryDTO`) for layer isolation
   - Business logic validation (e.g., duplicate movie titles)
   - Custom `LogicException` for business rule violations

4. **UserInterface** - Blazor server-side web application
   - Razor components in `Pages/` and `Shared/`
   - Dependency injection setup in `Program.cs`
   - Services and repositories are registered with DI

5. **BusinessLogicTest** - Unit tests (MSTest framework)
   - Tests for service layer business logic
   - Uses `[TestClass]`, `[TestInitialize]`, `[TestMethod]` attributes
   - Includes expected exception testing with `[ExpectedException]`

## Key Conventions

### DTO Conversion Pattern
Always use explicit conversion methods rather than direct casting:
- DTOs have `toEntity()` method to convert to Domain objects
- DTOs have static `fromEntity()` method to convert from Domain objects

Example:
```csharp
Movie newMovie = movieDTO.toEntity();
MovieDTO result = MovieDTO.fromEntity(newMovie);
```

### Validation Strategy
- **Domain layer**: Validates entity state during property assignment (via private setters and property logic)
- **Business logic layer**: Validates business rules (duplicates, cross-cutting concerns)
- Throw `DomainException` for domain validation failures
- Throw `LogicException` for business logic failures

### Dependency Injection and Abstractions
Current services are registered in `Program.cs`, and existing memory repositories
are registered directly:
```csharp
builder.Services.AddSingleton<MovieMemoryRepository>();
builder.Services.AddSingleton<MovieService>();
```

When touching persistence-facing code, prefer introducing or using interfaces such
as `IMovieRepository`, `IActorRepository`, and `ICategoryRepository`. Services
should depend on abstractions, not concrete memory or SQL classes. Keep
infrastructure choices in the composition root (`Program.cs`) or dedicated
infrastructure registration code.

Use constructor injection. Avoid service locators, static state, direct SQL access
from Razor components, and direct infrastructure coupling from the domain layer.

### Nullable Reference Types
All projects have `<Nullable>enable</Nullable>`. Use nullable annotations (`?`) appropriately:
- Use `string?` for optional strings
- Use `int?` for optional integers
- Non-nullable types require values

### Testing Patterns
- Create fresh repositories in `[TestInitialize]`
- Use `Assert.AreEqual()` for value comparisons
- Use `Assert.ThrowsException<ExceptionType>()` for expected failures
- Test both happy path and exception cases

## File Structure

```
MovieManager/
├── Domain/                          # Core entities
│   ├── Movie.cs
│   ├── Actor.cs
│   ├── Category.cs
│   └── Exceptions/
├── Repository/                      # Data access abstractions/implementations
│   ├── MovieMemoryRepository.cs
│   ├── ActorMemoryRepository.cs
│   └── CategoryMemoryRepository.cs
├── BusinessLogic/                   # Application services
│   ├── MovieService.cs
│   ├── ActorService.cs
│   ├── CategoryService.cs
│   ├── Dtos/                       # Data transfer objects
│   └── Exceptions/
├── UserInterface/                   # Blazor web UI
│   ├── Program.cs                   # DI setup
│   ├── Pages/                       # Razor pages
│   └── Shared/                      # Shared components
└── BusinessLogicTest/               # Unit tests
```

## Migration Context: Azure SQL Edge

The persistence layer is moving from memory-backed repositories to Azure SQL Edge
in Docker. Code written during the migration should be compatible with durable,
relational storage.

Guidelines:
- Do not assume IDs are assigned before save, start at `1`, or are sequential
  without gaps.
- Do not depend on object reference equality after reads; SQL-backed repositories
  may materialize new instances.
- Do not return mutable internal collections from repositories.
- Keep query, mapping, and transaction logic inside repository/infrastructure
  code, not in Blazor pages.
- Keep business rules in `BusinessLogic` and entity invariants in `Domain`.
- Prefer async repository APIs for new SQL-bound work when practical.
- Keep DTO/entity mapping explicit and centralized.

## SQL Migration Safety

When adding SQL schema or data migrations:
- Make migrations small, reviewable, and reversible where possible.
- Separate schema changes from data backfills when that lowers risk.
- Use explicit column types, nullability, indexes, foreign keys, and constraints.
- Avoid destructive changes unless there is a clear migration path and tests.
- Seed only deterministic reference data; do not hide application behavior in seed
  scripts.
- Store connection strings in configuration/user secrets/environment variables,
  not in code.
- Validate migrations against a fresh Azure SQL Edge Docker database before
  assuming they work.

Useful validation checks after a migration:
```sql
SELECT COUNT(*) FROM Movies;
SELECT COUNT(*) FROM Actors;
SELECT COUNT(*) FROM Categories;

SELECT m.Id, m.Title
FROM Movies m
WHERE m.Title IS NULL OR LTRIM(RTRIM(m.Title)) = '';

SELECT m.Id, m.CategoryId
FROM Movies m
LEFT JOIN Categories c ON c.Id = m.CategoryId
WHERE m.CategoryId IS NOT NULL AND c.Id IS NULL;
```

Adjust table/column names to the actual schema. Prefer targeted validation
queries that confirm row counts, required fields, relationships, uniqueness, and
rollback expectations.

## Maintainability Guidance for Agents

- Preserve the layered architecture: UI -> BusinessLogic -> Repository ->
  infrastructure/database.
- Keep Blazor components thin; they should coordinate UI state and call services.
- Do not introduce database-specific concepts into `Domain`.
- Add tests at the service layer for business rules and at the repository layer for
  SQL persistence behavior.
- When replacing memory repositories, keep behavior compatible from the service
  perspective unless the task explicitly changes behavior.
- Prefer clear names over matching current mistakes. For example, new actor or
  category methods should not copy the existing `AddMovie` naming pattern.
- Favor small, incremental migration steps over broad rewrites.

## Git Workflow

This project uses Gitflow.

- Before changing code, always check the current branch.
- Do not implement features directly on `develop`.
- Feature work must happen on a related branch whose name starts with `feat/`,
  for example `feat/sql-repository-abstractions`.
- If the current branch is `develop`, create or switch to an appropriate
  `feat/...` branch before editing.
- Keep commits focused and tied to the work performed.
- Commit after each agent run so the repository records the incremental work done
  by the agent.

## Common Tasks

### Adding a New Entity
1. Create entity class in `Domain/`
2. Add validation logic in property setters
3. Create corresponding repository in `Repository/`
4. Create DTO in `BusinessLogic/Dtos/`
5. Create service in `BusinessLogic/`
6. Add DTO conversion methods (`toEntity()` and `fromEntity()`)
7. Register repository and service in `UserInterface/Program.cs`
8. Add unit tests in `BusinessLogicTest/`

### Adding a New Service Method
1. Add method to service class in `BusinessLogic/`
2. Handle business logic validation and throw `LogicException` on violation
3. Use repository methods for data access
4. Return DTOs, not entities
5. Add corresponding unit tests

### Adding Unit Tests
- Test file should end with `Test.cs`
- Use `[TestInitialize]` to set up fresh dependencies
- Test both success and failure scenarios
- Use descriptive test names: `TestMethod_Scenario_ExpectedResult`
