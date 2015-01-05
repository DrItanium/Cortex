//-----------------------------------------------------------------------------
//Cortex
//Copyright (c) 2010-2015, Joshua Scoggins 
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
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace Cortex
{
  public static partial class MathExtensions
  {
    public static bool IsDivisibleBy(this int value, int by) 
    {
      return (value % by) == 0; 
    }
    public static bool IsDivisibleBy(this long value, long by) 
    {
      return (value % by) == 0; 
    }
    public static bool IsDivisibleBy(this uint value, uint by) 
    {
      return (value % by) == 0; 
    }
    public static bool IsDivisibleBy(this ulong value, ulong by) 
    {
      return (value % by) == 0; 
    }
    public static bool IsDivisibleBy(this short value, short by) 
    {
      return (value % by) == 0;
    }
    public static bool IsDivisibleBy(this ushort value, ushort by) 
    {
      return (value % by) == 0; 
    }
    public static bool IsDivisibleBy(this decimal value, decimal by) 
    {
      return (value % by) == 0; 
    }
    public static bool IsEven(this decimal value) 
    { 
      return value.IsDivisibleBy(2.0M); 
    }
    public static bool IsEven(this int value) 
    {
      return value == ((value >> 1) << 1); 
    }
    public static bool IsEven(this uint value) 
    {
      return value == ((value >> 1) << 1); 
    }
    public static bool IsEven(this long value) 
    { 
      return value == ((value >> 1) << 1); 
    }
    public static bool IsEven(this ulong value) 
    { 
      return value == ((value >> 1) << 1); 
    }
    public static bool IsEven(this ushort value) 
    {
      return value == ((value >> 1) << 1); 
    }
    public static bool IsEven(this short value) 
    {
      return value == ((value >> 1) << 1); 
    }
    public static float FastInverseSquareRoot(this float number)
    {
      const float threeHalfs = 1.5f;
      float x2 = number * 0.5f;
      float y = number;
      long i = (int)y;
      i = 0x5f3759df - (i >> 1);
      y = (float)i;
      y = y * (threeHalfs - (x2 * y * y));
      return y;
    }

  }
}
