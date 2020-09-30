namespace CcHelperCore.Apis
{
    public static partial class CoolQApi
    {
        /// <summary>
        /// 酷Q日志记录等级
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// 调试 灰色
            /// </summary>
            Debug = 0,
            /// <summary>
            /// 信息 黑色
            /// </summary>
            Info = 10,
            /// <summary>
            /// 信息(成功) 紫色
            /// </summary>
            InfoSuccess = 11,
            /// <summary>
            /// 信息(接收) 蓝色
            /// </summary>
            InfoRecv = 12,
            /// <summary>
            /// 信息(发送) 绿色
            /// </summary>
            InfoSend = 13,
            /// <summary>
            /// 警告 橙色
            /// </summary>
            Warning = 20,
            /// <summary>
            /// 错误 红色
            /// </summary>
            Error = 30,
            /// <summary>
            /// 致命错误 深红
            /// </summary>
            Fatal = 40
        }
    }
}