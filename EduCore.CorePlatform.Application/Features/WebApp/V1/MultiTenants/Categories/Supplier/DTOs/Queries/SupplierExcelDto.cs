using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.DTOs.Queries;
public class SupplierExcelDto
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string TaxCode { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string SupplierCode { get; set; } = default!;
    public string BankAccount { get; set; } = default!;
    public string BankName { get; set; } = default!;
    public string Note { get; set; } = default!;
}
