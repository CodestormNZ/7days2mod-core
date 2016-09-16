# 7days2mod-core

## Installation

Currently only the source code is available. To install and run you will need Visual Studio. 

[Visual Studio Community Edition](https://www.visualstudio.com/products/visual-studio-community-vs)

Once you have that installed you can open this project and run it to create a local instance server (You can get the repo directly into visual studio from the download button, select open in visual studio). For non localhost access you will need to publish to a server.

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

