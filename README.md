# DotWikiApi

Rest api written in ASP.NET Core for management of a wikipedia-like system

## Setup

### Requirements

[Dotnet](https://dotnet.microsoft.com/download)

> Postgres is the used dbms
>
> Create the database `dotwiki` in postgres and add the _**UserId**_ and _**Password**_ credentials
> You can add them by adding in the environment or by using the secret store

```shell
git clone https://github.com/Ola-jed/DotWikiApi.git
cd DotWikiApi/DotWikiApi
dotnet ef database update
dotnet run
```
