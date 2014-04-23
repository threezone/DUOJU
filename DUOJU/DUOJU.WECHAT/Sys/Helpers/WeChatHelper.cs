using DUOJU.Domain;
using DUOJU.FRAMEWORK.WeChat;

namespace DUOJU.WECHAT.Sys.Helpers
{
    public static class WeChatHelper
    {
        public static WeChat WeChat = new WeChat(CommonSettings.DUOJUWECHAT_TOKEN, CommonSettings.DUOJUWECHAT_APPID, CommonSettings.DUOJUWECHAT_APPSECRET);
    }
}