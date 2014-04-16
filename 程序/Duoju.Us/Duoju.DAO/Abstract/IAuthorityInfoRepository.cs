using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duoju.Model.Entity;

namespace Duoju.DAO.Abstract
{
    public interface IAuthorityInfoRepository
    {
        int AddAuthorityInfo(AuthorityInfo model);
        bool RemoveAuthorityList(List<int> idList, int uid, int gid);
    }
}
