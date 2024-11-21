using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Enums
{
    public enum MailStatus
    {
        Sent,
        Failed,
        Queued,
        Delivered,
        Bounced
    }

}
