using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled;

/// <summary>
/// Add method implementation for a loop.
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

/// <summary>
/// Builds <see cref="AddLoop"/> delegate instances.
/// </summary>
internal static class AddLoopMethodFactory
{
    private static readonly MethodInfo _TakesTwoIntsReturnsIntInvokeMethod = typeof(TakesTwoIntsReturnsInt).GetMethod("Invoke")!;
    private static readonly MethodInfo _AddMethodFactoryGetFromDynamicMethodMethod = typeof(AddMethodFactory).GetMethod("GetFromDynamicMethod")!;

    private static readonly object _delegateTarget = new();

    /// <summary>
    /// Builds a delegate executing a certain number of addition loops using DynamicMethod.
    /// The addition can be implemented either as a delegate call or embedded.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>A delegate to the built method.</returns>
    public static TakesAnInt GetFromDynamicMethod(AddImplementation addImplementation) =>
        addImplementation switch
        {
            AddImplementation.StaticDelegate => GetFromDynamicMethodStatic(),
            AddImplementation.InstanceDelegate => GetFromDynamicMethodInstance(),
            AddImplementation.Embedded => GetFromDynamicMethodEmbedded(),

            _ => throw new NotImplementedException()
        };

    /// <summary>
    /// Builds a static delegate executing a certain number of addition loops using DynamicMethod.
    /// The addition is be implemented as a static delegate call from the loop.
    /// </summary>
    /// <returns>A delegate to the built method.</returns>
    private static TakesAnInt GetFromDynamicMethodStatic()
    {
        DynamicMethod loopDynamicMethod = new("AddLoop", returnType: null, _staticParameterTypes);

        ILGenerator ILGenerator = loopDynamicMethod.GetILGenerator();

        LocalBuilder aLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 1);
        ILGenerator.Emit(OpCodes.Stloc, aLocal);

