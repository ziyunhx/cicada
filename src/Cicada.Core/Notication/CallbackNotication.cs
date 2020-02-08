using Cicada.Core.Enums;
using Cicada.Core.Interfaces;
using System;

namespace Cicada.Core.Notication
{
    public class CallbackNotication : INotication
    {
        public CallbackNotication()
        {

        }

        private string apiBaseUrl = null;
        //"http://127.0.0.1/api/notication?content={0}&level={1}&touser={2}&toparty={3}&totag={4}&timeout={5}";
        private string apiType = "GET";
        private string contentKey, levelKey, touserKey, topartyKey, totagKey;

        public bool Notify(string content, string[] toUser = null, string[] toParty = null, string[] toTag = null, EventLevel level = EventLevel.Normal, int timeOut = 10000)
        {
            throw new NotImplementedException();
        }
    }
}
