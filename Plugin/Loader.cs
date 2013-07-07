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
using System.Runtime.Remoting;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using Cortex.Messaging;

namespace Cortex.Plugin
{
  public class PluginLoader : MarshalByRefObject
  {
    private Dictionary<Guid, Plugin> dict;
    private Guid objectID;
    private string name, author;
    public string Name { get { return name; } protected set { name = value; } }
    public string Author { get { return author; } protected set { author = value; } }
    public Guid ObjectID { get { return objectID; } }
    public PluginLoader(string assembly, Action<PluginAttribute,Type> fn)
    {
      objectID = Guid.NewGuid();
      dict = new Dictionary<Guid, Plugin>();
      Assembly asm = Assembly.LoadFile(assembly);
      if(asm.IsDefined(typeof(PluginAssemblyAttribute), false))
      {
        PluginAssemblyAttribute paa = (PluginAssemblyAttribute)asm.GetCustomAttributes(typeof(PluginAssemblyAttribute), false)[0];
        Name = paa.Name;
        if(paa.Author != null && !paa.Author.Equals(string.Empty))
          Author = paa.Author;
        else
          Author = string.Empty;
        var query = from x in asm.GetTypes()
          where x.IsDefined(typeof(PluginAttribute), false)
          select new 
          {
            Header = x.GetCustomAttributes(typeof(PluginAttribute), false)[0] as PluginAttribute,
                   Type = x,
          };
        foreach(var v in query)
        {
          fn(v.Header, v.Type);
          Plugin p = (Plugin)Activator.CreateInstance(v.Type, new object[] { v.Header.Name });
          dict.Add(p.ObjectID, p);
        }
      }
    }
    public PluginLoader(string assembly) : this(assembly, (h,t) => { }) { }
    public int Count { get { return dict.Count; } }
    public IEnumerable<Guid> Names { get { return dict.Keys; } }
    public Plugin this[Guid name] { get { return dict[name]; } }
    public Message Invoke(Message input) { return this[input.Receiver].Invoke(input);  }
  }
}
