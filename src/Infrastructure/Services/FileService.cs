using Microsoft.AspNetCore.Http;
using Backend.Application.Common.Interfaces;

namespace Backend.Infrastructure.Services;

public class FileService : IFileService
{
    private Task<byte[]> GetFileBytesAsync(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return Task.FromResult(memoryStream.ToArray());
    }

    private string GetFileExtension(string file)
    {
        return file.Substring(file.IndexOf("/") + 1, file.IndexOf(";") - file.IndexOf("/") - 1);
    }

    private string GetDirectoryPathAndCreateIfNotExists(string folderName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    private string GetFilePath(string fileName, string folderName)
    {
        return Path.Combine(folderName, fileName);
    }

    public Task<string> SaveFileAsync(string file, string fileName, string folderName)
    {
        var fileExtension = GetFileExtension(file);
        fileName = $"{fileName}{fileExtension}";
        var path = GetDirectoryPathAndCreateIfNotExists(folderName);
        path = Path.Combine(path, fileName);
        var bytes = Convert.FromBase64String(file);
        File.WriteAllBytes(path, bytes);

        return Task.FromResult(GetFilePath(fileName, folderName));
    }

    public async Task<string> SaveFileAsync(IFormFile file, string fileName, string folderName)
    {
        var fileExtension = Path.GetExtension(file.FileName);
        fileName = $"{fileName}{fileExtension}";
        var path = GetDirectoryPathAndCreateIfNotExists(folderName);
        path = Path.Combine(path, fileName);
        File.WriteAllBytes(path, await GetFileBytesAsync(file));

        return GetFilePath(fileName, folderName);
    }

    public Task DeleteFileAsync(string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        return Task.CompletedTask;
    }

    public Task<string> GetFileAsBase64Async(string fileName, string folderName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, fileName);
        if (!File.Exists(path))
        {
            return Task.FromResult(string.Empty);
        }
        var bytes = File.ReadAllBytes(path);
        return Task.FromResult(Convert.ToBase64String(bytes));
    }
}
