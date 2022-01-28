# FTPProgram
Project DUT 2021 - 19.Nh12B - PBL4. Using .NET to create a small FTP program: FTP Server and FTP Client.

# Required Tools
- Visual Studio 2017 or later (recommend Visual Studio 2022);
- .NET Core 3.1 or later

# Screenshots
- Go to [here](SCREENSHOTS.md) for screenshot for FTP Program.

# Build and run
**Option 1:** If you want to open project to modify, you can run Visual Studio and open .sln project file:
- src\FTPClient\FTPClient.sln for FTP Client
- src\FTPServer\FTPServer.sln for FTP Server

**Option 2:** If you want to build and run directly, you can do this using following command:
```
dotnet build
dotnet run
```

# Note about [Build - Option 2](#build-and-run)
**1**: You will need to install .NET Core 3.1 SDK before executing above command. If you installed .NET 5.0 or above and failed to build, you can try to edit .csproj file following for `<TargetFramework>` tag:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>
...
```
to this
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>
...
```
`<TargetFramework>` can be edited to net5.0 or net6.0. [Click here for more info](https://docs.microsoft.com/en-us/dotnet/standard/frameworks).

File path:<br>
- `src\FTPClient\FTPClient.csproj` and `src\FTPClient.Library\FTPClient.Library.csproj` for FTP Client
- `src\FTPServer\FTPServer.csproj` for FTP Server

**2:** You can convert back to .NET Framework if you like.

# Known issue
Following issue can or can't be developed later:
- No IPv6 support.
- Only TYPE binary mode.
- No AUTH, SSL, TLS support.

For more issue, go to tab Issue (GitHub).

# References
- [File Transfer Protocol (FTP) \[RFC0959\] - J. Postel, J. Reynolds, ISI - October 1985](https://filezilla-project.org/specs/rfc0959.txt)
  - Page 39: Return code
  - Page 47: How-to-use command
- [C# FileStream tutorial - ZetCode - last modified 20/09/2021](https://zetcode.com/csharp/filestream/)
- [Creating an FTP Server in C# - with IPv6 Support - Rick Bassham (in Code Project) - 08/10/2013](https://www.codeproject.com/Articles/380769/Creating-an-FTP-Server-in-Csharp-with-IPv6-Support)
