using SixLabors.ImageSharp.Processing.Processors.Quantization;
using System.Diagnostics.SymbolStore;
using static ConsoleApp1.Program;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Runtime.InteropServices;
using System.Numerics;
using SixLabors.Fonts.Tables.AdvancedTypographic;
using CommandLine.Text;
using CommandLine;

namespace ConsoleApp1
{
  internal class Program
  {
    static int Radius = 0;
    static int metric = 0;
    static int r = 0;
    static int dx = 0;
    static int dy = 0;
    static List<(double,double)> pixels = new List<(double,double)>();
    static List<(double,double)> pixels1 = new List<(double,double)>();
    static double angle = 0;
    static int rad = 0;

    public class Options
    {
      [Option('s', "Symmetry", Required = true, HelpText = "how much symmetry should the image have? (5 to 12):")]
      public int Symmetry { get; set; }

      [Option('c', "Circle", Required = true, HelpText = "Do you want middle circle? (yes / no):")]
      public string Circle { get; set; }

      [Option('e', "EndCircle", Required = true, HelpText = " Do you want endedCircle? (yes / no):")]
      public string EndCircle { get; set; }

      [Option('d', "DividedShape", Required = true, HelpText = " Do you want dividedShape (yes / no):")]
      public string DividedShape { get; set; }

      [Option('w', "width", Required = true, HelpText = "How wide do you want the image to be? ( for example: 4096):")]
      public string Width { get; set; }

      [Option('h', "height", Required = true, HelpText = "how tall do you want the image to be? ( for example: 4096):")]
      public string Height { get; set; }

