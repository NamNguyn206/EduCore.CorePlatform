using EduCore.CorePlatform.Domain.Common.Enums.MultiTenants.CRM;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.MultiTenants.CRM;

public class Lead : MultiTenantEntity<Guid, Guid>
{
    public string FullName { get; private set; } = default!;
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }

    // Điểm số & Dự đoán
    public int Score { get; private set; }
    public int PredictionPercentage { get; private set; }
    public LeadTemperature Temperature { get; private set; } = LeadTemperature.Cold;

    // AI Next Action
    public string? AiNextAction { get; private set; }

    // Phân công phụ trách (VD: huyphongt25 - Nhân viên tuyển sinh)
    public Guid? AssignedUserId { get; private set; }
    public User? AssignedUser { get; private set; }

    // Navigation properties cho Hành vi Test (Hub, Quiz, Event)
    private readonly List<LeadActivity> _activities = new();
    public IReadOnlyCollection<LeadActivity> Activities => _activities;

    protected Lead() { }

    public Lead(string fullName, Guid tenantId, string? phoneNumber = null, string? email = null)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
        Score = 0;
        PredictionPercentage = 0;
        Temperature = LeadTemperature.Cold;
        CreatedAt = DateTime.UtcNow;

        SetTenant(tenantId);
    }

    public void UpdateScoreAndPrediction(int score, int predictionPercentage, LeadTemperature temperature)
    {
        Score = score;
        PredictionPercentage = predictionPercentage;
        Temperature = temperature;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAiNextAction(string action)
    {
        AiNextAction = action;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignToUser(Guid userId)
    {
        AssignedUserId = userId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddActivity(ActivityType type, string? details = null)
    {
        _activities.Add(new LeadActivity(Id, type, details));
    }
}