
# Recipes Portal


## Authors

- Naman Singh - [@SubLoadedZA](https://www.github.com/SubLoadedZA)


## Features

- .NET 5.0 ("Distributed app")
- EF Core (DB First)
- Role Based Auth* with JWT
- Flurl HTTP Client
- Bootstrap
- Twitter Summary Cards (see _Layout or inspect head tag in dev tools. Tweet button can be found in the single Recipe page)

## Requirements

.NET 5.0 SDK

MS SQL Server

Visual Studio

## Instructions

~ Install .NET 5.0 SDK if not present on host machine- https://dotnet.microsoft.com/en-us/download/dotnet/5.0

~ Clone the project (or download directly from website)

```bash
  git clone https://www.github.com/subloadedza/recipes-portal/
```

~ Run ALL SQL scripts, in the "SQL" archive, against the RecipesDB database.

~ Open Recipes.sln and change the SQL Connection String in the DependencyInjection.cs file (Recipes.Repository class library/project)

~ Enable 'Multiple startup projects' Start Action for "Recipes" and "Recipes.Web"  

## Login Credentials

Administrator (Create, Read, Update, Delete)

    username: admin 
    password: p@ssw0rd

General User (Read only)

    username: user 
    password: p@ssw0rd


## *Caveats

Role Based Auth is NOT working. I have set it up and and configured the startup.cs file however I think my approach with DI and Repositories and usage of DistributedMemoryCache is causing some issues in regards to authorization/authentication and the ordering of services or handling of session data in this regard. (custom authorization attribute seems to be disregarded entirely). Would love if the marker of this exercise would show me where I went wrong :) 

## Lessons Learned

Project set up with Repository and Unit of Work pattern in mind. Injecting Auth (Authentication/Authorization) caused some services to not be constructed correctly at first. 

Using JSON list, stored in SQL, introduced some challenges with serializing/deserializing to objects and enumeration.
Some time was lost. 

First time working with Twitter cards and role based auth (core) :) 



