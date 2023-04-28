using System;
using System.Linq;
using System.Reflection;

namespace DotNetBenchmarking.FunctionCall.UnitTests;

/// <summary>
/// <see cref="AddLoopMethodFactory"/> tests.
/// </summary>
public class AddLoopMethodFactoryTests
{
    /// <summary>
    /// Checks that <see cref="AddLoopMethodFactory.GetWithResultsFromDynamicMethod"/> builds a DynamicMethod 
    /// invoking a static delegate and creates a delegate for it.
    /// </summary>
    [Fact]
    public void GetWithResultsFromDynamicMethodStatic()
    {
        AddLoopWithResults add = AddLoopMethodFactory.GetWithResultsFromDynamicMethod(AddLoopMethodFactory.AddImplementation.StaticDelegate);
        TestExecution(add);
    }

    private static void TestExecution(AddLoopWithResults add)
    {
        MethodInfo builtMethod = add.Method;

        Assert.NotNull(builtMethod);
        Assert.Equal("AddIntegers", builtMethod.Name);
        Assert.Equal(typeof(int[]), builtMethod.ReturnType);
        Assert.Equal(new Type[] { typeof(object), typeof(int) }, builtMethod.GetParameters().Select(p => p.ParameterType).ToArray());

        Assert.True(builtMethod.IsPublic);
        Assert.True(builtMethod.IsStatic);

        Assert.IsType<object>(add.Target);

        int[] sums = add(loops: 100);
        Assert.Equal(Enumerable.Range(0, 100).Select(i => 1 + i).ToArray(), sums);
    }

    /// <summary>
    /// Checks that <see cref="AddLoopMethodFactory.GetWithResultsFromDynamicMethod"/> builds a DynamicMethod 
    /// invoking a static delegate and creates a delegate for it.
    /// </summary>
    [Fact]
    public void GetWithResultsFromDynamicMethodInstance()
    {
        AddLoopWithResults add = AddLoopMethodFactory.GetWithResultsFromDynamicMethod(AddLoopMethodFactory.AddImplementation.InstanceDelegate);
        TestExecution(add);
    }

    /// <summary>
    /// Checks that <see cref="AddLoopMethodFactory.GetWithResultsFromDynamicMethod"/> builds a DynamicMethod 
    /// embedding the addition implementation and creates a delegate for it.
    /// </summary>
    [Fact]
    public void GetWithResultsFromDynamicMethodEmbedded()
    {
        AddLoopWithResults add = AddLoopMethodFactory.GetWithResultsFromDynamicMethod(AddLoopMethodFactory.AddImplementation.Embedded);
        TestExecution(add);
    }
}