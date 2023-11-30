using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
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

public class Scan2 : DefaultPixelOrder
{
  public Scan2 (PixelAction callback) : base(callback)
  { }

  public override void Pass (int width, int height)
  {
    Debug.Assert(Callback != null);

    for (int x = 0; x < width; x++)
      for (int y = 0; y < height; y++)
        Callback(x, y);
  }
}

public class A : DefaultPixelOrder
{
  public int width = 0;
  public int height = 0;
  public int x = 0;
  public int y = 0;
  HashSet<(int,int)> keys = new HashSet<(int,int)>();
  public int yMin = 0;
  public int xMax = 0;
  public int xMin = 0;
  public int count = 0;
  public A (PixelAction callback) : base(callback)
  { }
  public override void Pass (int width, int height)
  {
    Debug.Assert(Callback != null);

    this.width = width;
    this.height = height;
    this.x = 0;
    this.y = height - 1;
    this.yMin = 0;
    this.xMax = width - 1;
    this.xMin = 1;

    Callback(0, height - 1);

    MainA();
  }
  public void Up ()
  {
    for (int i = y - 1; i >= yMin; i--)
    {
      keys.Add((x, i));
      Callback(x, i);
      count++;
    }
    y = yMin;
    yMin += 1;
  }
  public void Right ()
  {
    for (int i = x + 1; i <= xMax; i++)
    {
      keys.Add((i, y));
      Callback(i, y);
      count++;
    }
    x = xMax;
    xMax -= 1;
  }
  public void Down ()
  {
    for (int i = y + 1; i <= height - 1; i++)
    {
      keys.Add((x, i));
      Callback(x, i);
      count++;
    }
    y = height - 1;
  }
  public void Left ()
  {
    for (int i = x - 1; i >= xMin; i--)
    {
      keys.Add((i, y));
      Callback(i, y);
      count++;
    }
    x = xMin;
    xMin += 1;
  }
  public void MainA ()
  {
    while (true)
    {
      if (keys.Contains((x, y - 1)))
      {
        break;
      }
      Up();

      if (keys.Contains((x + 1, y)))
      {
        break;
      }

      Right();

      if (keys.Contains((x, y + 1)))
      {
        break;
      }

      Down();

      if (keys.Contains((x - 1, y)))
      {
        break;
      }

      Callback(x - 1, y);
      keys.Add((x - 1, y));
      xMax -= 1;
      x = x - 1;

      if (keys.Contains((x, y - 1)))
      {
        break;
      }

      Up();

      if (keys.Contains((x - 1, y)))
      {
        break;
      }

      Left();

      if (keys.Contains((x, y + 1)))
      {
        break;
      }

      Down();

      if (keys.Contains((x + 1, y)))
      {
        break;
      }

      Callback(x + 1, y);
      keys.Add((x + 1, y));
      xMin += 1;
      x = x + 1;
    }
  }
}
public class BFS : DefaultPixelOrder
{
  public Queue<(int,int)> queue = new Queue<(int,int)>();
  public (int,int)[] moves = {(1,0),(0,-1),(-1,0),(0,1)};
  public HashSet<(int, int)> visited = new HashSet<(int, int)>();
  public int width;
  public int height;
  public BFS (PixelAction callback) : base(callback)
  { }
  public override void Pass (int width, int height)
  {
    Debug.Assert(Callback != null);

    this.width = width;
    this.height = height;

    MainBFS();
  }
  public void MainBFS ()
  {
    queue.Enqueue((0, 0));
    Callback(0, 0);
    visited.Add((0, 0));

    while (queue.Count > 0)
    {
      (int,int) current = queue.Dequeue();

      for (int i = 0; i < moves.Length; i++)
      {
        int newX = current.Item1 + moves[i].Item1;
        int newY = current.Item2 + moves[i].Item2;

        if (newX < width && 0 <= newX && newY < height && 0 <= newY && !visited.Contains((newX, newY)))
        {
          queue.Enqueue((newX, newY));
          visited.Add((newX, newY));
          Callback(newX, newY);
        }
      }
    }
  }
}
