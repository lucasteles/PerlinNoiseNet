using PerlinNoiseNet;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {

        static void Main(string[] args)
        {

            var perlinNoise = new PerlinNoise();
            var random = new Random();

            var useRandom = false;


            _ = Task.Run(() =>
            {
                while (true)
                {
                    var key = Console.ReadKey();

                    if (key.Key == ConsoleKey.Spacebar)
                        useRandom = !useRandom;
                }
            });


            var t = 0d;
            while (true)
            {

                var r = useRandom
                    ? random.NextDouble()
                    : perlinNoise.Next(t);

                var n = (int)(r * 100);

                Console.WriteLine(new string('-', n));


                t += 0.05;
                Thread.Sleep(1000 / 20);
            }


        }
    }
}
