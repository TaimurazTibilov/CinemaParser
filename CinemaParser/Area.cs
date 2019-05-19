using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaParser
{
    public class Area
    {
        string admArea = String.Empty;
        string district;
        List<string> districts = new List<string>();
        public Area()
        {
        }

        public Area(string admArea, string district)
        {
            this.admArea = admArea;
            this.district = district;
        }

        public string AdmArea { get => admArea; set => admArea = value; }
        public string District { get => district; set => district = value; }
        public List<string> Districts { get => districts; set => districts = value; }
        public int GetNumberOfDistricts()
        {
            return districts.Count;
        }
    }
}
