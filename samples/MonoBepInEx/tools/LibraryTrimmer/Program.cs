using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 4)
{
    Console.Error.WriteLine("Usage: LibraryTrimmer <original.dll> <consumer.dll> <trimmed.dll> <plugin.dll>");
    return 2;
}

var originalPath = Path.GetFullPath(args[0]);
var consumerPath = Path.GetFullPath(args[1]);
var outputPath = Path.GetFullPath(args[2]);
var pluginPath = Path.GetFullPath(args[3]);

var reader = new ReaderParameters { ReadSymbols = false, InMemory = true };
var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(Path.GetDirectoryName(originalPath)!);
resolver.AddSearchDirectory(Path.GetDirectoryName(consumerPath)!);
var repositoryMonoReferences = Path.GetFullPath(Path.Combine(
    Path.GetDirectoryName(originalPath)!, "..", "..", "..", "References", "Mono"));
if (Directory.Exists(repositoryMonoReferences))
    resolver.AddSearchDirectory(repositoryMonoReferences);
reader.AssemblyResolver = resolver;
using var library = AssemblyDefinition.ReadAssembly(originalPath, reader);
using var consumer = AssemblyDefinition.ReadAssembly(consumerPath, reader);

var libraryName = library.Name.Name;
var libraryMethods = library.MainModule.Types
    .SelectMany(AllTypes)
    .SelectMany(type => type.Methods)
    .ToDictionary(method => method.FullName, StringComparer.Ordinal);
var reachable = new HashSet<MethodDefinition>();
var pending = new Queue<MethodDefinition>();

void Mark(MethodDefinition? method)
{
    if (method != null && method.Module == library.MainModule && reachable.Add(method))
        pending.Enqueue(method);
}

MethodDefinition? Resolve(MethodReference reference)
{
    if (libraryMethods.TryGetValue(reference.FullName, out var localMethod))
        return localMethod;

    try
    {
        var resolved = reference.Resolve();
        if (resolved?.Module == library.MainModule)
            return resolved;
    }
    catch (Exception) { return null; }
    return null;
}

bool IsLibraryReference(MethodReference reference) =>
    (reference.DeclaringType.Scope is AssemblyNameReference assembly && assembly.Name == libraryName) ||
    Resolve(reference) != null;

foreach (var type in consumer.MainModule.Types.SelectMany(AllTypes))
    foreach (var method in type.Methods)
        if (method.HasBody)
            foreach (var instruction in method.Body.Instructions)
                if (instruction.Operand is MethodReference called && IsLibraryReference(called))
                    Mark(Resolve(called));

foreach (var type in library.MainModule.Types.SelectMany(AllTypes))
    foreach (var method in type.Methods)
        if (method.IsConstructor || method.IsAbstract || method.IsVirtual ||
            method.CustomAttributes.Any(a => a.AttributeType.Name is "PreserveAttribute" or "RuntimeInitializeOnLoadMethodAttribute"))
            Mark(method);

while (pending.Count > 0)
{
    var method = pending.Dequeue();
    if (!method.HasBody)
        continue;

    foreach (var instruction in method.Body.Instructions)
    {
        if (instruction.Operand is MethodReference called && IsLibraryReference(called))
            Mark(Resolve(called));
    }
}

var removed = 0;
foreach (var type in library.MainModule.Types.SelectMany(AllTypes))
{
    foreach (var method in type.Methods.ToArray())
    {
        if (reachable.Contains(method) || method.IsConstructor || method.IsAbstract || method.IsVirtual)
            continue;
        type.Methods.Remove(method);
        removed++;
    }
}

Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
library.Write(outputPath, new WriterParameters { WriteSymbols = false });

if (File.Exists(pluginPath))
{
    var temporaryPluginPath = pluginPath + ".trimmed.tmp";
    var hasEmbeddedLibrary = false;
    using (var plugin = AssemblyDefinition.ReadAssembly(pluginPath, new ReaderParameters { ReadSymbols = false, InMemory = true }))
    {
        var resource = plugin.MainModule.Resources.OfType<EmbeddedResource>()
            .FirstOrDefault(r => r.Name.EndsWith("Libs.shadcnui.dll", StringComparison.OrdinalIgnoreCase));
        if (resource != null)
        {
            hasEmbeddedLibrary = true;
            plugin.MainModule.Resources.Remove(resource);
            plugin.MainModule.Resources.Add(new EmbeddedResource(resource.Name, resource.Attributes, File.ReadAllBytes(outputPath)));
            plugin.Write(temporaryPluginPath, new WriterParameters { WriteSymbols = false });
        }
    }
    if (hasEmbeddedLibrary)
    {
        File.Copy(temporaryPluginPath, pluginPath, true);
        File.Delete(temporaryPluginPath);
    }
}

var before = new FileInfo(originalPath).Length;
var after = new FileInfo(outputPath).Length;
Console.WriteLine($"Trimmed shadcnui.dll: {before:N0} -> {after:N0} bytes; removed {removed:N0} unreachable methods.");
return 0;

static IEnumerable<TypeDefinition> AllTypes(TypeDefinition type)
{
    yield return type;
    foreach (var nested in type.NestedTypes)
        foreach (var child in AllTypes(nested))
            yield return child;
}
