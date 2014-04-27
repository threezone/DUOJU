using DUOJU.Dao.Abstract;
using DUOJU.Domain.Entities;
using DUOJU.Domain.Models.Supplier;
using System.Linq;

namespace DUOJU.Dao.Concrete
{
    public class SupplierRepository : Repository, ISupplierRepository
    {
        public DUOJU_SUPPLIERS GetSupplierById(int supplierId)
        {
            return DBEntities.DUOJU_SUPPLIERS.SingleOrDefault(s => s.SUPPLIER_ID == supplierId);
        }

        public SupplierInfo GetSupplierInfoById(int supplierId)
        {
            return null;
        }
    }
}
