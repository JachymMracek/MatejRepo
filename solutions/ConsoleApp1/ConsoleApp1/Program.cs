using System;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using static ConsoleApp7.Program;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp7
{
  internal class Program
  {
    static int indexCentral = 0;
    static Vertex MIN1 = new Vertex(false,'\0', '\0', null,null,0);
    static Vertex MIN2 = new Vertex(false,'\0', '\0', null, null, 0);
    public class Vertex
    {
      public bool Tag = false;
      public int weight = '\0';
      public Vertex Left = null;
      public Vertex Right = null;
      public Vertex (bool tag, char symbol, int weight, Vertex left, Vertex right, int index)
      { }
    }
    public class Leaf : Vertex
    {
      public char Symbol = '\0';
      public Leaf (bool tag, char symbol, int weight) : base(tag, symbol, weight, null, null, 0)
      {
        this.Symbol = symbol;

        this.Tag = true;
        this.weight = weight;
        this.Left = Left;
        this.Right = Right;
      }
    }
    public class Node : Vertex
    {
      public int Index = '\0';

      public Node (Vertex left, Vertex right, int index) : base(false, '\0', '\0', left, right, index)
      {
        Index = index;

        this.Tag = false;
        this.weight = left.weight + right.weight;
        this.Left = Left;
        this.Right = Right;
      }
    }
    public class Min2
    {
      public int index2 = -1;
      public bool SetIndex2 (int i)
      {
        if (index2 == -1)
        {
          index2 = i;
          return true;
        }
        return false;
      }
      public bool CompareIndex2 (int i, Vertex vertex)
      {
        if (vertex.weight < MIN2.weight)
        {
          index2 = i;
          return true;
        }
        return false;
      }
      public bool EqualLeavesIndex2 (int i, Vertex vertex)
      {
        if (vertex is Leaf leaf && MIN2 is Leaf leaf1)
        {
          if (vertex.weight == MIN2.weight && vertex.Tag == true && MIN2.Tag == true && leaf.Symbol < leaf1.Symbol)
          {
            index2 = i;
            return true;
          }
        }
        return false;
      }
      public bool EqualNodesIndex2 (int i, Vertex vertex)
      {
        if (vertex is Node node && MIN2 is Node node1)
        {
          if (vertex.weight == MIN2.weight && vertex.Tag == false && MIN2.Tag == false && node.Index < node1.Index)
          {
            index2 = i;
            return true;
          }
        }
        return false;
      }
      public bool EqualLeaveNodeIndex2 (int i, Vertex vertex)
      {
        if (vertex.weight == MIN2.weight && vertex.Tag == true && MIN2.Tag == false)
        {
          index2 = i;
          return true;
        }
        return false;
      }
      public bool MainMin2 (int i, Vertex vertex)
      {
        if (SetIndex2(i))
        {
          return true;
        }
        else if (CompareIndex2(i, vertex))
        {
          return true;
        }
        else if (EqualLeavesIndex2(i, vertex))
        {
          return true;
        }
        else if (EqualNodesIndex2(i, vertex))
        {
          return true;
        }
        else if (EqualLeaveNodeIndex2(i, vertex))
        {
          return true;
        }
        return false;
      }
      public bool MainMin2_Min1 (int i, Vertex vertex)
      {
        if (MainMin2(i, vertex))
        {
          return true;
        }
        return false;
      }
    }
    public class Min1
    {
      public int index1 = -1;
      public int index2Potencial = -1;
      public bool change = false;
      public bool StartIndex1 (int i)
      {
        if (index1 == -1)
        {
          index1 = i;
          return true;
        }
        return false;
      }
      public bool CompareIndex1 (int i, Vertex vertex)
      {
        if (vertex.weight < MIN1.weight)
        {
          index2Potencial = index1;
          index1 = i;
          return true;
        }
        return false;
      }
      public bool EqualLeavesIndex1 (int i, Vertex vertex)
      {
        if (vertex.weight == MIN1.weight && vertex is Leaf leaf && MIN1 is Leaf leaf1)
        {
          if (leaf.Symbol < leaf1.Symbol)
          {
            index2Potencial = index1;
            index1 = i;
          }
          else
          {
            index2Potencial = i;
          }
          return true;
        }
        return false;
      }
      public bool EqualNodesIndex1 (int i, Vertex vertex)
      {
        if (vertex.weight == MIN1.weight && vertex is Node node && MIN1 is Node node1)
        {
          if (node.Index < node1.Index)
          {
            index2Potencial = index1;
            index1 = i;
          }
          else
          {
            index2Potencial = i;
          }
          return true;
        }
        return false;
      }
      public bool EqualLeaveNodeIndex1 (int i, Vertex vertex)
      {
        if (vertex.weight == MIN1.weight && vertex.Tag == true && MIN1.Tag == false)
        {
          index2Potencial = index1;
          index1 = i;
          return true;
        }
        else if (vertex.weight == MIN1.weight && vertex.Tag == false && MIN1.Tag == true)
        {
          index2Potencial = i;
          return true;
        }
        return false;
      }
      public void MainMin1 (int i, Vertex vertex)
      {
        while (true)
        {
          change = false;

          if (StartIndex1(i))
          {
            change = true;
            break;
          }
          else if (CompareIndex1(i, vertex))
          {
            change = true;
            break;
          }
          else if (EqualLeavesIndex1(i, vertex))
          {
            change = true;
            break;
          }
          else if (EqualNodesIndex1(i, vertex))
          {
            change = true;
            break;
          }
          else if (EqualLeaveNodeIndex1(i, vertex))
          {
            change = true;
            break;
          }
          break;
        }
      }
      public void SetMin2 (Min2 min2, Vertex vertex)
      {
        min2.MainMin2_Min1(index2Potencial, vertex);
      }
    }
    public class Memory
    {
      public List<Vertex> vertexList = new List<Vertex>();
      public void MakeVertex ()
      {
        Min1 min1 = new Min1();
        Min2 min2 = new Min2();

        MIN1 = new Vertex(false, '\0', '\0', null, null, 0);
        MIN2 = new Vertex(false, '\0', '\0', null, null, 0);

        for (int i = 0; i < vertexList.Count; i++)
        {
          min1.MainMin1(i, vertexList[i]);

          if (min1.change)
          {
            MIN1 = vertexList[min1.index1];

            if (min1.index2Potencial != -1)
            {
              min1.SetMin2(min2, vertexList[min1.index2Potencial]);
              MIN2 = vertexList[min2.index2];
            }
          }
          else
          {
            min2.MainMin2(i, vertexList[i]);
            MIN2 = vertexList[min2.index2];
          }
        }
        vertexList.RemoveAt(Math.Max(min1.index1, min2.index2));
        vertexList.RemoveAt(Math.Min(min1.index1, min2.index2));

        Console.WriteLine(MIN1.weight);

        Node vertex = new Node(MIN1, MIN2, indexCentral);
        indexCentral++;

        vertexList.Add(vertex);
      }
    }
    public class File
    {
      public string fileName;
      public File (string fileName)
      {
        this.fileName = fileName;
      }
    }
    public class Reader
    {
      public int[] bytes = new int[256];
      public bool errorFile = false;
      public int j = 0;
      public void Read (string fileName)
      {
        try
        {
          using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
          {
            long Byte;

            while ((Byte = fileStream.ReadByte()) != -1)
            {
              bytes[Byte]++;
              j++;
            }
          }
        }
        catch
        {
          errorFile = true;
        }
      }
    }
    static void OtherSymbols (Vertex vertex)
    {
      if (vertex == null)
      {
        return;
      }
      else if (vertex.Tag == false)
      {
        Console.Write(" ");
        Console.Write(vertex.weight);
      }
      else if (vertex is Leaf leaf)
      {
        Console.Write(" ");
        Console.Write("*");
        Console.Write((int)leaf.Symbol);
        Console.Write(":");
        Console.Write(vertex.weight);
      }

      OtherSymbols(vertex.Left);
      OtherSymbols(vertex.Right);
    }
    static void FirstSymbols (Vertex vertex)
    {
      if (vertex.Tag == false)
      {
        Console.Write(vertex.weight);
      }
      else if (vertex is Leaf leaf)
      {
        Console.Write("*");
        Console.Write((int)leaf.Symbol);
        Console.Write(":");
        Console.Write(vertex.weight);
      }

      OtherSymbols(vertex.Left);
      OtherSymbols(vertex.Right);
    }
    static void Main (string[] args)
    {
      if (args.Length != 1)
      {
        Console.WriteLine("Argument Error");
        return;
      }

      File file = new File(args[0]);

      Reader reader = new Reader();
      reader.Read(file.fileName);

      if (reader.errorFile)
      {
        Console.WriteLine("File Error");
        return;
      }

      Memory memory = new Memory();

      for (int i = 0; i < reader.bytes.Length; i++)
      {
        if (reader.bytes[i] != 0)
        {
          Leaf vertex = new Leaf(true,(char)i, reader.bytes[i]);
          memory.vertexList.Add(vertex);
          indexCentral++;
        }
      }

      if (memory.vertexList.Count == 0)
      {
        return;
      }

      while (memory.vertexList.Count > 1)
      {
        memory.MakeVertex();
      }

      FirstSymbols(memory.vertexList[0]);
    }
  }
}
