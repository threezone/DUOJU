using DUOJU.Domain.Models.User;
using DUOJU.FRAMEWORK.WeChat;

namespace DUOJU.Service.Abstract
{
    public interface IUserService
    {
        int AddWeChatUser(WeChatUserInfo info);

        int AddWeChatUser(WeChatUserInfo_OAuth info);

        void WeChatUserUnsubscribe(string openId);

        UserFinanceInfo GetUserFinanceInfoByOpenId(string openId);
    }
}
