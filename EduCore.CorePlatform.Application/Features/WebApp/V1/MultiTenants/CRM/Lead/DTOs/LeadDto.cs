using EduCore.CorePlatform.Domain.Common.Enums.MultiTenants.CRM;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs;

public class LeadDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int Score { get; set; }
    public int PredictionPercentage { get; set; }
    public LeadTemperature Temperature { get; set; }
    public string? AiNextAction { get; set; }
    public Guid? AssignedUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}