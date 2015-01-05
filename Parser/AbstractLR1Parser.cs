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
using System.Reflection;
using System.Text;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cortex;
using Cortex.Grammar;
using Cortex.LexicalAnalysis;

namespace Cortex.Parsing
{
  public abstract class AbstractLR1Parser<R,Lookahead,Encoding> : Parser<R,Encoding>
    where R : Rule
    where Encoding : struct
    where Lookahead : ILookaheadRule
    {
      protected List<List<Lookahead>> cPrime;
      public List<List<Lookahead>> CPrime { get { return cPrime; } }
      protected SemanticRule onAccept;
      public SemanticRule OnAccept { get { return onAccept; } }

      protected AbstractLR1Parser(AbstractGrammar<R,Encoding> g, string terminateSymbol,
          SemanticRule r, bool suppressMessages, bool setupRequired) 
        : base(g, terminateSymbol, suppressMessages, setupRequired)
      {
        this.onAccept = r;
      }
      public abstract string RetrieveTables(Dictionary<string, string> symbolTable);
      protected override void SetupParser()
      {
        cPrime = new List<List<Lookahead>>();
        SetupExtraParserElements();
        foreach(var v in Items())
          cPrime.Add(new List<Lookahead>(v));	
        PreTableConstruction();
        MakeTable();
        MakeGotoTable();
        PostTableConstruction();
      }
      protected abstract void MakeGotoTable();
      protected abstract void MakeTable();
      protected abstract void SetupExtraParserElements();
      protected abstract void PreTableConstruction();
      protected abstract void PostTableConstruction();
      public abstract IEnumerable<IEnumerable<Lookahead>> Items();
      public abstract IEnumerable<Lookahead> ComputeGoto(IEnumerable<Lookahead> i, string x);
      public IEnumerable<Lookahead> Closure(IEnumerable<Lookahead> rules)
      {
        return Closure(rules, TerminateSymbol);
      }
      public abstract IEnumerable<Lookahead> Closure(IEnumerable<Lookahead> rules,
          string terminateSymbol);
      public abstract IEnumerable<string> First(LookaheadRule rule, int lookahead);
    }	
}
