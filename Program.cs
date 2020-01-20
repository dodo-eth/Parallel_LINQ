using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; 

using System.Text.RegularExpressions;
 
using System.Globalization;

namespace rgr
{
    class Program
    {
        public static char SDS { get; set; }
        static System.Diagnostics.Stopwatch sw;
        public static int ProcCount = Environment.ProcessorCount;
        static Random rand; 
        public static  char[][] massiv ;
        public static char[][] massiv_posl;
        public static char[][] massiv_pall;
        public static char[][] massiv_task;
        public static char[][] massiv_parr_for;
        public static char[][] massiv_parr_linq;
        static int proc_count;
        public static double L;
        public static object obj = new object();
        public static  char de;
        public static int size;
        public static int block;

        public static char[] consonants = "AEIUOY".ToCharArray();

     


        private static string RndStr()
        {
            string s = "", symb = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rnd = new Random();

            for (int i = 0; i < 1; i++)
            {
                s += symb[rnd.Next(0, symb.Length)];
                Program.SDS = Convert.ToChar(s);
                    }
             de=Convert.ToChar(s);
            return s;
        }

        static void Main(string[] args)
        {

            sw = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swTaskReturn = new System.Diagnostics.Stopwatch();
            rand = new Random();

            Console.WriteLine("Введите размерность матрицы");
            while (true)
            {
                try
                {
                    size = Convert.ToInt32(Console.ReadLine());
                }
                catch
                { Console.WriteLine("Неверный формат введенных данных"); continue; }
                if (size <= 0)
                { Console.WriteLine("Размерность не может быть отрицательной или равной нулю"); continue; }
                break;


            }

            Console.WriteLine("Выводить Исходную и результирующую матрицу?");
            Console.WriteLine("1(да)/2(нет)\n");
            int Swe = Convert.ToInt32(Console.ReadLine());
            
            
            if(Swe==1)
            {
                block = 1;
            }
           
            proc_count = Convert.ToInt32(Environment.ProcessorCount);
            L = Convert.ToDouble(size) / Convert.ToDouble(proc_count);


             


            massiv = new char[size][];

            massiv_pall = new char[size][];
            massiv_posl = new char[size][];
            massiv_task = new char[size][];
            massiv_parr_for = new char[size][];
            massiv_parr_linq = new char[size][];
            for (int i = 0; i < massiv.Length; i++)
            { 
                massiv[i] = new char[size];
            massiv_pall[i] = new char[size];
            massiv_posl[i] = new char[size];
                massiv_task[i] = new char[size];
                massiv_parr_for[i] = new char[size];
                massiv_parr_linq[i] = new char[size];
            }
                for (int i = 0; i < massiv.Length; i++)
                {
                    for (int j = 0; j < massiv[i].Length; j++)
                    {
                        Thread.Sleep(12);
                        RndStr();
                        massiv[i][j] =de ;
                     massiv_posl[i][j] = de;
                        massiv_pall[i][j] = de;
                    massiv_task[i][j] = de;
                    massiv_parr_for[i][j] = de;
                    massiv_parr_linq[i][j] = de;
                    if(block==1)
                    {
                        Console.Write(de);
                    }
                  

                    }
                } 
           

          //  size = massiv.GetUpperBound(0);
           
            Console.WriteLine("\n____________________________________\n");


          
            //последовательный метод
            
            sw.Start();
            for (int i = 0; i < proc_count; i++)
            {
                Posl_function(i);
            }
            sw.Stop();
            Console.WriteLine("\nВремя  выполнения последовательного метода-  " + sw.ElapsedMilliseconds+ "\n");




            //параллельный треды
            sw.Reset();
            sw.Start();
            Thread[] th = new Thread[proc_count];  
            for (int i = 0; i < proc_count; i++)
            {
                th[i] = new Thread(Parr_function);
                th[i].Start(i);
            }
            for (int i = 0; i < proc_count; i++)
            {
                th[i].Join();
            } 
            sw.Stop();

            Console.WriteLine("\nВремя  выполнения параллельного метода - " + sw.ElapsedMilliseconds+ "\n");

            // task
            Task[] ts = new Task[ProcCount];
            sw.Reset();
            sw.Start();
            for (int i = 0; i < proc_count; i++)
            {
                ts[i] = new Task(Paral_Tasks, i);
                ts[i].Start();
            }
            Task.WaitAll(ts); 
            sw.Stop();
            Console.WriteLine("\n Время  выполнения Task-  " + sw.ElapsedMilliseconds+ "\n");

            //Parallel For
            sw.Reset();
            sw.Start();
            Parallel.For(0, proc_count, k =>
            {
                int N = (int)k;
                for (int i = Convert.ToInt32(L * N); i < Convert.ToInt32(L * (1 + N)); i++)
                {
                    for (int j = 0; j <= massiv.GetUpperBound(0); j++)
                    {
                        for (int h = 0; h <= consonants.GetUpperBound(0); h++)
                        {

                            if (consonants[h] == massiv_parr_for[i][j])
                            {
                                massiv_parr_for[i][j] = '%';


                            }
                        }
                        if (block == 1)
                        {
                            Console.Write(massiv_parr_for[i][j]);
                        }
                       
                    }
                }
            }
            ); 
            sw.Stop();
            Console.WriteLine("\n Время  выполнения Parallel For  -  " + sw.ElapsedMilliseconds+ "\n");

            // Ap Parallel Linq
            var columns = Enumerable.Range(0, size);
            sw.Reset();
            sw.Start();


             var itog_linq = columns.AsParallel().Select(
                  column =>
                {


                    for (int i = 0; i <= massiv.GetUpperBound(0); i++)
                    {
                        
                            for (int h = 0; h <= consonants.GetUpperBound(0); h++)
                            { 
                               char[] consonants2 = "AEIUOY".ToCharArray();

                            if (consonants2[h] == massiv[column][i])
                                {
                                    massiv_parr_linq[column][i] = '%';
                                 
                                 
                            }
                            }
                        
                      
                    }
                    return true;
                }


            ).ToArray();
             
            sw.Stop();


            for (int i = 0; i <= massiv.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= massiv.GetUpperBound(0); j++)
                {
                    if (block == 1)
                    {
                        Console.Write(massiv_parr_linq[i][j]);
                    }
                }
            }
            Console.WriteLine("\nВремя  выполнения AsParallel -" + sw.ElapsedMilliseconds+ "\n");

