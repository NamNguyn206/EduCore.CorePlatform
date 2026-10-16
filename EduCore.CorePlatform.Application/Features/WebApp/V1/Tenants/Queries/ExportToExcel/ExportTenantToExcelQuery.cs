using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.Tenants.Queries.ExportToExcel;

public record ExportTenantToExcelQuery(ExportTenantToExcelQueryRequest RequestData) : IRequest<Result<byte[]>>;