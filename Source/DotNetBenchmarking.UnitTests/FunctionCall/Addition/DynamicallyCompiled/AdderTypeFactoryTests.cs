using System;
using System.Linq;
using System.Reflection;

namespace DotNetBenchmarking.FunctionCall.UnitTests;

/// <summary>
/// <see cref="AdderTypeFactory"/> tests.
/// </summary>
public class AdderTypeFactoryTests
{
    /// <summary>
    /// Checks that <see cref="AdderTypeFactory.GetAdd"/> builds a type dynamically in a new module, 
    /// adds a static Add method to it and creates a delegate from it.
    /// </summary>
    [Fact]
    public void GetAddBuildModuleStatic()
    {
        TakesTwoIntsReturnsInt add = AdderTypeFactory.GetAdd(buildModule: true, DelegateType.Static);

        MethodInfo builtMethod = add.Method;
        Type builtType = builtMethod.DeclaringType!;
        Assert.True(builtType.IsPublic);

        Assert.NotNull(builtType);
        Assert.Equal("Adder", builtType.Name);

        Assert.NotNull(builtMethod);
        Assert.Equal("Add", builtMethod.Name);
        Assert.Equal(typeof(int), builtMethod.ReturnType);
        Assert.Equal(new Type[] { typeof(int), typeof(int) }, builtMethod.GetParameters().Select(p => p.ParameterType).ToArray());

        Assert.True(add.Method.IsPublic);
        Assert.True(add.Method.IsStatic);

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
    /// Checks that <see cref="AdderTypeFactory.GetAdd"/> builds a type dynamically in a new module, 
    /// adds an instance Add method to it and creates a delegate from it.
    /// </summary>
    [Fact]
    public void GetAddBuildModuleInstance()
    {
        TakesTwoIntsReturnsInt add = AdderTypeFactory.GetAdd(buildModule: true, DelegateType.Instance);

        MethodInfo builtMethod = add.Method;
        Type builtType = builtMethod.DeclaringType!;
        Assert.True(builtType.IsPublic);

        Assert.NotNull(builtType);
        Assert.Equal("Adder", builtType.Name);

        Assert.NotNull(builtMethod);
        Assert.Equal("Add", builtMethod.Name);
        Assert.Equal(typeof(int), builtMethod.ReturnType);
        Assert.Equal(new Type[] { typeof(int), typeof(int) }, builtMethod.GetParameters().Select(p => p.ParameterType).ToArray());

        Assert.True(add.Method.IsPublic);
        Assert.False(add.Method.IsStatic);

        Assert.IsType(builtType, add.Target);

        TestExecution(add);
    }

    /// <summary>
    /// Checks that <see cref="AdderTypeFactory.GetAdd"/> builds a type dynamically in an existing module, 
    /// adds a static Add method to it and creates a delegate from it.
    /// </summary>
    [Fact]
    public void GetAddExistingModuleStatic()
    {
        TakesTwoIntsReturnsInt add = AdderTypeFactory.GetAdd(buildModule: false, DelegateType.Static);

        MethodInfo builtMethod = add.Method;
        Type builtType = builtMethod.DeclaringType!;
        Assert.True(builtType.IsPublic);

        Assert.NotNull(builtType);
        Assert.StartsWith("Adder_", builtType.Name);

        Assert.NotNull(builtMethod);
        Assert.Equal("Add", builtMethod.Name);
        Assert.Equal(typeof(int), builtMethod.ReturnType);
        Assert.Equal(new Type[] { typeof(int), typeof(int) }, builtMethod.GetParameters().Select(p => p.ParameterType).ToArray());

        Assert.True(add.Method.IsPublic);
        Assert.True(add.Method.IsStatic);

        Assert.Null(add.Target);

        TestExecution(add);
    }

    /// <summary>
    /// Checks that <see cref="AdderTypeFactory.GetAdd"/> builds a type dynamically in an existing module, 
    /// adds an instance Add method to it and creates a delegate from it.
    /// </summary>
    [Fact]
    public void GetAddExistingModuleInstance()
    {
        TakesTwoIntsReturnsInt add = AdderTypeFactory.GetAdd(buildModule: false, DelegateType.Instance);

        MethodInfo builtMethod = add.Method;
        Type builtType = builtMethod.DeclaringType!;
        Assert.True(builtType.IsPublic);

        Assert.NotNull(builtType);
        Assert.StartsWith("Adder_", builtType.Name);

        Assert.NotNull(builtMethod);
        Assert.Equal("Add", builtMethod.Name);
        Assert.Equal(typeof(int), builtMethod.ReturnType);
        Assert.Equal(new Type[] { typeof(int), typeof(int) }, builtMethod.GetParameters().Select(p => p.ParameterType).ToArray());

        Assert.True(add.Method.IsPublic);
        Assert.False(add.Method.IsStatic);

        Assert.IsType(builtType, add.Target);

        TestExecution(add);
    }
}