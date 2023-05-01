using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled;

/// <summary>
/// Prototype for functions doing a certain number of integer additions (loops).
/// </summary>
/// <param name="loops">Number of additions to execute.</param>
/// <returns>The addition results.</returns>
internal delegate int[] AddLoopWithResults(int loops);

/// <summary>
/// Builds <see cref="AddLoopWithResults"/> delegate instances.
/// </summary>
internal static class AddLoopWithResultsMethodFactory
{
    private static readonly MethodInfo _TakesTwoIntsReturnsIntInvokeMethod = typeof(TakesTwoIntsReturnsInt).GetMethod("Invoke")!;
    private static readonly MethodInfo _AddMethodFactoryGetFromDynamicMethodMethod = typeof(AddMethodFactory).GetMethod("GetFromDynamicMethod")!;

    private static readonly object _delegateTarget = new();

    /// <summary>
    /// Builds a delegate executing a certain number of addition loops and returning the results using DynamicMethod.
    /// The addition can be implemented either as a delegate call or embedded.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>An instance delegate to the built method.</returns>
    public static AddLoopWithResults GetFromDynamicMethod(AddImplementation addImplementation) =>
        (addImplementation != AddImplementation.Embedded) ? GetFromDynamicMethodNonEmbedded(addImplementation)
                                                          : GetFromDynamicMethodEmbedded();

    /// <summary>
    /// Builds a delegate executing a certain number of addition loops and returning the results using DynamicMethod.
    /// The addition is be implemented as a delegate call from the loop.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>An instance delegate to the built method.</returns>
    private static AddLoopWithResults GetFromDynamicMethodNonEmbedded(AddImplementation addImplementation)
    {
        DynamicMethod loopDynamicMethod = new("AddLoop",
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

        // Get the delegate and store it in a local variable
        DelegateInstanceType delegateInstanceType = addImplementation switch
        {
            AddImplementation.StaticDelegate => DelegateInstanceType.Static,
            AddImplementation.InstanceDelegate => DelegateInstanceType.Instance,

            _ => throw new NotImplementedException()
        };

        LocalBuilder? addLocal = ILGenerator.DeclareLocal(typeof(TakesTwoIntsReturnsInt));
        ILGenerator.Emit(OpCodes.Ldc_I4, (int)delegateInstanceType);
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

    /// <summary>
    /// Builds a delegate executing a certain number of addition loops and returning the results using DynamicMethod.
    /// The addition is embedded in the loop.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>An instance delegate to the built method.</returns>
    private static AddLoopWithResults GetFromDynamicMethodEmbedded()
    {
        DynamicMethod loopDynamicMethod = new("AddLoop",
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

    /// <summary>
    /// Builds a dynamic loop method executing a certain number of addition loops and returning the results 
    /// implemented either as a delegate call or embedded using an expression tree.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>An instance delegate to the built method.</returns>
    public static AddLoopWithResults GetFromExpressionTree(AddImplementation addImplementation) =>
        (addImplementation != AddImplementation.Embedded) ? GetFromExpressionTreeNonEmbedded(addImplementation)
                                                          : GetFromExpressionTreeEmbedded();

    /// <summary>
    /// Builds a delegate executing a certain number of addition loops using an expression tree.
    /// The addition is be implemented as a delegate call from the loop.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>An instance delegate to the built method.</returns>
    private static AddLoopWithResults GetFromExpressionTreeNonEmbedded(AddImplementation addImplementation)
    {
        ParameterExpression loops = Expression.Parameter(typeof(int), "loops");

        ParameterExpression a = Expression.Variable(typeof(int), "a");
        ParameterExpression i = Expression.Variable(typeof(int), "i");

        ParameterExpression sum = Expression.Variable(typeof(int), "sum");
        ParameterExpression result = Expression.Variable(typeof(int[]), "result");

        ParameterExpression adder = Expression.Variable(typeof(TakesTwoIntsReturnsInt), "adder");

        LabelTarget breakLabel = Expression.Label("break");

        // Get the delegate and store it in a local variable
        DelegateInstanceType delegateInstanceType = addImplementation switch
        {
            AddImplementation.StaticDelegate => DelegateInstanceType.Static,
            AddImplementation.InstanceDelegate => DelegateInstanceType.Instance,

            _ => throw new NotImplementedException()
        };

        BlockExpression body = Expression.Block(new ParameterExpression[] { adder, a, i, result },
            Expression.Assign(result, Expression.NewArrayBounds(typeof(int), loops)),
            Expression.Assign(adder, Expression.Call(_AddMethodFactoryGetFromDynamicMethodMethod, Expression.Constant(delegateInstanceType))),
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
                    Expression.Assign(Expression.ArrayAccess(result, i), sum),
                    Expression.PostIncrementAssign(i)
                ),
                breakLabel
            ),
            result);

        LambdaExpression lambdaExpression = Expression.Lambda(typeof(AddLoopWithResults), body, loops);
        return (AddLoopWithResults)lambdaExpression.Compile();
    }

    /// <summary>
    /// Builds a delegate executing a certain number of addition loops using an expression tree.
    /// The addition is be embedded in the loop.
    /// </summary>
    /// <param name="addImplementation">Add method implementation.</param>
    /// <returns>An instance delegate to the built method.</returns>
    private static AddLoopWithResults GetFromExpressionTreeEmbedded()
    {
        ParameterExpression loops = Expression.Parameter(typeof(int), "loops");

        ParameterExpression a = Expression.Variable(typeof(int), "a");
        ParameterExpression i = Expression.Variable(typeof(int), "i");

        ParameterExpression sum = Expression.Variable(typeof(int), "sum");
        ParameterExpression result = Expression.Variable(typeof(int[]), "result");

        LabelTarget breakLabel = Expression.Label("break");

        BlockExpression body = Expression.Block(new ParameterExpression[] { a, i, result },
            Expression.Assign(result, Expression.NewArrayBounds(typeof(int), loops)),
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
                    Expression.Assign(Expression.ArrayAccess(result, i), sum),
                    Expression.PostIncrementAssign(i)
                ),
                breakLabel
            ),
            result);

        LambdaExpression lambdaExpression = Expression.Lambda(typeof(AddLoopWithResults), body, loops);
        return (AddLoopWithResults)lambdaExpression.Compile();
    }
}
