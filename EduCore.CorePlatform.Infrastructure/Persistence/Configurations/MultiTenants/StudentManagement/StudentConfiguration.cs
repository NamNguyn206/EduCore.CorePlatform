using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Entities.StudentManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.MultiTenants.StudentManagement;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(x => x.Id);

        // =========================
        // Thông tin sinh viên
        // =========================

        builder.Property(x => x.StudentCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.StudentCode)
            .IsUnique();

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Gender)
            .HasMaxLength(20);

        // =========================
        // Thông tin cá nhân
        // =========================

        builder.Property(x => x.PlaceOfBirth)
            .HasMaxLength(200);

        builder.Property(x => x.Hometown)
            .HasMaxLength(200);

        builder.Property(x => x.Nationality)
            .HasMaxLength(100);

        builder.Property(x => x.Ethnicity)
            .HasMaxLength(100);

        builder.Property(x => x.Religion)
            .HasMaxLength(100);

        // =========================
        // Thông tin đoàn thể
        // =========================

        builder.Property(x => x.PlaceOfOrigin)
            .HasMaxLength(200);

        // =========================
        // Địa chỉ thường trú
        // =========================

        builder.Property(x => x.PermanentAddress)
            .HasMaxLength(500);

        builder.Property(x => x.Ward)
            .HasMaxLength(100);

        builder.Property(x => x.District)
            .HasMaxLength(100);

        builder.Property(x => x.Province)
            .HasMaxLength(100);

        // =========================
        // Chính sách
        // =========================

        builder.Property(x => x.PolicyBeneficiary)
            .HasMaxLength(200);

        builder.Property(x => x.AllowanceBeneficiary)
            .HasMaxLength(200);

        builder.Property(x => x.TargetGroup)
            .HasMaxLength(200);

        // =========================
        // Liên hệ
        // =========================

        builder.Property(x => x.HomePhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.MobilePhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .HasMaxLength(200);

        // =========================
        // Giấy tờ
        // =========================

        builder.Property(x => x.IdentityNumber)
            .HasMaxLength(50);

        builder.HasIndex(x => x.IdentityNumber)
            .IsUnique()
            .HasFilter("[IdentityNumber] IS NOT NULL");

        // =========================
        // Địa chỉ liên hệ
        // =========================

        builder.Property(x => x.MailingAddress)
            .HasMaxLength(500);

        builder.Property(x => x.CurrentAddress)
            .HasMaxLength(500);

        // =========================
        // User - Student
        // One-to-One
        // =========================

        builder.HasOne(x => x.User)
            .WithOne(x => x.Student)
            .HasForeignKey<User>(x => x.StudentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}