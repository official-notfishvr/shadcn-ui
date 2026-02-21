#if NETSTANDARD2_0 || NETSTANDARD2_1
namespace System.Runtime.CompilerServices;

/// <summary>
/// Polyfill for record types support in .NET Standard 2.0/2.1
/// </summary>
internal static class IsExternalInit { }
#endif
