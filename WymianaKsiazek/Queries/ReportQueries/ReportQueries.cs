using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace WymianaKsiazek.Queries.ReportQueries
{
    public class ReportQueries : IReportQueries
    {
        private readonly Context _context;
        public ReportQueries(Context context)
        {
            _context = context;
        }
        public async Task ReportOffer(long id, string loggeduserid, long reasonid)
        {
            OfferReportsEntity report = new OfferReportsEntity();
            report.Offer_Id = id;
            report.User_Id = loggeduserid;
            report.Reason_Id = reasonid;
            await _context.AddAsync(report);
            await _context.SaveChangesAsync();
        }
        public async Task ReportUser(string userid, string loggeduserid, long reasonid)
        {
            UserReportsEntity report = new UserReportsEntity();
            report.ReportedUser_Id = userid;
            report.UserWhoReported_Id = loggeduserid;
            report.Reason_Id = reasonid;
            await _context.AddAsync(report);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DoesOfferReportExist(long id, string loggeduserid)
        {
            var report = await _context.OfferReports.Where(x => x.Offer_Id == id && x.User_Id == loggeduserid).FirstOrDefaultAsync();
            if (report == null) return false;
            else return true;
        }
        public async Task<bool> DoesUserReportExist(string userid, string loggeduserid)
        {
            var report = await _context.UserReports.Where(x => x.ReportedUser_Id == userid && x.UserWhoReported_Id == loggeduserid)
                .FirstOrDefaultAsync();
            if (report == null) return false;
            else return true;
        }
    }
}
