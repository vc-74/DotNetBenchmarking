# Dynamically compiled function compilation

## Description
This benchmark compares different methods of building a static/instance delegates adding two integers dynamically, equivalent to:

## Benchmarks

### DynamicMethodStaticDelegate
Creates a DynamicMethod, emits IL code and creates a static delegate

```csharp
public void DynamicMethodStaticDelegate()
{
    DynamicMethod addDynamicMethod = new("Add",
        MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard,
        returnType: typeof(int), parameterTypes: new Type[] { typeof(int), typeof(int) }, ...

    ILGenerator ILGenerator = addDynamicMethod.GetILGenerator();
    ...

    TakesTwoIntsReturnsInt add = (TakesTwoIntsReturnsInt)addDynamicMethod.CreateDelegate(typeof(TakesTwoIntsReturnsInt));
}
```

### DynamicMethodInstanceDelegate
Creates a DynamicMethod, emits IL code and creates an instance delegate

```csharp
public void DynamicMethodInstanceDelegate()
{
    DynamicMethod addDynamicMethod = new("Add",
        MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard,
        returnType: typeof(int), parameterTypes: new Type[] { typeof(object), typeof(int), typeof(int) }, ...

    ILGenerator ILGenerator = addDynamicMethod.GetILGenerator();
    ...

    TakesTwoIntsReturnsInt add = (TakesTwoIntsReturnsInt)addDynamicMethod.CreateDelegate(typeof(TakesTwoIntsReturnsInt), new object());
}
```

### DynamicTypeStaticDelegate
Builds a new type in a new module, adds a static method to it and creates a static delegate

```csharp
public void DynamicMethodStaticDelegate()
{
    AssemblyBuilder assemblyBuilder = ...
    ModuleBuilder moduleBuilder = ...
    TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

    MethodBuilder addMethodBuilder = typeBuilder.DefineMethod("Add", 
        MethodAttributes.Static | MethodAttributes.Public | MethodAttributes.HideBySig,
        typeof(int), new Type[] { typeof(int), typeof(int) });

    ILGenerator addILGenerator = addMethodBuilder.GetILGenerator();
    ...

    Type type = typeBuilder.CreateType();
    MethodInfo method = type.GetMethods()[0];

    TakesTwoIntsReturnsInt add = (TakesTwoIntsReturnsInt)method.CreateDelegate(typeof(TakesTwoIntsReturnsInt));
}
```

### DynamicTypeInstanceDelegate
Builds a new type in a new module, adds an instance method to it and creates an instance delegate

```csharp
public void DynamicMethodStaticDelegate()
{
    AssemblyBuilder assemblyBuilder = ...
    ModuleBuilder moduleBuilder = ...
    TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

    MethodBuilder addMethodBuilder = typeBuilder.DefineMethod("Add", 
        MethodAttributes.Public | MethodAttributes.HideBySig,
        typeof(int), new Type[] { typeof(int), typeof(int) });

    ILGenerator addILGenerator = addMethodBuilder.GetILGenerator();
    ...

    Type type = typeBuilder.CreateType();
    MethodInfo method = type.GetMethods()[0];

    TakesTwoIntsReturnsInt add = (TakesTwoIntsReturnsInt)method.CreateDelegate(typeof(TakesTwoIntsReturnsInt));
}
```

### ExpressionTreeBuilt
Builds an expression tree and creates a delegate

```csharp
public void ExpressionTreeBuilt()
{
    ParameterExpression aParameter = Expression.Parameter(typeof(int), "a");
    ParameterExpression bParameter = Expression.Parameter(typeof(int), "b");

    Expression sum = Expression.Add(aParameter, bParameter);

    LambdaExpression lambdaExpression = Expression.Lambda(typeof(TakesTwoIntsReturnsInt), sum, aParameter, bParameter);
    TakesTwoIntsReturnsInt add = (TakesTwoIntsReturnsInt)lambdaExpression.Compile();
    
    int a = 1;
}
```

### ExpressionTreeFromLambda
Gets an expression tree from a lambda and creates a delegate

```csharp
public void ExpressionTreeFromLambda()
{
    LambdaExpression lambdaExpression = (int a, int b) => a + b;
    Func<int, int, int> func = (Func<int, int, int>)lambdaExpression.Compile();
    TakesTwoIntsReturnsInt add = new TakesTwoIntsReturnsInt(func);
}
```

These tests only compile delegates, they don't execute them.

