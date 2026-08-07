using System.Reflection;
using KY.Core;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Reflection.Language;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;

namespace KY.Generator.AspDotNet.Readers;

public class AspDotNetHubReader
{
    private readonly ReflectionModelReader modelReader;
    private readonly Options options;
    private readonly List<ITransferObject> transferObjects;
    private readonly GeneratorTypeLoader typeLoader;

    public AspDotNetHubReader(ReflectionModelReader modelReader, Options options, List<ITransferObject> transferObjects, GeneratorTypeLoader typeLoader)
    {
        this.modelReader = modelReader;
        this.options = options;
        this.transferObjects = transferObjects;
        this.typeLoader = typeLoader;
    }

    public void Read(AspDotNetReadConfiguration configuration)
    {
        configuration.Hub.AssertIsNotNull($"SignalR: {nameof(configuration.Hub)}");
        configuration.Hub.Name.AssertIsNotNull($"SignalR: {nameof(configuration.Hub)}.{nameof(configuration.Hub.Name)}");
        configuration.Hub.Namespace.AssertIsNotNull($"SignalR: {nameof(configuration.Hub)}.{nameof(configuration.Hub.Namespace)}");
        Logger.Trace($"Read SignalR hub {configuration.Hub.Namespace}.{configuration.Hub.Name}...");
        Type type = this.typeLoader.Get(configuration.Hub.Namespace, configuration.Hub.Name);
        if (type.BaseType is not { IsGenericType: true, Name: "Hub`1" })
        {
            throw new InvalidOperationException($"Can not read SignalR hub '{type.FullName}'. Only types derived from the generic 'Hub<T>' can be generated, but '{type.Name}' derives from '{type.BaseType?.FullName ?? "nothing"}'. Implement 'Hub<T>' with T as the client notification interface instead.");
        }

        SignalRHubTransferObject hub = new();
        hub.Name = type.Name;
        hub.Language = ReflectionLanguage.Instance;
        // Without the mapping the options of the hub would not know its type and therefore neither the attributes of
        // the type nor the ones of its assembly (e.g. [assembly: GenerateServiceOutput])
        this.options.Map(hub, () => this.options.Get<GeneratorOptions>(type, null));

        MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        foreach (MethodInfo method in methods)
        {
            if (method.Name == "OnConnectedAsync" || method.Name == "OnDisconnectedAsync")
            {
                continue;
            }
            GeneratorOptions methodOptions = this.options.Get<GeneratorOptions>(method);
            HttpServiceActionTransferObject action = new();
            action.Name = method.Name;
            if (method.ReturnType.Name != typeof(void).Name && method.ReturnType.Name != nameof(Task))
            {
                Logger.Error($"Return type of method {method.Name} in {hub.Name} has to be void or Task");
            }
            foreach (ParameterInfo parameter in method.GetParameters())
            {
                action.Parameters.Add(new HttpServiceActionParameterTransferObject
                {
                    Name = parameter.Name,
                    Type = this.modelReader.Read(parameter.ParameterType, methodOptions)
                });
            }
            hub.Actions.Add(action);
        }

        Type notificationType = type.BaseType.GetGenericArguments().Single();
        MethodInfo[] notificationMethods = notificationType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        foreach (MethodInfo method in notificationMethods)
        {
            GeneratorOptions methodOptions = this.options.Get<GeneratorOptions>(method);
            HttpServiceActionTransferObject action = new();
            action.Name = method.Name;
            foreach (ParameterInfo parameter in method.GetParameters())
            {
                action.Parameters.Add(new HttpServiceActionParameterTransferObject
                {
                    Name = parameter.Name,
                    Type = this.modelReader.Read(parameter.ParameterType, methodOptions)
                });
            }
            hub.Events.Add(action);
        }
        this.transferObjects.Add(hub);
    }
}
