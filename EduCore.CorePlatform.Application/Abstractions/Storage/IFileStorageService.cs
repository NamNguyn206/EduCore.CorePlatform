using EduCore.CorePlatform.Application.DTOs.Shared.Files;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace EduCore.CorePlatform.Application.Abstractions.Storage;

public interface IFileStorageService
{
    Task<Stream> DownloadFileAsync(string filePath);
    Task<bool> DeleteFileAsync(string filePath);
    Task<bool> FileExistsAsync(string filePath);

    Task<FileMetadataDto> GetFileMetadataAsync(string filePath);
    Task UpdateFileMetadataAsync(string filePath, Dictionary<string, string> metadata);

    Task CreateDirectoryAsync(string directoryPath);
    Task<bool> DirectoryExistsAsync(string directoryPath);
    Task<IReadOnlyList<string>> ListFilesAsync(string directoryPath, string? pattern = null);
    Task<IReadOnlyList<string>> ListDirectoriesAsync(string directoryPath);

    Task CopyFileAsync(string sourcePath, string destinationPath);
    Task MoveFileAsync(string sourcePath, string destinationPath);
    Task<byte[]> ReadAllBytesAsync(string filePath);
    Task<string> ReadAllTextAsync(string filePath);

    Task<Stream> GenerateThumbnailAsync(string imagePath, int width, int height);
    Task<Stream> ResizeImageAsync(string imagePath, int maxWidth, int maxHeight);

    Task<bool> ValidateFileTypeAsync(Stream fileStream, string fileName, IEnumerable<string> allowedExtensions);
    Task<bool> ValidateFileSizeAsync(Stream fileStream, long maxSizeBytes);

    Task CleanupTempFilesAsync(TimeSpan olderThan);

    Task<string> GenerateSecureFilePathAsync(string fileName, Guid? tenantId = null, Guid? userId = null);
    Task<string> GeneratePresignedUrlAsync(string filePath, TimeSpan expiry);

    Task BackupFilesAsync(string sourceDirectory, string backupPath);
    Task RestoreFilesAsync(string backupPath, string targetDirectory);
    Task<string> UploadAsync(Stream fileStream, string fileName, string folder);
}