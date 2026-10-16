using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Queries;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Queries.GetList;

public record GetCountryListQuery(CountryListQueryRequest RequestData) : IRequest<Result<List<CountryDto>>>;