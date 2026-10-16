using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.Login.DTOs;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.Login.Commands;

public sealed record LoginCommand(LoginRequest Request) : IRequest<Result<LoginResponseDto>>;