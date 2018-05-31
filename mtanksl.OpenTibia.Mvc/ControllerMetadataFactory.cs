using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenTibia.Mvc
{
    public class ControllerMetadataFactory
    {
        public ControllerMetadataFactory()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies() )
            {
                foreach (var type in assembly.GetTypes() )
                {
                    PortAttribute portAttribute = type.GetCustomAttribute<PortAttribute>();

                    if (portAttribute != null)
                    {
                        foreach (var method in type.GetMethods() )
                        {
                            PacketAttribute packetAttribute = method.GetCustomAttribute<PacketAttribute>();

                            if (packetAttribute != null)
                            {
                                List<Type> parameters = new List<Type>();

                                foreach (var parameter in method.GetParameters() )
                                {
                                    parameters.Add(parameter.ParameterType);
                                }

                                Dictionary<int, ControllerMetadata> identifierContext;

                                if ( !portIdentifierControllerBaseMetadata.TryGetValue(portAttribute.Port, out identifierContext) )
                                {
                                    identifierContext = new Dictionary<int, ControllerMetadata>();

                                    portIdentifierControllerBaseMetadata.Add(portAttribute.Port, identifierContext);
                                }

                                ControllerMetadata context;

                                if ( !identifierContext.TryGetValue(packetAttribute.Identifier, out context) )
                                {
                                    context = new ControllerMetadata()
                                    {
                                        Port = portAttribute.Port,

                                        Identifier = packetAttribute.Identifier,

                                        Type = type,

                                        Method = method,

                                        ParameterTypes = parameters
                                    };

                                    identifierContext.Add(packetAttribute.Identifier, context);
                                }
                            }
                        }
                    }
                }
            }            
        }

        private Dictionary<int, Dictionary<int, ControllerMetadata>> portIdentifierControllerBaseMetadata = new Dictionary<int, Dictionary<int, ControllerMetadata>>();

        public ControllerMetadata Get(int port, byte identifier)
        {
            Dictionary<int, ControllerMetadata> identifierControllerBaseMetadata;

            if ( portIdentifierControllerBaseMetadata.TryGetValue(port, out identifierControllerBaseMetadata) )
            {
                ControllerMetadata controllerBaseMetadata;

                if (identifierControllerBaseMetadata.TryGetValue(identifier, out controllerBaseMetadata) )
                {
                    return controllerBaseMetadata;
                }
            }

            return null;
        }
    }
}