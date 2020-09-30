using CcHelperCore.Models;
using CcHelperCore.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CcHelperCore.Apis
{
    public static partial class CoolQApi
    {
        public static int AuthCode { get; set; }

        #region - Tools  -
        public static string Encode(string str, bool comma = true)
            => comma ?
                str.Replace("&", "&amp;").Replace("[", "&#91;").Replace("]", "&#93;").Replace(",", "&#44;") :
                str.Replace("&", "&amp;").Replace("[", "&#91;").Replace("]", "&#93;");
        public static string Decode(string str)
            => str.Replace("&amp;", "&").Replace("&#91;", "[").Replace("&#93;", "]").Replace("&#44", ",");
        public static string CQC_At(long qqId)
            => $@"[CQ:at,qq={(qqId == -1 ? "all" : qqId.ToString())}]";
        public static string CQC_Emoji(long ID)
            => $@"[CQ:emoji,id={ID}]";
        public static string CQC_Face(long ID)
            => $@"[CQ:face,id={ID}]";
        public static string CQC_BFace(long ID)
            => $@"[CQ:bface,id={ID}]";
        public static string CQC_Shake()
            => $@"[CQ:shake]";
        public static string CQC_Record(string file, bool magic)
            => $@"[CQ:record,file={Encode(file)}{(magic ? ",magic=true" : "")}]";
        public static string CQC_RPS(int ID)
            => $@"[CQ:rps,id={ID}]";
        public static string CQC_Dice(int ID)
            => $@"[CQ:dice,id={ID}]";
        public static string CQC_Share(string url, string title, string content, string image)
            => $@"[CQ:share,url={Encode(url)},title={Encode(title)},content={Encode(content)},image={Encode(image)}]";
        public static string CQC_Contact(string type, long ID)
            => $@"[CQ:contact,type={type},id={ID}]";
        public static string CQC_Anonymous(bool force = true)
            => $@"[CQ:anonymous{(force ? "" : "ignore")}]";
        public static string CQC_Image(string file)
            => $@"[CQ:image,file={Encode(file)}]";
        public static string CQC_Music(string source, long musicID, bool newstyle = true)
            => $@"[CQ:music.type={source},id={musicID}{(newstyle ? ",style=1" : "")}]";
        public static string CQC_MusicCustom(string url, string audio, string title, string content, string image)
            => $@"[CQ:music,type=custom,url={Encode(url)},audio={Encode(audio)}" +
            (string.IsNullOrEmpty(title) ? "" : ",title=" + Encode(title)) +
            (string.IsNullOrEmpty(content) ? "" : ",content=" + Encode(content)) +
            (string.IsNullOrEmpty(image) ? "" : ",image=" + Encode(image)) +
            "]";
        public static string CQC_Location(float latitude, float longitude, long zoom, string title, string address)
            => $@"[CQ:location,lat={latitude},lon={longitude}" +
            (zoom > 0 ? ",zoom=" + zoom : "") +
            $@",title={Encode(title)},content={Encode(address)}]";
        #endregion

        #region - API -
        /// <summary>
        /// 撤回消息
        /// </summary>
        /// <param name="msgId">消息Id</param>
        /// <returns></returns>
        public static int DeleteMsg(int msgId)
            => NativeMethods.CQ_deleteMsg(AuthCode, msgId);
        /// <summary>
        /// 发送私聊消息
        /// </summary>
        /// <param name="qqId">QQ号</param>
        /// <param name="message">消息内容</param>
        /// <returns>消息ID</returns>
        public static int SendPrivateMsg(long qqId, string message)
        {
            int result = 0;
            for (int i = 0; i < Math.Ceiling(message.Length / 500d); i++)
            {
                result = NativeMethods.CQ_sendPrivateMsg(AuthCode, qqId, message.Substring(i * 500, Math.Min(500, message.Length - i * 500)));
            }
            return result;
        }

        /// <summary>
        /// 发送群聊消息
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="message">消息内容</param>
        /// <returns>消息ID</returns>
        public static int SendGroupMsg(long groupId, string message)
        {
            int result = 0;
            for (int i = 0; i < Math.Ceiling(message.Length / 500d); i++)
            {
                result = NativeMethods.CQ_sendGroupMsg(AuthCode, groupId, message.Substring(i * 500, Math.Min(500, message.Length - i * 500)));
            }
            return result;
        }

        /// <summary>
        /// 发送讨论组消息
        /// </summary>
        /// <param name="DiscussID">讨论组ID</param>
        /// <param name="message">消息内容</param>
        /// <returns>消息ID</returns>
        public static int SendDiscussMsg(long DiscussID, string message) 
            => NativeMethods.CQ_sendDiscussMsg(AuthCode, DiscussID, message);
        /// <summary>
        /// 发送赞
        /// </summary>
        /// <param name="qqId">QQ号</param>
        /// <returns></returns>
        public static int SendLike(long qqId) 
            => NativeMethods.CQ_sendLikeV2(AuthCode, qqId, 1);
        /// <summary>
        /// 踢出群成员
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="qqId">QQ号</param>
        /// <param name="neverAllowAgain">不再接收加群申请</param>
        /// <returns></returns>
        public static int SetGroupKick(long groupId, long qqId, bool neverAllowAgain = false) 
            => NativeMethods.CQ_setGroupKick(AuthCode, groupId, qqId, neverAllowAgain);
        /// <summary>
        /// 禁言群成员
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="qqId">QQ号</param>
        /// <param name="duration">禁言秒数</param>
        /// <returns></returns>
        public static int SetGroupBan(long groupId, long qqId, long duration) 
            => NativeMethods.CQ_setGroupBan(AuthCode, groupId, qqId, duration);
        /// <summary>
        /// 设置群管理
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="qqId">QQ号</param>
        /// <param name="isAdmin">是否为管理员</param>
        /// <returns></returns>
        public static int SetGroupAdmin(long groupId, long qqId, bool isAdmin)
            => NativeMethods.CQ_setGroupAdmin(AuthCode, groupId, qqId, isAdmin);
        /// <summary>
        /// 群全员禁言
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="isBan">是否禁言状态</param>
        /// <returns></returns>
        public static int SetGroupWholeBan(long groupId, bool isBan) 
            => NativeMethods.CQ_setGroupWholeBan(AuthCode, groupId, isBan);
        /// <summary>
        /// 禁言匿名成员
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="anomymousID">匿名标识</param>
        /// <param name="duration">禁言秒数</param>
        /// <returns></returns>
        public static int SetGroupAnonymousBan(long groupId, string anomymousID, long duration) 
            => NativeMethods.CQ_setGroupAnonymousBan(AuthCode, groupId, anomymousID, duration);
        /// <summary>
        /// 设置匿名状态
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="isEnable">是否启用</param>
        /// <returns></returns>
        public static int SetGroupAnonymous(long groupId, bool isEnable) 
            => NativeMethods.CQ_setGroupAnonymous(AuthCode, groupId, isEnable);
        /// <summary>
        /// 设置群名片
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="qqId">QQ号</param>
        /// <param name="newName">群名片</param>
        /// <returns></returns>
        public static int SetGroupCard(long groupId, long qqId, string newName) 
            => NativeMethods.CQ_setGroupCard(AuthCode, groupId, qqId, newName);
        /// <summary>
        /// 退出群
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="isDisband">是否解散群</param>
        /// <returns></returns>
        public static int SetGroupLeave(long groupId, bool isDisband)
            => NativeMethods.CQ_setGroupLeave(AuthCode, groupId, isDisband);
        /// <summary>
        /// 设置专属头衔
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="qqId">QQ号</param>
        /// <param name="title">头衔 留空取消</param>
        /// <param name="seconds">有效秒数</param>
        /// <returns></returns>
        public static int SetGroupSpecialTitle(long groupId, long qqId, string title, long seconds)
            => NativeMethods.CQ_setGroupSpecialTitle(AuthCode, groupId, qqId, title, seconds);
        /// <summary>
        /// 退出讨论组
        /// </summary>
        /// <param name="discussID">讨论组ID</param>
        /// <returns></returns>
        public static int SetDiscussLeave(long discussID) 
            => NativeMethods.CQ_setDiscussLeave(AuthCode, discussID);
        /// <summary>
        /// 处理加好友请求
        /// </summary>
        /// <param name="responseFlag">标识</param>
        /// <param name="operation">操作</param>
        /// <param name="remark">好友备注</param>
        /// <returns></returns>
        public static int SetFriendAddRequest(string responseFlag, Request operation, string remark)
            => NativeMethods.CQ_setFriendAddRequest(AuthCode, responseFlag, operation, remark);
        /// <summary>
        /// 处理加群请求
        /// </summary>
        /// <param name="responseFlag">标识</param>
        /// <param name="type">类型</param>
        /// <param name="operation">操作</param>
        /// <param name="reason">理由</param>
        /// <returns></returns>
        public static int SetGroupAddRequestV2(string responseFlag, Request type, Request operation, string reason) 
            => NativeMethods.CQ_setGroupAddRequestV2(AuthCode, responseFlag, type, operation, reason);
        /// <summary>
        /// 获取群成员信息
        /// </summary>
        /// <param name="groupId">群号</param>
        /// <param name="qqId">QQ号</param>
        /// <param name="notCache">不使用缓存</param>
        /// <returns></returns>
        public static GroupMemberInfo GetGroupMemberInfoV2(long groupId, long qqId, bool notCache = false)
        {
            return ConvertAnsiBytesToGroupMemberInfo(NativeMethods.CQ_getGroupMemberInfoV2(AuthCode, groupId, qqId, notCache));
        }
        //public static List<GroupMemberInfo> GetGroupMemberInfoList(long groupId)
        //{
        //    string cookie = GetCookies();
        //    long bkn = GetBkn(BiliUtils.GetCookieValue(cookie, "skey"));
        //    string json = HttpHelper.HttpPost("https://qinfo.clt.qq.com/cgi-bin/qun_info/get_group_members_new", $"gc={groupId}&bkn={bkn}&src=qinfo_v3", cookie: cookie);
        //    JObject j = JObject.Parse(json);
        //    var x = j["mems"].Select(p => p["u"].ToObject<long>());
        //    return x.Select(p => GetGroupMemberInfoV2(groupId, p)).ToList();
        //}
        /// <summary>
        /// 获取群成员信息列表
        /// </summary>
        /// <param name="groupId">目标群号</param>
        /// <returns>群成员信息列表</returns>
        public static List<GroupMemberInfo> GetGroupMemberInfoList(long groupId)
        {
            string base64 = NativeMethods.CQ_getGroupMemberList(AuthCode, groupId);
            byte[] buffer = Convert.FromBase64String(base64);
            List<GroupMemberInfo> result = new List<GroupMemberInfo>();
            using (Unpacker unpacker = new Unpacker(buffer))
            {
                int memberCount = unpacker.GetInt32();
                for (int i = 0; i < memberCount; i++)
                {
                    unpacker.GetInt16();
                    result.Add(GetGroupMemberInfoFromUnpaker(unpacker));
                }
            }
            return result;
        }
        /// <summary>
        /// 获取群成员QQ列表
        /// </summary>
        /// <param name="groupId">目标群号</param>
        /// <returns>群成员信息列表</returns>
        public static long[] GetGroupMembers(long groupId)
        {
            string cookie = GetCookies();
            var match = System.Text.RegularExpressions.Regex.Match(cookie, @"skey=(?<s>\S+?)(?=&|;)");
            if (match.Success)
            {
                long bkn = GetBkn(match.Groups["s"].Value);
                string json = HttpHelper.HttpPost("https://qinfo.clt.qq.com/cgi-bin/qun_info/get_group_members_new", $"gc={groupId}&bkn={bkn}&src=qinfo_v3", cookie: cookie);
                JObject j = JObject.Parse(json);
                return j["mems"].Select(p => p["u"].ToObject<long>()).ToArray();
            }
            else
            {
                throw new KeyNotFoundException("给定的Cookie无效");
            }
        }
        /// <summary>
        /// 异步获取群成员QQ列表
        /// </summary>
        /// <param name="groupId">目标群号</param>
        /// <returns>群成员信息列表</returns>
        public static async Task<long[]> GetGroupMembersAsync(long groupId)
        {
            string cookie = GetCookies();
            var match = System.Text.RegularExpressions.Regex.Match(cookie, @"skey=(?<s>\S+?)(?=&|;)");
            if (match.Success)
            {
                long bkn = GetBkn(match.Groups["s"].Value);
                string json = await HttpHelper.HttpPostAsync("https://qinfo.clt.qq.com/cgi-bin/qun_info/get_group_members_new", $"gc={groupId}&bkn={bkn}&src=qinfo_v3", cookie: cookie);
                JObject j = JObject.Parse(json);
                return j["mems"].Select(p => p["u"].ToObject<long>()).ToArray();
            }
            else
            {
                throw new KeyNotFoundException("给定的Cookie无效");
            }
        }
        /// <summary>
        /// 获取陌生人信息
        /// </summary>
        /// <param name="qqId">QQ号</param>
        /// <param name="notCache">不使用缓存</param>
        /// <returns></returns>
        public static string GetStrangerInfo(long qqId, bool notCache = false)
            => NativeMethods.CQ_getStrangerInfo(AuthCode, qqId, notCache);
        /// <summary>
        /// 打日志
        /// </summary>
        /// <param name="priority">日志级别</param>
        /// <param name="category">分类</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public static int AddLog(LogLevel priority, string category, string content) 
            => NativeMethods.CQ_addLog(AuthCode, priority, category, content);
        /// <summary>
        /// 获取 Cookies
        /// </summary>
        /// <returns></returns>
        public static string GetCookies()
            => NativeMethods.CQ_getCookiesV2(AuthCode, "www.qq.com");
        /// <summary>
        /// 获取 CSRF Token
        /// </summary>
        /// <returns></returns>
        public static int GetCsrfToken() 
            => NativeMethods.CQ_getCsrfToken(AuthCode);
        /// <summary>
        /// 获取当前登录的 QQ 号
        /// </summary>
        /// <returns></returns>
        public static long GetLoginQQ()
            => NativeMethods.CQ_getLoginQQ(AuthCode);
        /// <summary>
        /// 获取当前登录的账号昵称
        /// </summary>
        public static string GetLoginNick() 
            => NativeMethods.CQ_getLoginNick(AuthCode);
        /// <summary>
        /// 获取数据储存文件夹路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDirectory()
            => NativeMethods.CQ_getAppDirectory(AuthCode);
        /// <summary>
        /// 抛出严重错误
        /// </summary>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public static int SetFatal(string errorInfo)
            => NativeMethods.CQ_setFatal(AuthCode, errorInfo);
        /// <summary>
        /// 获取图片。返回绝对路径
        /// </summary>
        /// <param name="fileName">文件名, [CQ:image...] 中的文件名部分</param>
        /// <returns></returns>
        public static string GetImage(string fileName)
            => NativeMethods.CQ_getImage(AuthCode, fileName);
        /// <summary>
        /// 获取语音消息文件
        /// </summary>
        /// <param name="file">文件ID</param>
        /// <param name="Format">期望文件格式</param>
        /// <returns></returns>
        public static string GetRecord(string file, string Format) 
            => NativeMethods.CQ_getRecordV2(AuthCode, file, Format);
        /// <summary>
        /// 计算得到页面操作用参数bkn
        /// </summary>
        /// <param name="skey"></param>
        /// <returns></returns>
        public static long GetBkn(string skey)
        {
            int hash = 5381;
            for (int i = 0; i < skey.Length; i++)
                hash += (hash << 5) + skey[i];
            return hash & int.MaxValue;
        }
        /// <summary>
        /// 计算得到页面操作用参数gtk
        /// </summary>
        /// <param name="skey"></param>
        /// <returns></returns>
        public static long GetGtk(string skey)
            => GetBkn(skey);
        #endregion

        private static GroupMemberInfo GetGroupMemberInfoFromUnpaker(Unpacker unpacker)
        {
            GroupMemberInfo gm = new GroupMemberInfo
            {
                GroupId = unpacker.GetInt64(),
                Number = unpacker.GetInt64(),
                NickName = unpacker.GetString(),
                InGroupName = unpacker.GetString(),
                Gender = unpacker.GetInt32() == 0 ? "男" : " 女",
                Age = unpacker.GetInt32(),
                Area = unpacker.GetString(),
                JoinTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unpacker.GetInt32()).ToLocalTime(),
                LastSpeakingTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unpacker.GetInt32()).ToLocalTime(),
                Level = unpacker.GetString()
            };
            var manager = unpacker.GetInt32();
            gm.Authority = manager == 3 ? "群主" : (manager == 2 ? "管理员" : "成员");
            gm.HasBadRecord = (unpacker.GetInt32() == 1);
            gm.Title = unpacker.GetString();
            gm.TitleExpirationTime = unpacker.GetInt32();
            gm.CanModifyInGroupName = (unpacker.GetInt32() == 1);
            return gm;
        }

        public static GroupMemberInfo ConvertAnsiBytesToGroupMemberInfo(string base64, bool leaveOpen = false)
            => ConvertAnsiBytesToGroupMemberInfo(Convert.FromBase64String(base64), leaveOpen);

        public static GroupMemberInfo ConvertAnsiBytesToGroupMemberInfo(byte[] buffer, bool leaveOpen = false)
        {
            if (buffer == null || buffer.Length < 40)
            {
                throw new ArgumentException("参数无效", "source");
            }
            return ConvertAnsiBytesToGroupMemberInfo(new MemoryStream(buffer), leaveOpen);
        }

        public static GroupMemberInfo ConvertAnsiBytesToGroupMemberInfo(MemoryStream ms, bool leaveOpen = false)
        {
            GroupMemberInfo gm = new GroupMemberInfo();
            using (Unpacker unpack = new Unpacker(ms, leaveOpen))
            {
                gm.GroupId = unpack.GetInt64();
                gm.Number = unpack.GetInt64();
                gm.NickName = unpack.GetString();
                gm.InGroupName = unpack.GetString();
                gm.Gender = unpack.GetInt32() == 0 ? "男" : " 女";
                gm.Age = unpack.GetInt32();
                gm.Area = unpack.GetString();
                gm.JoinTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unpack.GetInt32()).ToLocalTime();
                gm.LastSpeakingTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unpack.GetInt32()).ToLocalTime();
                gm.Level = unpack.GetString();
                var manager = unpack.GetInt32();
                gm.Authority = manager == 3 ? "群主" : (manager == 2 ? "管理员" : "成员");
                gm.HasBadRecord = (unpack.GetInt32() == 1);
                gm.Title = unpack.GetString();
                gm.TitleExpirationTime = unpack.GetInt32();
                gm.CanModifyInGroupName = (unpack.GetInt32() == 1);
            }
            return gm;
        }
    }
}