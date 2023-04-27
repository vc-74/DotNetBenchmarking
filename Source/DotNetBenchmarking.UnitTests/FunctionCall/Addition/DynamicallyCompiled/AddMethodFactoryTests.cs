using System;
using System.Linq;
using System.Reflection;

namespace DotNetBenchmarking.FunctionCall.UnitTests;

/// <summary>
/// <see cref="AddMethodFactory"/> tests.
/// </summary>
public class AddMethodFactoryTests
{
    /// <summary>
    /// Checks that <see cref="AddMethodFactory.GetFromDynamicMethod"/> builds a DynamicMethod 
    /// and creates a static delegate (no target) for it.
    /// </summary>
    [Fact]
    public void GetFromDynamicMethodStatic()
    {
        TakesTwoIntsReturnsInt add = AddMethodFactory.GetFromDynamicMethod(DelegateType.Static);

        MethodInfo builtMethod = add.Method;

        Assert.NotNull(builtMethod);
        Assert.Equal("Add", builtMethod.Name);
        Assert.Equal(typeof(int), builtMethod.ReturnType);
        Assert.Equal(new Type[] { typeof(int), typeof(int) }, builtMethod.GetParameters().Select(p => p.ParameterType).ToArray());

        Assert.True(builtMethod.IsPublic);
        Assert.True(builtMethod.IsStatic);

        Assert.Null(add.Target);

        TestExecution(add);
    }

    private static void TestExecution(TakesTwoIntsReturnsInt add)
    {
        TestExecution(add, a: 1, b: 1);
        TestExecution(add, a: 0, b: 0);
        TestExecution(add, a: 10, b: -2);
        TestExecution(add, a: -10, b: 2);
        TestExecution(add, a: int.MaxValue, b: int.MinValue);
    }

    private static void TestExecution(TakesTwoIntsReturnsInt add, int a, int b)
    {
        int actual = add(a, b);
        int expectedSum = a + b;

        Assert.Equal(expectedSum, actual);
    }

    /// <summary>
    /// Checks that <see cref="AddMethodFactory.GetFromDynamicMethod"/> builds a DynamicMethod 
    /// and creates an instance delegate (no target) for it.
    /// </summary>
    [Fact]
    public void GetFromDynamicMethodInstance()
    {
        TakesTwoIntsReturnsInt add = AddMethodFactory.GetFromDynamicMethod(DelegateType.Instance);

        MethodInfo builtMethod = add.Method;

        Assert.NotNull(builtMethod);
        Assert.Equal("Add", builtMethod.Name);
        Assert.Equal(typeof(int), builtMethod.ReturnType);
        Assert.Equal(new Type[] { typeof(object), typeof(int), typeof(int) }, builtMethod.GetParameters().Select(p => p.ParameterType).ToArray());

        Assert.True(add.Method.IsPublic);
        Assert.True(add.Method.IsStatic); // Dynamic methods are always static although the delegate has a target

        Assert.IsType<object>(add.Target);

        TestExecution(add);
    }

    /// <summary>
    /// Checks that <see cref="AddMethodFactory.GetFromExpressionTree"/> builds an expression tree
    /// and converts it to a delegate (no target).
    /// </summary>
    [Fact]
    public void GetFromExpressionTreeNoLambda()
    {
        TakesTwoIntsReturnsInt add = AddMethodFactory.GetFromExpressionTree(useLambda: false);

        MethodInfo builtMethod = add.Method;

        Assert.NotNull(builtMethod);
        Assert.StartsWith("lambda", builtMethod.Name);
        Assert.Equal(typeof(int), builtMethod.ReturnType);

        ParameterInfo[] parameters = builtMethod.GetParameters();
        Assert.NotNull(parameters[0]); // System.Runtime.CompilerServices.Closure
        Assert.Equal(typeof(int), parameters[1].ParameterType);
        Assert.Equal(typeof(int), parameters[2].ParameterType);

        Assert.True(add.Method.IsPublic);
        Assert.True(add.Method.IsStatic);

        Assert.NotNull(add.Target);
        Assert.Equal("System.Runtime.CompilerServices.Closure", add.Target!.GetType().FullName);

        TestExecution(add);
    }

    /// <summary>
    /// Checks that <see cref="AddMethodFactory.GetFromExpressionTree"/> gets an expression tree from a lambda
    /// and converts it to a delegate (no target).
    /// </summary>
    [Fact]
    public void GetFromExpressionTreeLambda()
    {
        TakesTwoIntsReturnsInt add = AddMethodFactory.GetFromExpressionTree(useLambda: true);

        MethodInfo builtMethod = add.Method;

        Assert.NotNull(builtMethod);
        Assert.StartsWith("Invoke", builtMethod.Name);
        Assert.Equal(typeof(int), builtMethod.ReturnType);

        ParameterInfo[] parameters = builtMethod.GetParameters();
        Assert.Equal(new Type[] { typeof(int), typeof(int) }, builtMethod.GetParameters().Select(p => p.ParameterType).ToArray());

        Assert.True(add.Method.IsPublic);
        Assert.False(add.Method.IsStatic); // Not a static delegate

        Assert.NotNull(add.Target);
        Assert.IsType<Func<int, int, int>>(add.Target);

        TestExecution(add);
    }
}