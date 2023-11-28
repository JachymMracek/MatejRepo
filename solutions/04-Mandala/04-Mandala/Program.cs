using CommandLine.Text;
using CommandLine;
using static ConsoleApp5.Program;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace ConsoleApp5
{
  internal class Program
  {
    static int Radius = 0;
    static int metric = 0;
    static int r = 0;
    static int rad = 0;
    static List<(int, int, int, int)> args = new List<(int, int, int, int)>();

    public class Options
    {
      [Option('s', "Symmetry", Required = true, HelpText = "how much symmetry should the image have? (5 or 6):")]
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

      [Option('n', "name", Required = true, HelpText = "\r\nWhat do you want to name the picture? (for example: picture.png):")]
      public string FileName { get; set; }
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
    public class Circle
    {
      public HashSet<(int, int)> points = new HashSet<(int, int)>();
      public Circle (double centerX, double centerY, int radius, Picture picture, int tag)
      {
        if (tag == 1)
        {
          for (int i = 0; i < picture.Height; i++)
          {
            for (int j = 0; j < picture.Width; j++)
            {
              double distance = Math.Sqrt((i - centerY) * (i - centerY) + (j - centerX) * (j - centerX));

              if (distance < radius)
              {
                var color = new Rgba32(0, 0, 255);
                picture.Image[j, i] = color;
                points.Add((j, i));
              }
            }
          }
          return;
        }

        if (tag == 2)
        {
          for (int i = 0; i < picture.Height; i++)
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

        if (tag == 3)
        {
          for (int i = 0; i < picture.Height; i++)
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

        if (tag == 4)
        {
          for (int i = 0; i < picture.Height; i++)
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

        if (tag == 5)
        {
          for (int i = 0; i < picture.Height; i++)
          {
            for (int j = 0; j < picture.Width; j++)
            {
              double distance = Math.Sqrt((i - centerY) * (i - centerY) + (j - centerX) * (j - centerX));

              if (distance < radius && distance < radius && distance > radius - 0.05 * radius && picture.Image[j, i] != new Rgba32(242, 193, 78))
              {
                var color = new Rgba32(128, 0, 128);
                picture.Image[j, i] = color;
                points.Add((j, i));
              }
            }
          }
          return;
        }

        for (int i = 0; i < picture.Height; i++)
        {
          for (int j = 0; j < picture.Width; j++)
          {
            double distance = Math.Sqrt((i - centerY) * (i - centerY) + (j - centerX) * (j - centerX));

            if (distance < radius && distance > radius - 0.05 * radius)
            {
              double intensity = 1.0 - distance / radius;
              var color = new Rgba32(0, 0, 0);
              picture.Image[j, i] = color;
              points.Add((j, i));
            }
          }
        }
      }
    }

    static void YlowXlow (Picture picture, Circle circleStart, Circle circleEnd)
    {
      var commonPoints = new HashSet<(int, int)>(circleStart.points);
      commonPoints.IntersectWith(circleEnd.points);

      List<(int, int)> list1 = commonPoints.OrderBy(point => point.Item1).ToList();

      int minItem2 = list1.Min(item => item.Item2);
      int maxItem2 = list1.Max(item => item.Item2);
      int minItem1 = list1.Min(item => item.Item1);
      int maxItem1 = list1.Max(item => item.Item1);

      rad = metric / 5;
      r = (int)Math.Sqrt((minItem2 - picture.Height / 2) * (minItem2 - picture.Height / 2) + (minItem1 - picture.Width / 2) * (minItem1 - picture.Width / 2));

      for (int i = minItem2; i <= maxItem2; i++)
      {
        bool start = false;

        for (int j = minItem1; j < maxItem1; j++)
        {
          if (circleStart.points.Contains((j, i)) && circleEnd.points.Contains((j, i)))
          {
            break;
          }
          if (circleEnd.points.Contains((j, i)))
          {
            start = false;
          }
          else if (circleStart.points.Contains((j, i)))
          {
            start = true;
          }
          else if (start)
          {
            var orangeColor = new Rgba32(242, 193, 78);
            picture.Image[j, i] = orangeColor;
            continue;
          }
        }
      }
      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    static void YlowXHigh (Picture picture, Circle circleStart, Circle circleEnd)
    {
      var commonPoints = new HashSet<(int, int)>(circleStart.points);
      commonPoints.IntersectWith(circleEnd.points);

      List<(int, int)> list1 = commonPoints.OrderBy(point => point.Item1).ToList();

      int minItem2 = list1.Min(item => item.Item2);
      int maxItem2 = list1.Max(item => item.Item2);
      int minItem1 = list1.Min(item => item.Item1);
      int maxItem1 = list1.Max(item => item.Item1);

      for (int i = minItem2; i <= maxItem2; i++)
      {
        bool start = false;
        bool colored = false;

        for (int j = 0; j < picture.Width; j++)
        {
          if (circleStart.points.Contains((j, i)) && circleEnd.points.Contains((j, i)))
          {
            break;
          }
          if (circleEnd.points.Contains((j, i)))
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
            var orangeColor = new Rgba32(242, 193, 78);
            picture.Image[j, i] = orangeColor;
            colored = true;
          }
        }
      }

      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    static void YhighXhigh (Picture picture, Circle circleStart, Circle circleEnd)
    {
      var commonPoints = new HashSet<(int, int)>(circleStart.points);
      commonPoints.IntersectWith(circleEnd.points);

      List<(int, int)> list1 = commonPoints.OrderBy(point => point.Item1).ToList();

      int minItem2 = list1.Min(item => item.Item2);
      int maxItem2 = list1.Max(item => item.Item2);
      int minItem1 = list1.Min(item => item.Item1);
      int maxItem1 = list1.Max(item => item.Item1);

      for (int j = minItem1; j < maxItem1; j++)
      {
        bool start = false;
        bool colored = false;

        for (int i = 0; i < picture.Height / 2 + r / 2; i++)
        {
          if (circleStart.points.Contains((j, i)) && circleEnd.points.Contains((j, i)))
          {
            break;
          }
          if (circleEnd.points.Contains((j, i)))
          {
            start = false;
          }
          else if (circleStart.points.Contains((j, i)) && colored)
          {
            break;
          }
          if (circleStart.points.Contains((j, i)))
          {
            start = true;
          }
          else if (start)
          {
            var orangeColor = new Rgba32((byte)242, (byte)193, (byte)78);
            picture.Image[j, i] = orangeColor;
            colored = true;
          }
        }
      }
      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    static void YmidXmid (Picture picture, Circle circleStart, Circle circleEnd)
    {
      var commonPoints = new HashSet<(int, int)>(circleStart.points);
      commonPoints.IntersectWith(circleEnd.points);

      List<(int, int)> list1 = commonPoints.OrderBy(point => point.Item1).ToList();

      int minItem2 = list1.Min(item => item.Item2);
      int maxItem2 = list1.Max(item => item.Item2);
      int minItem1 = list1.Min(item => item.Item1);
      int maxItem1 = list1.Max(item => item.Item1);


      for (int i = minItem2; i <= maxItem2; i++)
      {
        bool start = false;
        bool colored = false;

        for (int j = 0; j < picture.Width; j++)
        {
          if (circleStart.points.Contains((j, i)) && circleEnd.points.Contains((j, i)))
          {
            break;
          }
          if (circleEnd.points.Contains((j, i)))
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
            var orangeColor = new Rgba32((byte)242, (byte)193, (byte)78);
            picture.Image[j, i] = orangeColor;
            colored = true;
          }
        }
      }
      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    static void YhighXlow (Picture picture, Circle circleStart, Circle circleEnd)
    {
      var commonPoints = new HashSet<(int, int)>(circleStart.points);
      commonPoints.IntersectWith(circleEnd.points);

      List<(int, int)> list1 = commonPoints.OrderBy(point => point.Item1).ToList();

      int minItem2 = list1.Min(item => item.Item2);
      int maxItem2 = list1.Max(item => item.Item2);
      int minItem1 = list1.Min(item => item.Item1);
      int maxItem1 = list1.Max(item => item.Item1);

      for (int j = minItem1; j < maxItem1; j++)
      {
        bool start = false;
        bool colored = false;

        for (int i = 0; i < picture.Height / 2 + r / 2; i++)
        {
          if (circleStart.points.Contains((j, i)) && circleEnd.points.Contains((j, i)))
          {
            break;
          }
          if (circleEnd.points.Contains((j, i)))
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
            var orangeColor = new Rgba32((byte)242, (byte)193, (byte)78);
            picture.Image[j, i] = orangeColor;
            colored = true;
          }
        }
      }

      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    static void DeleteBlack (Picture picture)
    {
      for (int i = 0; i < picture.Image.Height; i++)
      {
        for (int j = 0; j < picture.Image.Width; j++)
        {
          Rgba32 pixelColor = picture.Image[j, i];

          if (pixelColor == new Rgba32(0, 0, 0))
          {
            picture.Image[j, i] = new Rgba32(255, 255, 255);
          }
        }
      }
    }
    static void MakeCircle (Picture picture)
    {
      Circle circle = new Circle(picture.Width / 2, picture.Height / 2, r, picture, 0);

      for (int i = picture.Image.Height / 2 - r + (int)(0.05 * r + 1); i <= picture.Image.Height / 2 + r - (int)(0.05 * r + 1); i++)
      {
        bool start = false;
        bool colored = false;

        for (int j = picture.Width / 2 - r + (int)(0.05 * r - 1); j <= picture.Width / 2 + r - (int)(0.05 * r - 1); j++)
        {
          Rgba32 pixelColor = picture.Image[j, i];

          if (circle.points.Contains((j, i)))
          {
            if (colored)
            {
              break;
            }
            else
            {
              start = true;
            }
          }
          else if (pixelColor == new Rgba32(255, 255, 255) && start)
          {
            picture.Image[j, i] = new Rgba32(247, 129, 84);
            circle.points.Add((j, i));
            colored = true;
          }
        }
      }
      DeleteBlack(picture);

      for (int j = 0; j < picture.Image.Width; j++)
      {
        for (int i = 0; i < picture.Image.Height; i++)
        {
          if (picture.Image[j, i] == new Rgba32(247, 129, 84) && !circle.points.Contains((j, i)))
          {
            picture.Image[j, i] = new Rgba32(255, 255, 255);
          }
        }
      }
    }
    static void YlowXlow (Picture picture, Circle circleStart, Circle circleEnd, Circle CircleSemi, Circle circleSemi2)
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

      r = (int)Math.Sqrt((minItem2 - picture.Height / 2) * (minItem2 - picture.Height / 2) + (minItem1 - picture.Width / 2) * (minItem1 - picture.Width / 2));

      for (int i = minItem2; i <= maxItem2; i++)
      {
        bool start = false;
        bool semi = false;
        bool colored = false;

        for (int j = 0; j < picture.Width; j++)
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
            if (i >= minItem2_ && circleSemi2.points.Contains((j, i)))
            {
              semi = true;
              continue;
            }
            if (i >= minItem2_ && semi)
            {
              var orangeColor1 = new Rgba32(242, 193, 78);
              picture.Image[j, i] = orangeColor1;
              colored = true;
              continue;
            }
            else if (i >= minItem2_ && !semi)
            {
              continue;
            }
            else if (i < minItem2_)
            {
              var orangeColor = new Rgba32(242, 193, 78);
              picture.Image[j, i] = orangeColor;
              colored = true;
              continue;
            }
          }
        }
      }
      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    static void YlowXHigh (Picture picture, Circle circleStart, Circle circleEnd, Circle circleSemi, Circle circleSemi2)
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

      for (int i = minItem2; i <= maxItem2; i++)
      {
        bool start = false;
        bool colored = false;
        bool semi = false;

        for (int j = 0; j < picture.Width; j++)
        {
          if (circleStart.points.Contains((j, i)) && circleEnd.points.Contains((j, i)))
          {
            break;
          }
          if (circleEnd.points.Contains((j, i)) || circleSemi.points.Contains((j, i)))
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
          if (start)
          {
            if (i > minItem2_ + metric / 137 && circleSemi2.points.Contains((j, i)))
            {
              semi = true;
              continue;
            }
            else if (i > minItem2_ + metric / 274 && semi)
            {
              var orangeColor1 = new Rgba32(242, 193, 78);
              picture.Image[j, i] = orangeColor1;
              colored = true;
              continue;
            }
            else if (i > minItem2_ + metric / 137 && !semi)
            {
              continue;
            }
            if (i <= minItem2_ + metric / 137 && picture.Image[j, i] != new Rgba32(0, 0, 0))
            {
              var orangeColor = new Rgba32(242, 193, 78);
              picture.Image[j, i] = orangeColor;
              colored = true;
              continue;
            }
          }
        }
      }
      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    static void EndCircle (Picture picture)
    {
      for (int i = 0; i < args.Count; i++)
      {
        if (i == 0)
        {
          Circle circle1 = new Circle(args[i].Item1, args[i].Item3, metric / 100, picture, 1);
        }
        else if (i == 1)
        {
          Circle circle1 = new Circle(args[i].Item2, args[i].Item3, metric / 100, picture, 1);
        }
        else if (i == 2)
        {
          Circle circle1 = new Circle(args[i].Item2, args[i].Item4, metric / 100, picture, 1);
        }
        else if (i == 3)
        {
          Circle circle1 = new Circle(args[i].Item1, args[i].Item4, metric / 100, picture, 1);
        }
        else if (i == 4)
        {
          Circle circle1 = new Circle(args[i].Item1, args[i].Item4,metric / 100, picture, 1);
        }
        else if (i == 5)
        {
          Circle circle1 = new Circle(args[i].Item2, args[i].Item3, metric / 100, picture, 1);
        }
      }
    }
    static void YhighXhigh (Picture picture, Circle circleStart, Circle circleEnd, Circle circleSemi, Circle circleSemi2)
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

      for (int i = minItem2; i < maxItem2; i++)
      {
        bool start = false;
        bool colored = false;
        bool semi = false;

        for (int j = 0; j < picture.Width; j++)
        {
          if (circleStart.points.Contains((j, i)) && circleEnd.points.Contains((j, i)))
          {
            break;
          }
          if (circleEnd.points.Contains((j, i)) || circleSemi.points.Contains((j, i)))
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
            if (circleEnd.points.Contains((j, i)) || circleSemi.points.Contains((j, i)))
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
            if (start)
            {
              if (i > maxItem2_ && picture.Image[j, i] != new Rgba32(0, 0, 0))
              {
                var orangeColor = new Rgba32(242, 193, 78);
                picture.Image[j, i] = orangeColor;
                colored = true;
                continue;
              }
              if (i > minItem2_ && circleSemi2.points.Contains((j, i)))
              {
                semi = true;
                continue;
              }
              else if (i > minItem2_ && semi)
              {
                var orangeColor1 = new Rgba32(242, 193, 78);
                picture.Image[j, i] = orangeColor1;
                colored = true;
                continue;
              }
              else if (i > minItem2_ && !semi)
              {
                continue;
              }
              if ((i <= minItem2_ && picture.Image[j, i] != new Rgba32(0, 0, 0)))
              {
                var orangeColor = new Rgba32(242, 193, 78);
                picture.Image[j, i] = orangeColor;
                colored = true;
                continue;
              }
            }
          }
        }
      }
      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    static void YhighXlow1 (Picture picture, Circle circleStart, Circle circleEnd, Circle circleSemi, Circle circleSemi2)
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

      rad = (int)Math.Sqrt((minItem2_ - picture.Height / 2) * (minItem2_ - picture.Height / 2) + (minItem1_ - picture.Width / 2) * (minItem1_ - picture.Width / 2)) + metric / 51;

      for (int i = minItem2; i < maxItem2; i++)
      {
        bool start = false;
        bool colored = false;
        bool semi = false;

        for (int j = 0; j < picture.Width; j++)
        {
          if (circleStart.points.Contains((j, i)) && circleEnd.points.Contains((j, i)))
          {
            break;
          }
          if (circleEnd.points.Contains((j, i)) || circleSemi.points.Contains((j, i)))
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
            if (i > maxItem2_ && picture.Image[j, i] != new Rgba32(0, 0, 0))
            {
              var orangeColor = new Rgba32(242, 193, 78);
              picture.Image[j, i] = orangeColor;
              colored = true;
              continue;
            }
            if (i > minItem2_ && circleSemi2.points.Contains((j, i)))
            {
              semi = true;
              continue;
            }
            else if (i > minItem2_ && semi)
            {
              var orangeColor1 = new Rgba32(242, 193, 78);
              picture.Image[j, i] = orangeColor1;
              colored = true;
              continue;
            }
            else if (i > minItem2_ && !semi)
            {
              continue;
            }
            if ((i <= minItem2_ && picture.Image[j, i] != new Rgba32(0, 0, 0)))
            {
              var orangeColor = new Rgba32(242, 193, 78);
              picture.Image[j, i] = orangeColor;
              colored = true;
              continue;
            }
          }
        }
      }
      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    static void YmidXlow (Picture picture, Circle circleStart, Circle circleEnd, Circle CircleSemi, Circle circleSemi2)
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

      for (int j = minItem1; j < maxItem1; j++)
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
            if (j < minItem1_ + metric / 100 && picture.Image[j, i] != new Rgba32(0, 0, 0))
            {
              var orangeColor = new Rgba32(242, 193, 78);
              picture.Image[j, i] = orangeColor;
              colored = true;
              continue;
            }
            if (i > minItem2_ + metric / 100 && circleSemi2.points.Contains((j, i)))
            {
              semi = true;
              continue;
            }
            else if (i > minItem2_ + metric / 100 && semi)
            {
              var orangeColor1 = new Rgba32(242, 193, 78);
              picture.Image[j, i] = orangeColor1;
              colored = true;
              continue;
            }
            else if (i > minItem2_ + metric / 100 && !semi)
            {
              continue;
            }
            if ((i <= minItem2_ && picture.Image[j, i] != new Rgba32(0, 0, 0)))
            {
              var orangeColor = new Rgba32(242, 193, 78);
              picture.Image[j, i] = orangeColor;
              colored = true;
              continue;
            }
          }
        }
      }
      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    static void YmidXhigh (Picture picture, Circle circleStart, Circle circleEnd, Circle CircleSemi, Circle circleSemi2)
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
              picture.Image[j, i] = orangeColor;
              colored = true;
              continue;
            }
          }
        }
      }
      args.Add((minItem1, maxItem1, minItem2, maxItem2));
    }
    public class Symmetry5
    {
      public Circle circle1;
      public Circle circle2;
      public Circle circle3;
      public Circle circle4;
      public Circle circle5;
      public void DrawFlowerCircles (Picture picture)
      {

        circle1 = new Circle(picture.Width / 2 - Radius * Math.Cos(18 * (Math.PI / 180)), picture.Height / 2 - Radius * Math.Sin(18 * (Math.PI / 180)), Radius, picture, 0);
        circle2 = new Circle(picture.Width / 2 - Radius * Math.Cos(54 * (Math.PI / 180)), picture.Height / 2 + Radius * Math.Sin(54 * (Math.PI / 180)), Radius, picture, 0);
        circle3 = new Circle(picture.Width / 2 + Radius * Math.Cos(54 * (Math.PI / 180)), picture.Height / 2 + Radius * Math.Sin(54 * (Math.PI / 180)), Radius, picture, 0);
        circle4 = new Circle(picture.Width / 2 + Radius * Math.Cos(18 * (Math.PI / 180)), picture.Height / 2 - Radius * Math.Sin(18 * (Math.PI / 180)), Radius, picture, 0);
        circle5 = new Circle(picture.Width / 2, picture.Height / 2 - Radius, Radius, picture, 0);

      }
      public void MakeFlowerParts (Picture picture)
      {
        YlowXlow(picture, circle5, circle1);
        YlowXHigh(picture, circle4, circle5);
        YhighXhigh(picture, circle3, circle4);
        YmidXmid(picture, circle3, circle2);
        YhighXlow(picture, circle2, circle1);
      }
      public void MakeFlower (Picture picture)
      {
        DrawFlowerCircles(picture);
        MakeFlowerParts(picture);
        DeleteBlack(picture);

        int radius = Radius/2;

        for (int i = 0; i < picture.Height; i++)
        {
          for (int j = 0; j < picture.Width; j++)
          {
            double distance = Math.Sqrt((i - picture.Height/2) * (i - picture.Height/2) + (j - picture.Width/2) * (j - picture.Width/2));

            if (distance < radius && picture.Image[j, i] == new Rgba32(255, 255, 255))
            {
              var color = new Rgba32(0,255 ,0);
              picture.Image[j, i] = color;
            }
          }
        }
      }
    }
    public class Symmetry6
    {
      public Circle circle1;
      public Circle circle2;
      public Circle circle3;
      public Circle circle4;
      public Circle circle5;
      public Circle circle6;
      public void DrawFlowerCircles (Picture picture)
      {
        circle1 = new Circle(picture.Width / 2 + Radius * Math.Cos(30 * (Math.PI / 180)), picture.Height / 2 - Radius * Math.Sin(30 * (Math.PI / 180)), Radius, picture, 0);
        circle2 = new Circle(picture.Width / 2 - Radius * Math.Cos(90 * (Math.PI / 180)), picture.Height / 2 - Radius * Math.Sin(90 * (Math.PI / 180)), Radius, picture, 0);
        circle3 = new Circle(picture.Width / 2 - Radius * Math.Cos(30 * (Math.PI / 180)), picture.Height / 2 - Radius * Math.Sin(30 * (Math.PI / 180)), Radius, picture, 0);
        circle4 = new Circle(picture.Width / 2 - Radius * Math.Cos(30 * (Math.PI / 180)), picture.Height / 2 + Radius * Math.Sin(30 * (Math.PI / 180)), Radius, picture, 0);
        circle5 = new Circle(picture.Width / 2 + Radius * Math.Cos(90 * (Math.PI / 180)), picture.Height / 2 + Radius * Math.Sin(90 * (Math.PI / 180)), Radius, picture, 0);
        circle6 = new Circle(picture.Width / 2 + Radius * Math.Cos(30 * (Math.PI / 180)), picture.Height / 2 + Radius * Math.Sin(30 * (Math.PI / 180)), Radius, picture, 0);
      }
      public void MakeFlowerParts (Picture picture)
      {
        YlowXlow(picture, circle2, circle3, circle1, circle4);
        YlowXHigh(picture, circle1, circle2, circle6, circle3);
        YhighXhigh(picture, circle6, circle5, circle1, circle4);
        YhighXlow1(picture, circle5, circle4, circle6, circle3);
        YmidXlow(picture, circle4, circle3, circle5, circle2);
        YmidXhigh(picture, circle6, circle1, circle5, circle2);

        for (int i = 5; i < picture.Height / 2; i++)
        {
          for (int j = 0; j < picture.Width; j++)
          {
            if (picture.Image[j, i - 1] == new Rgba32(242, 193, 78) && picture.Image[j, i] == new Rgba32(255, 255, 255))
            {
              picture.Image[j, i] = new Rgba32(242, 193, 78);
            }
          }
        }
        for (int i = picture.Height / 2; i < picture.Height; i++)
        {
          for (int j = 0; j < picture.Width; j++)
          {
            if (picture.Image[j, i - 1] == new Rgba32(242, 193, 78) && picture.Image[j, i] == new Rgba32(255, 255, 255))
            {
              picture.Image[j, i] = new Rgba32(242, 193, 78);
            }
          }
        }

      }
      public void MakeFlower (Picture picture)
      {
        DrawFlowerCircles(picture);
        MakeFlowerParts(picture);
        DeleteBlack(picture);

        int radius = Radius / 2;

        for (int i = 0; i < picture.Height; i++)
        {
          for (int j = 0; j < picture.Width; j++)
          {
            double distance = Math.Sqrt((i - picture.Height/2) * (i - picture.Height/2) + (j - picture.Width/2) * (j - picture.Width/2));

            if (distance < radius && picture.Image[j, i] == new Rgba32(255, 255, 255))
            {
              var color = new Rgba32(171,0,0);
              picture.Image[j, i] = color;
            }
          }
        }

        for (int j = picture.Width / 2 + r - 2; j < picture.Width; j++)
        {
          for (int i = 0; i < picture.Height; i++)
          {
            Rgba32 currentColor = picture.Image[j, i];

            if (i > 0 && picture.Image[j, i - 1] == new Rgba32(255, 255, 255) && currentColor == new Rgba32(242, 193, 78))
            {
              picture.Image[j, i] = new Rgba32(255, 255, 255);
            }
          }
        }
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
          Console.WriteLine("You added small side of picture");
        }
        if (o.Symmetry != 5 && o.Symmetry != 6)
        {
          Console.WriteLine("You added wrong argumets for symmetry. You must choose between 5 and 6!");
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

        if (o.Symmetry == 5)
        {
          Symmetry5 symmetry5 = new Symmetry5();

          symmetry5.MakeFlower(picture);

          if (o.Circle == "yes")
          {
            MakeCircle(picture);
          }
          if (o.EndCircle == "yes")
          {
            EndCircle(picture);
          }
          if (o.DividedShape == "yes")
          {
            Circle circle = new Circle(picture.Width/2, picture.Height/2,rad,picture,2);
            Circle circle1 = new Circle(picture.Width/2, picture.Height/2,rad + metric / 20,picture,3);
            Circle circle2 = new Circle(picture.Width/2, picture.Height/2,rad + metric / 10,picture,4);
            Circle circle3 = new Circle(picture.Width/2, picture.Height/2,rad + metric / 7,picture,5);
          }
        }
        else if (o.Symmetry == 6)
        {
          Symmetry6 symmetry6 = new Symmetry6();

          symmetry6.MakeFlower(picture);

          if (o.Circle == "yes")
          {
            MakeCircle(picture);
          }
          if (o.EndCircle == "yes")
          {
            EndCircle(picture);
          }
          if (o.DividedShape == "yes")
          {
            Circle circle = new Circle(picture.Width/2, picture.Height/2,rad,picture,2);
            Circle circle1 = new Circle(picture.Width/2, picture.Height/2,rad + metric / 20,picture,3);
            Circle circle2 = new Circle(picture.Width/2, picture.Height/2,rad + metric / 10,picture,4);
            Circle circle3 = new Circle(picture.Width/2, picture.Height/2,rad + metric / 7,picture,5);
          }
        }
        picture.Image.Save(o.FileName);
      });
    }
  }
}