      [Option('n', "name", Required = true, HelpText = "\r\nWhat do you want to name the picture? (for example: picture):")]
      public string FileName { get; set; }
    }
    public class Circle
    {
      public HashSet<(int, int)> points = new HashSet<(int, int)>();
      public Circle (double centerX, double centerY, int radius, Picture picture, string Tag)
      {
        if (Tag == "1")
        {
          for (int i = (int)centerY - radius; i < centerY + radius; i++)
          {
            for (int j = 0; j < picture.Width; j++)
            {
              double distance = Math.Sqrt((i - centerY) * (i - centerY) + (j - centerX) * (j - centerX));

              if (distance < radius)
              {
                var color = new Rgba32(0, 0, 255);
                picture.Image[j, i] = color;
                pixels1.Add((j, i));
              }
            }
          }
          return;
        }

        if (Tag == "2")
        {
          for (int i = (int)centerY - radius; i < centerY + radius; i++)
          {
            for (int j = 0; j < picture.Width; j++)
            {
              double distance = Math.Sqrt((i - centerY) * (i - centerY) + (j - centerX) * (j - centerX));

              if (distance < radius && distance < radius && distance > radius - 0.05 * radius && picture.Image[j, i] == new Rgba32(242, 193, 78))
              {
                var color = new Rgba32(247, 129, 84);
                picture.Image[j, i] = color;
                points.Add((j, i));
              }
            }
          }
          return;
        }

        if (Tag == "3")
        {
          for (int i = (int)centerY - radius; i < centerY + radius; i++)
          {
            for (int j = 0; j < picture.Width; j++)
            {
              double distance = Math.Sqrt((i - centerY) * (i - centerY) + (j - centerX) * (j - centerX));

              if (distance < radius && distance < radius && distance > radius - 0.05 * radius && picture.Image[j, i] == new Rgba32(242, 193, 78))
              {
                var color = new Rgba32(0, 172, 0);
                picture.Image[j, i] = color;
                points.Add((j, i));
              }
            }
          }
          return;
        }

        if (Tag == "4")
        {
          for (int i = (int)centerY - radius; i < centerY + radius; i++)
          {
            for (int j = 0; j < picture.Width; j++)
            {
              double distance = Math.Sqrt((i - centerY) * (i - centerY) + (j - centerX) * (j - centerX));

              if (distance < radius && distance < radius && distance > radius - 0.05 * radius && picture.Image[j, i] == new Rgba32(242, 193, 78))
              {
                var color = new Rgba32(0, 0, 100);
                picture.Image[j, i] = color;
                points.Add((j, i));
              }
            }
          }
          return;
        }
        if (Tag == "5")
        {
          for (int i = (int)centerY - radius; i < centerY + radius; i++)
          {
            for (int j = 0; j < picture.Width; j++)
            {
              double distance = Math.Sqrt((i - centerY) * (i - centerY) + (j - centerX) * (j - centerX));

              if (distance < radius && distance < radius && distance > radius - 0.05 * radius && picture.Image[j, i] != new Rgba32(242, 193, 78) && picture.Image[j, i] != new Rgba32(0, 0, 255))
              {
                var color = new Rgba32(128, 0, 128);
                picture.Image[j, i] = color;
                points.Add((j, i));
              }
            }
          }
          return;
        }

        if (Tag == "6")
        {
          for (int i = (int)centerY - radius; i < centerY + radius; i++)
          {

            for (int j = 0; j < picture.Width; j++)
            {
              double distance = Math.Sqrt((i - centerY) * (i - centerY) + (j - centerX) * (j - centerX));
              Rgba32 pixelColor = picture.Image[j, i];

              if (distance < radius && pixelColor == new Rgba32(255, 255, 255))
              {
                picture.Image[j, i] = new Rgba32(247, 129, 84);
              }
            }
          }
          return;
        }

        for (int i = (int)centerY - radius; i < centerY + radius; i++)
        {
          for (int j = 0; j < picture.Width; j++)
          {
            double distance = Math.Sqrt((i - centerY) * (i - centerY) + (j - centerX) * (j - centerX));

            if (distance < radius && distance > radius - 0.05 * radius)
            {
              var color = new Rgba32(0, 0, 0);
              picture.Image[j, i] = color;
              points.Add((j, i));
            }
          }
        }
      }
    }
    public class Picture
    {
      public Image<Rgba32> Image { get; set; }
      public int Width { get; set; }
      public int Height { get; set; }
      public Picture (int width, int height)
      {
        this.Image = new Image<Rgba32>(width, height);
        this.Width = width;
        this.Height = height;

        for (int i = 0; i < width; i++)
        {
          for (int j = 0; j < height; j++)
          {
            var color = new Rgba32(255, 255, 255);
            this.Image[i, j] = color;
          }
        }
      }
    }
    static void Colored (Picture picture, Circle circleStart, Circle circleEnd, Circle CircleSemi, Circle circleSemi2)
    {
      var commonPoints = new HashSet<(int, int)>(circleStart.points);
      commonPoints.IntersectWith(circleEnd.points);

      var commonPoints1 = new HashSet<(int, int)>(circleStart.points);
      commonPoints1.IntersectWith(circleSemi2.points);

      List<(int, int)> list1 = commonPoints.OrderBy(point => point.Item1).ToList();

      int minItem2 = list1.Min(item => item.Item2);
      int maxItem2 = list1.Max(item => item.Item2);
      int minItem1 = list1.Min(item => item.Item1);
      int maxItem1 = list1.Max(item => item.Item1);

      List<(int, int)> list2 = commonPoints1.OrderBy(point => point.Item1).ToList();
      int minItem2_ = list2.Min(item => item.Item2);
      int maxItem2_ = list2.Max(item => item.Item2);
      int minItem1_ = list2.Min(item => item.Item1);
      int maxItem1_ = list2.Max(item => item.Item1);

      r = (int)Math.Sqrt((minItem2 - picture.Height / 2) * (minItem2 - picture.Height / 2) + (maxItem1 - picture.Width / 2) * (maxItem1 - picture.Width / 2));
      dx = maxItem1;
      dy = minItem2;
      rad = (int)Math.Sqrt((minItem2_ - picture.Height / 2) * (minItem2_ - picture.Height / 2) + (minItem1_ - picture.Width / 2) * (minItem1_ - picture.Width / 2)) + metric / 51;


      for (int j = maxItem1; j > minItem1; j--)
      {
        bool start = false;
        bool colored = false;
        bool semi = false;

        for (int i = 0; i < picture.Height; i++)
        {
          if (circleStart.points.Contains((j, i)) && circleEnd.points.Contains((j, i)))
          {
            break;
          }
          if (circleEnd.points.Contains((j, i)) || CircleSemi.points.Contains((j, i)))
          {
            start = false;
          }
          else if (circleStart.points.Contains((j, i)) && colored)
          {
            break;
          }
          else if (circleStart.points.Contains((j, i)))
          {
            start = true;
          }
          else if (start)
          {
            if (j > maxItem1_ - metric / 100 && picture.Image[j, i] != new Rgba32(0, 0, 0))
            {
              var orangeColor = new Rgba32(242, 193, 78);
              pixels.Add((j, i));
              picture.Image[j, i] = orangeColor;
              colored = true;
              continue;
            }
            if (i > minItem2_ - metric / 100 && circleSemi2.points.Contains((j, i)))
            {
              semi = true;
              continue;
            }
            else if (i > minItem2_ - metric / 100 && semi)
            {
              var orangeColor1 = new Rgba32(242, 193, 78);
              pixels.Add((j, i));
              picture.Image[j, i] = orangeColor1;
              colored = true;
              continue;
            }
            else if (i > minItem2_ - metric / 100 && !semi)
            {
              continue;
            }
            if ((i <= minItem2_ && picture.Image[j, i] != new Rgba32(0, 0, 0)))
            {
              var orangeColor = new Rgba32(242, 193, 78);
              pixels.Add((j, i));
              picture.Image[j, i] = orangeColor;
              colored = true;
              continue;
            }
          }
        }
      }
    }
    static void DeleteBlack (Picture picture)
    {
      for (int i = picture.Image.Height / 2 - 2 * Radius; i < picture.Image.Height / 2 + 2 * Radius; i++)
      {
        for (int j = picture.Image.Width / 2 - 2 * Radius; j < picture.Image.Width / 2 + 2 * Radius; j++)
        {
          Rgba32 pixelColor = picture.Image[j, i];

          if (pixelColor == new Rgba32(0, 0, 0))
          {
            picture.Image[j, i] = new Rgba32(255, 255, 255);
          }
        }
      }
    }

