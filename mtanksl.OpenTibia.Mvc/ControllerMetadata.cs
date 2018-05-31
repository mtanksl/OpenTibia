using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenTibia.Mvc
{
    public class ControllerMetadata
    {
        public int Port { get; set; }

        public byte Identifier { get; set; }

        public Type Type { get; set; }

        public MethodInfo Method { get; set; }

        public List<Type> ParameterTypes { get; set; }
    }
}