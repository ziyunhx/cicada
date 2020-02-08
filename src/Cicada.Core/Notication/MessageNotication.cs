using Cicada.Core.Enums;
using Cicada.Core.Helper;
using Cicada.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cicada.Core.Notication
{
    public class MessageNotication : INotication
    {
        private string product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
        private string domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
        private string accessKeyId = "";//你的accessKeyId，参考本文档步骤2
        private string accessKeySecret = "";//你的accessKeySecret，参考本文档步骤2
        private string signName = "";
        private string templateCode = "";

        public bool Notify(string content, string[] toUser = null, string[] toParty = null, string[] toTag = null, EventLevel level = EventLevel.Normal, int timeOut = 10000)
        {
            List<string> phones = null;// UserHelper.GetPhoneFromMembers(toUser, toParty, toTag);

            try
            {
                if (phones != null && phones.Count(f => !string.IsNullOrWhiteSpace(f)) > 0)
                {
                    string phoneNumber = string.Join(",", phones.Where(f => !string.IsNullOrWhiteSpace(f)));

                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
