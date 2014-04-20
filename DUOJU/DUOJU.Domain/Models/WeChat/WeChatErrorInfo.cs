﻿using DUOJU.Domain.Enums.WeChat;

namespace DUOJU.Domain.Models.WeChat
{
    public class WeChatErrorInfo
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