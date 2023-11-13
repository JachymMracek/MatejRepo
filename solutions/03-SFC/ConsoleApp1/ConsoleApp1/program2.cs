using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace _03_SFC;

using PixelAction = Action<int, int>;

public interface IPixelOrder
{
  void Pass (int width, int height);
}
public abstract class DefaultPixelOrder : IPixelOrder
{

  protected PixelAction Callback;
  protected DefaultPixelOrder (PixelAction callback) => Callback = callback;

  public abstract void Pass (int width, int height);
}
public class ScanLine : DefaultPixelOrder
{
  public ScanLine (PixelAction callback) : base(callback)
  { }
  public override void Pass (int width, int height)
  {
    Debug.Assert(Callback != null);

    for (int y = 0; y < height; y++)
      for (int x = 0; x < width; x++)
        Callback(x, y);
  }
}
public class SimpleDiagonal : DefaultPixelOrder
{
  public int x;
  public int y;

  public int width;
  public int height;

  public SimpleDiagonal (PixelAction callback) : base(callback)
  { }
  public override void Pass (int width, int height)
  {
    Debug.Assert(Callback != null);
    x = 0;
    y = 0;

    this.width = width;
    this.height = height;

    this.y = 0;
    this.x = 0;

    SimleDiagonal();
  }
  public void SimleDiagonal ()
  {
    int diagonal = 0;

    while (true)
    {
      if (Check(x, y))
      {
        Callback(x, y);

        x += 1;
        y += 1;
      }
      else if (diagonal == height - 1)
      {
        break;
      }
      else
      {
        diagonal++;

        x = 0;
        y = diagonal;
      }

    }

    diagonal = 0;

    while (true)
    {
      if (Check(x, y))
      {
        Callback(x, y);

        x += 1;
        y += 1;
      }
      else if (diagonal == width - 1)
      {
        break;
      }
      else
      {
        diagonal++;

        x = diagonal;
        y = 0;
      }
    }
  }
  public bool Check (int x, int y)
  {
    if (x >= 0 && x < width && y >= 0 && y < height)
    {
      return true;
    }
    return false;
  }
}
public class DFSCurve : DefaultPixelOrder
{

  public int width;
  public int height;

  public DFSCurve (PixelAction callback) : base(callback)
  {
    this.width = width;
    this.height = height;
  }

  public override void Pass (int width, int height)
  {
    Debug.Assert(Callback != null);

    this.width = width;
    this.height = height;

    MainDFS(0, 0);
  }
  public void MainDFS (int startX, int startY)
  {
    Stack<(int, int)> stack = new Stack<(int, int)>();
    HashSet<(int, int)> visited = new HashSet<(int, int)>();

    stack.Push((startX, startY));

    while (stack.Count > 0)
    {
      var (x, y) = stack.Pop();

      if (x < 0 || x >= width || y < 0 || y >= height || visited.Contains((x, y)))
      {
        continue;
      }

      visited.Add((x, y));
      Callback(x, y);

      stack.Push((x + 1, y));
      stack.Push((x, y + 1));
      stack.Push((x - 1, y));
      stack.Push((x, y - 1));
    }
  }
}

public class Spirala : DefaultPixelOrder
{
  public Image<Rgba32> image;

  public int xMax;
  public int xMin;

  public int yMax;
  public int yMin;

  public int x = 0;
  public int y = 0;

  public bool done = false;

  public Spirala (PixelAction callback) : base(callback)
  {
    this.image = image;
  }

  public override void Pass (int width, int height)
  {
    Debug.Assert(Callback != null);

    this.xMax = width - 1;
    this.xMin = 0;

    this.yMax = height - 1;
    this.yMin = 0;

    Callback(x, y);
    MainSpirala();
  }

  public void MainSpirala ()
  {
    while (true)
    {
      done = false;

      Right();
      Down();
      Left();
      Up();

      if (!done)
      {
        break;
      }
    }
  }
  public void Right ()
  {
    while (true)
    {
      x += 1;

      if (Check(x, y))
      {
        Callback(x, y);
        done = true;
      }
      else
      {
        x -= 1;
        yMin += 1;
        break;
      }
    }
  }
  public void Down ()
  {
    while (true)
    {
      y += 1;

      if (Check(x, y))
      {
        Callback(x, y);
        done = true;
      }
      else
      {
        y -= 1;
        xMax -= 1;
        break;
      }
    }
  }
  public void Left ()
  {
    while (true)
    {
      x -= 1;

      if (Check(x, y))
      {
        Callback(x, y);
        done = true;
      }
      else
      {
        x += 1;
        yMax -= 1;
        break;
      }
    }
  }
  public void Up ()
  {
    while (true)
    {
      y -= 1;

      if (Check(x, y))
      {
        Callback(x, y);
        done = true;
      }
      else
      {
        y += 1;
        xMin += 1;
        break;
      }
    }
  }
  public bool Check (int x, int y)
  {
    if (x >= xMin && x <= xMax && y >= yMin && y <= yMax)
    {
      return true;
    }
    return false;
  }
}

