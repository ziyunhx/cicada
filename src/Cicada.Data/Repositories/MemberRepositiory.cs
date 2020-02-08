using Cicada.EFCore.Shared.DBContexts;
using Cicada.EFCore.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cicada.Data.Repositories
{
    public class MemberRepositiory : IMemberRepository
    {
        private readonly ILogger<MemberRepositiory> _logger;
        private readonly CicadaDbContext _cicadaDbContext;

        public MemberRepositiory(ILogger<MemberRepositiory> logger, CicadaDbContext cicadaDbContext)
        {
            _logger = logger;
            _cicadaDbContext = cicadaDbContext;
        }

        public List<string> GetEmailFromMembers(string[] toUser = null, string[] toParty = null, string[] toTag = null)
        {
            HashSet<Member> users = GetMembers(toUser, toParty, toTag);
            if (users != null && users.Count > 0)
            {
                try
                {
                    return users.Select(f => f.Email).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                }
                catch { }
            }

            return null;
        }

        public List<string> GetPhoneFromMembers(string[] toUser = null, string[] toParty = null, string[] toTag = null)
        {
            HashSet<Member> users = GetMembers(toUser, toParty, toTag);
            if (users != null && users.Count > 0)
            {
                try
                {
                    return users.Select(f => f.Mobile).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                }
                catch { }
            }

            return null;
        }

        public List<string> GetExtendIdsFromMembers(string[] toUser = null, string[] toParty = null, string[] toTag = null)
        {
            HashSet<Member> users = GetMembers(toUser, toParty, toTag);
            if (users != null && users.Count > 0)
            {
                try
                {
                    return users.Select(f => f.ExtendId).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                }
                catch { }
            }

            return null;
        }

        private HashSet<Member> GetMembers(string[] toUser = null, string[] toParty = null, string[] toTag = null)
        {
            HashSet<Member> users = new HashSet<Member>();

            if (toUser != null && toUser.Length > 0)
            {
                var tempusers = _cicadaDbContext.Members.Where(f => toUser.Contains(f.MemberId));
                if (tempusers != null)
                {
                    foreach (var user in tempusers)
                        users.Add(user);
                }
            }

            if (toParty != null && toParty.Length > 0)
            {
                var partys = _cicadaDbContext.Parties.Where(f => toParty.Contains(f.PartyId));
                foreach (var party in partys)
                {
                    if (party.MemberParties != null)
                    {
                        foreach (var user in party.MemberParties)
                        {
                            users.Add(user.Member);
                        }
                    }
                }
            }

            if (toTag != null && toTag.Length > 0)
            {
                //通过API获取tag下所有用户和部门来支持该特性，开销较大，使用频率低
                var tags = _cicadaDbContext.Tags.Where(f => toTag.Contains(f.TagId));
                foreach (var tag in tags)
                {
                    if (tag.UserTags != null)
                    {
                        foreach (var user in tag.UserTags)
                            users.Add(user.Member);
                    }

                    if (tag.TagParties != null)
                    {
                        foreach (var party in tag.TagParties)
                        {
                            if (party.Party != null && party.Party.MemberParties != null)
                            {
                                foreach (var user in party.Party.MemberParties)
                                {
                                    users.Add(user.Member);
                                }
                            }
                        }
                    }
                }
            }

            return users;
        }
    }
}