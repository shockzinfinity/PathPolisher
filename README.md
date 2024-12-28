# PathPolisher

자음모음 분리 현상을 해결하기 위한 tool.

## Usage

### by source

```bash
$ dotnet publish
$ cd ./PathPolisher/bin/Release/net9.0/win-x64/publish
# default: current directory
$ pathpolisher --directory <target directory> # e.g. c:/users/username/documents
```

### by dotnet tool

```bash
$ dotnet pack
# install dotnet global tool
$ dotnet tool install --global --add-source ./nupkg PathPolisher
# default: current directory
$ polish --directory <target directory> # e.g. c:/users/username/documents
```

```bash
# uninstall
$ dotnet tool uninstall -g PathPolisher
```

## Reference

- [자습서: 시작 System.CommandLine](https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/get-started-tutorial)
