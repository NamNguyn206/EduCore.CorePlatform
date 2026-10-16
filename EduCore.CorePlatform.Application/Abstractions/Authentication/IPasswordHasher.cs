using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Abstractions.Authentication;

/// <summary>
/// Password hashing abstraction
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string passwordHash);
}
