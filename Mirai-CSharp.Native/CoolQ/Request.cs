#pragma warning disable CA1027 // Mark enums with FlagsAttribute
namespace CcHelperCore.Apis
{
    public static partial class CoolQApi
    {
        /// <summary>
        /// 请求
        /// </summary>
        public enum Request
        {
            /// <summary>
            /// 通过
            /// </summary>
            Allow = 1,
            /// <summary>
            /// 拒绝
            /// </summary>
            Deny = 2,
            /// <summary>
            /// 群添加
            /// </summary>
            GroupAdd = 1,
            /// <summary>
            /// 群邀请
            /// </summary>
            GourpInvite = 2
        }
    }
}