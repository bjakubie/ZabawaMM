using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabawaMM
{
    class Zrodlo
    {
        double lambda { get; set; }
        Random generator = new Random();
        Random generator1 = new Random();
        public double h = 1.0; //na stałe od MS podany parametr(!)
        //public Pakiet pakiet;
        //public double czas_kolejnego_pakietu;

        public Zrodlo(double lambda)
        {

            this.lambda = lambda;
            
        }

        

        public double zwrocCzasNastepnegoPakietu()
        {
            double x = generator.NextDouble();
            return -Math.Log(1.0 - x) / lambda;
        }


        public double zwrocDlugoscPakietu()
        {

            double x = generator1.NextDouble();
            double zmienna = -Math.Log(1.0 - x) * h;
            //Console.WriteLine("zmienna: {0}", zmienna);
            //return zmienna;
            return 1;
        }
    }
}
