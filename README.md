# dotnet-optionspattern

## 1. Introduction

This repository aims to provide a clear and concise demonstration of how the options interfaces in ASP.NET Core, namely `IOptions<T>`, `IOptionsSnapshot<T>`, and `IOptionsMonitor<T>`, can be effectively utilized in the context of a real ASP.NET Core project.

These options interfaces are of paramount importance in ASP.NET Core applications as they enable the implementation of strongly-typed configurations. Each of them possesses distinctive features and offers unique advantages in terms of configuration management, whether it be for simple use, dynamic configuration, or real-time refresh of configuration settings.

In this repository, we will explore these options interfaces, discuss their respective advantages, demonstrate their implementation through code examples, and show how to judiciously choose among them based on your specific configuration needs.

Whether you are a beginner or an experienced developer with ASP.NET Core, this guide aims to help you understand and master the use of options interfaces for better configuration management in your ASP.NET Core applications. We hope you find it useful!

## 2. ASP.NET Core Options Interfaces

ASP.NET Core provides several ways to handle strongly-typed configurations through the implementation of `IOptions<T>`, `IOptionsSnapshot<T>`, and `IOptionsMonitor<T>` interfaces. Each of these interfaces has specific characteristics and serves distinct objectives to meet various configuration needs.

In this section, we will delve into each interface in detail, discuss their pros and cons, and illustrate how to use them in your code.

### 2.1 IOptions<T>

The `IOptions<T>` interface is the original options interface in ASP.NET Core. It is particularly useful when you do not need to reload the configuration on the fly or to name options. Here are some key points to note about `IOptions<T>`:

- It is registered as a singleton service and can be injected anywhere.
- It binds configuration values only once at the registration, and returns the same values every time.
- It does not support configuration reloading or named options.

Here is how one might implement `IOptions<T>` in code:

##### Configuration in Program.cs
```csharp
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
```

##### Injection in a Controller
```csharp
private readonly EmailSettings _emailSettingsOptions;

public NotificationsController(IOptions<EmailSettings> emailOptions) {
    _emailSettingsOptions = emailOptions.Value;
}
```

##### Usage
```csharp
[HttpGet("email-options")]
public IActionResult GetEmailOptions() => Ok(Map(_emailSettingsOptions));
```

In this example, email settings are retrieved from the configuration and mapped into an `EmailSettings` object using `IOptions<T>`. These settings are then available for use throughout the application via dependency injection.

### 2.2 IOptionsSnapshot<T>

The `IOptionsSnapshot<T>` interface is an extension of `IOptions<T>` that offers more functionalities. It is particularly suitable if you want to reload configuration on-the-fly and use named options. Here are some key points to note about `IOptionsSnapshot<T>`:

- It is registered as a service with a scope limited to the request.
- It supports configuration reloading.
- It cannot be injected into singleton services.
- Values are reloaded with each request.
- It supports named options.

Here is how one might implement `IOptionsSnapshot<T>` in code:

##### Configuration in Program.cs
```csharp
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
```

##### Injection in a Controller
```csharp
private readonly EmailSettings _emailSettingsOptionsSnapshot;

public NotificationsController(IOptionsSnapshot<EmailSettings> emailOptionsSnapshot) {
    _emailSettingsOptionsSnapshot = emailOptionsSnapshot.Value;
}
```

##### Usage
```csharp
[HttpGet("email-options-snapshot")]
public IActionResult GetEmailOptionsSnapShot() => Ok(Map(_emailSettingsOptionsSnapshot));
```

In this example, the `EmailSettings` object is configured in the same way as for `IOptions<T>`. However, thanks to `IOptionsSnapshot<T>`, changes made to the configuration at runtime are reflected in `_emailSettingsOptionsSnapshot` with each new HTTP request, thus enabling dynamic configuration reloading.

### 2.3 IOptionsMonitor<T>

The `IOptionsMonitor<T>` interface is another extension of `IOptions<T>` that offers even more functionalities. It is particularly useful if you want to reload configuration on-the-fly, use named options, and inject these options into singleton services. Here are some key points to note about `IOptionsMonitor<T>`:

- It is registered as a singleton service.
- It supports configuration reloading.
- It can be injected into any service of any lifetime.
- Values are cached and reloaded immediately when the configuration changes.
- It supports named options.

Here is how one might implement `IOptionsMonitor<T>` in code:

##### Configuration in Program.cs
```csharp
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
```

##### Injection in a Controller
```csharp
private EmailSettings _emailSettingsOptionsMonitor;

public NotificationsController(IOptionsMonitor<EmailSettings> emailOptionsMonitor) {
    _emailSettingsOptionsMonitor = emailOptionsMonitor.CurrentValue;
    emailOptionsMonitor.OnChange(settings => HandleEmailSettingsChanges(settings));
}

private static void HandleEmailSettingsChanges(EmailSettings settings)
{
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine("Changed");
    Console.WriteLine("From: {0}", settings.From);
    Console.ResetColor();
}
```

##### Usage
```csharp
[HttpGet("email-options-monitor")]
public IActionResult GetEmailOptionsMonitor() => Ok(Map(_emailSettingsOptionsMonitor));
```

In this example, the `EmailSettings` object is configured in the same way as for `IOptions<T>` and `IOptionsSnapshot<T>`. However, thanks to `IOptionsMonitor<T>`, changes made to the configuration at runtime are reflected in `_emailSettingsOptionsMonitor` immediately after the configuration change. Moreover, the `OnChange` method allows an action to be performed each time the configuration changes, which is demonstrated by the console of the changed information.

![Alt text](image.png)

## 3. How to Choose the Right Options Interface

Although ASP.NET Core offers three options interfaces, the best one to use depends on your specific needs. If you do not need to reload the configuration on-the-fly and you do not need named options, `IOptions<T>` will be the best choice. If you need to reload the configuration on-the-fly and use named options, you can use either `IOptionsSnapshot<T>` or `IOptionsMonitor<T>`. 

The main difference between these last two is that `IOptionsMonitor<T>` can be injected into other singleton services while `IOptionsSnapshot<T>` cannot. Additionally, `IOptionsMonitor<T>` reloads values immediately when a configuration change occurs, while `IOptionsSnapshot<T>` reloads values for each new request.

## 4. Comparison of Options Interfaces

To help you make an informed decision, here is a summary table of the characteristics of each options interface:

| Feature | IOptions<T> | IOptionsSnapshot<T> | IOptionsMonitor<T> |
| --- | --- | --- | --- |
| Service Registration Type | Singleton | Scoped | Singleton |
| Supports Configuration Reloading | No | Yes | Yes |
| Can Be Injected Into Any Service | Yes | No | Yes |
| Supports Named Options | No | Yes | Yes |
| Value Reload Timing | At Application Startup | At Each Request | Immediately After A Change |

## 5. Conclusion

The options interfaces in ASP.NET Core offer a flexible and powerful way to manage strongly typed configurations in your application. Depending on your specific needs for configuration reloading and named options, you can choose between `IOptions<T>`, `IOptionsSnapshot<T>`, and `IOptionsMonitor<T>`. 

Remember that the main goal is to keep your code clean, readable, and maintainable. By understanding these options interfaces well and using them wisely, you can greatly improve the quality and testing of your code and its long-term maintenance.