using EduCore.CorePlatform.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.Logout.DTOs;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.Logout.Commands;

public sealed record LogoutCommand(LogoutRequest RequestData) : IRequest<Result>;
