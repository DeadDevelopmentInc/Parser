using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models.Template
{
    [Obsolete]
    internal class ResumeTemplate
    {
        public bool newTypeOfTemplate { get; set; } = false;
        public int ItExp { get; set; }
        public List<string> ProfessiconalProfile { get; set; }
        public List<string> ProfessionalSkills { get; set; } = new List<string>();
        public List<string> Certifications { get; set; } = new List<string>();
        public List<string> ProfessionalsExperience { get; set; } = new List<string>();
        public List<string> EducationAndTraining { get; set; } = new List<string>();
    }
}
