using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabawaMM
{
    class Pakiet
    {
        public double dlugosc { get; set; }
        //public double lambda;
        //public double czas_do_kolejnego_pakietu;
        public double czas_oczekiwania_w_kolejce { get; set; }
        //public double h = 1.0; //na stałe od MS podany parametr(!)
        //Random generator = new Random();
        public double numer_pakietu;
        

        
        public Pakiet(double czas_oczekiwania_w_kolejce, double numer_pakietu, double dlugosc)
        {
            //this.lambda = lambda;
            this.numer_pakietu = numer_pakietu;
            this.czas_oczekiwania_w_kolejce = czas_oczekiwania_w_kolejce;
            this.dlugosc = dlugosc;
            //dlugosc = zwrocDlugoscPakietu();
        }

        /*
        public double zwrocDlugoscPakietu()
        {
            
            double x = generator.NextDouble();
            double zmienna = -Math.Log(1.0 - x) * h;
            Console.WriteLine("zmienna: {0}", zmienna);
            return zmienna;
            //return 1;
        }*/

    }
}
