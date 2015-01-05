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
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Cortex;
using Cortex.Collections;
using Cortex.LexicalAnalysis;

namespace Cortex.Symbolic
{
  public class RegexSymbol : Word, IComparable<RegexSymbol>
  {
    public string Name { get; set; }
    //	public bool CachingIsAllowed { get; protected set; }
    public RegexSymbol(string input, string name, string type)
      : base(input, type)
    {
      Name = name;
    }
    public override ShakeCondition<string> AsShakeCondition()
    {
      Regex currentRegex = new Regex(TargetWord);
      return LexicalExtensions.GenerateRegexCond<string>(
          (x,y,z) => 
          {
          Match m = currentRegex.Match(x,y,z - y);	
          Segment output = m.Success ? new Segment(m.Length, m.Index + y) : null;
          return new Tuple<bool, Segment>(m.Success, output);
          });
    }
    public override TypedShakeCondition<string> AsTypedShakeCondition()
    {
      Regex currentRegex = new Regex(TargetWord);
      return LexicalExtensions.GenerateTypedRegexCond<string>(
          (x,y,z) => 
          {
          Match m = currentRegex.Match(x,y,z - y);	
          TypedSegment output = m.Success ? new TypedSegment(m.Length, WordType, m.Index + y) : null;
          return new Tuple<bool, TypedSegment>(m.Success, output);
          });
    }

    public override object Clone()
    {
      return new RegexSymbol(TargetWord, Name, TargetWord);
    }
    public virtual int CompareTo(RegexSymbol other)
    {
      return Name.CompareTo(other.Name) + base.CompareTo((Word)other);
    }

  }
}
