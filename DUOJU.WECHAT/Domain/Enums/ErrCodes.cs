﻿namespace Domain.Enums
{
    public enum ErrCodes
    {
        CODE_Negative1 = -1,    // 系统繁忙
        CODE_0 = 0,             // 请求成功
        CODE_40001 = 40001,     // 获取access_token时AppSecret错误，或者access_token无效
        CODE_40002 = 40002,     // 不合法的凭证类型
        CODE_40003 = 40003,     // 不合法的OpenID
        CODE_40004 = 40004,     // 不合法的媒体文件类型
        CODE_40005 = 40005,     // 不合法的文件类型
        CODE_40006 = 40006,     // 不合法的文件大小
        CODE_40007 = 40007,     // 不合法的媒体文件id
        CODE_40008 = 40008,     // 不合法的消息类型
        CODE_40009 = 40009,     // 不合法的图片文件大小
        CODE_40010 = 40010,     // 不合法的语音文件大小
        CODE_40011 = 40011,     // 不合法的视频文件大小
        CODE_40012 = 40012,     // 不合法的缩略图文件大小
        CODE_40013 = 40013,     // 不合法的APPID
        CODE_40014 = 40014,     // 不合法的access_token
        CODE_40015 = 40015,     // 不合法的菜单类型
        CODE_40016 = 40016,     // 不合法的按钮个数
        CODE_40017 = 40017,     // 不合法的按钮个数
        CODE_40018 = 40018,     // 不合法的按钮名字长度
        CODE_40019 = 40019,     // 不合法的按钮KEY长度
        CODE_40020 = 40020,     // 不合法的按钮URL长度
        CODE_40021 = 40021,     // 不合法的菜单版本号
        CODE_40022 = 40022,     // 不合法的子菜单级数
        CODE_40023 = 40023,     // 不合法的子菜单按钮个数
        CODE_40024 = 40024,     // 不合法的子菜单按钮类型
        CODE_40025 = 40025,     // 不合法的子菜单按钮名字长度
        CODE_40026 = 40026,     // 不合法的子菜单按钮KEY长度
        CODE_40027 = 40027,     // 不合法的子菜单按钮URL长度
        CODE_40028 = 40028,     // 不合法的自定义菜单使用用户
        CODE_40029 = 40029,     // 不合法的oauth_code
        CODE_40030 = 40030,     // 不合法的refresh_token
        CODE_40031 = 40031,     // 不合法的openid列表
        CODE_40032 = 40032,     // 不合法的openid列表长度
        CODE_40033 = 40033,     // 不合法的请求字符，不能包含\uxxxx格式的字符
        CODE_40035 = 40035,     // 不合法的参数
        CODE_40038 = 40038,     // 不合法的请求格式
        CODE_40039 = 40039,     // 不合法的URL长度
        CODE_40050 = 40050,     // 不合法的分组id
        CODE_40051 = 40051,     // 分组名字不合法
        CODE_41001 = 41001,     // 缺少access_token参数
        CODE_41002 = 41002,     // 缺少appid参数
        CODE_41003 = 41003,     // 缺少refresh_token参数
        CODE_41004 = 41004,     // 缺少secret参数
        CODE_41005 = 41005,     // 缺少多媒体文件数据
        CODE_41006 = 41006,     // 缺少media_id参数
        CODE_41007 = 41007,     // 缺少子菜单数据
        CODE_41008 = 41008,     // 缺少oauth code
        CODE_41009 = 41009,     // 缺少openid
        CODE_42001 = 42001,     // access_token超时
        CODE_42002 = 42002,     // refresh_token超时
        CODE_42003 = 42003,     // oauth_code超时
        CODE_43001 = 43001,     // 需要GET请求
        CODE_43002 = 43002,     // 需要POST请求
        CODE_43003 = 43003,     // 需要HTTPS请求
        CODE_43004 = 43004,     // 需要接收者关注
        CODE_43005 = 43005,     // 需要好友关系
        CODE_44001 = 44001,     // 多媒体文件为空
        CODE_44002 = 44002,     // POST的数据包为空
        CODE_44003 = 44003,     // 图文消息内容为空
        CODE_44004 = 44004,     // 文本消息内容为空
        CODE_45001 = 45001,     // 多媒体文件大小超过限制
        CODE_45002 = 45002,     // 消息内容超过限制
        CODE_45003 = 45003,     // 标题字段超过限制
        CODE_45004 = 45004,     // 描述字段超过限制
        CODE_45005 = 45005,     // 链接字段超过限制
        CODE_45006 = 45006,     // 图片链接字段超过限制
        CODE_45007 = 45007,     // 语音播放时间超过限制
        CODE_45008 = 45008,     // 图文消息超过限制
        CODE_45009 = 45009,     // 接口调用超过限制
        CODE_45010 = 45010,     // 创建菜单个数超过限制
        CODE_45015 = 45015,     // 回复时间超过限制
        CODE_45016 = 45016,     // 系统分组，不允许修改
        CODE_45017 = 45017,     // 分组名字过长
        CODE_45018 = 45018,     // 分组数量超过上限
        CODE_46001 = 46001,     // 不存在媒体数据
        CODE_46002 = 46002,     // 不存在的菜单版本
        CODE_46003 = 46003,     // 不存在的菜单数据
        CODE_46004 = 46004,     // 不存在的用户
        CODE_47001 = 47001,     // 解析JSON/XML内容错误
        CODE_48001 = 48001,     // api功能未授权
        CODE_50001 = 50001      // 用户未授权该api
    }
}
