using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.EFCore.Shared.Enums
{
    public enum EmailProviderType
    {
        Default = 0,
        Mailgun = 1,
        Sendgrid = 2,
        Smtp = 3
    }
}
