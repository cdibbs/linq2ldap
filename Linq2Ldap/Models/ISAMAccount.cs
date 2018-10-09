using System;
using System.Collections.Generic;
using System.Text;

namespace Linq2Ldap.Models
{
    public interface ISAMAccount
    {
        string Id { get; set; }

        // byte[] ObjectSid { get; set; }

        string UserPrincipalName { get; set; }

        string SamAccountName { get; set; }

        long[] SIdHistory { get; set; }
    }
}
