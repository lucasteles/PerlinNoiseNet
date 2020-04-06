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
            var perlinNoise2 = new PerlinNoisePermutation();
            var random = new Random();

            var useRandom = false;

            var genIndex = 0;

            _ = Task.Run(() =>
            {
                while (true)
                {
                    var key = Console.ReadKey();

                    if (key.Key == ConsoleKey.Spacebar)
                        useRandom = !useRandom;


                    genIndex++;
                    if (genIndex > 2)
                        genIndex = 0;

                }
            });


            var t = 0d;
            while (true)
            {

                var r = genIndex switch
                {
                    0 => perlinNoise.Next(t),
                    1 => perlinNoise2.Next(t),
                    2 => random.NextDouble(),
                    _ => 0d,
                };


                var n = (int)(r * 100);

                Console.WriteLine(new string('-', n));


                t += 0.05;
                Thread.Sleep(1000 / 20);
            }


        }
    }
}
