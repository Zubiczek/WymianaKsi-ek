using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Queries.ReportQueries
{
    public interface IReportQueries
    {
        Task ReportOffer(long id, string loggeduserid, long reasonid);
        Task ReportUser(string userid, string loggeduserid, long reasonid);
        Task<bool> DoesOfferReportExist(long id, string loggeduserid);
        Task<bool> DoesUserReportExist(string userid, string loggeduserid);
    }
}
