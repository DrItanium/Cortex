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
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;


namespace Cortex.Imaging
{
  public unsafe delegate void UnsafeImageTransformation(BitmapData data, byte* b);
  public static partial class ImageExtensions
  {
    public static int Encode32(byte a, byte r, byte g, byte b)
    {
      int alpha = (int)a << 24;
      int red = (int)r << 16;
      int green = (int)g << 8;
      int blue = (int)b;
      return alpha + red + green + blue;
    }
    public static int Encode32(byte[] tuple, int offset)
    {
      return Encode32(tuple[offset],
          tuple[offset + 1],
          tuple[offset + 2],
          tuple[offset + 3]);
    }
    public static int Encode32(byte[] tuple)
    {
      return Encode32(tuple, 0);
    }
    public static byte[] Decode32(int value)
    {
      return new byte[]
      {
        (byte)(value >> 24),
          (byte)(value >> 16),
          (byte)(value >> 8),
          (byte)value,
      };
    }
    public static void Decode32(int value, byte[] container, int offset)
    {
      container[offset] = (byte)(value >> 24);
      container[offset+1] = (byte)(value >> 16);
      container[offset+2] = (byte)(value >> 8);
      container[offset+3] = (byte)value;
    }
    public static void Decode32(int value, byte[] container)
    {
      Decode32(value, container, 0);
    }
    public static bool IsColorImage(this Bitmap image)
    {
      for(int i = 0; i < image.Width; i++)
      {
        for(int j = 0; j < image.Height; j++)
        {
          Color c = image.GetPixel(i,j);
          if((c.R != c.G) || (c.G != c.B) || (c.R != c.B))
            return true;
        }
      }
      return false;
    }
    public unsafe static void ApplyTranslationTableToBitmap(this Bitmap image, byte[] translationTable)
    {
      image.UnsafeTransformation((data, pointer) => ApplyTranslationTable(data, pointer, translationTable));
    }
    public unsafe static void ApplyTranslationTable(BitmapData data, byte* pointer, byte[] translationTable)
    {
      for(int i = 0; i < data.Width; i++)
      {
        for(int j = 0; j < data.Height; j++)
        {
          byte tmp;
          if(TryGetGrayScaleValue(pointer, out tmp))
            SetGrayScaleValue(pointer, translationTable[tmp]);
          else
          {
            //color
          }
          pointer += 4;
        }
      }
    }
    public static IEnumerable<int> GrabNeighborhood(this int[][] image, int centerX, int centerY, int width, int height, int squareSize)
    {
      //generate a starting location that would define this properly
      //since we know centerX and centerY 
      //compute startPoint, the factor that determines how to grab values from
      //this neighborhood
      int startPoint = (squareSize - 1) / 2; //this is the factor
      int endPoint = startPoint + 1; //this is the end condition
      for(int x = centerX - startPoint; x < centerX + endPoint; x++)
      {
        if(x >= 0 && x < width)
        {
          for(int y = centerY - startPoint; y < centerY + endPoint; y++)
          {
            if(y >= 0 && y < height)
              yield return image[x][y];
          }
        }
      }
    }
    public static IEnumerable<byte> GrabNeighborhood(this byte[][] image, int centerX, int centerY, int width, int height, int squareSize)
    {
      //generate a starting location that would define this properly
      //since we know centerX and centerY 
      //compute startPoint, the factor that determines how to grab values from
      //this neighborhood
      int startPoint = (squareSize - 1) / 2; //this is the factor
      int endPoint = startPoint + 1; //this is the end condition
      for(int x = centerX - startPoint; x < centerX + endPoint; x++)
      {
        if(x >= 0 && x < width)
        {
          for(int y = centerY - startPoint; y < centerY + endPoint; y++)
          {
            if(y >= 0 && y < height)
              yield return image[x][y];
          }
        }
      }
    }
    public static unsafe byte[,] ToGreyScaleImage(byte* input, int width, int height)
    {
      byte[,] elements = new byte[width, height];
      for(int i = 0; i < width; i++)
      {
        for(int j = 0; j < height; j++)
        {
          elements[i, j] = input[0];
          input += 4;
        }
      }
      return elements;
      //assume 32bit rgb
    }
    public static unsafe byte[,] ToGreyScaleImage(byte* input, BitmapData data)
    {
      return ToGreyScaleImage(input, data.Width, data.Height);
    }
    public static unsafe byte[,] ToGreyScaleImage(this Bitmap b)
    {
      byte[,] result = null;
      b.UnsafeTransformation((x,y) => result = ToGreyScaleImage(y, x));
      return result;
    }
    private static void Empty(int input) { }
    public static unsafe void ApplyByteArrayToImage(this byte[,] b, int width, int height, byte* data)
    {
      TraverseAcrossImage(width, height, (x, y) => {
          unsafe{
          SetGrayScaleValue(data, b[x,y]);
          data += 4;
          }});
    }
    public static void TraverseAcrossImage(this BitmapData data, Action<int, int> inner)
    {
      TraverseAcrossImage(data, inner, Empty);
    }
    public static void TraverseAcrossImage(this BitmapData data, Action<int, int> inner, Action<int> outer)
    {
      TraverseAcrossImage(data.Width, data.Height, inner, outer);
    }
    public static void TraverseAcrossImage(int width, int height, Action<int, int> inner)
    {
      TraverseAcrossImage(width, height, inner, Empty);
    }
    public static void TraverseAcrossImage(int width, int height, Action<int,int> inner, Action<int> outer)
    {
      for(int i = 0; i < width; i++)
      {
        outer(i);
        for(int j = 0; j < height; j++)
          inner(i,j);
      }
    }
    public static unsafe void UnsafeTransformation(this Bitmap b, UnsafeImageTransformation transformer, PixelFormat format, ImageLockMode m)
    {
      BitmapData bits = b.LockBits(b.GetImageSizeRectangle(), m, format);
      unsafe
      {
        byte* input = (byte*)bits.Scan0;
        transformer(bits, input);
      }
      b.UnlockBits(bits);
    }
    public static unsafe void UnsafeTransformation(this Bitmap b, UnsafeImageTransformation transformer, PixelFormat format)
    {
      UnsafeTransformation(b, transformer, format, ImageLockMode.ReadWrite);
    }
    public static unsafe void UnsafeTransformation(this Bitmap b, UnsafeImageTransformation transformer)
    {
      UnsafeTransformation(b, transformer, b.PixelFormat);
    }
    public static Rectangle GetImageSizeRectangle(this Bitmap b)
    {
      return new Rectangle(0, 0, b.Width, b.Height);
    }
    public static unsafe void SetColorValue(byte* data, byte r, byte g, byte b)
    {
      data[0] = r;
      data[1] = g;
      data[2] = b;
    }
    public static unsafe bool TryGetGrayScaleValue(byte* data, out byte value)
    {
      byte r = data[0];
      byte g = data[1];
      byte b = data[2];
      bool gotValue = r == g && g == b;
      if(gotValue)
        value = r;
      else
        value = (byte)0;
      return gotValue;
    }
    public static unsafe void SetGrayScaleValue(byte* data, byte value)
    {
      data[0] = value;
      data[1] = value;
      data[2] = value;
      data[3] = (byte)255;
    }
  }
}
