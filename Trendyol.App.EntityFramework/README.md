### Trendyol.App.EntityFramework
``` 
Install-Package Trendyol.App.EntityFramework
```

Trendyol.App.EntityFramework provides an easy way to initialize DbContext instances with some predefined settings.

```csharp
TrendyolAppBuilder.Instance
                    .UseDataContext<SampleDataContext>()
                        .WithAutomaticMigrations<Configuration>()
                        .Then()
                    .Build();
```