using System;

namespace PerlinNoiseNet
{

    public class PerlinNoise
    {
        const int PERLIN_YWRAPB = 4;
        const int PERLIN_YWRAP = 1 << PERLIN_YWRAPB;
        const int PERLIN_ZWRAPB = 8;
        const int PERLIN_ZWRAP = 1 << PERLIN_ZWRAPB;
        const int PERLIN_SIZE = 4095;

        double perlin_octaves = 4; // default to medium smooth
        double perlin_amp_falloff = 0.5; // 50% reduction/octave

        double scaled_cosine(double i) => 0.5 * (1.0 - Math.Cos(i * Math.PI));

        Random random;

        double[] perlin = null; // will be initialized lazily by noise() 

        public PerlinNoise(int? seed = null)
        {

            if (seed.HasValue)
                SetNoiseSeed(seed.Value);
            else
                random = new Random();

        }


        public double Next(double x, double y = 0, double z = 0)
        {
            if (perlin == null)
            {
                perlin = new double[PERLIN_SIZE + 1];
                for (var i = 0; i < PERLIN_SIZE + 1; i++)
                {
                    perlin[i] = random.NextDouble();
                }
            }

            if (x < 0)
            {
                x = -x;
            }
            if (y < 0)
            {
                y = -y;
            }
            if (z < 0)
            {
                z = -z;
            }

            int xi = (int)Math.Floor(x),
                yi = (int)Math.Floor(y),
                zi = (int)Math.Floor(z);

            var xf = x - xi;
            var yf = y - yi;
            var zf = z - zi;
            double rxf, ryf;

            var r = 0d;
            var ampl = 0.5;

            double n1, n2, n3;

            for (var o = 0; o < perlin_octaves; o++)
            {
                var of = xi + (yi << PERLIN_YWRAPB) + (zi << PERLIN_ZWRAPB);

                rxf = scaled_cosine(xf);
                ryf = scaled_cosine(yf);

                n1 = perlin[of & PERLIN_SIZE];
                n1 += rxf * (perlin[(of + 1) & PERLIN_SIZE] - n1);
                n2 = perlin[(of + PERLIN_YWRAP) & PERLIN_SIZE];
                n2 += rxf * (perlin[(of + PERLIN_YWRAP + 1) & PERLIN_SIZE] - n2);
                n1 += ryf * (n2 - n1);

                of += PERLIN_ZWRAP;
                n2 = perlin[of & PERLIN_SIZE];
                n2 += rxf * (perlin[(of + 1) & PERLIN_SIZE] - n2);
                n3 = perlin[(of + PERLIN_YWRAP) & PERLIN_SIZE];
                n3 += rxf * (perlin[(of + PERLIN_YWRAP + 1) & PERLIN_SIZE] - n3);
                n2 += ryf * (n3 - n2);

                n1 += scaled_cosine(zf) * (n2 - n1);

                r += n1 * ampl;
                ampl *= perlin_amp_falloff;
                xi <<= 1;
                xf *= 2;
                yi <<= 1;
                yf *= 2;
                zi <<= 1;
                zf *= 2;

                if (xf >= 1.0)
                {
                    xi++;
                    xf--;
                }
                if (yf >= 1.0)
                {
                    yi++;
                    yf--;
                }
                if (zf >= 1.0)
                {
                    zi++;
                    zf--;
                }
            }
            return r;

        }


        public void SetNoiseDetail(double lod, double falloff)
        {
            if (lod > 0)
                perlin_octaves = lod;
            if (falloff > 0)
                perlin_amp_falloff = falloff;
        }



        public void SetNoiseSeed(int seed) => random = new Random(seed);


    }
}
