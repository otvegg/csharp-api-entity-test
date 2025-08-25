using workshop.wwwapi.Models;

namespace workshop.wwwapi.Data
{
    public class Seeder
    {

        private List<string> _firstnames = new List<string>()
        {
            "Audrey",
            "Donald",
            "Elvis",
            "Barack",
            "Oprah",
            "Jimi",
            "Mick",
            "Kate",
            "Charles",
            "Kate",
            "Oyvind Timian",
            "Rafael",
            "Oyvind",
            "Timian",
            "James",
            "Roger"
        };

        private List<string> _lastnames = new List<string>()
        {
            "Hepburn",
            "Trump",
            "Presley",
            "Obama",
            "Winfrey",
            "Hendrix",
            "Jagger",
            "Winslet",
            "Windsor",
            "Middleton",
            "Husveg",
            "Nadal",
            "Bond",
            "Federer"

        };
        private List<Patient> _patients = new List<Patient>();
        public Seeder()
        {
            Random patientRandom = new Random(1);

            for (int x = 1; x < 30; x++)
            {
                Patient patient = new Patient();
                patient.Id = x;
                string FirstName = _firstnames[patientRandom.Next(_firstnames.Count)];
                string LastName = _lastnames[patientRandom.Next(_lastnames.Count)];
                patient.FullName = $"{FirstName} {LastName}";
                _patients.Add(patient);
            }
        }

        public List<Patient> Patients { get { return _patients; } }
    }
}
