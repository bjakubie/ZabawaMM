using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabawaMM
{
    class Symulator
    {
        double obecny_czas = 0;
        double najkrotszy_czas = 0;
        double czas_symulacji;
        int dlugosc_kolejki;
        double lambda_stala;

        int ilosc_zrodel = 1;
        double czas_kolejnego_pakietu;
        double czas_serwera;

        //Zrodlo zrodlo;
        Pakiet pakiet; //pojedynczy pakiet ten który na poczatku wchodzi i wypad
        Pakiet nowy_pakiet = null;
        Pakiet pierwszy_w_kolejce = null;
        Zrodlo zrodlo;

        Queue<Pakiet> kolejka_fifo = new Queue<Pakiet>();


        double suma_wszystkich_pakietow=0;
        double suma_czasow_przebywania_w_kolejce = 0;
        double suma_odrzuconych_pakietow = 0;





        public void glownaSymulacja()
        {
            Console.WriteLine("Podaj czas symulacji:");
            czas_symulacji = double.Parse(Console.ReadLine());
            Console.WriteLine("Podaj dlugosc kolejki:");
            dlugosc_kolejki = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Podaj lambde dla zrodla:");
            lambda_stala = double.Parse(Console.ReadLine());

            zrodlo = new Zrodlo(lambda_stala);

            czas_kolejnego_pakietu = zrodlo.zwrocCzasNastepnegoPakietu();
            Console.WriteLine("Czas w ktorym pierwszy pakiet: {0} ", czas_kolejnego_pakietu);
            obecny_czas += czas_kolejnego_pakietu;

            czas_kolejnego_pakietu = 0; //no bo sie zeruje

            pakiet = new Pakiet(0, suma_wszystkich_pakietow, zrodlo.zwrocDlugoscPakietu());
            czas_kolejnego_pakietu = zrodlo.zwrocCzasNastepnegoPakietu();
            kolejka_fifo.Enqueue(pakiet);

            Console.WriteLine("Pakiet 0 wchodzi do kolejki, obecny czas = {0} ", obecny_czas);

            suma_wszystkich_pakietow += 1;
            pierwszy_w_kolejce = kolejka_fifo.Peek();
            suma_czasow_przebywania_w_kolejce += pierwszy_w_kolejce.czas_oczekiwania_w_kolejce;
            czas_serwera = pierwszy_w_kolejce.dlugosc;


            Console.WriteLine("Pakiet 0 wychodz z kolejki, obecny czas = {0} ", obecny_czas);

            kolejka_fifo.Dequeue();


            while (true)
            {

                if (obecny_czas >= czas_symulacji)
                {
                    Console.WriteLine("Koniec symulacji");
                    Console.WriteLine("Średni czas przebywania pakietu w kolejce: {0} ", suma_czasow_przebywania_w_kolejce / suma_wszystkich_pakietow);
                    //Console.WriteLine("Poziom strat: {0}", suma_odrzuconych_pakietow / suma_wszystkich_pakietow);
                    Console.WriteLine("Poziom strat: {0}", (suma_odrzuconych_pakietow / (suma_odrzuconych_pakietow + suma_wszystkich_pakietow)));
                    Console.ReadKey();
                    Environment.Exit(0);
                }


                if (czas_kolejnego_pakietu < czas_serwera) //czyli jezeli zrodlo wczesniej niz serwer reaguje
                {
                    //Console.WriteLine("mniejszy czas zrodla");
                    //tutaj jest taka akcja ze moga byc jakies czasy w kolejce, jesli tak to ten najkrotszy czas trzeba do nich dodac
                    if (kolejka_fifo.Count < dlugosc_kolejki)
                    {
                        nowy_pakiet = null;
                        pierwszy_w_kolejce = null;
                        najkrotszy_czas = czas_kolejnego_pakietu;
                        obecny_czas += najkrotszy_czas;
                        czas_serwera -= najkrotszy_czas;
                        

                        //Console.WriteLine("Obecny czas {0}", obecny_czas);
                        //Console.WriteLine("Czas zrodla {0}", czas_kolejnego_pakietu);

                        czas_kolejnego_pakietu -= najkrotszy_czas;

                        if (kolejka_fifo.Count > 0) //gdy kolejka nie pusta
                        {
                            zwiekszCzasyOczekiwanPakietowWKolejceFIFO();
                        }

                        nowy_pakiet = new Pakiet(0, suma_wszystkich_pakietow, zrodlo.zwrocDlugoscPakietu());
                        

                        kolejka_fifo.Enqueue(nowy_pakiet);

                        Console.WriteLine("Pakiet {0} wchodzi do kolejki, obecny czas = {1} ", nowy_pakiet.numer_pakietu, obecny_czas);

                        //Console.WriteLine("czas oczekiwania w kolejce nowego pakietu: {0}", nowy_pakiet.czas_oczekiwania_w_kolejce);
                        //suma_wszystkich_pakietow += 1;

                        czas_kolejnego_pakietu = zrodlo.zwrocCzasNastepnegoPakietu(); //wyzerowal sie i podajemy mu kolejny czas


                        
                    }
                    else
                    {
                        nowy_pakiet = null;
                        pierwszy_w_kolejce = null;
                        najkrotszy_czas = czas_kolejnego_pakietu;
                        obecny_czas += najkrotszy_czas;
                        czas_serwera -= najkrotszy_czas;
                        czas_kolejnego_pakietu -= najkrotszy_czas;
                        czas_kolejnego_pakietu = zrodlo.zwrocCzasNastepnegoPakietu();

                        suma_odrzuconych_pakietow += 1;
                    }

                    
                }
                else
                { //gdy czas serwera najkrotszy
                    najkrotszy_czas = czas_serwera;
                    obecny_czas += najkrotszy_czas;
                    czas_kolejnego_pakietu -= najkrotszy_czas;
                    
                    pierwszy_w_kolejce = null;
                    nowy_pakiet = null;

                    //Console.WriteLine("Obecny czas ze strony serwera {0}", obecny_czas);
                    //Console.WriteLine("Czas Serwera {0}", czas_serwera);

                    czas_serwera -= najkrotszy_czas;

                    if (kolejka_fifo.Count > 0)
                    {
                        
                        zwiekszCzasyOczekiwanPakietowWKolejceFIFO();
                        pierwszy_w_kolejce = kolejka_fifo.Peek();
                        suma_czasow_przebywania_w_kolejce += pierwszy_w_kolejce.czas_oczekiwania_w_kolejce;
                        //nowy czas serwera =
                        czas_serwera = pierwszy_w_kolejce.dlugosc;
                        
                        //Console.WriteLine("Czas oczekiwania pakietu: {0}", pierwszy_w_kolejce.czas_oczekiwania_w_kolejce);
                        kolejka_fifo.Dequeue();

                        Console.WriteLine("Pakiet {0} wychodzi z kolejki, obecny czas = {1} ",pierwszy_w_kolejce.numer_pakietu, obecny_czas);
                        Console.WriteLine("Dodany czas: {0}", pierwszy_w_kolejce.czas_oczekiwania_w_kolejce);
                        suma_wszystkich_pakietow += 1;
                    }
                    else
                    {   //przechodzimy tutaj do momentu kiedy czas zrodla bedzie musial sie wyzerowac a czas serwera tez = 0 bo czeka
                        
                        najkrotszy_czas = czas_kolejnego_pakietu;
                        obecny_czas += najkrotszy_czas;
                        
                        //pojawia sie nowy pakiet w tym momencie:

                        nowy_pakiet = new Pakiet(0, suma_wszystkich_pakietow, zrodlo.zwrocDlugoscPakietu());
                        //Console.WriteLine("czas oczekiwania w kolejce nowego pakietu: {0}", nowy_pakiet.czas_oczekiwania_w_kolejce);
                        kolejka_fifo.Enqueue(nowy_pakiet);

                        Console.WriteLine("Pakiet {0} wchodzi do kolejki, obecny czas = {1} ", nowy_pakiet.numer_pakietu, obecny_czas);

                        pierwszy_w_kolejce = kolejka_fifo.Peek();
                        czas_serwera = pierwszy_w_kolejce.dlugosc;
                        
                        suma_czasow_przebywania_w_kolejce += pierwszy_w_kolejce.czas_oczekiwania_w_kolejce; // (+0)
                        //Console.WriteLine("Czas oczekiwania pakietu: {0}", pierwszy_w_kolejce.czas_oczekiwania_w_kolejce);

                        czas_kolejnego_pakietu = zrodlo.zwrocCzasNastepnegoPakietu();

                        kolejka_fifo.Dequeue();

                        Console.WriteLine("Pakiet {0} wychodzi z kolejki, obecny czas = {1} ", pierwszy_w_kolejce.numer_pakietu, obecny_czas);
                        Console.WriteLine("Dodany czas: {0}", pierwszy_w_kolejce.czas_oczekiwania_w_kolejce);
                        suma_wszystkich_pakietow += 1;
                    }

                }


            }

            
            
        }

        public void zwiekszCzasyOczekiwanPakietowWKolejceFIFO()
        {
            foreach (Pakiet element in kolejka_fifo)
            {
                //Console.WriteLine("zwiekszam o {0}", najkrotszy_czas);
                element.czas_oczekiwania_w_kolejce += najkrotszy_czas;
                
            }
        }
    }
}