        LocalBuilder loopsLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldarg_0);
        ILGenerator.Emit(OpCodes.Stloc, loopsLocal);

        // Get the delegate and store it in a local variable
        LocalBuilder? addLocal = ILGenerator.DeclareLocal(typeof(TakesTwoIntsReturnsInt));
        ILGenerator.Emit(OpCodes.Ldc_I4, (int)DelegateInstanceType.Static);
        ILGenerator.Emit(OpCodes.Call, _AddMethodFactoryGetFromDynamicMethodMethod);
        ILGenerator.Emit(OpCodes.Stloc, addLocal);

        Label loopCheckLabel = ILGenerator.DefineLabel();
        Label loopStartLabel = ILGenerator.DefineLabel();

        LocalBuilder indexLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 0);
        ILGenerator.Emit(OpCodes.Stloc, indexLocal);

        ILGenerator.Emit(OpCodes.Br_S, loopCheckLabel);

        ILGenerator.MarkLabel(loopStartLabel);

        // Invoke the add delegate
        ILGenerator.Emit(OpCodes.Ldloc, addLocal);
        ILGenerator.Emit(OpCodes.Ldloc, aLocal);
        ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
        ILGenerator.Emit(OpCodes.Callvirt, _TakesTwoIntsReturnsIntInvokeMethod);

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

        return (TakesAnInt)loopDynamicMethod.CreateDelegate(typeof(TakesAnInt));
    }
    private static readonly Type[] _staticParameterTypes = new Type[] { typeof(int) };

    /// <summary>
    /// Builds an instance delegate executing a certain number of addition loops using DynamicMethod.
    /// The addition is be implemented as an instance delegate call from the loop.
    /// </summary>
    /// <returns>A delegate to the built method.</returns>
    private static TakesAnInt GetFromDynamicMethodInstance()
    {
        DynamicMethod loopDynamicMethod = new("AddLoop", returnType: null, _instanceParameterTypes);

        ILGenerator ILGenerator = loopDynamicMethod.GetILGenerator();

        LocalBuilder aLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 1);
        ILGenerator.Emit(OpCodes.Stloc, aLocal);

        LocalBuilder loopsLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldarg_1);
        ILGenerator.Emit(OpCodes.Stloc, loopsLocal);

        LocalBuilder? addLocal = ILGenerator.DeclareLocal(typeof(TakesTwoIntsReturnsInt));
        ILGenerator.Emit(OpCodes.Ldc_I4, (int)DelegateInstanceType.Instance);
        ILGenerator.Emit(OpCodes.Call, _AddMethodFactoryGetFromDynamicMethodMethod);
        ILGenerator.Emit(OpCodes.Stloc, addLocal);

        Label loopCheckLabel = ILGenerator.DefineLabel();
        Label loopStartLabel = ILGenerator.DefineLabel();

        LocalBuilder indexLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 0);
        ILGenerator.Emit(OpCodes.Stloc, indexLocal);

        ILGenerator.Emit(OpCodes.Br_S, loopCheckLabel);

        ILGenerator.MarkLabel(loopStartLabel);

        // Invoke the add delegate
        ILGenerator.Emit(OpCodes.Ldloc, addLocal);
        ILGenerator.Emit(OpCodes.Ldloc, aLocal);
        ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
        ILGenerator.Emit(OpCodes.Callvirt, _TakesTwoIntsReturnsIntInvokeMethod);

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

        return (TakesAnInt)loopDynamicMethod.CreateDelegate(typeof(TakesAnInt), _delegateTarget);
    }
    private static readonly Type[] _instanceParameterTypes = new Type[] { typeof(object), typeof(int) };

    /// <summary>
    /// Builds a delegate executing a certain number of addition loops using DynamicMethod.
    /// The addition is embedded in the loop.
    /// </summary>
    /// <returns>A delegate to the built method.</returns>
    private static TakesAnInt GetFromDynamicMethodEmbedded()
    {
        DynamicMethod loopDynamicMethod = new("AddLoop", returnType: null, _instanceParameterTypes);

        ILGenerator ILGenerator = loopDynamicMethod.GetILGenerator();

        LocalBuilder aLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 1);
        ILGenerator.Emit(OpCodes.Stloc, aLocal);

        LocalBuilder loopsLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldarg_1);
        ILGenerator.Emit(OpCodes.Stloc, loopsLocal);

        Label loopCheckLabel = ILGenerator.DefineLabel();
        Label loopStartLabel = ILGenerator.DefineLabel();

        LocalBuilder indexLocal = ILGenerator.DeclareLocal(typeof(int));
        ILGenerator.Emit(OpCodes.Ldc_I4, 0);
        ILGenerator.Emit(OpCodes.Stloc, indexLocal);

        ILGenerator.Emit(OpCodes.Br_S, loopCheckLabel);

        ILGenerator.MarkLabel(loopStartLabel);

        // Add
        ILGenerator.Emit(OpCodes.Ldloc, aLocal);
        ILGenerator.Emit(OpCodes.Ldloc, indexLocal);
        ILGenerator.Emit(OpCodes.Add);

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

        return (TakesAnInt)loopDynamicMethod.CreateDelegate(typeof(TakesAnInt), _delegateTarget);
    }

    /// <summary>
    /// Builds a dynamic loop method executing a certain number of addition loops and returning the results 
    /// implemented either as a delegate call or embedded using an expression tree.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>A delegate to the built method.</returns>
    public static TakesAnInt GetFromExpressionTree(AddImplementation addImplementation) =>
        (addImplementation != AddImplementation.Embedded) ? GetFromExpressionTreeNonEmbedded(addImplementation)
                                                          : GetFromExpressionTreeEmbedded();

    /// <summary>
    /// Builds a delegate executing a certain number of addition loops using an expression tree.
    /// The addition is be implemented as a static delegate call from the loop.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>A delegate to the built method.</returns>
    private static TakesAnInt GetFromExpressionTreeNonEmbedded(AddImplementation addImplementation)
    {
        ParameterExpression loops = Expression.Parameter(typeof(int), "loops");

        ParameterExpression a = Expression.Variable(typeof(int), "a");
        ParameterExpression i = Expression.Variable(typeof(int), "i");

        ParameterExpression sum = Expression.Variable(typeof(int), "sum");

        ParameterExpression adder = Expression.Variable(typeof(TakesTwoIntsReturnsInt), "adder");

        LabelTarget breakLabel = Expression.Label("break");

        BlockExpression body = Expression.Block(new ParameterExpression[] { adder, a, i },
            Expression.Assign(adder, Expression.Call(_AddMethodFactoryGetFromDynamicMethodMethod, Expression.Constant(addImplementation))),
            Expression.Assign(i, Expression.Constant(0)),
            Expression.Assign(a, Expression.Constant(1)),
            Expression.Loop
            (
                Expression.Block(new ParameterExpression[] { sum },
                    Expression.IfThen
                    (
                        Expression.GreaterThanOrEqual(i, loops),
                        Expression.Break(breakLabel)
                    ),

                    Expression.Assign(sum, Expression.Invoke(adder, a, i)),
                    Expression.PostIncrementAssign(i)
                ),
                breakLabel
            ),
            Expression.Constant(null));

        LambdaExpression lambdaExpression = Expression.Lambda(typeof(TakesAnInt), body, loops);
        return (TakesAnInt)lambdaExpression.Compile();
    }

    /// <summary>
    /// Builds a delegate executing a certain number of addition loops using an expression tree.
    /// The addition is be embedded in the loop.
    /// </summary>
    /// <returns>A delegate to the built method.</returns>
    private static TakesAnInt GetFromExpressionTreeEmbedded()
    {
        ParameterExpression loops = Expression.Parameter(typeof(int), "loops");

        ParameterExpression a = Expression.Variable(typeof(int), "a");
        ParameterExpression i = Expression.Variable(typeof(int), "i");

        ParameterExpression sum = Expression.Variable(typeof(int), "sum");

        LabelTarget breakLabel = Expression.Label("break");

        BlockExpression body = Expression.Block(new ParameterExpression[] { a, i },
            Expression.Assign(i, Expression.Constant(0)),
            Expression.Assign(a, Expression.Constant(1)),
            Expression.Loop
            (
                Expression.Block(new ParameterExpression[] { sum },
                    Expression.IfThen
                    (
                        Expression.GreaterThanOrEqual(i, loops),
                        Expression.Break(breakLabel)
                    ),

                    Expression.Assign(sum, Expression.Add(a, i)),
                    Expression.PostIncrementAssign(i)
                ),
                breakLabel
            ),
            Expression.Constant(null));

        LambdaExpression lambdaExpression = Expression.Lambda(typeof(TakesAnInt), body, loops);
        return (TakesAnInt)lambdaExpression.Compile();
    }
}
