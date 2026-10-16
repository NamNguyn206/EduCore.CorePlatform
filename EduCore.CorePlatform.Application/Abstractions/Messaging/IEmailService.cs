using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace EduCore.CorePlatform.Application.Abstractions.Messaging;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default);
    
    Task SendTemplatedEmailAsync(string templateName, Dictionary<string, object> parameters, string to, CancellationToken cancellationToken = default);
    
    Task SendVerificationEmailAsync(string email, string verificationToken, CancellationToken cancellationToken = default);
    Task SendPasswordResetEmailAsync(string email, string resetToken, CancellationToken cancellationToken = default);
    Task SendWelcomeEmailAsync(string email, string userName, CancellationToken cancellationToken = default);

    
    Task DeleteEmailTemplateAsync(string templateName, CancellationToken cancellationToken = default);

    
    Task TestConnectionAsync(CancellationToken cancellationToken = default);
    
}