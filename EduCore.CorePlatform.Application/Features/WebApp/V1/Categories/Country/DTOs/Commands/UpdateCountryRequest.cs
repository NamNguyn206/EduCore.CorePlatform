using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Commands;

public class UpdateCountryRequest : UpdateRequest
{
    public string Name { get; init; } = default!;
}
