using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Common.Entities;

namespace EduCore.CorePlatform.Domain.Entities.StudentManagement;

public class Student : AggregateRoot<Guid>
{
    // Thông tin sinh viên
    public string StudentCode { get; private set; } = default!;
    public string FullName { get; private set; } = default!;
    public DateTime DateOfBirth { get; private set; }
    public string? Gender { get; private set; }
    public Guid? UserId { get; private set; }

    // Thông tin cá nhân
    public string? PlaceOfBirth { get; private set; }
    public string? Hometown { get; private set; }
    public string? Nationality { get; private set; }
    public string? Ethnicity { get; private set; }
    public string? Religion { get; private set; }

    // Thông tin đoàn thể
    public string? PlaceOfOrigin { get; private set; }
    public DateTime? UnionJoinDate { get; private set; }
    public DateTime? PartyJoinDate { get; private set; }

    // Địa chỉ thường trú
    public string? PermanentAddress { get; private set; }
    public string? Ward { get; private set; }
    public string? District { get; private set; }
    public string? Province { get; private set; }

    // Chính sách
    public string? PolicyBeneficiary { get; private set; }
    public string? AllowanceBeneficiary { get; private set; }
    public string? TargetGroup { get; private set; }

    // Liên hệ
    public string? HomePhoneNumber { get; private set; }
    public string? MobilePhoneNumber { get; private set; }
    public string? Email { get; private set; }

    // Giấy tờ
    public string? IdentityNumber { get; private set; }

    // Địa chỉ liên hệ
    public string? MailingAddress { get; private set; }
    public string? CurrentAddress { get; private set; }

    //Navigation Properties
    public User? User { get; private set; }
}