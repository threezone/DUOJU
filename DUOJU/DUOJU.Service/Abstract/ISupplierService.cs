using DUOJU.Domain.Models.Supplier;

namespace DUOJU.Service.Abstract
{
    public interface ISupplierService
    {
        SupplierInfo GetSupplierInfoById(int supplierId);
    }
}
