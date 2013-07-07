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
  public static partial class SpatialFilteringExtensions
  {
    public static Bitmap Correlation(this Bitmap input, byte[,] mask, int maskWidth, int maskHeight)
    {
      Bitmap clone = input.Clone() as Bitmap;
      int total = 0;
      int a = (maskWidth - 1) >> 1;
      int b = (maskHeight - 1) >> 1;
      for(int x = 0; x < input.Width; x++)
      {
        for(int y = 0; y < input.Height; y++)
        {
          for(int s = 0, _s = -a; s < maskWidth; s++, _s++)
          {
            int wX = x + _s;
            if(wX < 0 || wX >= input.Width)
              continue;
            for(int t = 0, _t = -b; t < maskHeight; t++, _t++)
            {
              int wY = y + _t;
              if(wY < 0 || wY >= input.Height)
                continue;
              int w = mask[s, t];
              int f = input.GetPixel(wX, wY).R;
              total += (w * f);
            }
          }	
          byte value = (byte)total;
          clone.SetPixel(x,y, Color.FromArgb(255, value, value, value));
          total = 0;
        }
      }
      return clone;
    }
    public static Bitmap Convolution(this Bitmap input, byte[,] mask, int maskWidth, int maskHeight)
    {
      Bitmap clone = input.Clone() as Bitmap;
      int total = 0;
      int a = (maskWidth - 1) >> 1;
      int b = (maskHeight - 1) >> 1;
      for(int x = 0; x < input.Width; x++)
      {
        for(int y = 0; y < input.Height; y++)
        {
          for(int s = 0, _s = -a; s < maskWidth; s++, _s++)
          {
            int fX = x - _s;
            if(fX >= 0)
              continue;
            for(int t = 0, _t = -b; t < maskHeight; t++, _t++)
            {
              int fY = y - _t;
              if(fY >= 0)
                continue;
              int w = mask[s, t];
              int f = input.GetPixel(fX, fY).R;
              total += (w * f);
            }
          }	
          byte value = (byte)total;
          clone.SetPixel(x,y, Color.FromArgb(255, value, value, value));
          total = 0;
        }
      }
      return clone;
    }
  }
}
