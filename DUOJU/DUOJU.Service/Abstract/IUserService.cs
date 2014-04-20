using DUOJU.FRAMEWORK.WeChat;

namespace DUOJU.Service.Abstract
{
    public interface IUserService
    {
        int? AddWeChatUser(WeChatUserInfo info);
    }
}
