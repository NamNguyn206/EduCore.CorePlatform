using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.RefreshToken.DTOs;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.RefreshToken.Commands;

public sealed record RefreshTokenCommand(RefreshTokenRequest RequestData) : IRequest<Result<LoginResponseDto>>;
