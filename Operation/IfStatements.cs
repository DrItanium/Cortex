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

namespace Cortex.Operation
{
  public class IfThenStatement : NonStandardOperation
  {
    public override Operation First()
    {
      return Condition.First();
    }
    public BooleanOperation Condition
    {
      get
      {
        return (BooleanOperation)this[0];
      }
      set
      {
        this[0] = value;
      }
    }
    public Block OnTrue 
    {
      get 
      {
        return (Block)this[1];
      }
      set
      {
        this[1] = value;
      }
    }

    public IfThenStatement(BooleanOperation condition, Block onTrue)
    {
      Add(condition);
      Add(onTrue);
    }
    public override Operation Coalesce()
    {
      Condition = (BooleanOperation)Condition.Coalesce();
      OnTrue = (Block)OnTrue.Coalesce();
      return this;
    }
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("\t{0}\n",GetType().ToString());
      sb.AppendFormat("\t\t{0}\n",Condition.ToString());
      sb.AppendFormat("\t\t{0}\n",OnTrue.ToString());
      return sb.ToString();
    }
    public override void InitialFulfill()
    {
      Condition.OnTrueHook = OnTrue.First();
      Condition.InitialFulfill();
      RegisterHook((x) => Condition.ResolveHooks(x));
      OnTrue.InitialFulfill();
      RegisterHook((x) => OnTrue.ResolveHooks(x));	
    }
    public override void Enumerate(IGraphBuilder builder)
    {
      base.Enumerate(builder);
      foreach(var v in this)
        v.Enumerate(builder);
    }
    public override void Build(IGraphBuilder builder)
    {
      Condition.Build(builder);
      OnTrue.Build(builder);
    }
  }
  public class IfThenElseStatement : IfThenStatement
  {
    public Block OnFalse
    {
      get
      {
        return (Block)this[2];
      }
      set
      {
        this[2] = value; 
      }
    }
    public IfThenElseStatement(BooleanOperation condition, 
        Block onTrue, Block onFalse)
      : base(condition, onTrue)
    {
      Add(onFalse);
    }
    public override Operation Coalesce()
    {
      Condition = (BooleanOperation)Condition.Coalesce();
      OnFalse = (Block)OnFalse.Coalesce();
      OnTrue = (Block)OnTrue.Coalesce();
      return this;
    }
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("\t{0}\n",GetType().ToString());
      sb.AppendFormat("\t\t{0}\n",Condition.ToString());
      sb.AppendFormat("\t\t{0}\n",OnTrue.ToString());
      sb.AppendFormat("\t\t{0}",OnFalse.ToString());
      return sb.ToString();
    }
    public override void InitialFulfill()
    {
      Condition.OnTrueHook = OnTrue.First();
      Condition.InitialFulfill();
      Condition.ResolveHooks(OnFalse.First());
      OnTrue.InitialFulfill();
      RegisterHook((x) => OnTrue.ResolveHooks(x));
      OnFalse.InitialFulfill();
      RegisterHook((x) => OnFalse.ResolveHooks(x));
    }
    public override void Build(IGraphBuilder builder)
    {
      //we already gave the proper links so just act as a forwarder
      Condition.Build(builder);
      OnTrue.Build(builder);
      OnFalse.Build(builder);
      //OnTrue and OnFalse
    }
  }
}
