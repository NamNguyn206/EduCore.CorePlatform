using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.Identity;

public class UserDocument : SoftDeleteEntity<Guid>
{
    public Guid UserId { get; private set; }

    public UserDocumentType Type { get; private set; }

    public string FileUrl { get; private set; } = default!;

    //navigation property
    public User User { get; private set; } = default!;

    protected UserDocument()
    {
    }

    public UserDocument(
        Guid userId,
        UserDocumentType type,
        string fileUrl)
    {
        UserId = userId;
        Type = type;
        FileUrl = fileUrl;
    }
}