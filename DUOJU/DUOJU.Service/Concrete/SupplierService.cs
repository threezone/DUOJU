using DUOJU.Dao.Abstract;
using DUOJU.Dao.Concrete;
using DUOJU.Domain.Models.Supplier;
using DUOJU.Service.Abstract;

namespace DUOJU.Service.Concrete
{
    public class SupplierService : ISupplierService
    {
        public ISupplierRepository SupplierRepository { get; set; }


        public SupplierService()
        {
            SupplierRepository = new SupplierRepository();
        }


        public SupplierInfo GetSupplierInfoById(int supplierId)
        {
            return null;
        }
    }
}
