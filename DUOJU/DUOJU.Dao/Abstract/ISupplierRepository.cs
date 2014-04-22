using DUOJU.Domain.Models.Supplier;

namespace DUOJU.Dao.Abstract
{
    public interface ISupplierRepository
    {
        SupplierInfo GetSupplierInfoById(int supplierId);
    }
}
