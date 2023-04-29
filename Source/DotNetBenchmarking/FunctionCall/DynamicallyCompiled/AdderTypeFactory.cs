using System.Reflection;
using System.Reflection.Emit;

namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled;

/// <summary>
/// Builds adder types with an Add method adding two integers.
/// </summary>
internal static class AdderTypeFactory
{
    static AdderTypeFactory()
    {
        _moduleBuilder = GetModuleBuilder();
    }
    private static readonly ModuleBuilder _moduleBuilder;

    private static ModuleBuilder GetModuleBuilder()
    {
        // Build the module builder used to build new types
        _builtModuleCount++;

        string assemblyName = $"DotNetBenchmarking.FunctionCall.Dynamic_{_builtModuleCount}";
        AssemblyName strongAssemblyName = new(assemblyName);
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(strongAssemblyName, AssemblyBuilderAccess.Run);

        return assemblyBuilder.DefineDynamicModule(assemblyName);
    }
    private static int _builtModuleCount = 0;

    /// <summary>
    /// Builds a new type with an Add method adding two integers and returns a delegate for it.
    /// </summary>
    /// <param name="buildModule">Indicates whether the default module builder can be reused (false) or a new one has to be created (true).</param>
    /// <param name="delegateType">Type of delegate to build.</param>
    /// <returns>The delegate instance.</returns>
    public static TakesTwoIntsReturnsInt GetAdd(bool buildModule, DelegateInstanceType delegateType)
    {
        _builtMethodCount++;

        string typeName;
        ModuleBuilder moduleBuilder;
        if (buildModule)
        {
            typeName = "Adder";
            moduleBuilder = GetModuleBuilder();
        }
        else
        {
            typeName = $"Adder_{_builtMethodCount}";
            moduleBuilder = _moduleBuilder;
        }

        TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

        MethodBuilder addMethodBuilder;

        switch (delegateType)
        {
            case DelegateInstanceType.Static:
                addMethodBuilder = typeBuilder.DefineMethod("Add", MethodAttributes.Static | MethodAttributes.Public | MethodAttributes.HideBySig,
                    typeof(int), _inputParameterTypes);
                break;

            case DelegateInstanceType.Instance:
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public,
                    CallingConventions.Standard, Type.EmptyTypes);

                ILGenerator constructorILGenerator = constructorBuilder.GetILGenerator();
                constructorILGenerator.Emit(OpCodes.Ret);

                addMethodBuilder = typeBuilder.DefineMethod("Add", MethodAttributes.Public | MethodAttributes.HideBySig,
                    typeof(int), _inputParameterTypes);
                break;

            default:
                throw new NotImplementedException();
        }

        ILGenerator addILGenerator = addMethodBuilder.GetILGenerator();

        if (delegateType == DelegateInstanceType.Static)
        {
            addILGenerator.Emit(OpCodes.Ldarg_0);
            addILGenerator.Emit(OpCodes.Ldarg_1);
        }
        else
        {
            addILGenerator.Emit(OpCodes.Ldarg_1);
            addILGenerator.Emit(OpCodes.Ldarg_2);
        }

        addILGenerator.Emit(OpCodes.Add);
        addILGenerator.Emit(OpCodes.Ret);

        Type type = typeBuilder.CreateType();
        MethodInfo method = type.GetMethods()[0];

        if (delegateType == DelegateInstanceType.Static)
        {
            return (TakesTwoIntsReturnsInt)method.CreateDelegate(typeof(TakesTwoIntsReturnsInt));
        }
        else
        {
            object instance = type.GetConstructors()[0].Invoke(Array.Empty<object>());
            return (TakesTwoIntsReturnsInt)method.CreateDelegate(typeof(TakesTwoIntsReturnsInt), instance);
        }
    }
    private static int _builtMethodCount = 0;
    private static readonly Type[] _inputParameterTypes = new[] { typeof(int), typeof(int) };
}