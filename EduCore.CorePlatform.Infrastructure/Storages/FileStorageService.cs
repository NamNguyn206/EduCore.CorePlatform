using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.DTOs.Shared.Files;
using EduCore.CorePlatform.Domain.Common.Settings.System;
using EduCore.CorePlatform.Application.Abstractions.Storage;

namespace EduCore.CorePlatform.Infrastructure.Storages;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;
    private readonly FileStorageSettings _fileStorageSettings;
    public FileStorageService(IWebHostEnvironment env,
                            IOptions<FileStorageSettings> fileStorageSettings)
    {
        _env = env;
        _fileStorageSettings = fileStorageSettings.Value;
    }

    private string GetFullPath(string path)
    {
        return Path.Combine(_env.WebRootPath, path.TrimStart('/'));
    }

    // =========================
    // BASIC
    // =========================

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string folder)
    {
        // 1. Tạo đường dẫn thư mục (ví dụ: wwwroot/uploads/assets)
        var relativeFolder = Path.Combine(_fileStorageSettings.DownloadRootPath, folder);
        var directory = GetFullPath(relativeFolder);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        // 2. Tạo tên file duy nhất để tránh ghi đè
        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
        var filePath = Path.Combine(directory, uniqueFileName);

        // 3. Lưu file từ Stream
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }

        // 4. Trả về đường dẫn tương đối để lưu vào Database (ví dụ: uploads/assets/abc.jpg)
        return Path.Combine(relativeFolder, uniqueFileName).Replace("\\", "/");
    }

    public void DeleteFile(string path)
    {
        var fullPath = GetFullPath(path);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public Task<Stream> DownloadFileAsync(string filePath)
    {
        var fullPath = GetFullPath(filePath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException();

        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        return Task.FromResult(stream);
    }

    public Task<bool> DeleteFileAsync(string filePath)
    {
        var fullPath = GetFullPath(filePath);

        if (!File.Exists(fullPath))
            return Task.FromResult(false);

        File.Delete(fullPath);
        return Task.FromResult(true);
    }

    public Task<bool> FileExistsAsync(string filePath)
    {
        return Task.FromResult(File.Exists(GetFullPath(filePath)));
    }

    // =========================
    // METADATA
    // =========================

    public Task<FileMetadataDto> GetFileMetadataAsync(string filePath)
    {
        var fullPath = GetFullPath(filePath);

        var fileInfo = new FileInfo(fullPath);

        return Task.FromResult(new FileMetadataDto
        {
            FileName = fileInfo.Name,
            Size = fileInfo.Length,
            Created = fileInfo.CreationTime,
            Modified = fileInfo.LastWriteTime,
            Extension = fileInfo.Extension
        });
    }

    public Task UpdateFileMetadataAsync(string filePath, Dictionary<string, string> metadata)
    {
        // Local FS không support metadata custom → có thể lưu DB nếu cần
        return Task.CompletedTask;
    }

    // =========================
    // DIRECTORY
    // =========================

    public Task CreateDirectoryAsync(string directoryPath)
    {
        Directory.CreateDirectory(GetFullPath(directoryPath));
        return Task.CompletedTask;
    }

    public Task<bool> DirectoryExistsAsync(string directoryPath)
    {
        return Task.FromResult(Directory.Exists(GetFullPath(directoryPath)));
    }

    public Task<IReadOnlyList<string>> ListFilesAsync(string directoryPath, string? pattern = null)
    {
        var path = GetFullPath(directoryPath);

        if (!Directory.Exists(path))
            return Task.FromResult((IReadOnlyList<string>)new List<string>());

        var files = Directory.GetFiles(path, pattern ?? "*.*")
            .Select(Path.GetFileName)
            .ToList();

        return Task.FromResult((IReadOnlyList<string>)files);
    }

    public Task<IReadOnlyList<string>> ListDirectoriesAsync(string directoryPath)
    {
        var path = GetFullPath(directoryPath);

        if (!Directory.Exists(path))
            return Task.FromResult((IReadOnlyList<string>)new List<string>());

        var dirs = Directory.GetDirectories(path)
            .Select(Path.GetFileName)
            .ToList();

        return Task.FromResult((IReadOnlyList<string>)dirs);
    }

    // =========================
    // FILE OPS
    // =========================

    public Task CopyFileAsync(string sourcePath, string destinationPath)
    {
        File.Copy(GetFullPath(sourcePath), GetFullPath(destinationPath), true);
        return Task.CompletedTask;
    }

    public Task MoveFileAsync(string sourcePath, string destinationPath)
    {
        File.Move(GetFullPath(sourcePath), GetFullPath(destinationPath), true);
        return Task.CompletedTask;
    }

    public Task<byte[]> ReadAllBytesAsync(string filePath)
    {
        return File.ReadAllBytesAsync(GetFullPath(filePath));
    }

    public Task<string> ReadAllTextAsync(string filePath)
    {
        return File.ReadAllTextAsync(GetFullPath(filePath));
    }

    // =========================
    // IMAGE (basic stub)
    // =========================

    public Task<Stream> GenerateThumbnailAsync(string imagePath, int width, int height)
    {
        // TODO: dùng ImageSharp hoặc SkiaSharp
        return DownloadFileAsync(imagePath);
    }

    public Task<Stream> ResizeImageAsync(string imagePath, int maxWidth, int maxHeight)
    {
        return DownloadFileAsync(imagePath);
    }

    // =========================
    // VALIDATION
    // =========================

    public async Task<bool> ValidateFileTypeAsync(Stream fileStream, string fileName, IEnumerable<string> allowedExtensions)
    {
        var ext = Path.GetExtension(fileName).ToLower();
        return allowedExtensions.Contains(ext);
    }

    public async Task<bool> ValidateFileSizeAsync(Stream fileStream, long maxSizeBytes)
    {
        return fileStream.Length <= maxSizeBytes;
    }

    // =========================
    // STORAGE
    // =========================

    public Task CleanupTempFilesAsync(TimeSpan olderThan)
    {
        var tempPath = Path.Combine(_env.WebRootPath, "temp");

        if (!Directory.Exists(tempPath))
            return Task.CompletedTask;

        var files = Directory.GetFiles(tempPath);

        foreach (var file in files)
        {
            var info = new FileInfo(file);
            if (DateTime.Now - info.CreationTime > olderThan)
                File.Delete(file);
        }

        return Task.CompletedTask;
    }

    // =========================
    // SECURITY
    // =========================

    public Task<string> GenerateSecureFilePathAsync(string fileName, Guid? tenantId = null, Guid? userId = null)
    {
        var safeName = $"{tenantId}_{userId}_{Guid.NewGuid()}_{fileName}";
        return Task.FromResult($"secure/{safeName}");
    }

    public Task<string> GeneratePresignedUrlAsync(string filePath, TimeSpan expiry)
    {
        // Local: fake URL
        return Task.FromResult($"/{filePath}?expires={DateTime.UtcNow.Add(expiry):O}");
    }

    // =========================
    // BACKUP
    // =========================

    public Task BackupFilesAsync(string sourceDirectory, string backupPath)
    {
        var source = GetFullPath(sourceDirectory);
        var backup = GetFullPath(backupPath);

        Directory.CreateDirectory(backup);

        foreach (var file in Directory.GetFiles(source))
        {
            var dest = Path.Combine(backup, Path.GetFileName(file));
            File.Copy(file, dest, true);
        }

        return Task.CompletedTask;
    }

    public Task RestoreFilesAsync(string backupPath, string targetDirectory)
    {
        var backup = GetFullPath(backupPath);
        var target = GetFullPath(targetDirectory);

        Directory.CreateDirectory(target);

        foreach (var file in Directory.GetFiles(backup))
        {
            var dest = Path.Combine(target, Path.GetFileName(file));
            File.Copy(file, dest, true);
        }

        return Task.CompletedTask;
    }
}