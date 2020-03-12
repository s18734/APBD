using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace cw2
{
    class Program

    {
        static int licznikBledow = 0;
        
        static void Main(string[] args)
        {
           
            HashSet<Student> students = new HashSet<Student>(new OwnComparer());
            FileStream logfile = File.Create("log.txt");

            


            try
            {

                if (args.Length != 3)
                {
                    args = new string[]
                    {
                        @"data.csv",
                        @"wynik.xml",
                        "xml"
                           
                    };
                }
                        
               
                
                var path = args[0];
                var lines = File.ReadLines(path);
                
                StreamWriter sw = new StreamWriter(logfile);
                
                
                var dzisiaj = DateTime.Parse(DateTime.Now.ToString()).ToShortDateString();
                var xmlNode =
                    new XElement("uczelnia",
                        new XAttribute("createdAt", dzisiaj),
                        new XAttribute("author", "Julian Mikołajczyk"),
                        new XElement("studenci")
                    );
                
                
                using (var stream = new StreamReader(File.OpenRead(path)))
                {
                    bool flag = true;
                    string line = null;
                    int counterLinii = 0;
                    while ((line = stream.ReadLine()) != null)
                    {
                        string[] student = line.Split(',');

                        foreach(var x in student)
                        {
                            if (string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x))
                            {
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            var st = new Student
                            {
                                Imie = student[0],
                                Nazwisko = student[1],
                                Kierunek = student[2],
                                Tryb = student[3],
                                Eska = student[4],
                                DataCzas = student[5],
                                Email = student[6],
                                Mama = student[7],
                                Tata = student[8]
                            };
                            
                            

                            if (!students.Add(st))
                            {
                                sw.WriteLine("Błąd numer " + licznikBledow + " Student juz istnieje w pliczku");
                                sw.WriteLine("Student z linii " + counterLinii + " nie zostal wpisany do pliku");
                                licznikBledow++;
                            }
                           
                            
                        }
                        else
                        {
                            sw.WriteLine("Błąd numer " + licznikBledow + " Niepoprawna ilosc informacji o studencie");
                            sw.WriteLine("Student z linii " + counterLinii + " nie zostal wpisany do pliku");
                            licznikBledow++;
                        }

                        
                        
                        flag = true;
                        counterLinii++;
                    }
                }
                
                List<Kurs> lista = new List<Kurs>();
                
                foreach (var x in students)
                {
                    var i = lista.FindIndex(kurs => kurs.nazwaKursu == x.Kierunek);

                    if (i >= 0)
                    {
                        lista[i].iluStudentow += 1;
                    }
                    else
                    {
                        lista.Add(new Kurs
                        {
                            nazwaKursu = x.Kierunek, 
                            iluStudentow = 1
                        });
                    }
                }

                foreach (var x in students)
                {
                    var studentNode = new XElement("student",
                        new XAttribute("indexNumber",x.Eska),
                            new XElement("fname",x.Imie),
                            new XElement("lname",x.Nazwisko),
                            new XElement("birthdate",x.DataCzas),
                            new XElement("email",x.Email),
                            new XElement("mothersName",x.Mama),
                            new XElement("fathersName",x.Tata),
                            new XElement("studies",
                                   new XElement("name",x.Kierunek),
                                   new XElement("mode",x.Tryb)));
                            
                    
                    xmlNode.Element("studenci").Add(studentNode);
                }
                
                
                var XmlStudiesNode = new XElement("activeStudies");
                xmlNode.Add(XmlStudiesNode);
                
                
                
                foreach (var x in lista)
                {
                    var kierunek = 
                        new XElement("studies",
                            new XAttribute("name",x.nazwaKursu),
                            new XAttribute("numberOfStudents",x.iluStudentow));
                    
                    xmlNode.Element("activeStudies").Add(kierunek);
                }
                xmlNode.Save(args[1]);

               

            }
            catch (ArgumentException e) 

            {
                Console.WriteLine("Podana ścieżka jest niepoprawna");
                StreamWriter sw = new StreamWriter(logfile);
                
                sw.WriteLine("Błąd numer " + licznikBledow + " Podana ścieżka jest niepoprawna  " );
                licznikBledow++;
            } catch (FileNotFoundException e)
            {
                Console.WriteLine("Plik nazwa nie istnieje");
                StreamWriter sw = new StreamWriter(logfile);
                sw.WriteLine("Błąd numer " + licznikBledow + " Plik nazwa nie istnieje");
            }
            //ICollection<string> list = new List<string>();



        }
    }
}
