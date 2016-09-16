# 7days2mod-core

## Setup 

Create a config file (or set the values AppSettings:GitHubClientId and AppSettings:GitHubClientSecret as environment variables on the server)

For example:
In the root project folder __appsettings.Development.json__ or __appsettings.Production.json__ depending on the value of ASPNETCORE_ENVIRONMENT

```
{
  "AppSettings": {
    "GitHubClientId": "",
    "GitHubClientSecret": ""
  }
}
```

You can also set these values in the __appsettings.json__ in the AppSettings section but never store these values in source control.

To obtain these values visit https://github.com/settings/applications/new and put the base url (where the app is served from) as both the Homepage URL and Authorization callback URL.