            Console.ReadKey();
        }

        public static void Posl_function(int N)
        {
            for (int i = Convert.ToInt32(L * N); i < Convert.ToInt32(L * (1 + N)); i++)
            {
                for (int j = 0; j <= massiv.GetUpperBound(0); j++)
                {
                    for (int h = 0; h < consonants.GetUpperBound(0); h++)
                    {
                        
                        if (consonants[h] == massiv_posl[i][j])
                        {
                            massiv_posl[i][j] = '%';

                        }
                    }
                    if (block == 1)
                    {
                        Console.Write(massiv_posl[i][j]);
                    }
                }
            }
        }

        public static void Parr_function(Object o)
        {
            int N= size;
            N = (int)o;

            for (int i = Convert.ToInt32(L * N); i < Convert.ToInt32(L * (1 + N)); i++)
            {
                for (int j = 0; j <= massiv.GetUpperBound(0); j++)
                {
                    for (int h = 0; h < consonants.GetUpperBound(0); h++)
                    {

                        if (consonants[h] == massiv_pall[i][j])
                        {
                            massiv_pall[i][j] = '%';


                        }
                    }
                    if (block == 1)
                    {
                        Console.Write(massiv_pall[i][j]);

                    }
                }
            }
        }

        public static void Paral_Tasks(Object o)
        {
            int N = (int)o;
            for (int i = Convert.ToInt32(L * N); i < Convert.ToInt32(L * (1 + N)); i++)
            {
                for (int j = 0; j <= massiv.GetUpperBound(0); j++)
                {
                    for (int h = 0; h < consonants.GetUpperBound(0); h++)
                    {

                        if (consonants[h] == massiv_task[i][j])
                        {
                            massiv_task[i][j] = '%';


                        }
                    }
                    if (block == 1)
                    {
                        Console.Write(massiv_task[i][j]);
                    }
                }
            }
        }


    }

}

