using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duoju.Model.Entity;

namespace Duoju.DAO.Abstract
{
    public interface IActionInfoRepository
    {
        IList<ActionInfo> GetActionInfoByGroupIdOrUserId(int? gid, int? uid);
        IList<ActionInfo> GetAllAction();
        IList<ActionInfo> GetActionByPath(string path);
    }
}
