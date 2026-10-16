using EduCore.CorePlatform.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.RefreshToken.DTOs;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.RefreshToken.Commands;

public sealed record RefreshTokenCommand(RefreshTokenRequest RequestData) : IRequest<Result<LoginResponseDto>>;
