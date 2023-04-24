using System.Reflection;
using System.Reflection.Emit;

namespace DotNetBenchmarking.FunctionCall;

/// <summary>
/// Prototype for functions doing a certain number of integer additions (loops).
/// </summary>
/// <param name="loops">Number of additions to execute.</param>
public delegate void AddIntegers(int loops);

/// <summary>
/// Builds <see cref="AddIntegers"/> delegate instances.
/// </summary>
internal static class AddLoopMethodFactory
{
    /// <summary>
    /// Add method implementation.
    /// </summary>
    public enum AddImplementation
    {
        // The add implementation is a static delegate
        StaticDelegate,

        // The add implementation is an instance delegate
        InstanceDelegate,

        // The add implementation is embedded in the loop
        Embedded
    };

    private static readonly MethodInfo _TakesTwoIntsReturnsIntInvokeMethod = typeof(TakesTwoIntsReturnsInt).GetMethod("Invoke")!;
    private static readonly MethodInfo _AddMethodFactoryBuildDynamicMethodMethod = typeof(AddMethodFactory).GetMethod("BuildDynamicMethod")!;

    private static readonly object _delegateTarget = new();

    /// <summary>
    /// Builds a dynamic loop method taking a number of loops and a delegate invoking a delegate to add 2 integers.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>An instance delegate to the built method.</returns>
    public static AddIntegers Build(AddImplementation addImplementation)
    {
        DynamicMethod loopDynamicMethod = new("AddIntegers",
            returnType: null, parameterTypes: new Type[] { typeof(object), typeof(int) });

        ILGenerator ILGenerator = loopDynamicMethod.GetILGenerator();

        LocalBuilder loopsLocal = ILGenerator.DeclareLocal(typeof(int));

        ILGenerator.Emit(OpCodes.Ldarg_1);
        ILGenerator.Emit(OpCodes.Stloc, loopsLocal);

        LocalBuilder addLocal = ILGenerator.DeclareLocal(typeof(TakesTwoIntsReturnsInt));
        ILGenerator.Emit(OpCodes.Ldc_I4, 0); // buildStaticDelegates: false
        ILGenerator.Emit(OpCodes.Call, _AddMethodFactoryBuildDynamicMethodMethod);
        ILGenerator.Emit(OpCodes.Stloc, addLocal);

        Label loopCheckLabel = ILGenerator.DefineLabel();
        Label loopStartLabel = ILGenerator.DefineLabel();

        LocalBuilder bLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 2);
        ILGenerator.Emit(OpCodes.Stloc, bLocal);

        LocalBuilder indexLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 0);
        ILGenerator.Emit(OpCodes.Stloc, indexLocal);

        ILGenerator.Emit(OpCodes.Br_S, loopCheckLabel);

        ILGenerator.MarkLabel(loopStartLabel);

        if (addImplementation == AddImplementation.Embedded)
        {
            // Add
            ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
            ILGenerator.Emit(OpCodes.Ldloc, bLocal);
            ILGenerator.Emit(OpCodes.Add);
        }
        else
        {
            // Invoke the add delegate
            ILGenerator.Emit(OpCodes.Ldloc, addLocal);
            ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
            ILGenerator.Emit(OpCodes.Ldloc, bLocal);
            ILGenerator.Emit(OpCodes.Callvirt, _TakesTwoIntsReturnsIntInvokeMethod);
        }
        ILGenerator.Emit(OpCodes.Pop);

        ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
        ILGenerator.Emit(OpCodes.Ldc_I4, 1);
        ILGenerator.Emit(OpCodes.Add);
        ILGenerator.Emit(OpCodes.Stloc, indexLocal);

        ILGenerator.MarkLabel(loopCheckLabel);

        ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
        ILGenerator.Emit(OpCodes.Ldloc, loopsLocal);
        ILGenerator.Emit(OpCodes.Blt_S, loopStartLabel);

        ILGenerator.Emit(OpCodes.Ret);

        return (AddIntegers)loopDynamicMethod.CreateDelegate(typeof(AddIntegers), _delegateTarget);
    }
}
