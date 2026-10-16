using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Queries.ExportToExcel;

public record ExportSupplyCategoryToExcelQuery(ExportSupplyCategoryToExcelQueryRequest RequestData) : IRequest<Result<byte[]>>;