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
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace Cortex
{
  public static partial class RangeExtensions
  {
    public static bool InRange(this int target, int from, int to, int uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }
    public static bool InRange(this long target, long from, long to, long uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }
    public static bool InRange(this double target, double from, double to, double uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }
    public static bool InRange(this float target, float from, float to, float uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }
    public static bool InRange(this decimal target, decimal from, decimal to, decimal uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }

    public static bool InRange(this uint target, uint from, uint to, uint uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }

    public static bool InRange(this ushort target, ushort from, ushort to, ushort uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }
    public static bool InRange(this ulong target, ulong from, ulong to, ulong uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }
    public static bool InRange(this short target, short from, short to, short uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }
    public static bool InRange(this byte target, byte from, byte to, byte uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }
    public static bool InRange(this sbyte target, sbyte from, sbyte to, sbyte uncertainty)
    {
      return (target <= (to + uncertainty)) && (target >= (from - uncertainty));
    }
    public static bool InRange(this int target, int from, int to)
    {
      return InRange(target, from, to ,0);
    }
    public static bool InRange(this decimal target, decimal from, decimal to)
    {
      return InRange(target, from, to ,0.0M);
    }
    public static bool InRange(this double target, double from, double to)
    {
      return InRange(target, from, to ,0.0);
    }
    public static bool InRange(this float target, float from, float to)
    {
      return InRange(target, from, to ,0.0f);
    }
    public static bool InRange(this long target, long from, long to)
    {
      return InRange(target, from, to ,0L);
    }
    public static bool InRange(this short target, short from, short to)
    {
      return InRange(target, from, to ,(short)0);
    }
    public static bool InRange(this uint target, uint from, uint to)
    {
      return InRange(target, from, to ,(uint)0);
    }
    public static bool InRange(this ushort target, ushort from, ushort to)
    {
      return InRange(target, from, to ,(ushort)0);
    }
    public static bool InRange(this ulong target, ulong from, ulong to)
    {
      return InRange(target, from, to ,(ulong)0);
    }
    public static bool InRange(this byte target, byte from, byte to)
    {
      return InRange(target, from, to ,(byte)0);
    }
    public static bool InRange(this sbyte target, sbyte from, sbyte to)
    {
      return InRange(target, from, to ,(sbyte)0);
    }
  }
}
