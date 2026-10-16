using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Entities;


namespace EduCore.CorePlatform.Domain.Entities.MultiTenants.Categories;
public class Supplier : MultiTenantEntity<Guid, Guid>
{
    public string Name { get; private set; } = default!;
    public string? TaxCode { get; private set; }
    public string? Address { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }

    public string? BankAccount { get; private set; }
    public Guid? BankId { get; private set; }
    public string? BankBranch { get; private set; }

    public Guid? SupplyCategoryId { get; private set; }
    public string? Note { get; private set; }

    //Nabigation property
    public SupplyCategory? SupplyCategory { get; private set; }
    public Bank? Bank { get; private set; }
    
    protected Supplier() { }

    public Supplier(
        Guid tenantId,
        string name,
        string? address,
        string? taxCode,        
        string? phone,
        string? email,
        string? bankAccount,
        Guid? bankId,
        string? bankBranch,
        Guid? supplyCategoryId,
        string? note
    )
    {
        Name = name;
        TaxCode = taxCode;
        Address = address;
        Phone = phone;
        Email = email;
        BankAccount = bankAccount;
        BankId = bankId;
        BankBranch = bankBranch;
        SupplyCategoryId = supplyCategoryId;
        Note = note;

        SetTenant(tenantId);
    }

    public void Update(
        string name,
        string? address,
        string? taxCode,        
        string? phone,
        string? email,
        string? bankAccount,
        Guid? bankId,
        string? bankBranch,
        Guid? supplyCategoryId,
        string? note
    )
    {
        Name = name;
        Address = address;
        TaxCode = taxCode;        
        Phone = phone;
        Email = email;
        BankAccount = bankAccount;
        BankId = bankId;
        BankBranch = bankBranch;
        SupplyCategoryId = supplyCategoryId;
        Note = note;

        UpdatedAt = DateTime.UtcNow;
    }
}
