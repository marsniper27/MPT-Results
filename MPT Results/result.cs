using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPT_Results
{
    public class Result
    {
        public string GamePlayResult { get; set; }
        public int TotalExpectedWin { get; set; }
        public int GameTotalWin { get; set; }
        public string GamePlayName { get; set; }
        public object GamePlayId { get; set; }
    }
}