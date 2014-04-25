namespace DUOJU.FRAMEWORK.WeChat
{
    public class WeChatReturnInfo
    {
        public bool IsValid
        {
            get
            {
                return errcode == (int)ErrCodes.CODE_0;
            }
        }

        public int errcode { get; set; }

        public string errmsg { get; set; }
    }
}
