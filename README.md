# PathPolisher

자음모음 분리 현상을 해결하기 위한 tool.

## Usage

### by source

```bash
$ dotnet build && dotnet publish
$ cd ./PathPolisher/bin/Release/net9.0/win-x64/publish

# default: current directory
$ pathpolisher --directory <target directory> # e.g. c:/users/username/documents
$ pathpolisher -d <target directory>
```

### by dotnet tool

```bash
$ dotnet build && dotnet pack

# install dotnet global tool
$ dotnet tool install --global --add-source ./nupkg PathPolisher

# default: current directory
$ polish --directory <target directory> # e.g. c:/users/username/documents
$ polish -d <target directory>
```

```bash
# uninstall
$ dotnet tool uninstall -g PathPolisher
```

## Misc
> build 를 위한 `.csproj` 설정

- for build
```xml
<PropertyGroup>
...
<RuntimeIdentifier>win-x64</RuntimeIdentifier>
<SelfContained>true</SelfContained>
<PublishSingleFile>true</PublishSingleFile>
<IncludeAllContentForSelfExtract>true</IncludeAllContentForSelfExtract>
...
</PropertyGroup>
```

- for dotnet tool
```xml
<PropertyGroup>
...
<PackAsTool>true</PackAsTool>
<ToolCommandName>polish</ToolCommandName>
<PackageOutputPath>./nupkg</PackageOutputPath>
...
</PropertyGroup>
```

## Reference

- [자습서: 시작 System.CommandLine](https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/get-started-tutorial)
