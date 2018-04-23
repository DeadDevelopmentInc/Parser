using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
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

    internal class Date
    {
        double _month;
        double _year;

        public string Month
        {
            get { return _month.ToString(); }
            set
            {
                switch (value)
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

        public double GetLenght(Date date)
        {
            double years = 0;
            years = Math.Abs(this.YearInt - date.YearInt + 1) +
                Math.Abs((12 - this.MonthInt + date.MonthInt) / 12);
            return years;
        }

        public void Update(Date date)
        {
            _year = date._year;
            _month = date._month;
        }

        public bool NotIntersect(Date date)
        {
            return _year < date._year ? true : (_year == date._year ? (_month < date._month ? true : false) : false);
        }
    }
}
