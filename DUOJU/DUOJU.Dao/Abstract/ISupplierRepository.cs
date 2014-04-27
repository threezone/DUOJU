using DUOJU.Domain.Entities;
using DUOJU.Domain.Models.Supplier;

namespace DUOJU.Dao.Abstract
{
    public interface ISupplierRepository
    {
        DUOJU_SUPPLIERS GetSupplierById(int supplierId);

        SupplierInfo GetSupplierInfoById(int supplierId);
    }
}
