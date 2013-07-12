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

namespace Cortex.Operation
{
  public class BinaryOperation : BooleanOperation
  {
    public override Operation First()
    {
      //we don't want the first symbol of the binary grouping,
      //this resolves a granularity issue
      return this;
    }
    public BinaryOperation(StandardOperation op0,
        string operation, StandardOperation op1)
      : base(op0, operation, op1)
    {

    }
    public override void InitialFulfill()
    {
      //we don't actually register this
      RegisterHook((x) => { OnFalseHook = x.First(); });
    }
    public override void Enumerate(IGraphBuilder builder)
    {
      Index = builder.Index(this); //lets not even index the components
    }
    public override void Build(IGraphBuilder builder)
    {
      //do nothing 
      //we can do the linkage here
      if(OnFalseHook != null)
        builder.Link(Index, OnFalseHook.First().Index, "[label = \"f\"]");
      if(OnTrueHook != null)
        builder.Link(Index, OnTrueHook.First().Index, "[label = \"t\"]");
    }
  }
}
