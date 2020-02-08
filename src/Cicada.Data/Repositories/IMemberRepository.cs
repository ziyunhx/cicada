using Cicada.EFCore.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicada.Data.Repositories
{
    public interface IMemberRepository
    {
        List<string> GetEmailFromMembers(string[] toUser = null, string[] toParty = null, string[] toTag = null);
        List<string> GetPhoneFromMembers(string[] toUser = null, string[] toParty = null, string[] toTag = null);
        List<string> GetExtendIdsFromMembers(string[] toUser = null, string[] toParty = null, string[] toTag = null);
    }
}
