using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.Tenants.Commands.ImportFromExcel;

public sealed record ImportTenantFromExcelCommand(ImportTenantFromExcelRequest RequestData) : IRequest<Result>;