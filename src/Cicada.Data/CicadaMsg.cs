using System;
using Newtonsoft.Json;

namespace Cicada.EFCore.Shared
{
    /// <summary>
    /// Cicada message.
    /// </summary>
    public class CicadaMsg
    {
        /// <summary>
        /// Gets or sets the pid.
        /// </summary>
        /// <value>The pid.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string pid { set; get; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id { set; get; }
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string command { set; get; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string msg { set; get; }
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int level { set; get; }
        /// <summary>
        /// Ises the heart beat.
        /// </summary>
        /// <returns><c>true</c>, if heart beat was ised, <c>false</c> otherwise.</returns>
        public Boolean IsHeartBeat()
        {
            return this.command == "__heartbeat";
        }
    }

}
