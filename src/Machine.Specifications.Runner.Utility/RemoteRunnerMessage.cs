﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if !NET6_0_OR_GREATER
using System.Runtime.Remoting.Messaging;
#endif
using System.Xml.Linq;

namespace Machine.Specifications.Runner.Utility
{
#if !NET6_0_OR_GREATER
    internal class RemoteRunnerMessage : MarshalByRefObject, IMessage
    {
        public RemoteRunnerMessage(XElement root, MemberInfo info = null)
        {
            // This is certainly not the most elegant approach. But instead of building an XML based abstraction
            // over MemberInfo we use the member info directly and marshal it over to the core app domain.
            Properties = new Dictionary<string, object>
            {
                { "data", root.ToString() },
                { "member", info },
            };
        }

        public IDictionary Properties { get; }
    }
#endif
}
