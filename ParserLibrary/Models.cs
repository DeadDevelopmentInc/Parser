using ParserLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    enum Months
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }


    public class ModelSkill
    {
        public string _id { get; set; } = Convert.ToString(Guid.NewGuid());
        public string name { get; set; }
        public List<string> allNames { get; set; } = new List<string>();
        public string type { get; set; } = "Other";
        public bool isSkillNew { get; set; } = true;

        public ModelSkill()
        { }

        public ModelSkill(string id, string name, List<string> allNames, string type, bool isNew)
        {
            _id = id;
            this.name = name;
            this.type = type;
            isSkillNew = isNew;
            this.allNames = allNames;
        }        
    }

    internal class SkillLevel
    {
        public string _id { get; set; }
        public string level { get; set; }
    }

    internal class BufferClass
    {
        public string name { get; set; }
        public string level { get; set; } = "Working knowledge";
        public string type { get; set; } = "Other";
        public List<string> allNames { get; set; } = new List<string>();

        internal void AddLevel(string newLevel)
        {
            if ((level == "Expert" | level == "Experienced") &
                (newLevel != "Expert" | newLevel != "Experienced")) { level = "Expert"; return; }
            if ((level == "Working knowledge" | level == "Extensive knowledge") &
                (newLevel != "Working knowledge" | newLevel != "Extensive knowledge")) { level = "Experienced"; return; }
            level = "Expert";
        }
    }

    internal class SkillDate
    {
        double _month;
        double _year;

        public string Month
        {
            get { return _month.ToString(); }
            set
            {
                switch(value)
                {
                    case "January": { _month = (int)Months.January; } break;
                    case "February": { _month = (int)Months.February; } break;
                    case "March": { _month = (int)Months.March; } break;
                    case "April": { _month = (int)Months.April; } break;
                    case "May": { _month = (int)Months.May; } break;
                    case "June": { _month = (int)Months.June; } break;
                    case "July": { _month = (int)Months.July; } break;
                    case "August": { _month = (int)Months.August; } break;
                    case "September": { _month = (int)Months.September; } break;
                    case "October": { _month = (int)Months.October; } break;
                    case "November": { _month = (int)Months.November; } break;
                    case "December": { _month = (int)Months.December; } break;
                }
            }
        }

        public double MonthInt
        {
            get { return _month; }
            set { _month = value; }
        }

        public string Year
        {
            get { return _year.ToString(); }
            set { _year = int.Parse(value); }
        }

        public double YearInt
        {
            get { return _year; }
            set { _year = value; }
        }

        public double GetLenght(SkillDate date)
        {
            double years = 0;
            years = Math.Abs(this.YearInt - date.YearInt + 1) + 
                Math.Abs((12 - this.MonthInt + date.MonthInt) / 12);
            return years;
        }

        public void Update(SkillDate date)
        {
            _year = date._year;
            _month = date._month;
        }

        public bool NotIntersect(SkillDate date)
        {
            return _year < date._year ? true : (_year == date._year ? (_month < date._month ? true : false) : false);
        }
    }
}
