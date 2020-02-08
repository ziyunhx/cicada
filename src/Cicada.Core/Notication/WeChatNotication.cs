using Cicada.Core.Enums;
using Cicada.Core.Helper;
using Cicada.Core.Interfaces;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.AdvancedAPIs.Mass;
using Senparc.Weixin.Work.Containers;
using System.Collections.Generic;

namespace Cicada.Core.Notication
{
    public class WeChatNotication : INotication
    {
        private static string corpID = "";
        private static string corpSecret = "";

        public bool Notify(string content, string[] toUser = null, string[] toParty = null, string[] toTag = null, EventLevel level = EventLevel.Normal, int timeOut = 10000)
        {
            try
            {
                List<string> members = null;// UserHelper.GetExtendIdsFromMembers(toUser, toParty, toTag);

                string token = AccessTokenContainer.TryGetToken(corpID, corpSecret);
                string users = null;

                if (members != null && members.Count > 0)
                    users = string.Join("|", members);

                if (string.IsNullOrEmpty(users))
                    users = "@all";

                MassResult massresult = MassApi.SendText(token, users, null, null, "0", content, 0, timeOut);

                if (massresult.errcode == 0)
                    return true;
            }
            catch { }

            return false;
        }
    }
}
