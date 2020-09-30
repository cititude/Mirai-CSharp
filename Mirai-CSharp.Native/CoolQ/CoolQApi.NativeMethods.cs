using System.Runtime.InteropServices;

#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
namespace CcHelperCore.Apis
{
    public static partial class CoolQApi
    {
        public static class NativeMethods
        {
            private const string DllName = "CQP.dll";

            [DllExport("AppInfo", CallingConvention.StdCall)]
            public static string AppInfo() => "9,com.cchelper.robot";

            [DllExport("Initialize", CallingConvention = CallingConvention.StdCall)]
            public static int Initialize(int i) 
            { 
                AuthCode = i; 
                return 0;
            }

            [DllImport(DllName, EntryPoint = "CQ_sendPrivateMsg")]
            public static extern int CQ_sendPrivateMsg(int authCode, long qqId, string msg);

            [DllImport(DllName, EntryPoint = "CQ_sendGroupMsg")]
            public static extern int CQ_sendGroupMsg(int authCode, long groupId, string msg);

            [DllImport(DllName, EntryPoint = "CQ_sendDiscussMsg")]
            public static extern int CQ_sendDiscussMsg(int authCode, long discussId, string msg);

            [DllImport(DllName, EntryPoint = "CQ_deleteMsg")]
            public static extern int CQ_deleteMsg(int authCode, long msgId);

            [DllImport(DllName, EntryPoint = "CQ_sendLikeV2")]
            public static extern int CQ_sendLikeV2(int authCode, long qqId, int count);

            [DllImport(DllName, EntryPoint = "CQ_getCookiesV2")]
            public static extern string CQ_getCookiesV2(int authCode, string domain);

            [DllImport(DllName, EntryPoint = "CQ_getRecordV2")]
            public static extern string CQ_getRecordV2(int authCode, string file, string format);

            [DllImport(DllName, EntryPoint = "CQ_getCsrfToken")]
            public static extern int CQ_getCsrfToken(int authCode);

            [DllImport(DllName, EntryPoint = "CQ_getAppDirectory")]
            public static extern string CQ_getAppDirectory(int authCode);

            [DllImport(DllName, EntryPoint = "CQ_getLoginQQ")]
            public static extern long CQ_getLoginQQ(int authCode);

            [DllImport(DllName, EntryPoint = "CQ_getLoginNick")]
            public static extern string CQ_getLoginNick(int authCode);

            [DllImport(DllName, EntryPoint = "CQ_setGroupKick")]
            public static extern int CQ_setGroupKick(int authCode, long groupId, long qqId, bool refuses);

            [DllImport(DllName, EntryPoint = "CQ_setGroupBan")]
            public static extern int CQ_setGroupBan(int authCode, long groupId, long qqId, long time);

            [DllImport(DllName, EntryPoint = "CQ_setGroupAdmin")]
            public static extern int CQ_setGroupAdmin(int authCode, long groupId, long qqId, bool isSet);

            [DllImport(DllName, EntryPoint = "CQ_setGroupSpecialTitle")]
            public static extern int CQ_setGroupSpecialTitle(int authCode, long groupId, long qqId, string title, long durationTime);

            [DllImport(DllName, EntryPoint = "CQ_setGroupWholeBan")]
            public static extern int CQ_setGroupWholeBan(int authCode, long groupId, bool isOpen);

            [DllImport(DllName, EntryPoint = "CQ_setGroupAnonymousBan")]
            public static extern int CQ_setGroupAnonymousBan(int authCode, long groupId, string anonymous, long banTime);

            [DllImport(DllName, EntryPoint = "CQ_setGroupAnonymous")]
            public static extern int CQ_setGroupAnonymous(int authCode, long groupId, bool isOpen);

            [DllImport(DllName, EntryPoint = "CQ_setGroupCard")]
            public static extern int CQ_setGroupCard(int authCode, long groupId, long qqId, string newCard);

            [DllImport(DllName, EntryPoint = "CQ_setGroupLeave")]
            public static extern int CQ_setGroupLeave(int authCode, long groupId, bool isDisband);

            [DllImport(DllName, EntryPoint = "CQ_setDiscussLeave")]
            public static extern int CQ_setDiscussLeave(int authCode, long disscussId);

            [DllImport(DllName, EntryPoint = "CQ_setFriendAddRequest")]
            public static extern int CQ_setFriendAddRequest(int authCode, string identifying, Request requestType, string appendMsg);

            [DllImport(DllName, EntryPoint = "CQ_setGroupAddRequestV2")]
            public static extern int CQ_setGroupAddRequestV2(int authCode, string identifying, Request requestType, Request responseType, string appendMsg);

            [DllImport(DllName, EntryPoint = "CQ_addLog")]
            public static extern int CQ_addLog(int authCode, LogLevel priority, string type, string msg);

            [DllImport(DllName, EntryPoint = "CQ_setFatal")]
            public static extern int CQ_setFatal(int authCode, string errorMsg);

            [DllImport(DllName, EntryPoint = "CQ_getGroupMemberInfoV2")]
            public static extern string CQ_getGroupMemberInfoV2(int authCode, long groudId, long qqId, bool isCache);

            [DllImport(DllName, EntryPoint = "CQ_getGroupMemberList")]
            public static extern string CQ_getGroupMemberList(int authCode, long groupId);

            [DllImport(DllName, EntryPoint = "CQ_getGroupList")]
            public static extern string CQ_getGroupList(int authCode);

            [DllImport(DllName, EntryPoint = "CQ_getStrangerInfo")]
            public static extern string CQ_getStrangerInfo(int authCode, long qqId, bool notCache);

            [DllImport(DllName, EntryPoint = "CQ_canSendImage")]
            public static extern int CQ_canSendImage(int authCode);

            [DllImport(DllName, EntryPoint = "CQ_canSendRecord")]
            public static extern int CQ_canSendRecord(int authCode);

            [DllImport(DllName, EntryPoint = "CQ_getImage")]
            public static extern string CQ_getImage(int authCode, string file);

            [DllImport(DllName, EntryPoint = "CQ_getGroupInfo")]
            public static extern string CQ_getGroupInfo(int authCode, long groupId, bool notCache);

            [DllImport(DllName, EntryPoint = "CQ_getFriendList")]
            public static extern string CQ_getFriendList(int authCode, bool reserved);
        }
    }
}