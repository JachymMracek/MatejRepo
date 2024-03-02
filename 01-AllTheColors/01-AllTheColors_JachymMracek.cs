using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Reflection;
using System.Text;
using CommandLine.Text;
using CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.PixelFormats;
using static System.Net.Mime.MediaTypeNames;
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

            [Option('w', "name", Required = true, HelpText = "Jakou chcete šířku obrázku: (doporučeno 4095)")]
            public string Width { get; set; }

            [Option('h', "height", Required = true, HelpText = "Jakou chcete výšku obrázku: (doporučeno 4095)")]
            public string Height { get; set; }

        }
        public class Picture
        {
            public int width;
            public int height;
            public Image<Rgba32> image;
            public List<(byte, byte, byte)> colors;

           
            public Picture(int widthInput, int heightInput)
            {
                width = widthInput;
                height = heightInput;
                colors = new List<(byte, byte, byte)>();
                image = new Image<Rgba32>(Configuration.Default, width, height);

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
            }
            public void GenerateTrivialPicture()
            {
                int pixel = 0;
                int index = 0;

                for (int i = 0;i < colors.Count; i++) 
                {
                    image[pixel % width, pixel / height] = new Rgba32(colors[index].Item1, colors[index].Item2, colors[index].Item3);
                    pixel++;
                    index++;

                    if (index == colors.Count)
                    {
                        index = 0;
                    }
                }
            }
            public void GenerateRandomPicture()
            {
                int index = 0;
                Random random = new Random(Guid.NewGuid().GetHashCode());
                colors = colors.OrderBy(_ => random.Next()).ToList();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        image[x, y] = new Rgba32(colors[index].Item1, colors[index].Item2, colors[index].Item3);
                        index++;

                        if (index == colors.Count)
                        {
                            index = 0;
                            colors = colors.OrderBy(_ => random.Next()).ToList();
                        }
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
                int index = 0;

                int r = 0;
                int g = 0;
                int b = 0;
                int pixel = 0;
                int sideX = (width - 1) / 2;
                int sideY = (height - 1) / 2;
                int maxSize = Math.Max(width, height);

                double roundMaxDouble = (Math.Sqrt(maxSize * maxSize) - 1) / 2;
                int roundMax = (int)Math.Ceiling(roundMaxDouble);

                while (round <= roundMax)
                {
                    
                    for (int i = -round; i <= round; i++)
                    {
                        if (sideX + i < width && 0 <= sideX + i && sideY + round < height && 0 <= sideY + round)
                        {
                            image[sideX + i, sideY + round] = new Rgba32(colors[index].Item1, colors[index].Item2, colors[index].Item3);

                            (int, int, int) RGBpixel1 = GetRGBpixel(r, g, b);
                            r = RGBpixel1.Item1;
                            g = RGBpixel1.Item2;
                            b = RGBpixel1.Item3;
                            pixel++;
                            index++;

                            if (index == colors.Count)
                            {
                                index = 0;
                            }
                        }

                        if (round != 0 && sideX + i < width && 0 <= sideX + i && sideY - round < height && 0 <= sideY - round)
                        {
                            image[sideX + i, sideY - round] = new Rgba32(colors[index].Item1, colors[index].Item2, colors[index].Item3);

                            (int, int, int) RGBpixel2 = GetRGBpixel(r, g, b);
                            r = RGBpixel2.Item1;
                            g = RGBpixel2.Item2;
                            b = RGBpixel2.Item3;
                            pixel++;
                            index++;

                            if (index == colors.Count)
                            {
                                index = 0;
                            }
                        }
                    }

                    for (int i = -round + 1; i <= round - 1; i++)
                    {
                        if (round != 0 && sideX + round < width && 0 <= sideX + round && sideY + i < height && 0 <= sideY + i)
                        {
                            image[sideX + round, sideY + i] = new Rgba32(colors[index].Item1, colors[index].Item2, colors[index].Item3);

                            (int, int, int) RGBpixel1 = GetRGBpixel(r, g, b);
                            r = RGBpixel1.Item1;
                            g = RGBpixel1.Item2;
                            b = RGBpixel1.Item3;
                            pixel++;
                            index++;

                            if (index == colors.Count)
                            {
                                index = 0;
                            }
                        }

                        if (round != 0 && sideX - round < width && 0 <= sideX - round && sideY + i < height && 0 <= sideY + i)
                        {
                            
                            image[sideX - round, sideY + i] = new Rgba32(colors[index].Item1, colors[index].Item2, colors[index].Item3);

                            (int, int, int) RGBpixel = GetRGBpixel(r, g, b);
                            r = RGBpixel.Item1;
                            g = RGBpixel.Item2;
                            b = RGBpixel.Item3;
                            pixel += 1;
                            index+= 1;

                            if (index == colors.Count)
                            {
                                index = 0;
                            }
                        }
                    }
                    round++;

                }
            }
        }

        static void Main(string[] args)
        {
            int width, height;

            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                if (o.Mode == null || o.FileName == null || o.Width == null || o.Height == null)
                {
                    Console.WriteLine("Napište všechny parametry");
                    return;
                }

                if (o.Mode != "trivial" && o.Mode != "pattern" && o.Mode != "random")
                {
                    Console.WriteLine("Špatný mode");
                    return;
                }

                if (int.TryParse(o.Width, out width) && int.TryParse(o.Height, out height))
                {
                    if (width * height < 4096 * 4096)
                    {
                        Console.WriteLine("Plocha má málo pixelů.");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Nesprávný vstup pro výšku nebo šířku.");
                    return;
                }

                if (o.Mode == "trivial")
                {
                    Picture pictureTrivial = new Picture(width, height);
                    pictureTrivial.GenerateTrivialPicture();
                    pictureTrivial.image.Save($"{o.FileName}.png");
                }
                else if (o.Mode == "random")
                {
                    Picture pictureRandom = new Picture(width,height);
                    pictureRandom.GenerateRandomPicture();
                    pictureRandom.image.Save($"{o.FileName}.png");
                }
                else if (o.Mode == "pattern")
                {
                    Picture picturePattern = new Picture(width, height);
                    picturePattern.GeneratePatternPicture();
                    picturePattern.image.Save($"{o.FileName}.png");
                }
            });
        }
    }
}
