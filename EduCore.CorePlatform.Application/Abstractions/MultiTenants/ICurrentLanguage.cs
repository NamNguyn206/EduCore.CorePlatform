using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Abstractions.MultiTenants;

public interface ICurrentLanguage
{
    string LanguageCode { get; }
}