using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Domain.Common.Enums.MultiTenants.CRM;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs.Commands;

public class UpdateLeadRequest : UpdateRequest
{
    public string FullName { get; init; } = default!;
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public int Score { get; init; }
    public int PredictionPercentage { get; init; }
    public LeadTemperature Temperature { get; init; }
    public string? AiNextAction { get; init; }
    public Guid? AssignedUserId { get; init; }
}