using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using static System.Net.Mime.MediaTypeNames;
using CommandLine;
using System.Reflection.Metadata.Ecma335;

namespace ConsoleApp60
{
    internal class Program
    {
        public class Options
        {
            [Option('m', "mode", Required = true, HelpText = "Jaký řežím chcete? trivial,random nebo pattern:")]
            public string Mode { get; set; }

            [Option('n', "name", Required = true, HelpText = "Jak chcete obrázek pojmenovat:")]
            public string FileName { get; set; }
        }
        public class Picture
        {
            public int width;
            public int height;
            public Image<Rgba32> image;
            public Picture(int widthInput, int heightInput)
            {
                width = widthInput;
                height = heightInput;
                image = new Image<Rgba32>(Configuration.Default, width, height);
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
                            image[pixel % width, pixel / height] = new Rgba32((byte)r, (byte)g, (byte)b);
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

                // Tvoříme čtverce. Hodnota 2047 je poslední kolo čtverce, který se vejde na plochu 4096*4096.

                while (round <= 2047) // (2*round + 1) ** 2 = 4096 ** 2 > round = 2047,5 > 2047 celých kol.
                {
                    for (int i = -round; i <= round; i++) // Cyklus pro horní část čtverce.
                    {
                        image[2047 + i, 2047 + round] = new Rgba32((byte)r, (byte)g, (byte)b); // středový pixel je 2047, jelikož 4095 // 2 = 2047, kde 4095 je maximální index pixelu.
                        //  na konci zbude pouze horní část čtverce a pracvá část čtverce. jelikož nám vyšlo necelé číslo počet kol.

                        (int, int, int) RGBpixel1 = GetRGBpixel(r, g, b);
                        r = RGBpixel1.Item1;
                        g = RGBpixel1.Item2;
                        b = RGBpixel1.Item3;
                        pixel++;

                        if (round != 0) // abychom nepřebarvili počáteční pixel.
                        {
                            image[2047 + i, 2047 - round] = new Rgba32((byte)r, (byte)g, (byte)b); // pro dolní část čtverce

                            (int, int, int) RGBpixel2 = GetRGBpixel(r, g, b);
                            r = RGBpixel2.Item1;
                            g = RGBpixel2.Item2;
                            b = RGBpixel2.Item3;
                            pixel++;
                        }

                    }
                    if (round != 0) // abychom nepřebarvili počáteční pixel.
                    {
                        for (int i = -round + 1; i <= round - 1; i++) // pro strany čtverce.
                        {
                            image[2047 + round, 2047 + i] = new Rgba32((byte)r, (byte)g, (byte)b); // pro strany čtverce.

                            (int, int, int) RGBpixel1 = GetRGBpixel(r, g, b);
                            r = RGBpixel1.Item1;
                            g = RGBpixel1.Item2;
                            b = RGBpixel1.Item3;

                            image[2047 - round, 2047 + i] = new Rgba32((byte)r, (byte)g, (byte)b); // pro strany čtverce.

                            (int, int, int) RGBpixel = GetRGBpixel(r, g, b);
                            r = RGBpixel.Item1;
                            g = RGBpixel.Item2;
                            b = RGBpixel.Item3;
                            pixel += 2;
                        }
                    }

                    round++;
                }

                // dokončíme paletu, kde chceme vybarvit dvě části

                for (int i = -round + 1; i <= round; i++)  // první část. HORNÍ
                {
                    image[2047 + i, 2047 + round] = new Rgba32((byte)r, (byte)g, (byte)b);

                    (int, int, int) RGBpixel1 = GetRGBpixel(r, g, b);
                    r = RGBpixel1.Item1;
                    g = RGBpixel1.Item2;
                    b = RGBpixel1.Item3;
                    pixel += 1;
                }
                for (int i = -round + 1; i <= round - 1; i++) // druhá část. PRAVÁ
                {
                    image[2047 + round, 2047 + i] = new Rgba32((byte)r, (byte)g, (byte)b);

                    (int, int, int) RGBpixel = GetRGBpixel(r, g, b);
                    r = RGBpixel.Item1;
                    g = RGBpixel.Item2;
                    b = RGBpixel.Item3;
                    pixel += 1;
                }

                // Console.WriteLine(pixel); 16777216 = 4096 ** 2. Máme všechny pixely.
            }
        }
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                if (o.Mode == null || o.FileName == null)
                {
                    Console.WriteLine("Napište všechny parametry");
                    return;
                }

                if (o.Mode != "trivial" && o.Mode != "pattern" && o.Mode != "random")
                {
                    Console.WriteLine("Špatný mode");
                    return;
                }

                if (o.Mode == "trivial")
                {
                    Picture pictureTrivial = new Picture(4096, 4096);
                    pictureTrivial.GenerateTrivialPicture();
                    pictureTrivial.image.Save($"{o.FileName}.png");
                }
                else if (o.Mode == "random")
                {
                    Picture pictureRandom = new Picture(4096, 4096);
                    pictureRandom.GenerateRandomPicture();
                    pictureRandom.image.Save($"{o.FileName}.png");
                }
                else if (o.Mode == "pattern")
                {
                    Picture picturePattern = new Picture(4096, 4096);
                    picturePattern.GeneratePatternPicture();
                    picturePattern.image.Save($"{o.FileName}.png");
                }
            });
        }

    }
}