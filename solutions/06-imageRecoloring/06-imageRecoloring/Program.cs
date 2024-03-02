using CommandLine.Text;
using CommandLine;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using static _06_imageRecoloring.Program;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.ColorSpaces;

namespace _06_imageRecoloring
{
  internal class Program
  {
    static HashSet<(int,int)> pixelsSkin = new HashSet<(int,int)> ();
    public class Options
    {
      [Option('i', "input image", Required = true, HelpText = "Please, write down path of picture witch you want recoloring:")]
      public string Input { get; set; }

      [Option('h', "hsv change", Required = true, HelpText = "Please, select the color for recoloring selected picture:")]
      public string H { get; set; }

      [Option('o', "output image", Required = true, HelpText = "Please, select the name of recoloring picture (example: obrazek) ")]
      public string Output { get; set; }

      [Option('d', "delete wrong pixels", Required = true, HelpText = "Do you want improve coloring (if yes, write down > yes)?")]
      public string Delete { get; set; }
    }
    public class Picture
    {
      public string ImageOutput;
      public Image<Rgba32> InputImage;
      public Image<Rgba32> OutputImage;
      public float delta = 0;

      public Picture (string imageInput, string imageOutput)
      {
        ImageOutput = imageOutput;
        InputImage = Image.Load<Rgba32>(imageInput);
        OutputImage = new Image<Rgba32>(InputImage.Width, InputImage.Height);
      }

      public void Recoloring (float deltaH)
      {
        delta = deltaH;

        for (int i = 0; i < InputImage.Height; i++)
        {
          for (int j = 0; j < InputImage.Width; j++)
          {
            Rgb inputColor = InputImage[j, i];
            Rgb inputRgb = new Rgb(inputColor.R, inputColor.G, inputColor.B);

            if (IsSkinColor(inputRgb))
            {
              OutputImage[j, i] = inputColor;
              pixelsSkin.Add((j,i));
            }
            else
            {
              Hsv inputHsv = ColorSpaceConverter.ToHsv(inputRgb);
              Hsv outputHsv = new Hsv((inputHsv.H + deltaH) % 360, inputHsv.S, inputHsv.V);
              Rgb outputRgb = ColorSpaceConverter.ToRgb(outputHsv);
              OutputImage[j, i] = new Rgb(outputRgb.R, outputRgb.G, outputRgb.B);
            }
          }
        }
      }
      
        private bool IsSkinColor (Rgba32 pixel)
        {
        int R = pixel.R;
        int G = pixel.G;
        int B = pixel.B;

        double Y = 0.299 * R + 0.587 * G + 0.114 * B;
        double Cb = -0.1687 * R - 0.3313 * G + 0.5 * B + 128;
        double Cr = 0.5 * R - 0.4187 * G - 0.0813 * B + 128;

        return (10 < Y && Y < 220) && (130 < Cr && Cr < 180) && (70 < Cb && Cb < 130);

      }
      public void Check()
      {
        int side = 15;
        long correctness = 0;

        for (int i = 0; i < InputImage.Height; i++)
        {
          for (int j = 0; j < InputImage.Width; j++)
          {
            correctness = 0;

            for (int k = i; k < i + side; k++)
            {
              for (int l = j; l < j + side; l++)
              {
                if (pixelsSkin.Contains((l, k)))
                {
                  correctness++;
                }
                else if ( -1 < k && k < InputImage.Height && -1 < l && l < InputImage.Width)
                {
                  correctness--;
                }
              }
            }
            if (correctness >= 0)
            {
              Rgb inputColor = InputImage[j, i];
              Rgb inputRgb = new Rgb(inputColor.R, inputColor.G, inputColor.B);

              Hsv inputHsv = ColorSpaceConverter.ToHsv(inputRgb);
              Hsv outputHsv = new Hsv((inputHsv.H - delta) % 360, inputHsv.S, inputHsv.V);
              Rgb outputRgb = ColorSpaceConverter.ToRgb(outputHsv);
              OutputImage[j, i] = new Rgb(outputRgb.R, outputRgb.G, outputRgb.B);

              OutputImage[j, i] = inputColor;
              pixelsSkin.Add((j, i));
            }
          }
        }
      }
    }

    static void Main (string[] args)
    {
      Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
      {
        float h = 0;
        if (o.Input == null || o.H == null || o.Output == null)
        {
          Console.WriteLine("Napište všechny parametry");
          return;
        }
        try
        {
          h = float.Parse(o.H);
        }
        catch (FormatException)
        {
          Console.WriteLine("Nelze převést řetězec na float.");
        }

        Picture picture = new Picture(o.Input,o.Output);
        picture.Recoloring(h);

        if (o.Delete == "yes")
        {
          picture.Check();
        }
        picture.OutputImage.Save($"{o.Output}.png");
      });
    }
  }
}
