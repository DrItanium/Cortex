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

namespace Cortex.LexicalAnalysis
{
  public class Token<T> : Hunk<T>
  {
    public string TokenType { get; set; }
    public Token(T value, string tokenType, int size, int offset, bool isBig)
      : base(value, size, offset, isBig)
    {
      TokenType = tokenType;
    }
    public Token(T value, string tokenType, int size, int offset)
      : this(value, tokenType, size, offset, false)
    {

    }
    public Token(T value, string tokenType, int size)
      : this(value, tokenType, size, 0)
    {

    }
    public Token(Hunk<T> hunk)
      : this(hunk.Value, string.Empty, hunk.Length, hunk.Start, hunk.IsBig)
    {
    }
    public override bool Equals(object other)
    {
      Token<T> tok = (Token<T>)other;
      return (tok.TokenType.Equals(TokenType)) && base.Equals(tok);
    }
    public bool Equals(Hunk<T> other)
    {
      return base.Equals(other);
    }
    public override int GetHashCode()
    {
      return base.GetHashCode() + TokenType.GetHashCode();
    }
  }
}
