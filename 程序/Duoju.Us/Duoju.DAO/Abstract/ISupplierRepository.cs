using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duoju.Model.Entity;

namespace Duoju.DAO.Abstract
{
    public interface ISupplierRepository
    {
        IList<SupplierInfo> GetAllSupplierList(int pageStart, int pageLimit, out int total);
        SupplierInfo GetSupplierBySupplierId(int sid);
        bool SaveOrUpdateSupplierInfo(SupplierInfo supplierInfo);
        int AddSupplierInfo(SupplierInfo supplierInfo);
        bool UpdateSupplierInfo(SupplierInfo supplierInfo);
        bool DeleteSupplierById(int sid);
        bool DeleteSupplierByIdList(string sidList);
        IList<SupplierInfo> SearchSupplier(string keyWord, int pageStart, int pageLimit, out int total);
        IList<SupplierInfo> GetSupplierByName(string name);
    }
}
