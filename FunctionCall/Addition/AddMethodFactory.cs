using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace DotNetBenchmarking.FunctionCall;

/// <summary>
/// Prototype for functions taking two integers in parameter and returning an integer.
/// </summary>
public delegate int TakesTwoIntsReturnsInt(int a, int b);

/// <summary>
/// Builds <see cref="TakesTwoIntsReturnsInt"/> delegate instances adding integers.
/// </summary>
public static class AddMethodFactory
{
    /// <summary>
    /// Builds a <see cref="TakesTwoIntsReturnsInt"/> delegate adding two integers by generating IL code.
    /// </summary>
    /// <param name="delegateType">Type of delegate to build.</param>
    /// <returns>The delegate instance.</returns>
    public static TakesTwoIntsReturnsInt BuildDynamicMethodIL(DelegateType delegateType)
    {
        switch (delegateType)
        {
            case DelegateType.Instance:
                {
                    DynamicMethod addDynamicMethod = new("Add",
                            returnType: typeof(int), parameterTypes: new Type[] { typeof(object), typeof(int), typeof(int) });

                    ILGenerator ILGenerator = addDynamicMethod.GetILGenerator();

                    ILGenerator.Emit(OpCodes.Ldarg_1);
                    ILGenerator.Emit(OpCodes.Ldarg_2);

                    ILGenerator.Emit(OpCodes.Add);
                    ILGenerator.Emit(OpCodes.Ret);

                    return (TakesTwoIntsReturnsInt)addDynamicMethod.CreateDelegate(typeof(TakesTwoIntsReturnsInt), _delegateTarget);
                }

            case DelegateType.Static:
                {
                    DynamicMethod addDynamicMethod = new("Add",
                        MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard,
                        returnType: typeof(int), parameterTypes: new Type[] { typeof(int), typeof(int) },
                        owner: typeof(AddLoopMethodFactory), skipVisibility: true);

                    ILGenerator ILGenerator = addDynamicMethod.GetILGenerator();

                    ILGenerator.Emit(OpCodes.Ldarg_0);
                    ILGenerator.Emit(OpCodes.Ldarg_1);

                    ILGenerator.Emit(OpCodes.Add);
                    ILGenerator.Emit(OpCodes.Ret);

                    return (TakesTwoIntsReturnsInt)addDynamicMethod.CreateDelegate(typeof(TakesTwoIntsReturnsInt));
                }

            default:
                throw new NotImplementedException();
        }
    }

    // Cache the delegate target to avoid allocating
    private static readonly object _delegateTarget = new();

    /// <summary>
    /// Builds a <see cref="TakesTwoIntsReturnsInt"/> delegate adding two integers by creating an expression tree.
    /// </summary>
    /// <returns>The delegate instance.</returns>
    public static TakesTwoIntsReturnsInt BuildDynamicMethodExpressionTree()
    {
        ParameterExpression aParameter = Expression.Parameter(typeof(int), "a");
        ParameterExpression bParameter = Expression.Parameter(typeof(int), "b");

        Expression aPlusb = Expression.Add(aParameter, bParameter);

        LambdaExpression lambdaExpression = Expression.Lambda(typeof(TakesTwoIntsReturnsInt), aPlusb,
            aParameter, bParameter);

        return (TakesTwoIntsReturnsInt)lambdaExpression.Compile();
    }
}
