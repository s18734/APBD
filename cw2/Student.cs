using System;
using System.Xml.Serialization;

namespace cw2
{
    [Serializable]
    public class Student
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Kierunek { get; set; }
        public string Tryb { get; set; }
        
        public string Eska { get; set; }
        public string DataCzas { get; set; }
        public string Email { get; set; }
        public string Mama { get; set; }
        public string Tata { get; set; }

    }
}