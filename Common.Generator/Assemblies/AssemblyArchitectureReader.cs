using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using KY.Core.DataAccess;

namespace KY.Generator;

/// <summary>
/// Reads the architecture an assembly was compiled for directly from its PE header.
/// <see cref="AssemblyName.ProcessorArchitecture"/> can not be used for this: it is a .NET Framework concept and
/// always returns <see cref="ProcessorArchitecture.None"/> on .NET (Core).
/// </summary>
public static class AssemblyArchitectureReader
{
    /// <summary>
    /// Returns the architecture of the assembly at the given path or <see cref="ProcessorArchitecture.None"/> if it
    /// can not be determined. An AnyCPU assembly - including AnyCPU with "prefer 32 bit", which runs in a 64 bit
    /// process as well - is reported as <see cref="ProcessorArchitecture.MSIL"/>.
    /// </summary>
    public static ProcessorArchitecture Read(string path)
    {
        using Stream stream = FileSystem.OpenRead(path);
        using PEReader reader = new(stream);
        PEHeaders headers = reader.PEHeaders;
        CorHeader? corHeader = headers.CorHeader;
        if (corHeader == null)
        {
            // No managed assembly, the caller can not load it anyway
            return ProcessorArchitecture.None;
        }
        bool isILOnly = (corHeader.Flags & CorFlags.ILOnly) != 0;
        bool requires32Bit = (corHeader.Flags & CorFlags.Requires32Bit) != 0;
        bool prefers32Bit = (corHeader.Flags & CorFlags.Prefers32Bit) != 0;
        switch (headers.CoffHeader.Machine)
        {
            case Machine.I386 when isILOnly && (!requires32Bit || prefers32Bit):
                // AnyCPU and AnyCPU (prefer 32 bit) both run in a 64 bit process
                return ProcessorArchitecture.MSIL;
            case Machine.I386:
                return ProcessorArchitecture.X86;
            case Machine.Amd64:
                return ProcessorArchitecture.Amd64;
            case Machine.IA64:
                return ProcessorArchitecture.IA64;
            case Machine.Arm:
            case Machine.ArmThumb2:
            case Machine.Arm64:
                return ProcessorArchitecture.Arm;
            case Machine.Unknown when isILOnly:
                return ProcessorArchitecture.MSIL;
            default:
                return ProcessorArchitecture.None;
        }
    }

    /// <summary>
    /// Returns the architecture of the currently running process.
    /// </summary>
    public static ProcessorArchitecture Current()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => ProcessorArchitecture.X86,
            Architecture.X64 => ProcessorArchitecture.Amd64,
            Architecture.Arm => ProcessorArchitecture.Arm,
            Architecture.Arm64 => ProcessorArchitecture.Arm,
            _ => ProcessorArchitecture.None
        };
    }

    /// <summary>
    /// Checks whether an assembly of the given architecture can be loaded into a process of the given architecture.
    /// </summary>
    public static bool IsCompatible(ProcessorArchitecture assembly, ProcessorArchitecture process)
    {
        return assembly == ProcessorArchitecture.None
               || assembly == ProcessorArchitecture.MSIL
               || process == ProcessorArchitecture.None
               || assembly == process;
    }
}
