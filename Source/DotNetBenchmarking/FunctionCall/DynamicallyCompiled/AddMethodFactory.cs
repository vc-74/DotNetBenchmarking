using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled;

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
    public static TakesTwoIntsReturnsInt GetFromDynamicMethod(DelegateInstanceType delegateType)
    {
        switch (delegateType)
        {
            case DelegateInstanceType.Instance:
                {
                    DynamicMethod addDynamicMethod = new("Add",
                        MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard,
                        returnType: typeof(int), parameterTypes: new Type[] { typeof(object), typeof(int), typeof(int) },
                        owner: typeof(AddLoopMethodFactory), skipVisibility: true);

                    ILGenerator ILGenerator = addDynamicMethod.GetILGenerator();

                    ILGenerator.Emit(OpCodes.Ldarg_1);
                    ILGenerator.Emit(OpCodes.Ldarg_2);

                    ILGenerator.Emit(OpCodes.Add);
                    ILGenerator.Emit(OpCodes.Ret);

                    return (TakesTwoIntsReturnsInt)addDynamicMethod.CreateDelegate(typeof(TakesTwoIntsReturnsInt), _delegateTarget);
                }

            case DelegateInstanceType.Static:
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

    private static readonly object _delegateTarget = new();

    /// <summary>
    /// Builds a <see cref="TakesTwoIntsReturnsInt"/> delegate adding two integers by creating an expression tree.
    /// </summary>
    /// <param name="useLambda">Indicates whether the expression tree has to be built from a lambda expression (true) or manually (false).</param>
    /// <returns>The delegate instance.</returns>
    public static TakesTwoIntsReturnsInt GetFromExpressionTree(bool useLambda)
    {
        if (useLambda)
        {
            LambdaExpression lambdaExpression = (int a, int b) => a + b;
            Func<int, int, int> func = (Func<int, int, int>)lambdaExpression.Compile();
            return new TakesTwoIntsReturnsInt(func);
        }
        else
        {
            ParameterExpression aParameter = Expression.Parameter(typeof(int), "a");
            ParameterExpression bParameter = Expression.Parameter(typeof(int), "b");

            Expression sum = Expression.Add(aParameter, bParameter);

            LambdaExpression lambdaExpression = Expression.Lambda(typeof(TakesTwoIntsReturnsInt), sum, aParameter, bParameter);
            return (TakesTwoIntsReturnsInt)lambdaExpression.Compile();
        }
    }
}
