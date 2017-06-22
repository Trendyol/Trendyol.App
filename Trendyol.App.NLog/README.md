### Trendyol.App.NLog
``` 
Install-Package Trendyol.App.NLog
```

Trendyol.App uses Commonlogging internally. This package allows you to plug NLog adapter to Commonlogging infrastructure.
> You also need to add NLog.config to your project root path.

```csharp
TrendyolAppBuilder.Instance
                .UseNLog()
                .Build();
```