﻿using System;
using System.IO;
#if !NET6_0_OR_GREATER
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
#endif
using System.Xml.Linq;
using System.Xml.XPath;

namespace Machine.Specifications.Runner.Utility
{
    /// <summary>
    /// The remote run listener is a decorator class which takes the burden to implement IMessageSink and translates
    /// information about specification execution over app domain boundaries.
    /// </summary>
#if !NET6_0_OR_GREATER
    [Serializable]
#endif
    internal class RemoteRunListener : LongLivedMarshalByRefObject, ISpecificationRunListener
#if !NET6_0_OR_GREATER
        , IMessageSink
#endif
    {
        private readonly ISpecificationRunListener runListener;

        public RemoteRunListener(ISpecificationRunListener runListener)
        {
            this.runListener = runListener;
        }

        public virtual void OnAssemblyStart(AssemblyInfo assemblyInfo)
        {
            runListener.OnAssemblyStart(assemblyInfo);
        }

        public virtual void OnAssemblyEnd(AssemblyInfo assemblyInfo)
        {
            runListener.OnAssemblyEnd(assemblyInfo);
        }

        public virtual void OnRunStart()
        {
            runListener.OnRunStart();
        }

        public virtual void OnRunEnd()
        {
            runListener.OnRunEnd();
        }

        public virtual void OnContextStart(ContextInfo contextInfo)
        {
            runListener.OnContextStart(contextInfo);
        }

        public virtual void OnContextEnd(ContextInfo contextInfo)
        {
            runListener.OnContextEnd(contextInfo);
        }

        public virtual void OnSpecificationStart(SpecificationInfo specificationInfo)
        {
            runListener.OnSpecificationStart(specificationInfo);
        }

        public virtual void OnSpecificationEnd(SpecificationInfo specificationInfo, Result result)
        {
            runListener.OnSpecificationEnd(specificationInfo, result);
        }

        public virtual void OnFatalError(ExceptionResult exceptionResult)
        {
            runListener.OnFatalError(exceptionResult);
        }

#if !NET6_0_OR_GREATER
        public IMessage SyncProcessMessage(IMessage msg)
        {
            if (msg is IMethodCallMessage methodCall)
            {
                return RemotingServices.ExecuteMessage(this, methodCall);
            }

            // This is all a bit ugly but gives us version independance for the moment
            var value = (string) msg.Properties["data"];
            var doc = XDocument.Load(new StringReader(value));
            var element = doc.XPathSelectElement("/listener/*");
            var listener = (ISpecificationRunListener) this;

            switch (element?.Name.ToString())
            {
                case "onassemblystart":
                    listener.OnAssemblyStart(AssemblyInfo.Parse(element.XPathSelectElement("//onassemblystart/*")?.ToString()));
                    break;

                case "onassemblyend":
                    listener.OnAssemblyEnd(AssemblyInfo.Parse(element.XPathSelectElement("//onassemblyend/*")?.ToString()));
                    break;

                case "onrunstart":
                    listener.OnRunStart();
                    break;

                case "onrunend":
                    listener.OnRunEnd();
                    break;

                case "oncontextstart":
                    listener.OnContextStart(ContextInfo.Parse(element.XPathSelectElement("//oncontextstart/*")?.ToString()));
                    break;

                case "oncontextend":
                    listener.OnContextEnd(ContextInfo.Parse(element.XPathSelectElement("//oncontextend/*")?.ToString()));
                    break;

                case "onspecificationstart":
                    listener.OnSpecificationStart(SpecificationInfo.Parse(element.XPathSelectElement("//onspecificationstart/*")?.ToString()));
                    break;

                case "onspecificationend":
                    listener.OnSpecificationEnd(
                        SpecificationInfo.Parse(element.XPathSelectElement("//onspecificationend/specificationinfo")?.ToString()), 
                        Result.Parse(element.XPathSelectElement("//onspecificationend/result")?.ToString()));
                    break;

                case "onfatalerror":
                    listener.OnFatalError(ExceptionResult.Parse(element.XPathSelectElement("//onfatalerror/*")?.ToString()));
                    break;
            }

            return null;
        }

        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }

        public IMessageSink NextSink { get; private set; }
#endif
    }
}
