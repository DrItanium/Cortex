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
  public class ByteImage 
  {
    private byte[][] image;
    private int width, height;
    private long size;
    public byte[][] Image { get { return image; } }
    public int Width { get { return width; } }	
    public int Height { get { return height; } }
    public long PixelCount { get { return size; } }
    public ByteImage(byte[][] image)
    {
      this.image = image;
      this.width = image.Length;
      this.height = image[0].Length;
      this.size = (long)width * (long)height;
    }
    public byte?[][] Distribute(int newWidth, int newHeight)
    {
      return Distribute(new ScaleInfo(width, height, newWidth, newHeight));
    }
    ///<summary>
    /// Creates a new image that is of a new resolution.
    /// If the resolution is smaller than the current resolution
    /// then shrink the image
    ///<summary/>
    public byte?[][] Distribute(ScaleInfo si)
    {
      float wFac = si.WidthScalingFactor;
      float hFac = si.HeightScalingFactor;
      byte?[][] result = new byte?[si.ResultWidth][];
      for(int i = 0; i < si.ResultWidth; i++)
        result[i] = new byte?[si.ResultHeight];
      if(si.IsZooming)
      {
        Console.WriteLine("Scaling Up!");
        float iFactor = 0.0f;
        for(int i = 0; i < width; i++)
        {
          float jFactor = 0.0f;	
          for(int j = 0; j < height; j++)
          {
            result[(int)Math.Round(iFactor)][(int)Math.Round(jFactor)] = image[i][j];
            jFactor += hFac;
          }
          iFactor += wFac;
        }
        return result;
      }
      if(si.IsShrinking)
      {
        for(int i = 0; i < si.ResultWidth; i++)
        {
          for(int j = 0; j < si.ResultHeight; j++)
          {
            //overlay...its an interesting idea	
            int r1 = Math.Min(width - 1, 
                Math.Max(0, (int)Math.Floor(i / wFac)));
            int r2 = Math.Min(height - 1, 
                Math.Max(0, (int)Math.Floor(j / hFac)));
            result[i][j] = image[r1][r2];
          }
        }
        return result;
      }
      else
        return Convert(image);
    }
    public ByteImage PadImage(int factor)
    {
      //check to see if the width and height are of the proper
      //size already
      int modW = width % factor;
      int modH = height % factor;
      if(modW == 0 && modH == 0)
        return this;
      else
      {
        //get the difference between this and the new item
        int wDiff = width + (factor - modW);
        int hDiff = height + (factor - modH);
        return ReplicationScale(wDiff, hDiff);
      }
    }
    public ByteImage ReplicationScale(int newWidth, int newHeight)
    {
      ScaleInfo si = new ScaleInfo(width, height, newWidth, newHeight);
      byte?[][] newImage = Distribute(si);	
      if(si.IsZooming)
      {
        byte? curr = null;
        int rWidth = newImage.Length;
        int rHeight = newImage[0].Length;
        int i = 0, j = 0;
        for(j = 0; j < rHeight; j++)
        {
          curr = null;
          for(i = 0; i < rWidth; i++)
          {
            if(newImage[i][j] != null)
              //set that as the new color
              curr = newImage[i][j];
            else
              newImage[i][j] = curr;
          }
        }	
        for(i = 0; i < rWidth; i++)
        {
          curr = null;
          for(j = 0; j < rHeight; j++)
          {
            if(newImage[i][j] != null)
              //set that as the new color
              curr = newImage[i][j];
            else
              newImage[i][j] = curr;
          }
        }	
        return new ByteImage(Convert(newImage));
      }
      else if(si.IsShrinking)
        return new ByteImage(Convert(newImage));
      else
        return this;
    }
    public RunLengthEncoding<byte> RunLengthEncode()
    {
      return RunLengthEncoding<byte>.Encode(image);
    }
    public byte this[int x, int y] { get { return image[x][y]; } }
    public static byte?[][] Convert(byte[][] input)
    {
      int width = input.Length;
      int height = input[0].Length;
      byte?[][] result = new byte?[width][];
      for(int i = 0; i < width; i++)
      {
        result[i] = new byte?[height];
        for(int j = 0; j < height; j++)
          result[i][j] = (byte?)input[i][j];
      }
      return result;
    }
    public static byte[][] Convert(byte?[][] input)
    {
      int width = input.Length;
      int height = input[0].Length;
      byte[][] result = new byte[width][];
      for(int i = 0; i < width; i++)
      {
        result[i] = new byte[height];
        for(int j = 0; j < height; j++)
          result[i][j] = (byte)input[i][j];
      }
      return result;
    }
  }

}
