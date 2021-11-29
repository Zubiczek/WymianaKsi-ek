using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class UserReportsEntity
    {
        public long Id { get; set; }
        public long Reason_Id { get; set; }
        public virtual ReportReasonsEntity Reason { get; set; }
        public string ReportedUser_Id { get; set; }
        public virtual UserEntity ReportedUser { get; set; }
        public string UserWhoReported_Id { get; set; }
        public virtual UserEntity UserWhoReported { get; set; }
    }
}
