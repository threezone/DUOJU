﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DUOJU.Domain.Resources {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ExceptionResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionResource() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DUOJU.Domain.Resources.ExceptionResource", typeof(ExceptionResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到该验证码信息！ 的本地化字符串。
        /// </summary>
        public static string BasicSystemException_CanNotFindIdentifierException {
            get {
                return ResourceManager.GetString("BasicSystemException_CanNotFindIdentifierException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 小聚无法找到该聚会信息哦！ 的本地化字符串。
        /// </summary>
        public static string BasicSystemException_CanNotFindPartyException {
            get {
                return ResourceManager.GetString("BasicSystemException_CanNotFindPartyException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 该验证码已过期！ 的本地化字符串。
        /// </summary>
        public static string BasicSystemException_IdentifierHasBeenExpiredException {
            get {
                return ResourceManager.GetString("BasicSystemException_IdentifierHasBeenExpiredException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 该验证码已使用！ 的本地化字符串。
        /// </summary>
        public static string BasicSystemException_IdentifierHasBeenUsedException {
            get {
                return ResourceManager.GetString("BasicSystemException_IdentifierHasBeenUsedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 需要由该聚会的发起人来确定聚会哦亲！ 的本地化字符串。
        /// </summary>
        public static string BasicSystemException_NoPartySInitiatorException {
            get {
                return ResourceManager.GetString("BasicSystemException_NoPartySInitiatorException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 该聚会已截止报名啦！ 的本地化字符串。
        /// </summary>
        public static string BasicSystemException_PartyWasClosedException {
            get {
                return ResourceManager.GetString("BasicSystemException_PartyWasClosedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 您还未关注小聚哦，请先关注吧！ 的本地化字符串。
        /// </summary>
        public static string BasicSystemException_UserDidNotConcernException {
            get {
                return ResourceManager.GetString("BasicSystemException_UserDidNotConcernException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 您已经报名了哦！ 的本地化字符串。
        /// </summary>
        public static string BasicSystemException_UserHasBeenParticipateThePartyException {
            get {
                return ResourceManager.GetString("BasicSystemException_UserHasBeenParticipateThePartyException", resourceCulture);
            }
        }
    }
}
