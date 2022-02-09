﻿using System.Collections.Generic;
using System.Linq;

namespace MrCMS.Website.Auth
{
    public class PerformAclCheck : IPerformAclCheck
    {
        private readonly IGetAclRoles _getAclRoles;

        public PerformAclCheck(IGetAclRoles getAclRoles)
        {
            _getAclRoles = getAclRoles;
        }

        public bool CanAccessLogic(StandardLogicCheckResult result, IList<string> keys)
        {
            return result.CanAccess ?? _getAclRoles.GetRoles(result.Roles, keys).Any();
        }
    }
}