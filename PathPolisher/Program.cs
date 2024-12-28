using System.CommandLine;

internal class Program
{
  private static async Task<int> Main(string[] args)
  {
    var rootCommand = new RootCommand("파일/디렉토리 이름 자음모음분리 현상 해결을 위한 툴");

    var directoryParameter = new Option<DirectoryInfo>(
      aliases: ["--directory", "-d"],
      getDefaultValue: () => new DirectoryInfo(Environment.CurrentDirectory),
      description: "Target directory"
      );

    rootCommand.AddOption(directoryParameter);

    rootCommand.SetHandler((directoryInfo) =>
    {
      try
      {
        if (directoryInfo is null || !directoryInfo.Exists)
        {
          Console.WriteLine($"오류: 지정된 디렉터리 '{directoryInfo?.FullName}'는 존재하지 않습니다.");
          return;
        }

        Console.WriteLine($"대상 디렉토리: '{directoryInfo.FullName}'");
        NormalizeDirectory(directoryInfo.FullName);
      }
      catch (DirectoryNotFoundException ex)
      {
        Console.WriteLine($"대상 디렉터리를 확인할 수 없음: {ex.Message}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"알 수 없는 오류 발생: {ex.Message}");
      }
    }, directoryParameter);

    return await rootCommand.InvokeAsync(args);
  }

  private static void NormalizeDirectory(string path)
  {
    Parallel.ForEach(Directory.GetFiles(path), HandleFileNormalization);

    Parallel.ForEach(Directory.GetDirectories(path), HandleDirectoryNormalization);
  }

  private static void HandleFileNormalization(string file)
  {
    try
    {
      string filename = Path.GetFileName(file);
      if (!filename.IsNormalized() && !IsSystemOrHidden(file))
      {
        string normalizedFileName = filename.Normalize();
        string normalizedPath = Path.Combine(Path.GetDirectoryName(file)!, normalizedFileName);

        if (!File.Exists(normalizedPath) && !file.Equals(normalizedPath, StringComparison.OrdinalIgnoreCase))
        {
          File.Move(file, normalizedPath);
          Console.WriteLine($"파일 정규화: {filename} -> {normalizedFileName}");
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"파일 정규화 중 오류 발생: {ex.Message}");
    }
  }

  private static void HandleDirectoryNormalization(string dir)
  {
    try
    {
      string dirName = Path.GetFileName(dir);
      string parentPath = Path.GetDirectoryName(dir)!;

      if (!dirName.IsNormalized() && !IsSystemOrHidden(dir))
      {
        string normalizedDirName = dirName.Normalize();
        string normalizedDirPath = Path.Combine(parentPath, normalizedDirName);

        if (!Directory.Exists(normalizedDirPath) && !dir.Equals(normalizedDirPath, StringComparison.OrdinalIgnoreCase))
        {
          Directory.Move(dir, normalizedDirPath);
          Console.WriteLine($"디렉토리 정규화: {dirName} -> {normalizedDirName}");

          NormalizeDirectory(normalizedDirPath);
        }
        else
        {
          NormalizeDirectory(dir);
        }
      }
      else
      {
        NormalizeDirectory(dir);
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"디렉토리 정규화 중 오류 발생: {ex.Message}");
    }
  }

  private static bool IsSystemOrHidden(string path)
  {
    var attributes = File.GetAttributes(path);
    return (attributes & FileAttributes.Hidden) == FileAttributes.Hidden ||
      (attributes & FileAttributes.System) == FileAttributes.System;
  }
}