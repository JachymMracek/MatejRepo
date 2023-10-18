using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Reflection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using static System.Net.Mime.MediaTypeNames;


namespace ConsoleApp60
{
    internal class Program
    {
        public class Picture
        {
            public int side;
            public Image<Rgba32> image;
            public Picture(int sideEntry)
            {
                side = sideEntry;
                image = new Image<Rgba32>(Configuration.Default, side, side);
            }
            public void GenerateTrivialPicture()
            {
                int pixel = 0;

                for (int r = 0; r < 256; r++)
                {
                    for (int g = 0; g < 256; g++)
                    {
                        for (int b = 0; b < 256; b++)
                        {
                            image[pixel % side, pixel / side] = new Rgba32((byte)r, (byte)g, (byte)b);
                            pixel++;
                        }
                    }
                }
            }
            public void GenerateRandomPicture()
            {
                List<(byte, byte, byte)> colors = new List<(byte, byte, byte)>();
                for (int r = 0; r < 256; r++)
                {
                    for (int g = 0; g < 256; g++)
                    {
                        for (int b = 0; b < 256; b++)
                        {
                            colors.Add(((byte)r, (byte)g, (byte)b));
                        }
                    }
                }

                Random random = new Random(Guid.NewGuid().GetHashCode());
                colors = colors.OrderBy(_ => random.Next()).ToList();

                int index = 0;

                for (int y = 0; y < 4096; y++)
                {
                    for (int x = 0; x < 4096; x++)
                    {
                        image[x, y] = new Rgba32(colors[index].Item1, colors[index].Item2, colors[index].Item3);
                        index++;
                    }
                }
            }
            public (int, int, int) GetRGBpixel(int rStart, int gStart, int bStart)
            {
                int r = rStart;
                int g = gStart;
                int b = bStart;

                r++;

                if (r == 256)
                {
                    r = 0;
                    g++;
                }
                if (g == 256)
                {
                    g = 0;
                    b++;
                }

                return (r, g, b);
            }
            public void GeneratePatternPicture()
            {
                int round = 0;

                int r = 0;
                int g = 0;
                int b = 0;
                int pixel = 0;

                while (round <= 2047)
                {
                    for (int i = -round; i <= round; i++)
                    {
                        image[2047 + i, 2047 + round] = new Rgba32((byte)r, (byte)g, (byte)b);

                        (int, int, int) RGBpixel1 = GetRGBpixel(r, g, b);
                        r = RGBpixel1.Item1;
                        g = RGBpixel1.Item2;
                        b = RGBpixel1.Item3;
                        pixel++;

                        if (round != 0)
                        {
                            image[2047 + i, 2047 - round] = new Rgba32((byte)r, (byte)g, (byte)b);

                            (int, int, int) RGBpixel2 = GetRGBpixel(r, g, b);
                            r = RGBpixel2.Item1;
                            g = RGBpixel2.Item2;
                            b = RGBpixel2.Item3;
                            pixel++;
                        }

                    }
                    if (round != 0)
                    {
                        for (int i = -round + 1; i <= round - 1; i++)
                        {
                            image[2047 + round, 2047 + i] = new Rgba32((byte)r, (byte)g, (byte)b);

                            (int, int, int) RGBpixel1 = GetRGBpixel(r, g, b);
                            r = RGBpixel1.Item1;
                            g = RGBpixel1.Item2;
                            b = RGBpixel1.Item3;

                            image[2047 - round, 2047 + i] = new Rgba32((byte)r, (byte)g, (byte)b);

                            (int, int, int) RGBpixel = GetRGBpixel(r, g, b);
                            r = RGBpixel.Item1;
                            g = RGBpixel.Item2;
                            b = RGBpixel.Item3;
                            pixel += 2;
                        }
                    }

                    round++;
                }
                for (int i = -round + 1; i <= round; i++)
                {
                    image[2047 + i, 2047 + round] = new Rgba32((byte)r, (byte)g, (byte)b);

                    (int, int, int) RGBpixel1 = GetRGBpixel(r, g, b);
                    r = RGBpixel1.Item1;
                    g = RGBpixel1.Item2;
                    b = RGBpixel1.Item3;
                    pixel += 1;
                }
                for (int i = -round + 1; i <= round - 1; i++)
                {
                    image[2047 + round, 2047 + i] = new Rgba32((byte)r, (byte)g, (byte)b);

                    (int, int, int) RGBpixel = GetRGBpixel(r, g, b);
                    r = RGBpixel.Item1;
                    g = RGBpixel.Item2;
                    b = RGBpixel.Item3;
                    pixel += 1;
                }
            }
        }
        static void Main(string[] args)
        {
            Picture pictureTrivial = new Picture(4096);
            Picture pictureRandom = new Picture(4096);
            Picture picturePattern = new Picture(4096);

            pictureTrivial.GenerateTrivialPicture();
            pictureRandom.GenerateRandomPicture();
            picturePattern.GeneratePatternPicture();

            pictureTrivial.image.Save("1.png");
            pictureRandom.image.Save("2.png");
            picturePattern.image.Save("3.png");
        }
    }
}