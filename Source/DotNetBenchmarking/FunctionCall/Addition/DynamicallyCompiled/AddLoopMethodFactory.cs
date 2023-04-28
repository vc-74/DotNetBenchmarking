using System.Reflection;
using System.Reflection.Emit;

namespace DotNetBenchmarking.FunctionCall;

/// <summary>
/// Prototype for functions doing a certain number of integer additions (loops).
/// </summary>
/// <param name="loops">Number of additions to execute.</param>
public delegate void AddLoop(int loops);

/// <summary>
/// Prototype for functions doing a certain number of integer additions (loops).
/// </summary>
/// <param name="loops">Number of additions to execute.</param>
/// <returns>The addition results.</returns>
internal delegate int[] AddLoopWithResults(int loops);

/// <summary>
/// Builds <see cref="AddLoop"/> delegate instances.
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
    private static readonly MethodInfo _AddMethodFactoryGetFromDynamicMethodMethod = typeof(AddMethodFactory).GetMethod("GetFromDynamicMethod")!;

    private static readonly object _delegateTarget = new();

    /// <summary>
    /// Builds a dynamic loop method executing a certain number of addition loops 
    /// implemented either as a delegate call or embedded.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>An instance delegate to the built method.</returns>
    public static AddLoop GetFromDynamicMethod(AddImplementation addImplementation)
    {
        DynamicMethod loopDynamicMethod = new("AddIntegers",
            returnType: null, parameterTypes: new Type[] { typeof(object), typeof(int) });

        ILGenerator ILGenerator = loopDynamicMethod.GetILGenerator();

        LocalBuilder aLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 1);
        ILGenerator.Emit(OpCodes.Stloc, aLocal);

        LocalBuilder loopsLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldarg_1);
        ILGenerator.Emit(OpCodes.Stloc, loopsLocal);

        LocalBuilder? addLocal;
        if (addImplementation != AddImplementation.Embedded)
        {
            // Get the delegate and store it in a local variable
            addLocal = ILGenerator.DeclareLocal(typeof(TakesTwoIntsReturnsInt));
            ILGenerator.Emit(OpCodes.Ldc_I4, (int)DelegateType.Instance);
            ILGenerator.Emit(OpCodes.Call, _AddMethodFactoryGetFromDynamicMethodMethod);
            ILGenerator.Emit(OpCodes.Stloc, addLocal);
        }
        else
        {
            addLocal = null;
        }

        Label loopCheckLabel = ILGenerator.DefineLabel();
        Label loopStartLabel = ILGenerator.DefineLabel();

        LocalBuilder indexLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 0);
        ILGenerator.Emit(OpCodes.Stloc, indexLocal);

        ILGenerator.Emit(OpCodes.Br_S, loopCheckLabel);

        ILGenerator.MarkLabel(loopStartLabel);

        if (addLocal is null)
        {
            // Add
            ILGenerator.Emit(OpCodes.Ldloc, aLocal);
            ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
            ILGenerator.Emit(OpCodes.Add);
        }
        else
        {
            // Invoke the add delegate
            ILGenerator.Emit(OpCodes.Ldloc, addLocal);
            ILGenerator.Emit(OpCodes.Ldloc, aLocal);
            ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
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

        return (AddLoop)loopDynamicMethod.CreateDelegate(typeof(AddLoop), _delegateTarget);
    }

    /// <summary>
    /// Builds a dynamic loop method executing a certain number of addition loops and returning the results
    /// implemented either as a delegate call or embedded.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>An instance delegate to the built method.</returns>
    public static AddLoopWithResults GetWithResultsFromDynamicMethod(AddImplementation addImplementation)
    {
        DynamicMethod loopDynamicMethod = new("AddIntegers",
            returnType: typeof(int[]), parameterTypes: new Type[] { typeof(object), typeof(int) });

        ILGenerator ILGenerator = loopDynamicMethod.GetILGenerator();

        LocalBuilder aLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 1);
        ILGenerator.Emit(OpCodes.Stloc, aLocal);

        LocalBuilder loopsLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldarg_1);
        ILGenerator.Emit(OpCodes.Stloc, loopsLocal);

        LocalBuilder resultLocal = ILGenerator.DeclareLocal(typeof(int[]));
        ILGenerator.Emit(OpCodes.Ldloc, loopsLocal);
        ILGenerator.Emit(OpCodes.Newarr, typeof(int));
        ILGenerator.Emit(OpCodes.Stloc, resultLocal);

        LocalBuilder? addLocal;
        if (addImplementation != AddImplementation.Embedded)
        {
            // Get the delegate and store it in a local variable
            addLocal = ILGenerator.DeclareLocal(typeof(TakesTwoIntsReturnsInt));
            ILGenerator.Emit(OpCodes.Ldc_I4, (int)DelegateType.Instance);
            ILGenerator.Emit(OpCodes.Call, _AddMethodFactoryGetFromDynamicMethodMethod);
            ILGenerator.Emit(OpCodes.Stloc, addLocal);
        }
        else
        {
            addLocal = null;
        }

        Label loopCheckLabel = ILGenerator.DefineLabel();
        Label loopStartLabel = ILGenerator.DefineLabel();

        LocalBuilder indexLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 0);
        ILGenerator.Emit(OpCodes.Stloc, indexLocal);

        ILGenerator.Emit(OpCodes.Br_S, loopCheckLabel);

        ILGenerator.MarkLabel(loopStartLabel);

        if (addLocal is null)
        {
            // Add
            ILGenerator.Emit(OpCodes.Ldloc, aLocal);
            ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
            ILGenerator.Emit(OpCodes.Add);
        }
        else
        {
            // Invoke the add delegate
            ILGenerator.Emit(OpCodes.Ldloc, addLocal);
            ILGenerator.Emit(OpCodes.Ldloc, aLocal);
            ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
            ILGenerator.Emit(OpCodes.Callvirt, _TakesTwoIntsReturnsIntInvokeMethod);
        }

        LocalBuilder sumLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Stloc, sumLocal);

        ILGenerator.Emit(OpCodes.Ldloc, resultLocal);
        ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
        ILGenerator.Emit(OpCodes.Ldloc, sumLocal);
        ILGenerator.Emit(OpCodes.Stelem_I4);

        ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
        ILGenerator.Emit(OpCodes.Ldc_I4, 1);
        ILGenerator.Emit(OpCodes.Add);
        ILGenerator.Emit(OpCodes.Stloc, indexLocal);

        ILGenerator.MarkLabel(loopCheckLabel);

        ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
        ILGenerator.Emit(OpCodes.Ldloc, loopsLocal);
        ILGenerator.Emit(OpCodes.Blt_S, loopStartLabel);

        ILGenerator.Emit(OpCodes.Ldloc, resultLocal);
        ILGenerator.Emit(OpCodes.Ret);

        return (AddLoopWithResults)loopDynamicMethod.CreateDelegate(typeof(AddLoopWithResults), _delegateTarget);
    }
}
