using DUOJU.FRAMEWORK.WeChat;

namespace DUOJU.Service.Abstract
{
    public interface IUserService
    {
        int AddWeChatUser(WeChatUserInfo info);

        int AddWeChatUser(WeChatUserInfo_OAuth info);

        void WeChatUserUnsubscribe(string openId);
    }
}
