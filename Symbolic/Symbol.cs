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
using Cortex.Collections;
using Cortex.LexicalAnalysis;

namespace Cortex.Symbolic
{
  public class Symbol : Word, IComparable<Symbol>
  {
    public new char TargetWord { get { return base.TargetWord[0]; } 
      set { base.TargetWord = value.ToString(); } }
    public string Name { get; protected set; }
    public Symbol(char input, string name, string type)
      : base(input.ToString(), type)
    {
      Name = name;
    }
    public Symbol(char input, string name)
      : this(input, name, input.ToString())
    {

    }
    public Symbol(char input) : this(input, string.Empty) { }

    public override ShakeCondition<string> AsShakeCondition()
    {
      var fn = LexicalExtensions.GenerateSingleCharacterCond(TargetWord);
      return (x) => fn(x);
    }
    public override TypedShakeCondition<string> AsTypedShakeCondition()
    {
      var fn = LexicalExtensions.GenerateSingleTypedCharacterCond(TargetWord, WordType);
      return (x) => fn(x);
    }

    public override object Clone()
    {
      return new Symbol(TargetWord);
    }

    public virtual int CompareTo(Symbol other)
    {
      return TargetWord.CompareTo(other.TargetWord);
    }

  }
}
