using Domain.Models.WeChat;

namespace Service.Abstract
{
    public interface IUserService
    {
        int? AddWeChatUser(WeChatUserInfo info);
    }
}
