using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockLocker.Library.Models
{
    public class CheckStatusResponse : Response
    {
        public List<Status> Statuses { get; set; }
    }
    public class Status
    {
        public int LockNo { get; set; }
        public bool IsLocked { get; set; }
    }
}
