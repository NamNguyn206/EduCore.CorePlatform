using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Queries.ExportToExcel;

public record ExportCountryToExcelQuery(ExportCountryToExcelQueryRequest RequestData) : IRequest<Result<byte[]>>;