    static void Rotation (Picture picture, double angle, int round, string Tag)
    {
      List<(double, double)> list;
      var Color = new Rgba32(0, 0, 255);

      if (Tag == "1")
      {
        list = pixels1;
        new Rgba32(0, 0, 255);
      }
      else
      {
        list = pixels;
        Color = new Rgba32(242, 193, 78);
      }

      double sina = Math.Sin(angle * round * (Math.PI / 180));
      double cosa = Math.Cos(angle * round * (Math.PI / 180));

      for (int i = 0; i < list.Count; i++)
      {
        double x = list[i].Item1 - picture.Width / 2;
        double y = list[i].Item2 - picture.Height / 2;

        double newX = x * cosa - y * sina;
        double newY = x * sina + y * cosa;

        newX += picture.Width / 2;
        newY += picture.Height / 2;

        var orangeColor = new Rgba32(242, 193, 78);

        int roundedX = (int)Math.Round(newX);
        int roundedY = (int)Math.Round(newY);

        if (roundedX >= 1 && roundedX + 1 < picture.Width && roundedY >= 1 && roundedY + 1 < picture.Height)
        {
          picture.Image[roundedX, roundedY] = Color;

          if (picture.Image[roundedX + 1, roundedY] == new Rgba32(255, 255, 255))
          {
            picture.Image[roundedX + 1, roundedY] = Color;
          }
          if ((picture.Image[roundedX - 1, roundedY] == new Rgba32(255, 255, 255)))
          {
            picture.Image[roundedX - 1, roundedY] = Color;
          }
          if ((picture.Image[roundedX, roundedY - 1] == new Rgba32(255, 255, 255)))
          {
            picture.Image[roundedX, roundedY - 1] = Color;
          }
          if ((picture.Image[roundedX, roundedY + 1] == new Rgba32(255, 255, 255)))
          {
            picture.Image[roundedX, roundedY + 1] = Color;
          }
        }
      }
    }
    static void MakeFlower (int symmetry, Picture picture)
    {
      angle = 360 / symmetry;
      double currentAngle = -angle / 2 - angle;
      List<Circle> circleList = new List<Circle>();

      for (int i = 1; i <= 4; i++)
      {
        Circle circle = new Circle(picture.Width / 2 + Radius * Math.Cos(currentAngle * (Math.PI / 180)), picture.Height / 2 - Radius * Math.Sin(currentAngle * (Math.PI / 180)), Radius, picture,"0");
        circleList.Add(circle);
        currentAngle += angle;
      }

      Colored(picture, circleList[1], circleList[2], circleList[0], circleList[3]);
      DeleteBlack(picture);

      for (int i = 2; i <= symmetry; i++)
      {
        Rotation(picture, angle, i - 1, "0");
      }

      int radius = Radius / 2;

      for (int i = picture.Height / 2 - radius; i < picture.Height / 2 + radius + 1; i++)
      {
        for (int j = picture.Width / 2 - radius; j < picture.Width / 2 + radius + 1; j++)
        {
          double distance = Math.Sqrt((i - picture.Height/2) * (i - picture.Height/2) + (j - picture.Width/2) * (j - picture.Width/2));

          if (distance < radius && picture.Image[j, i] == new Rgba32(255, 255, 255))
          {
            var color = new Rgba32(171,0,0);
            picture.Image[j, i] = color;
          }
        }
      }
    }
    static void MakeCircle (Picture picture)
    {
      Circle circle = new Circle(picture.Width / 2, picture.Height / 2, r, picture,"6");


      DeleteBlack(picture);
    }
    static void EndCircle (Picture picture, int symmetry)
    {
      Circle circle1 = new Circle(dx, dy, metric / 80, picture,"1");

      for (int i = 2; i <= symmetry; i++)
      {
        Rotation(picture, angle, i - 1, "1");
      }
    }
    static void Main (string[] args)
    {
      Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
      {

        if (o.Symmetry == null || o.Circle == null || o.EndCircle == null || o.Width == null || o.Height == null || o.DividedShape == null)
        {
          Console.WriteLine("You must set all arguments!");
          return;
        }
        if (int.Parse(o.Width) < 200 || int.Parse(o.Height) < 200)
        {
          Console.WriteLine("You added small side of picture, The picture would by nice, but is to small");
        }
        if (o.Symmetry < 5 || o.Symmetry > 12)
        {
          Console.WriteLine("You added wrong argumets for symmetry. You must choose number from 5 to 12!");
          return;
        }
        if (o.Circle != "yes" && o.Circle != "no")
        {
          Console.WriteLine("Your desition was badly written. Please write yes or no!");
          return;
        }
        if (o.EndCircle != "yes" && o.EndCircle != "no")
        {
          Console.WriteLine("Your desition was badly written. Please write yes or no!");
          return;
        }
        if (!int.TryParse(o.Width, out _))
        {
          Console.WriteLine("Width must be added as a number. Please use positive integer!");
          return;
        }
        if (!int.TryParse(o.Height, out _))
        {
          Console.WriteLine("Height must be added as a number. Please use positive integer!");
          return;
        }

        Picture picture = new Picture(int.Parse(o.Width),int.Parse(o.Height));
        metric = Math.Min(picture.Width, picture.Height);
        Radius = metric / 4;

        MakeFlower(o.Symmetry, picture);

        if (o.Circle == "yes")
        {
          MakeCircle(picture);
        }
        if (o.EndCircle == "yes")
        {
          EndCircle(picture, o.Symmetry);
        }
        if (o.DividedShape == "yes")
        {
          Circle circle = new Circle(picture.Width/2, picture.Height/2,rad + metric / 18,picture,"2");
          Circle circle1 = new Circle(picture.Width/2, picture.Height/2,rad + 2 * metric / 14,picture,"3");
          Circle circle2 = new Circle(picture.Width/2, picture.Height/2,rad + 2 * metric / 10,picture,"4");
          Circle circle3 = new Circle(picture.Width/2, picture.Height/2,rad + 2 * metric / 8,picture,"5");
        }

        picture.Image.Save($"{o.FileName}.png");
      });
    }
  }
}
