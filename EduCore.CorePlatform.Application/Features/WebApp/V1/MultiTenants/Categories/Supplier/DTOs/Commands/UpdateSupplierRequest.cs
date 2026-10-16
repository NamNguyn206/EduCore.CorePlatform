using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.DTOs.Commands;

public class UpdateSupplierRequest: UpdateRequest
{
    public string Name { get; init; } = default!;
    public string? TaxCode { get; init; }
    public string? Address { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? BankAccount { get; init; }
    public Guid? BankId { get; init; }
    public string? BankBranch { get; init; }
    public Guid? SupplyCategoryId { get; init; }
    public string? Note { get; init; }
}