public class Hilbert : DefaultPixelOrder
{
  public Image<Rgba32> image;

  public int xMax;
  public int xMin;

  public int yMax;
  public int yMin;

  public int x = 0;
  public int y = 0;

  public bool done = false;

  public int count = 0;

  public Hilbert (PixelAction callback) : base(callback)
  { }

  public override void Pass (int width, int height)
  {
    Debug.Assert(Callback != null);

    if (width >= height)
    {
      MainHilbert(0, 0, width, 0, 0, height);
    }
    else
    {
      MainHilbert(0, 0, 0, height, width, 0);
    }
  }
  private void MainHilbert (int startX, int startY, int deltaX, int deltaY, int deltaBx, int deltaBy)
  {
    int width = Math.Abs(deltaX + deltaY);
    int height = Math.Abs(deltaBx + deltaBy);

    int signDeltaX = Math.Sign(deltaX);
    int signDeltaY = Math.Sign(deltaY);
    int signDeltaBx = Math.Sign(deltaBx);
    int signDeltaBy = Math.Sign(deltaBy);

    if (height == 1)
    {
      GenerateHorizontalLine(startX, startY, width, signDeltaX, signDeltaY);
      return;
    }

    if (width == 1)
    {
      GenerateVerticalLine(startX, startY, height, signDeltaX, signDeltaY);
      return;
    }

    int halfDeltaX = deltaX / 2;
    int halfDeltaY = deltaY / 2;
    int halfDeltaBx = deltaBx / 2;
    int halfDeltaBy = deltaBy / 2;

    int halfWidth = Math.Abs(halfDeltaX + halfDeltaY);
    int halfHeight = Math.Abs(halfDeltaBx + halfDeltaBy);

    if (2 * halfWidth > 3 * halfHeight)
    {
      if ((halfWidth % 2) != 0 && (halfWidth > 2))
      {
        halfDeltaX = halfDeltaX + signDeltaX;
        halfDeltaY = halfDeltaY + signDeltaY;
      }

      MainHilbert(startX, startY, halfDeltaX, halfDeltaY, deltaBx, deltaBy);
      MainHilbert(startX + halfDeltaX, startY + halfDeltaY, deltaX - halfDeltaX, deltaY - halfDeltaY, deltaBx, deltaBy);
    }
    else
    {
      if ((halfHeight % 2) != 0 && (halfHeight > 2))
      {
        halfDeltaBx = halfDeltaBx + signDeltaBx;
        halfDeltaBy = halfDeltaBy + signDeltaBy;
      }

      MainHilbert(startX, startY, halfDeltaBx, halfDeltaBy, halfDeltaX, halfDeltaY);
      MainHilbert(startX + halfDeltaBx, startY + halfDeltaBy, deltaX, deltaY, deltaBx - halfDeltaBx, deltaBy - halfDeltaBy);
      MainHilbert(startX + (deltaX - signDeltaX) + (halfDeltaBx - signDeltaBx), startY + (deltaY - signDeltaY) + (halfDeltaBy - signDeltaBy), -halfDeltaBx, -halfDeltaBy, -(deltaX - halfDeltaX), -(deltaY - halfDeltaY));
    }
  }

  private void GenerateHorizontalLine (int startX, int startY, int length, int signDeltaX, int signDeltaY)
  {
    for (int i = 0; i < length; ++i)
    {
      Callback(startX, startY);
      count++;
      startX += signDeltaX;
      startY += signDeltaY;
    }
  }

  private void GenerateVerticalLine (int startX, int startY, int length, int signDeltaX, int signDeltaY)
  {
    for (int i = 0; i < length; ++i)
    {
      Callback(startX, startY);
      startX += signDeltaX;
      startY += signDeltaY;
    }
  }
}

public class Scan2 : DefaultPixelOrder
{
  public Scan2 (PixelAction callback) : base(callback)
  { }

  public override void Pass (int width, int height)
  {
    Debug.Assert(Callback != null);

    for (int y = 0; y < width; y++)
      for (int x = 0; x < height; x++)
        Callback(x, y);
  }
}

