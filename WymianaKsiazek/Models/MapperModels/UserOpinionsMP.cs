using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class UserOpinionsMP
    {
        public long Opinion_Id { get; set; }
        public uint OpinionSumValue { get; set; }
        public uint TotalOpinions { get; set; }
        public uint AverageOpinion()
        {
            if(this.TotalOpinions == 0)
            {
                return 0;
            }
            double average = (double)this.OpinionSumValue / this.TotalOpinions;
            return Convert.ToUInt32(Math.Round(average));
        }
    }
}
