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
using System.Runtime;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Cortex.Messaging
{
  public class Message : TaggedObject 
  {
    private Guid receiver, sender;
    public Guid Sender { get { return sender; } set { sender = value; } }
    public Guid Receiver { get { return receiver; } set { receiver = value; } }
    private MessageOperationType operationType;
    public MessageOperationType OperationType { get { return operationType; } set { operationType = value; } }
    private object value;
    public object Value { get { return value; } set { this.value = value; } }

    public Message(Guid id, Guid sender, Guid receiver, MessageOperationType type, object value)
      : base(id)
    {
      this.sender = sender;
      this.receiver = receiver;
      this.operationType = type;
      this.value = value;
    }
    public Message(Guid sender, Guid receiver, MessageOperationType type, object value)
      : this(Guid.NewGuid(), sender, receiver, type, value)
    {

    }
  }
  public enum MessageOperationType
  {
    ///<summary>
    /// Tells the receiever that they are passing data. This is 
    /// the default action.
    ///</summary>
    Pass,
    ///<summary>
    /// Tells the receiever to execute the data they are
    /// being sent. This is usually a function. Most of the
    /// time an execute message is sent before a pass message to 
    /// tell another domain to get ready to execute something.
    ///</summary>
    Execute,
    ///<summary>
    /// Tells the reciever that this the result of an
    /// execution that occured remotely
    ///</summary>
    Return,
    ///<summary>
    /// Tells the reciever that a message runtime action is occuring.
    /// This is how this library can extend functionality without needing
    /// a recompile.
    ///</summary>
    System,
    ///<summary>
    /// Used by the message back end to ensure that a link can be
    /// established and a pipe can be created.
    ///</summary>
    Init,
    ///<summary>
    /// Message used to describe a failure to another node.
    /// This is an internal type used by the library
    ///</summary>
    Failure, 
  }
}
