using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Integration.User;

public class UserRegisteredIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; }
    public string Username { get; }
    public string Email { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public DateTime RegisteredAt { get; }

    public UserRegisteredIntegrationEvent(Guid userId, string username, string email,
                                         string? firstName, string? lastName, DateTime registeredAt)
    {
        UserId = userId;
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        RegisteredAt = registeredAt;
    }
}