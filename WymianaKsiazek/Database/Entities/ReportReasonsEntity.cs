using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class ReportReasonsEntity
    {
        public long Reason_Id { get; set; }
        public string Reason { get; set; }
        public virtual ICollection<UserReportsEntity> UserReports { get; set; }
        public virtual ICollection<OfferReportsEntity> OfferReports { get; set; }
        public ReportReasonsEntity()
        {
            UserReports = new List<UserReportsEntity>();
            OfferReports = new List<OfferReportsEntity>();
        }
    }
}
