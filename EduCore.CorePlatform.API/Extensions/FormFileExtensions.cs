﻿using EduCore.CorePlatform.Application.Common.Models;

namespace EduCore.CorePlatform.API.Extensions;

public static class FormFileExtensions
{
    public static UploadFileRequest ToUploadFileRequest(
        this IFormFile file)
    {
        return new UploadFileRequest
        {
            Stream = file.OpenReadStream(),
            FileName = file.FileName,
            ContentType = file.ContentType
        };
    }

    public static List<UploadFileRequest> ToUploadFileRequests(
        this IEnumerable<IFormFile>? files)
    {
        if (files == null)
            return [];

        return files
            .Select(x => x.ToUploadFileRequest())
            .ToList();
    }
}