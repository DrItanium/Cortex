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
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace Cortex.Collections
{
  public class CompressionList<T> 
  {
    private T defaultValue;
    private Dictionary<int,int> translationTable;
    private List<T> storage;
    public int Count { get { return storage.Count; } }
    public int Capacity { get { return storage.Capacity; } }
    public CompressionList(T defaultValue)
    {
      this.defaultValue = defaultValue;
    }
    public T this[int index]
    {
      get
      {
        if(translationTable == null || !translationTable.ContainsKey(index))
          return defaultValue;
        else
          return storage[translationTable[index]];
      }
      set
      {
        if(translationTable == null)
        {
          translationTable = new Dictionary<int,int>();
          storage = new List<T>();
        }
        if(!translationTable.ContainsKey(index))
        {
          int target = 0;
          if(Contains(value))
            target = storage.IndexOf(value);
          else
          {
            target = storage.Count;
            storage.Add(value);
          }
          translationTable[index] = target;
        }
        else
          storage[translationTable[index]] = value;
      }
    }
    public bool Contains(T value)
    {
      return storage.Contains(value);
    }
    public bool Exists(Predicate<T> value)
    {
      return storage.Exists(value);
    }
  }
}
