using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.Delete;


public sealed record DeleteCountryCommand(DeleteCountryRequest RequestData) : IRequest<Result>;