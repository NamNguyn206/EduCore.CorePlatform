using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.DTOs.Commands.Create;

public class CreateSupplierRequest
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Address { get; set; }
    public string? TaxCode { get; set; }
    public string? Phone { get; set; }
    public string? BankAccount { get; set; }
    public Guid? BankId { get; set; }
    public string? BankBranch { get; set; }
    public Guid? SupplyCategoryId { get; set; }
    public string? Note { get; set; }
    public string? Email { get; set; }
}
