using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Commands;

public class CreateCountryRequest
{
    public string Name { get; set; } = default!;
}
