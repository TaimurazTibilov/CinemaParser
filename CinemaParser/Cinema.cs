using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaParser
{
    public class Cinema
    {
        string admArea = String.Empty;
        string district = String.Empty;
        int rownum;
        string commonName = String.Empty;
        string fullName = String.Empty;
        string shortName = String.Empty;
        string chiefOrg = String.Empty;
        Area area;
        string address = String.Empty;
        string chiefName = String.Empty;
        string chiefPosition = String.Empty;
        string publicPhone = String.Empty;
        string fax = String.Empty;
        string email = String.Empty;
        string workingHours = String.Empty;
        string clarificationOfWorkingHours = String.Empty;
        string webSite = String.Empty;
        string okpo = String.Empty;
        string inn = String.Empty;
        string numberOfHalls = String.Empty;
        string totalSeatsAmount = String.Empty;
        string x_WGS = String.Empty;
        string y_WGS = String.Empty;
        string globalID = String.Empty;

        public int Rownum { get => rownum; set => rownum = value; }
        public string CommonName { get => commonName; set => commonName = value; }
        public string FullName { get => fullName; set => fullName = value; }
        public string ShortName { get => shortName; set => shortName = value; }
        public string ChiefOrg { get => chiefOrg; set => chiefOrg = value; }
        public string Address { get => address; set => address = value; }
        public string ChiefName { get => chiefName; set => chiefName = value; }
        public string ChiefPosition { get => chiefPosition; set => chiefPosition = value; }
        public string PublicPhone { get => publicPhone; set => publicPhone = value; }
        public string Fax { get => fax; set => fax = value; }
        public string Email { get => email; set => email = value; }
        public string WorkingHours { get => workingHours; set => workingHours = value; }
        public string ClarificationOfWorkingHours { get => clarificationOfWorkingHours; set => clarificationOfWorkingHours = value; }
        public string WebSite { get => webSite; set => webSite = value; }
        public string Okpo { get => okpo; set => okpo = value; }
        public string Inn { get => inn; set => inn = value; }
        public string NumberOfHalls { get => numberOfHalls; set => numberOfHalls = value; }
        public string TotalSeatsAmount { get => totalSeatsAmount; set => totalSeatsAmount = value; }
        public string X_WGS { get => x_WGS; set => x_WGS = value; }
        public string Y_WGS { get => y_WGS; set => y_WGS = value; }
        public string GlobalID { get => globalID; set => globalID = value; }
        public string AdmArea { get => admArea; set => admArea = value; }
        public string District { get => district; set => district = value; }

        public Area Area { get => area; set => area = value; }

        public List<string> GetInfo()
        {
            List<string> info = new List<string>();
            info.Add(Rownum.ToString());
            info.Add(commonName);
            info.Add(fullName);
            info.Add(shortName);
            info.Add(chiefOrg);
            info.Add(AdmArea);
            info.Add(District);
            info.Add(address);
            info.Add(chiefName);
            info.Add(chiefPosition);
            info.Add(publicPhone);
            info.Add(fax);
            info.Add(email);
            info.Add(workingHours);
            info.Add(clarificationOfWorkingHours);
            info.Add(webSite);
            info.Add(okpo);
            info.Add(inn);
            info.Add(numberOfHalls);
            info.Add(totalSeatsAmount);
            info.Add(x_WGS);
            info.Add(y_WGS);
            info.Add(globalID);
            return info;
        }
    }
}
