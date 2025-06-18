using Microsoft.AspNetCore.Http;

namespace Backend.Application.Common.Interfaces;

public interface IFileService
{
    /// <summary>
    /// Takes a base64 string and saves it as an image file
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileName"></param>
    /// <param name="folderName"></param>
    /// <returns></returns>
    Task<string> SaveFileAsync(string file, string fileName, string folderName);


    /// <summary>
    /// Takes a byte array and saves it as an image file
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileName"></param>
    /// <param name="folderName"></param>
    /// <returns></returns>
    Task<string> SaveFileAsync(IFormFile file, string fileName, string folderName);


    /// <summary>
    /// Deletes an image file
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task DeleteFileAsync(string fileName);

    /// <summary>
    /// Returns an image file as a base64 string
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="folderName"></param>
    /// <returns></returns>
    Task<string> GetFileAsBase64Async(string fileName, string folderName);

}
