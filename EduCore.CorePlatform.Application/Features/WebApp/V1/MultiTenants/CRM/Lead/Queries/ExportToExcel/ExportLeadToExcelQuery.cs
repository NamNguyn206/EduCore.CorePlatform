using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs.Queries;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Queries.ExportToExcel;

public record ExportLeadToExcelQuery(ExportLeadToExcelQueryRequest RequestData) : IRequest<Result<byte[]>>;

