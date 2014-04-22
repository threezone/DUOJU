using DUOJU.Dao.Abstract;
using DUOJU.Domain.Models.Supplier;

namespace DUOJU.Dao.Concrete
{
    public class SupplierRepository : Repository, ISupplierRepository
    {
        public SupplierInfo GetSupplierInfoById(int supplierId)
        {
            return null;
        }
    }
}
