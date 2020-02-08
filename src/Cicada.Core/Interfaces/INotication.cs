using Cicada.Core.Enums;

namespace Cicada.Core.Interfaces
{
    /// <summary>
    /// the interface of notication
    /// </summary>
    public interface INotication
    {
        /// <summary>
        /// notify the message to users.
        /// </summary>
        /// <param name="content">message content.</param>
        /// <param name="toUser">the users.</param>
        /// <param name="toParty">the partys.</param>
        /// <param name="toTag">the tags.</param>
        /// <param name="level">message level.</param>
        /// <param name="timeOut">message send timeout.</param>
        /// <returns>is success?</returns>
        bool Notify(string content, string[] toUser = null, string[] toParty = null, string[] toTag = null, EventLevel level = 0, int timeOut = 10000);
    }
}