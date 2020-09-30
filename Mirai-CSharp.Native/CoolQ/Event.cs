#pragma warning disable CA1027 // Mark enums with FlagsAttribute
namespace CcHelperCore.Apis
{
    public static partial class CoolQApi
    {
        /// <summary>
        /// 事件
        /// </summary>
        public enum Event
        {
            /// <summary>
            /// 忽略
            /// </summary>
            Ignore = 0,
            /// <summary>
            /// 截断
            /// </summary>
            Block = 1
        }
    }
}