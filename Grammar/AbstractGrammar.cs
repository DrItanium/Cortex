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

namespace Cortex.Grammar
{
  public abstract class AbstractGrammar<R,Encoding> : List<R>
    where R : Rule
    where Encoding : struct
    {
      private string name;
      public string Name { get { return name; } set { name = value; } }
      public abstract IEnumerable<string> SymbolTable { get; }
      public abstract IEnumerable<string> TerminalSymbols { get; } 
      public abstract IEnumerable<string> NonTerminalSymbols { get; }
      public abstract int NumberOfProductions { get; }
      protected AbstractGrammar() { }
      protected AbstractGrammar(IEnumerable<R> rules)
        : this()
      {
        AddRange(rules);
      }
      public bool IsTerminalSymbol(string symbol)
      {
        return !Exists(symbol);
      }
      public abstract void UpdateSymbolTable();

      public abstract Encoding ReverseLookup(string ruleName, Production target);
      public abstract IFixedRule LookupRule(Encoding index);
      public abstract IProduction LookupProduction(Encoding index);
      public abstract bool Exists(string rule);
      public abstract R this[string name] { get; }
      public abstract int IndexOf(string name);

      public new void AddRange(IEnumerable<R> rules)
      {
        foreach(var v in rules)
          Add_Impl(v, true);
        UpdateSymbolTable();
      }
      protected void BaseAdd(R r)
      {
        base.Add(r);
      }
      public new void Add(R r)
      {
        Add_Impl(r, false);
      }
      protected abstract void Add_Impl(R rule, bool delayUpdate);
      public new bool Remove(R rule)
      {
        return Remove_Impl(rule);	
      }
      protected abstract bool Remove_Impl(R rule);
      protected abstract void RemoveAt_Impl(int index);
      public new void RemoveAt(int index)
      {
        RemoveAt_Impl(index);
      }
      public new int RemoveAll(Predicate<R> pred)
      {
        //YAY Closures!
        return base.RemoveAll(MakeRemoveAllFunction(pred));
      }
      protected abstract Predicate<R> MakeRemoveAllFunction(Predicate<R> pred);

      public new void Clear()
      {
        base.Clear();
        Clear_Impl();
      }
      protected abstract void Clear_Impl();
    }
}
