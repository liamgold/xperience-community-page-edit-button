# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a NuGet package for Xperience by Kentico that provides content editors with a button displayed on web channel pages to quickly navigate to the administration interface for editing that page. The button only appears when a user is authenticated to an admin portal on the **same domain** as the front-end site.

## Technology Stack

- **Framework**: ASP.NET Core 8.0 (Razor SDK)
- **Target**: .NET 8.0
- **Package Manager**: Central Package Management (CPM)
- **Xperience Version**: 29.1.4

## Build and Package Commands

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Build and create NuGet package (automatic via GeneratePackageOnBuild)
dotnet build

# Pack manually if needed
dotnet pack
```

## Project Structure

The package contains three main components that work together:

1. **PageEditButtonViewComponent** (`src/ViewComponents/PageEditButton/PageEditButtonViewComponent.cs`)
   - Main view component that renders the edit button
   - Uses `IPageUrlGenerator` to generate admin URL pointing to the Page Builder tab
   - Checks authentication via `AdminIdentityConstants.APPLICATION_SCHEME`
   - Retrieves current page context via `IWebPageDataContextRetriever`
   - Hides button when preview mode is enabled

2. **PageEditButtonStylesTagHelper** (`src/TagHelpers/PageEditButtonStylesTagHelper.cs`)
   - Tag helper for conditionally rendering CSS stylesheet
   - Uses same authentication and context checks as view component
   - Outputs link to `/_content/XperienceCommunity.PageEditButton/css/edit-page-button.css`
   - Note: Namespace is `Goldfinch.Web.TagHelpers` (not matching project namespace)

3. **Default.cshtml** (`src/Views/Shared/Components/PageEditButton/Default.cshtml`)
   - View template that receives admin URL as model
   - Renders fixed-position button in bottom-right corner

## Authentication and Visibility Logic

Both the TagHelper and ViewComponent implement identical logic:
- Only render if `IWebPageDataContextRetriever.TryRetrieve()` succeeds
- Hide when preview mode is enabled (`HttpContext.Kentico().Preview().Enabled`)
- Only show for authenticated admin users via `HttpContext.AuthenticateAsync(AdminIdentityConstants.APPLICATION_SCHEME)`

## Package Configuration

- Uses Central Package Management via `Directory.Packages.props`
- Package version defined in `.csproj` file (currently 1.0.0)
- Requires nullable reference types enabled (WarningsAsErrors for nullable)
- Includes icon, README, and LICENSE in NuGet package

## Key Dependencies

- `Kentico.Xperience.Admin` - Admin interface integration
- `Kentico.Xperience.webapp` - Web application APIs
- Both pinned to version 29.1.4

## Static Assets

- CSS file located at `src/wwwroot/css/edit-page-button.css`
- Packaged as embedded resource accessible via `_content/XperienceCommunity.PageEditButton/`