The benchmarks building a new type (DynamicTypeNewModule and DynamicTypeExistingModule) cannot be automatically executed since creating a new type gets slower and slower. 
This is less true if a module is created for each type but the differences with DynamicMethod is still very important.

## Environment
<p>
BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2846/21H2/November2021Update)<br/>
12th Gen Intel Core i9-12900H, 1 CPU, 20 logical and 14 physical cores<br/>
.NET SDK=7.0.203<br/>
  [Host]               : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2<br/>
  .NET 7.0             : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2<br/>
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256<br/>
</p>

## Results
|                                    Method |              Runtime |         Mean |      StdDev | Ratio |   Gen0 |   Gen1 |   Gen2 | Allocated | Alloc Ratio |
|------------------------------------------ |--------------------- |-------------:|------------:|------:|-------:|-------:|-------:|----------:|------------:|
|               DynamicMethodStaticDelegate |             .NET 7.0 |     2.714 us |   0.0349 us |  1.00 | 0.0935 | 0.0916 | 0.0076 |   1.13 KB |        1.00 |
|             DynamicMethodInstanceDelegate |             .NET 7.0 |     2.781 us |   0.1559 us |  0.96 | 0.0954 | 0.0916 | 0.0038 |   1.14 KB |        1.01 |
|        DynamicTypeNewModuleStaticDelegate |             .NET 7.0 |   361.717 us | 104.9694 us |     ? | 0.3662 | 0.1221 |      - |   5.17 KB |           ? |
|      DynamicTypeNewModuleInstanceDelegate |             .NET 7.0 |   202.223 us |  39.9838 us |     ? | 0.3662 | 0.1221 |      - |   5.49 KB |           ? |
|   DynamicTypeExistingModuleStaticDelegate |             .NET 7.0 | 1,267.232 us | 428.8095 us |     ? |      - |      - |      - |   4.29 KB |           ? |
| DynamicTypeExistingModuleInstanceDelegate |             .NET 7.0 | 1,240.984 us | 495.7913 us |     ? |      - |      - |      - |   4.61 KB |           ? |
|                       ExpressionTreeBuilt |             .NET 7.0 |    37.092 us |   0.6562 us | 13.66 | 0.3662 | 0.3357 |      - |   4.67 KB |        4.12 |
|                  ExpressionTreeFromLambda |             .NET 7.0 |    32.632 us |   4.5963 us |  9.46 | 0.3662 | 0.3357 |      - |   4.73 KB |        4.18 |
|                                           |                      |              |             |       |        |        |        |           |             |
|               DynamicMethodStaticDelegate | .NET Framework 4.7.2 |     5.359 us |   0.2187 us |  1.00 | 0.1945 | 0.0954 | 0.0229 |    1.2 KB |        1.00 |
|             DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |     3.281 us |   0.1075 us |  0.61 | 0.1945 | 0.0954 | 0.0229 |   1.21 KB |        1.01 |
|        DynamicTypeNewModuleStaticDelegate | .NET Framework 4.7.2 |   224.154 us |  60.5683 us |     ? | 0.9766 | 0.2441 |      - |   6.31 KB |           ? |
|      DynamicTypeNewModuleInstanceDelegate | .NET Framework 4.7.2 |   269.535 us | 106.6449 us |     ? | 0.9766 | 0.2441 |      - |   6.54 KB |           ? |
|   DynamicTypeExistingModuleStaticDelegate | .NET Framework 4.7.2 | 1,144.369 us | 454.3576 us |     ? | 0.4883 |      - |      - |   4.83 KB |           ? |
| DynamicTypeExistingModuleInstanceDelegate | .NET Framework 4.7.2 | 1,214.710 us | 531.7984 us |     ? | 0.4883 |      - |      - |   5.06 KB |           ? |
|                       ExpressionTreeBuilt | .NET Framework 4.7.2 |    18.782 us |   0.1357 us |  3.59 | 0.8240 | 0.3967 | 0.0305 |   5.19 KB |        4.33 |
|                  ExpressionTreeFromLambda | .NET Framework 4.7.2 |    17.408 us |   0.1666 us |  3.32 | 0.7935 | 0.3967 | 0.0305 |   4.92 KB |        4.10 |

## Conclusions:
- DynamicMethod compilation is much faster (up to >150*) than creating a new type and adding a method to it
- Creating a new module before creating a type is faster (~5*) than reusing an existing module
- As expected, generating IL is faster (~12*) than creating an expression tree (which is then compiled to IL)
- Compilation is faster on .NET 7.0 except for lambdas
- Lambdas and dynamic types allocate significantly more than DynamicMethod
