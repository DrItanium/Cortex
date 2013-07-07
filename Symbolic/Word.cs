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
  public abstract class Word : IComparable<Word>, ICloneable, IComparable<string>
  {
    public string TargetWord { get; protected set; }
    public string WordType { get; protected set; }
    public Word(string input, string type)
    {
      TargetWord = input;
      WordType = type;
    }

    public Word(string input)
      : this(input, string.Empty)
    {
    }

    public abstract ShakeCondition<string> AsShakeCondition();
    public abstract TypedShakeCondition<string> AsTypedShakeCondition();

    public virtual int CompareTo(Word other)
    {
      return TargetWord.CompareTo(other.TargetWord) + WordType.CompareTo(other.WordType);
    }

    public virtual int CompareTo(string other)
    {
      return TargetWord.CompareTo(other);
    }

    public abstract object Clone();

    public override int GetHashCode()
    {
      return TargetWord.GetHashCode() + WordType.GetHashCode();
    }

    public override bool Equals(object other)
    {
      Word w = (Word)other;
      return TargetWord.Equals(w.TargetWord) && WordType.Equals(w.WordType);
    }

    public override string ToString()
    {
      return string.Format("[{0},{1}]", TargetWord, WordType);
    }

    public static implicit operator ShakeCondition<string>(Word k)
    {
      return k.AsShakeCondition();
    }
  }
}