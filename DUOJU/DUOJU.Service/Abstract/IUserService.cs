using DUOJU.Domain.Models.WeChat;

namespace DUOJU.Service.Abstract
{
    public interface IUserService
    {
        int? AddWeChatUser(WeChatUserInfo info);
    }
}
