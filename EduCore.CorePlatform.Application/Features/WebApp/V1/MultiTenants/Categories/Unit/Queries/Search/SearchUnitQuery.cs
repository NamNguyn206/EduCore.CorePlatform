using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Queries.Search;

public record SearchUnitQuery(SearchUnitQueryRequest RequestData) : IRequest<Result<List<UnitLookupDto>>>;