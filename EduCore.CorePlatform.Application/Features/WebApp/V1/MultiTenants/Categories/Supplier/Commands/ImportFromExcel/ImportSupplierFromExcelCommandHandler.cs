using ClosedXML.Excel;
using MediatR;
using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Commands;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Specifications.Commands;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Enums;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Categories;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Commands.ImportFromExcel;
public sealed class ImportSupplierFromExcelCommandHandler : IRequestHandler<ImportSupplierFromExcelCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public ImportSupplierFromExcelCommandHandler(IUnitOfWork unitOfWork,
                                                ICurrentTenant currentTenant,
                                                ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(
        ImportSupplierFromExcelCommand request,
        CancellationToken cancellationToken)
    {
        if (request.RequestData.FileBytes == null || request.RequestData.FileBytes.Length == 0)
            return Result.Failure(ErrorCodeConstants.FileEmpty, await _systemMessageProvider.GetAsync(ErrorCodeConstants.FileEmpty));

        using var stream = new MemoryStream(request.RequestData.FileBytes);
        using var workbook = new XLWorkbook(stream);
        var ws = workbook.Worksheets.First();
        var rows = ws.RowsUsed().Skip(1); // bỏ header

        if (!rows.Any())
        {
            return Result.Failure(
                ErrorCodeConstants.FileEmpty,
                await _systemMessageProvider.GetAsync(ErrorCodeConstants.FileEmpty));
        }

        var repoSupplier = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Supplier>();
        var repoSupplyCategory = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.SupplyCategory>();
        var repoBank = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Bank>();

        var supplierSpec = new SupplierForImportFromExcelSpec(_currentTenant);
        var supplyCategorySpec = new SupplyCategoryListForUpdateSpec(_currentTenant, null);
        var bankSpec = new BankListForUpdateSpec(_currentTenant, null);

        var supplies = await repoSupplier.ListAsync(supplierSpec, cancellationToken);
        var supplyCategories = await repoSupplyCategory.ListAsync(supplyCategorySpec, cancellationToken);        
        var banks = await repoBank.ListAsync(bankSpec, cancellationToken);

        foreach (var row in rows)
        {
            var name = row.Cell(1).GetString().Trim();
            var address = row.Cell(2).GetString().Trim();
            var taxCode = row.Cell(3).GetString().Trim();
            var phone = row.Cell(4).GetString().Trim();
            var supplyCategoryName = row.Cell(5).GetString().Trim();            
            var bankAccount = row.Cell(6).GetString().Trim();
            var bankName = row.Cell(7).GetString().Trim();
            var bankBranch = row.Cell(8).GetString().Trim();            
            var note = row.Cell(9).GetString().Trim();
  
            if (string.IsNullOrEmpty(name))
                return Result.Failure(ErrorCodeConstants.SupplierNameRequired, await _systemMessageProvider.GetAsync(ErrorCodeConstants.SupplierNameRequired));

            var findSupplyCategory = supplyCategories.FirstOrDefault(c => c.Name == supplyCategoryName);
            if (findSupplyCategory == null)
            {
                findSupplyCategory = new Domain.Entities.MultiTenants.Categories.SupplyCategory(_currentTenant.TenantId!.Value, supplyCategoryName);
                await repoSupplyCategory.AddAsync(findSupplyCategory, cancellationToken);
                supplyCategories.Add(findSupplyCategory);
            }

            var findBank = banks.FirstOrDefault(c => c.Name == bankName);
            if (findBank == null)
            {
                findBank = new Domain.Entities.MultiTenants.Categories.Bank(_currentTenant.TenantId!.Value, bankName);
                await repoBank.AddAsync(findBank, cancellationToken);
                banks.Add(findBank);
            }

            var existingSupplier = supplies.FirstOrDefault(x => x.Name.Equals(name));
            if (existingSupplier == null)
            {
                var entity = new Domain.Entities.MultiTenants.Categories.Supplier(
                    _currentTenant.TenantId!.Value,
                    name,
                    address,
                    taxCode,                    
                    phone,
                    string.Empty,
                    bankAccount,
                    findBank.Id, 
                    bankBranch,
                    findSupplyCategory.Id,
                    note
                );

                await repoSupplier.AddAsync(entity, cancellationToken);
                supplies.Add(entity);
            }
            else
            {
                existingSupplier.Update(
                    name, 
                    address,
                    taxCode,
                    phone, 
                    string.Empty,
                    bankAccount,
                    findBank.Id,
                    bankBranch,
                    findSupplyCategory.Id, 
                    note
                );
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
