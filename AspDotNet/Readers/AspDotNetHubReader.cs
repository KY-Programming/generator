using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KY.Core;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Reflection.Language;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.AspDotNet.Readers
{
    public class AspDotNetHubReader
    {
        private readonly ReflectionModelReader modelReader;

        public AspDotNetHubReader(ReflectionModelReader modelReader)
        {
            this.modelReader = modelReader;
        }

        public void Read(AspDotNetReadConfiguration configuration, List<ITransferObject> transferObjects)
        {
            configuration.Hub.AssertIsNotNull($"SignalR: {nameof(configuration.Hub)}");
            configuration.Hub.Name.AssertIsNotNull($"SignalR: {nameof(configuration.Hub)}.{nameof(configuration.Hub.Name)}");
            configuration.Hub.Namespace.AssertIsNotNull($"SignalR: {nameof(configuration.Hub)}.{nameof(configuration.Hub.Namespace)}");
            Logger.Trace($"Read SignalR hub {configuration.Hub.Namespace}.{configuration.Hub.Name}...");
            Type type = GeneratorTypeLoader.Get(configuration.Hub.Assembly, configuration.Hub.Namespace, configuration.Hub.Name);
            if (type == null || type.BaseType == null || type.BaseType.Name != "Hub`1")
            {
                return;
            }
            if (!type.BaseType.IsGenericType)
            {
                Logger.Error("Implement generic Hub<T> instead of non-generic Hub type");
            }

            SignalRHubTransferObject hub = new SignalRHubTransferObject();
            hub.Name = type.Name;
            hub.Language = ReflectionLanguage.Instance;

            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in methods)
            {
                if (method.Name == "OnConnectedAsync" || method.Name == "OnDisconnectedAsync")
                {
                    continue;
                }
                HttpServiceActionTransferObject action = new HttpServiceActionTransferObject();
                action.Name = method.Name;
                if (method.ReturnType.Name != typeof(void).Name && method.ReturnType.Name != nameof(Task))
                {
                    Logger.Error($"Return type of method {method.Name} in {hub.Name} has to be void or Task");
                }
                foreach (ParameterInfo parameter in method.GetParameters())
                {
                    this.modelReader.Read(parameter.ParameterType, transferObjects);
                    action.Parameters.Add(new HttpServiceActionParameterTransferObject
                                          {
                                              Name = parameter.Name,
                                              Type = parameter.ParameterType.ToTransferObject()
                                          });
                }
                hub.Actions.Add(action);
            }

            Type notificationType = type.BaseType.GetGenericArguments().Single();
            MethodInfo[] notificationMethods = notificationType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in notificationMethods)
            {
                HttpServiceActionTransferObject action = new HttpServiceActionTransferObject();
                action.Name = method.Name;
                foreach (ParameterInfo parameter in method.GetParameters())
                {
                    this.modelReader.Read(parameter.ParameterType, transferObjects);
                    action.Parameters.Add(new HttpServiceActionParameterTransferObject
                                          {
                                              Name = parameter.Name,
                                              Type = parameter.ParameterType.ToTransferObject()
                                          });
                }
                hub.Events.Add(action);
            }
            transferObjects.Add(hub);
        }
    }
}
