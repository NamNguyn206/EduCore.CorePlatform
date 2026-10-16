using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Queries.GetById;

public record GetCountryByIdQuery(CountryByIdQueryRequest RequestData) : IRequest<Result<CountryDto>>;