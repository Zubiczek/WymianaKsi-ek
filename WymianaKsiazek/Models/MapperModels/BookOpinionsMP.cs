using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class BookOpinionsMP
    {
        public long Opinion_Id { get; set; }
        public uint Opinion5 { get; set; }
        public uint Opinion4 { get; set; }
        public uint Opinion3 { get; set; }
        public uint Opinion2 { get; set; }
        public uint Opinion1 { get; set; }
        public uint TotalOpinions()
        {
            return this.Opinion1 + this.Opinion2 + this.Opinion3 + this.Opinion4 + this.Opinion5;
        }
        public uint AverageOpinion()
        {
            uint sumofopinions = (5 * this.Opinion5) + (4 * this.Opinion4) + (3 * this.Opinion3) + (2 * this.Opinion2) + this.Opinion1;
            uint totalopinions = this.TotalOpinions();
            if(totalopinions == 0)
            {
                return 0;
            }
            double average = (double)sumofopinions / totalopinions;
            return Convert.ToUInt32(Math.Round(average));
        }
        public (uint, uint, uint, uint, uint) OpinionsWithPercentage()
        {
            uint totalopinions = this.TotalOpinions();
            if(totalopinions == 0)
            {
                return (0,0,0,0,0);
            }
            return ((this.Opinion5 * 100) / totalopinions, (this.Opinion4 * 100) / totalopinions, (this.Opinion3 * 100) / totalopinions, (this.Opinion2 * 100) / totalopinions, (this.Opinion1 * 100) / totalopinions);
        }
    }
}
