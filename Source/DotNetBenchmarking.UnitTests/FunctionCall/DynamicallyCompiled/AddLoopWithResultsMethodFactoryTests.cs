using System;
using System.Linq;
using System.Reflection;

namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled.UnitTests;

/// <summary>
/// <see cref="AddLoopWithResultsMethodFactory"/> tests.
/// </summary>
public class AddLoopWithResultsMethodFactoryTests
{
    /// <summary>
    /// Checks that <see cref="AddLoopWithResultsMethodFactory.GetFromDynamicMethod"/> builds a DynamicMethod 
    /// invoking a static delegate and creates a delegate for it.
    /// </summary>
    [Fact]
    public void GetWithResultsFromDynamicMethodStatic()
    {
        AddLoopWithResults add = AddLoopWithResultsMethodFactory.GetFromDynamicMethod(AddImplementation.StaticDelegate);
        TestDynamicMethodExecution(add);
    }

    private static void TestDynamicMethodExecution(AddLoopWithResults add)
    {
        MethodInfo builtMethod = add.Method;

        Assert.NotNull(builtMethod);
        Assert.Equal("AddLoop", builtMethod.Name);
        Assert.Equal(typeof(int[]), builtMethod.ReturnType);
        Assert.Equal(new Type[] { typeof(object), typeof(int) }, builtMethod.GetParameters().Select(p => p.ParameterType).ToArray());

        Assert.True(builtMethod.IsPublic);
        Assert.True(builtMethod.IsStatic);

        Assert.IsType<object>(add.Target);

        int[] sums = add(loops: 100);
        Assert.Equal(Enumerable.Range(0, 100).Select(i => 1 + i).ToArray(), sums);
    }

    /// <summary>
    /// Checks that <see cref="AddLoopWithResultsMethodFactory.GetFromDynamicMethod"/> builds a DynamicMethod 
    /// invoking a static delegate and creates a delegate for it.
    /// </summary>
    [Fact]
    public void GetWithResultsFromDynamicMethodInstance()
    {
        AddLoopWithResults add = AddLoopWithResultsMethodFactory.GetFromDynamicMethod(AddImplementation.InstanceDelegate);
        TestDynamicMethodExecution(add);
    }

    /// <summary>
    /// Checks that <see cref="AddLoopWithResultsMethodFactory.GetFromDynamicMethod"/> builds a DynamicMethod 
    /// embedding the addition implementation and creates a delegate for it.
    /// </summary>
    [Fact]
    public void GetWithResultsFromDynamicMethodEmbedded()
    {
        AddLoopWithResults add = AddLoopWithResultsMethodFactory.GetFromDynamicMethod(AddImplementation.Embedded);
        TestDynamicMethodExecution(add);
    }

    /// <summary>
    /// Checks that <see cref="AddLoopMethodFactory.GetFromExpressionTreeWithResults"/> builds an expression tree
    /// invoking a static delegate and creates a delegate for it.
    /// </summary>
    [Fact]
    public void GetWithResultsFromExpressionTreeStatic()
    {
        AddLoopWithResults add = AddLoopWithResultsMethodFactory.GetFromExpressionTree(AddImplementation.StaticDelegate);
        TestExpressionTreeExecution(add);
    }

    private static void TestExpressionTreeExecution(AddLoopWithResults add)
    {
        MethodInfo builtMethod = add.Method;

        Assert.NotNull(builtMethod);
        Assert.StartsWith("lambda_method", builtMethod.Name);
        Assert.Equal(typeof(int[]), builtMethod.ReturnType);

        ParameterInfo[] builtMethodParameters = builtMethod.GetParameters();
        Assert.Equal(2, builtMethodParameters.Length);
        Assert.Equal("System.Runtime.CompilerServices.Closure", builtMethodParameters[0].ParameterType.FullName);
        Assert.Equal(typeof(int), builtMethodParameters[1].ParameterType);

        Assert.True(builtMethod.IsPublic);
        Assert.True(builtMethod.IsStatic);

        Assert.NotNull(add.Target);
        Assert.Equal("System.Runtime.CompilerServices.Closure", add.Target.GetType().FullName);

        int[] sums = add(loops: 100);
        Assert.Equal(Enumerable.Range(0, 100).Select(i => 1 + i).ToArray(), sums);
    }

    /// <summary>
    /// Checks that <see cref="AddLoopMethodFactory.GetFromExpressionTreeWithResults"/> builds an expression tree
    /// invoking an instance delegate and creates a delegate for it.
    /// </summary>
    [Fact]
    public void GetWithResultsFromExpressionTreeInstance()
    {
        AddLoopWithResults add = AddLoopWithResultsMethodFactory.GetFromExpressionTree(AddImplementation.InstanceDelegate);
        TestExpressionTreeExecution(add);
    }

    /// <summary>
    /// Checks that <see cref="AddLoopMethodFactory.GetFromExpressionTreeWithResults"/> builds an expression tree
    /// embedding the addition and creates a delegate for it.
    /// </summary>
    [Fact]
    public void GetWithResultsFromExpressionTreeEmbedded()
    {
        AddLoopWithResults add = AddLoopWithResultsMethodFactory.GetFromExpressionTree(AddImplementation.Embedded);
        TestExpressionTreeExecution(add);
    }
}