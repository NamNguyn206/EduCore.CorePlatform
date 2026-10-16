using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Common.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Specifications.Commands;

public sealed class BankByIdsForDeleteSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Bank, Guid>
{
    public BankByIdsForDeleteSpec(ICurrentTenant currentTenant,
                                            IReadOnlyCollection<Guid> ids)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => ids.Contains(x.Id));

        EnableTracking(); // ⭐ cần để SoftDelete
    }
}
