//-----------------------------------------------------------------------------
//Cortex
//Copyright (c) 2010-2013, Joshua Scoggins 
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without
//modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright
//      notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright
//      notice, this list of conditions and the following disclaimer in the
//      documentation and/or other materials provided with the distribution.
//    * Neither the name of Cortex nor the
//      names of its contributors may be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//DISCLAIMED. IN NO EVENT SHALL Joshua Scoggins BE LIABLE FOR ANY
//DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//-----------------------------------------------------------------------------
using System;
using System.Reflection;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cortex.Collections;


namespace Cortex.Imaging 
{
  public class Histogram 
  {
    //8-bit histogram
    public const int NUM_VALUES = 256;
    private long[] contents, totals;
    private byte[] globalEqualizedIntensity; 
    private double[] pk;
    private int width, height;
    private long totalPixelCount;
    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public long PixelCount { get { return totalPixelCount; } }
    public byte[] GlobalEqualizedIntensity { get { return globalEqualizedIntensity; } }
    public double[] PK { get { return pk; } }
    public long this[byte intensity] { get { return contents[(int)intensity]; } }
    public long this[int intensity] { get { return contents[intensity]; } }
    public long this[int from, int to]
    {
      get
      {
        if(from == 0)
          return totals[to - 1];
        else if((to - from) == 1) //only one item
          return totals[from];
        else
        {
          long total = 0L;
          for(int i = from; i < to; i++)
            total += (long)this[i];
          return total;
        }
      }
    }
    private Histogram()
    {
      contents = new long[NUM_VALUES];
      totals = new long[NUM_VALUES];
      pk = new double[NUM_VALUES];
      globalEqualizedIntensity = new byte[NUM_VALUES];

    }

    public Histogram(int width, int height) 
      : this()
    {
      this.width = width;
      this.height = height;
      totalPixelCount = width * height;
    }
    public Histogram(IEnumerable<byte> data)
      : this()
    {
      foreach(var v in data)
      {
        contents[(int)v]++;
        totalPixelCount++;
      }
      SetupExtraneousData();
    }
    public Histogram(Bitmap bitmap)
      : this(bitmap.Width, bitmap.Height)
    {
      PerformActionAcrossTheImageAndSetup((i,j) => contents[bitmap.GetPixel(i,j).R]++);
    }
    private void PerformActionAcrossTheImageAndSetup(Action<int,int> body)
    {
      for(int i = 0; i < width; i++)
        for(int j = 0; j < height; j++)
          body(i,j);
      SetupExtraneousData();

    }
    public Histogram(byte[][] value)
      : this(value.Length, value[0].Length)
    {
      PerformActionAcrossTheImageAndSetup((x,y) => contents[(int)value[x][y]]++);
    }
    public Histogram(int width, int height, byte[,] value)
      : this(width, height)
    {
      PerformActionAcrossTheImageAndSetup((i,j) => contents[(int)value[i,j]]++);
    }	
    public void Repurpose(IEnumerable<byte> elements)
    {
      for(int i = 0; i < 256; i++)
        contents[i] = 0;
      totalPixelCount = 0;
      foreach(byte b in elements)
      {
        contents[(int)b]++;
        totalPixelCount++;
      }
      SetupExtraneousData();
    }
    private void SetupExtraneousData()
    {
      double pixelCount = (double)totalPixelCount;
      byte previousIntensity = (byte)0;
      for(int i = 0; i < NUM_VALUES; i++)
      {
        //do this in a single pass
        double amount = (double)SetupTotalsIteration(i);
        SetupPkIteration(i, pixelCount);
        SetupEqualizedIntensityIteration(i, pixelCount, amount,
            ref previousIntensity);
      }
    }
    private void SetupPkIteration(int i, double pixelCount)
    {
      pk[i] = contents[i] / pixelCount;
    }
    private void SetupEqualizedIntensityIteration(int i, 
        double count, double amount, 
        ref byte previousIntensity)
    {
      if(contents[i] == 0)
        globalEqualizedIntensity[i] = previousIntensity;
      else
      {
        double result = (255.0 * (amount / count));
        globalEqualizedIntensity[i] = (byte)result;
        previousIntensity = (byte)result;
      }
    }
    private long SetupTotalsIteration(int i)
    {
      if(i == 0)
        totals[i] = contents[i]; //just copy over the number of pixels
      else
        totals[i] = totals[i - 1] + contents[i];
      return totals[i];
    }
    public static Histogram MakeHistogram(RunLengthEncoding<byte> b)
    {
      return new Histogram(from r in (IEnumerable<RunLengthEncodingEntry<byte>>)b select r.Value);
    }
    public static Histogram[] MakeColorHistogram(RunLengthEncoding<int> i)
    {
      Histogram[] grams = new Histogram[4];
      var q = (from x in (IEnumerable<RunLengthEncodingEntry<int>>)i
          let binds = ImageExtensions.Decode32(x.Value)
          select new 
          {
          Alpha = binds[0],
          Red = binds[1],
          Green = binds[2],
          Blue = binds[3],
          });
      //alpha
      grams[0] = new Histogram(from r in q select r.Alpha);
      grams[1] = new Histogram(from r in q select r.Red);
      grams[2] = new Histogram(from r in q select r.Green);
      grams[3] = new Histogram(from r in q select r.Blue);
      return grams; 
    }
  }
}
