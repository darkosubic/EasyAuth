# EasyAuth

The goal of this project is to find out how to disable built-in Endpoints of ASP.NET Core Identity 

**Project setup**
1. Clone project
2. Run migration by running Update-Database in the Package Manager Console
3. In Program.cs edit MapIdentityApiFilterable properties to include or exclude routes
4. Run App

**Where is actual logic?**
[here](https://github.com/darkosubic/EasyAuth/blob/master/Overrides/IdentityApiEndpointRouteBuilderExtensions.cs)

**How does it work?**
Code from [WebApplication](https://github.com/dotnet/aspnetcore/blob/1b454f57c664c1d43348c6c74cde7a2119813c8a/src/DefaultBuilder/src/WebApplication.cs#L271) is coppied to this project and modiffied to exclude Endpoints as needed. This is controlled by a [builder class](https://github.com/darkosubic/EasyAuth/blob/master/Overrides/IdentityApiEndpointRouteBuilderOptions.cs).

**How can I filter out routes?**
Use something like this in the Program.cs:
```
app.MapIdentityApiFilterable<User>(new IdentityApiEndpointRouteBuilderOptions()
{
    ExcludeRegisterPost = true,
    ExcludeLoginPost = true,
    ExcludeRefreshPost = true,
    ExcludeConfirmEmailGet = true,
    ExcludeResendConfirmationEmailPost = true,
    ExcludeForgotPasswordPost = true,
    ExcludeResetPasswordPost = true,
    // setting ExcludeManageGroup to false will disable
    // 2FA and both Info Actions
    ExcludeManageGroup = true,
    Exclude2faPost = true,
    ExcludegInfoGet = true,
    ExcludeInfoPost = true,
});
```

More info can be found on [my Medium post](https://darko-subic.medium.com/how-to-disable-asp-net-core-identity-auto-generated-routes-6dbd09b5e815).
