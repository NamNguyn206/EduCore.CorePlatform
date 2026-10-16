using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Exceptions;

public class ForbiddenAccessException : ApplicationException
{
    public ForbiddenAccessException()
        : base("You don't have permission to access this resource")
    {
    }

    public ForbiddenAccessException(string message)
        : base(message)
    {
    }
}