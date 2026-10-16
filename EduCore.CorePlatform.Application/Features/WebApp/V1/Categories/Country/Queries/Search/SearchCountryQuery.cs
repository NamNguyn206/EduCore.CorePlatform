using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Queries.Search;

public record SearchCountryQuery(SearchCountryQueryRequest RequestData) : IRequest<Result<List<CountryLookupDto>>>;