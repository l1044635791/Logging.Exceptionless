# Logging.Exceptionless

[![NuGet](https://img.shields.io/nuget/v/Logging.Exceptionless.svg)](https://www.nuget.org/packages/Logging.Exceptionless/)

Exceptionless日志扩展组件

## 快速开始

- NuGet获取[Logging.Exceptionless](https://www.nuget.org/packages/Logging.Exceptionless/)

- 示例

```json
appsettings.json

{
    "Exceptionless": {
        "ApiKey": "你的apikey",
        "ServerUrl": "https://test.exceptionless.io",
        "enableSSL": true,
        "DefaultData": {
        "JSON_OBJECT": "{ \"Name\": \"Blake\" }",
        "Boolean": true,
        "Number": 1,
        "Array": "1,2,3"
        },
        "DefaultTags": ["exceptionless"],
        "Settings": {
            "FeatureXYZEnabled": false
        }
    },
}
```

```csharp
Startup.cs

public Startup (IConfiguration configuration) {
    Configuration = configuration;
}

public IConfiguration Configuration { get; }

public void Configure (IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
    if (env.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
    } else {
        app.UseExceptionHandler ("/Error");
        app.UseHsts ();
    }

    // 启用 Exceptionless
    app.UseExceptionless (Configuration);

    app.UseHttpsRedirection ();

    app.UseStaticFiles ();

    // 启用默认路由
    app.UseMvc (routes => {
        routes.MapRoute (
            name: "default",
            template: "{controller}/{action=Index}/{id?}");
    });
}
```

```csharp
Program.cs

public static IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .ConfigureAppConfiguration((builderContext, config) =>
        {
            config.AddEnvironmentVariables();
        })
        .ConfigureLogging((hostingContext, builder) =>
        {
            builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            builder.AddConsole();
            builder.AddDebug();
            // 添加 Exceptionless 日志功能
            builder.AddExceptionless (options => {
                options.Delay = 200;
                options.MaxQueueData = 50;
            });
        })
        .Build();
```

## 环境要求

- 需要`.NET Core 2.0` 及以上.

## 参考文档

- [文档](https://github.com/exceptionless/Exceptionless/wiki)
