# Dynamically built function compilation and execution

## Description
This benchmark compares different methods of compiling and executing a static/instance delegate adding two integers built dynamically, equivalent to:

## Benchmarks
### NoFunction
Baseline implementation without using a function

```csharp
public void NoFunction()
{
    int a = 1;

    for (int i = 0; i < Loops; i++)
    {
        int _ = a + i;
    }
}
```

### StaticMethod
Addition implemented as a static method

```csharp
public void StaticMethod()
{
    int a = 1;

    for (int i = 0; i < Loops; i++)
    {
        int _ = AddStatic(a, i);
    }
}

private static int AddStatic(int a, int b) => a + b;
```

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

    int a = 1;

    for (int i = 0; i < Loops; i++)
    {
        add(a, i);
    }
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
    
    int a = 1;

    for (int i = 0; i < Loops; i++)
    {
        add(a, i);
    }
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
    
    int a = 1;

    for (int i = 0; i < Loops; i++)
    {
        add(a, i);
    }
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
    
    int a = 1;

    for (int i = 0; i < Loops; i++)
    {
        add(a, i);
    }
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

    for (int i = 0; i < Loops; i++)
    {
        add(a, i);
    }
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
    
    int a = 1;

    for (int i = 0; i < Loops; i++)
    {
        add(a, i);
    }
}
```

### LoopDynamicMethodStatic
Creates a DynamicMethod, emits IL to create a loop executing a static delegate and creates a static delegate

```csharp
public void LoopDynamicMethodStatic()
{
    DynamicMethod loopDynamicMethod = new("AddLoop", returnType: null, new Type[] { typeof(int) });

    ILGenerator ILGenerator = loopDynamicMethod.GetILGenerator();
    ...
    
    TakesAnInt addLoop = (TakesAnInt)loopDynamicMethod.CreateDelegate(typeof(TakesAnInt));
    addLoop(Loops);
}
```

### LoopDynamicMethodInstance
Creates a DynamicMethod, emits IL to create a loop executing an instance delegate and creates an instance delegate

```csharp
public void LoopDynamicMethodStatic()
{
    DynamicMethod loopDynamicMethod = new("AddLoop", returnType: null, new Type[] { typeof(object), typeof(int) });

    ILGenerator ILGenerator = loopDynamicMethod.GetILGenerator();
    ...
    
    TakesAnInt addLoop = (TakesAnInt)loopDynamicMethod.CreateDelegate(typeof(TakesAnInt));
    addLoop(Loops);
}
```

### LoopDynamicMethodEmbedded
Creates a DynamicMethod, emits IL to create a loop implementing the addition (does not call a delegate) and creates an instance delegate

```csharp
public void LoopDynamicMethodEmbedded()
{
    DynamicMethod loopDynamicMethod = new("AddLoop", returnType: null, new Type[] { typeof(object), typeof(int) });

    ILGenerator ILGenerator = loopDynamicMethod.GetILGenerator();
    ...
    
    TakesAnInt addLoop = (TakesAnInt)loopDynamicMethod.CreateDelegate(typeof(TakesAnInt));
    addLoop(Loops);
}
```

### LoopExpressionTreeStatic
Builds a loop expression tree invoking a static delegate and creates a delegate

```csharp
public void LoopExpressionTreeStatic()
{
    // Build expression tree invoking static delegate
    ...
    
    LambdaExpression lambdaExpression = Expression.Lambda(typeof(TakesAnInt), ...);
    
    TakesAnInt addLoop = (TakesAnInt)lambdaExpression.Compile();
    addLoop(Loops);
}
```

### LoopExpressionTreeInstance
Builds a loop expression tree invoking an instance delegate and creates a delegate

```csharp
public void LoopExpressionTreeInstance()
{
    // Build expression tree invoking instance delegate
    ...
    
    LambdaExpression lambdaExpression = Expression.Lambda(typeof(TakesAnInt), ...);
    
    TakesAnInt addLoop = (TakesAnInt)lambdaExpression.Compile();
    addLoop(Loops);
}
```

### LoopExpressionTreeEmbedded
Builds a loop expression tree embedding the addition and creates a delegate

```csharp
public void LoopExpressionTreeInstance()
{
    // Build expression tree embedding the addition
    ...
    
    LambdaExpression lambdaExpression = Expression.Lambda(typeof(TakesAnInt), ...);
    
    TakesAnInt addLoop = (TakesAnInt)lambdaExpression.Compile();
    addLoop(Loops);
}
```

## Results
The idea here is to determine the number of loops before which the compilation is the main driver for the compilation/duration duration.

|                        Method |              Runtime |  Loops |       Mean |     StdDev | Ratio |   Gen0 |   Gen1 | Allocated | Alloc Ratio |
|------------------------------ |--------------------- |------- |-----------:|-----------:|------:|-------:|-------:|----------:|------------:|
|                    NoFunction |             .NET 7.0 |  10000 |   2.590 us |  0.0023 us |  1.00 |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 |  10000 |   2.591 us |  0.0030 us |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 |  10000 |  31.172 us |  0.0335 us | 12.04 | 0.0610 |      - |    1152 B |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 |  10000 |  25.216 us |  0.1596 us |  9.74 | 0.0916 | 0.0610 |    1167 B |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 |  10000 | 231.111 us | 39.5919 us |     ? | 0.3662 | 0.1221 |    5296 B |           ? |
|   DynamicTypeInstanceDelegate |             .NET 7.0 |  10000 | 233.241 us | 40.5950 us |     ? | 0.3662 | 0.1221 |    5624 B |           ? |
|           ExpressionTreeBuilt |             .NET 7.0 |  10000 |  26.043 us |  0.1523 us | 10.06 | 0.3662 | 0.3357 |    4783 B |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 |  10000 |  37.517 us |  0.1103 us | 14.49 | 0.3662 | 0.3052 |    4847 B |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 |  10000 |  64.609 us |  0.4049 us | 24.95 | 0.1221 |      - |    2715 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 |  10000 |  59.803 us |  0.1781 us | 23.09 | 0.1221 |      - |    2731 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |  10000 |  22.109 us |  0.1836 us |  8.54 | 0.0916 | 0.0610 |    1467 B |          NA |
|      LoopExpressionTreeStatic |             .NET 7.0 |  10000 |  91.214 us |  0.2870 us | 35.22 | 1.0986 | 0.9766 |   14359 B |          NA |
|    LoopExpressionTreeInstance |             .NET 7.0 |  10000 |  91.353 us |  0.7275 us | 35.27 | 1.0986 | 0.9766 |   14359 B |          NA |
|    LoopExpressionTreeEmbedded |             .NET 7.0 |  10000 |  32.250 us |  0.1487 us | 12.45 | 0.9155 | 0.8545 |   11911 B |          NA |
|                               |                      |        |            |            |       |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 |  10000 |   2.591 us |  0.0028 us |  1.00 |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 |  10000 |   2.592 us |  0.0032 us |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |  10000 |  32.479 us |  0.0645 us | 12.54 | 0.1831 | 0.0610 |    1228 B |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |  10000 |  25.472 us |  0.0538 us |  9.83 | 0.1831 | 0.0916 |    1234 B |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 |  10000 | 174.410 us | 18.3008 us |     ? | 0.9766 | 0.2441 |    6460 B |           ? |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 |  10000 | 176.438 us | 17.3806 us |     ? | 0.9766 | 0.2441 |    6700 B |           ? |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |  10000 |  31.407 us |  0.2333 us | 12.12 | 0.7935 | 0.3662 |    5315 B |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |  10000 |  40.163 us |  0.0979 us | 15.50 | 0.7935 | 0.3662 |    5041 B |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |  10000 | 134.368 us |  0.6367 us | 51.87 | 0.2441 | 0.2441 |    2933 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |  10000 | 128.878 us |  1.1306 us | 49.75 | 0.2441 | 0.2441 |    2949 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |  10000 |  28.223 us |  0.1229 us | 10.89 | 0.2441 | 0.1221 |    1605 B |          NA |
|      LoopExpressionTreeStatic | .NET Framework 4.7.2 |  10000 | 208.126 us |  0.9711 us | 80.33 | 2.4414 | 1.2207 |   16530 B |          NA |
|    LoopExpressionTreeInstance | .NET Framework 4.7.2 |  10000 | 208.557 us |  0.6164 us | 80.50 | 2.4414 | 1.2207 |   16530 B |          NA |
|    LoopExpressionTreeEmbedded | .NET Framework 4.7.2 |  10000 |  53.238 us |  0.1432 us | 20.55 | 2.1362 | 1.0376 |   13463 B |          NA |
|                               |                      |        |            |            |       |        |        |           |             |
|                    NoFunction |             .NET 7.0 |  50000 |  12.924 us |  0.0110 us |  1.00 |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 |  50000 |  12.922 us |  0.0132 us |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 |  50000 | 110.981 us |  0.1792 us |  8.59 |      - |      - |    1136 B |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 |  50000 |  85.392 us |  0.0914 us |  6.61 |      - |      - |    1144 B |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 |  50000 | 232.713 us | 17.4130 us |     ? | 0.2441 |      - |    5312 B |           ? |
|   DynamicTypeInstanceDelegate |             .NET 7.0 |  50000 | 210.029 us | 17.5868 us |     ? | 0.2441 |      - |    5640 B |           ? |
|           ExpressionTreeBuilt |             .NET 7.0 |  50000 |  76.373 us |  0.1793 us |  5.91 | 0.3662 | 0.2441 |    4783 B |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 |  50000 | 127.637 us |  0.3445 us |  9.88 | 0.2441 |      - |    4836 B |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 |  50000 | 147.928 us |  0.4673 us | 11.45 |      - |      - |    2688 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 |  50000 | 122.085 us |  0.2587 us |  9.45 |      - |      - |    2704 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |  50000 |  30.786 us |  0.0742 us |  2.38 | 0.0610 |      - |    1461 B |          NA |
|      LoopExpressionTreeStatic |             .NET 7.0 |  50000 | 154.599 us |  0.7531 us | 11.96 | 0.9766 | 0.7324 |   14353 B |          NA |
|    LoopExpressionTreeInstance |             .NET 7.0 |  50000 | 154.812 us |  0.6242 us | 11.98 | 0.9766 | 0.7324 |   14350 B |          NA |
|    LoopExpressionTreeEmbedded |             .NET 7.0 |  50000 |  44.302 us |  0.3712 us |  3.43 | 0.9155 | 0.8545 |   11911 B |          NA |
|                               |                      |        |            |            |       |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 |  50000 |  12.928 us |  0.0028 us |  1.00 |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 |  50000 |  12.937 us |  0.0105 us |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |  50000 | 114.868 us |  0.1152 us |  8.88 | 0.1221 | 0.1221 |    1220 B |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |  50000 |  76.850 us |  0.2117 us |  5.94 | 0.1221 | 0.1221 |    1228 B |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 |  50000 | 256.962 us | 16.9868 us |     ? | 0.9766 | 0.2441 |    6460 B |           ? |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 |  50000 | 233.435 us | 15.1694 us |     ? | 0.9766 | 0.2441 |    6700 B |           ? |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |  50000 |  81.581 us |  0.4177 us |  6.32 | 0.7324 | 0.3662 |    5314 B |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |  50000 | 130.248 us |  0.3329 us | 10.07 | 0.7324 | 0.2441 |    5040 B |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |  50000 | 367.354 us |  0.7542 us | 28.42 |      - |      - |    2908 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |  50000 | 331.895 us |  1.0627 us | 25.68 |      - |      - |    2924 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |  50000 |  38.399 us |  0.1151 us |  2.97 | 0.2441 | 0.1221 |    1605 B |          NA |
|      LoopExpressionTreeStatic | .NET Framework 4.7.2 |  50000 | 417.369 us |  1.2185 us | 32.28 | 2.4414 | 0.9766 |   16532 B |          NA |
|    LoopExpressionTreeInstance | .NET Framework 4.7.2 |  50000 | 417.783 us |  1.6590 us | 32.32 | 2.4414 | 0.9766 |   16532 B |          NA |
|    LoopExpressionTreeEmbedded | .NET Framework 4.7.2 |  50000 |  63.953 us |  0.3889 us |  4.94 | 2.0752 | 0.9766 |   13462 B |          NA |
|                               |                      |        |            |            |       |        |        |           |             |
|                    NoFunction |             .NET 7.0 | 100000 |  25.835 us |  0.0247 us |  1.00 |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 | 100000 |  25.837 us |  0.0208 us |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 | 100000 | 239.899 us |  0.1129 us |  9.29 |      - |      - |    1136 B |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 | 100000 | 165.332 us |  3.9539 us |  6.45 |      - |      - |    1144 B |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 | 100000 | 307.918 us |  7.0990 us |     ? |      - |      - |    5281 B |           ? |
|   DynamicTypeInstanceDelegate |             .NET 7.0 | 100000 | 291.438 us | 17.4916 us |     ? | 0.2441 |      - |    5640 B |           ? |
|           ExpressionTreeBuilt |             .NET 7.0 | 100000 | 140.859 us |  0.3458 us |  5.45 | 0.2441 |      - |    4776 B |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 | 100000 | 243.431 us |  0.3777 us |  9.42 |      - |      - |    4816 B |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 | 100000 | 251.946 us |  0.6825 us |  9.75 |      - |      - |    2688 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 | 100000 | 201.316 us |  0.3097 us |  7.79 |      - |      - |    2704 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 | 100000 |  43.700 us |  0.0564 us |  1.69 | 0.0610 |      - |    1461 B |          NA |
|      LoopExpressionTreeStatic |             .NET 7.0 | 100000 | 230.635 us |  0.4741 us |  8.93 | 0.9766 | 0.4883 |   14346 B |          NA |
|    LoopExpressionTreeInstance |             .NET 7.0 | 100000 | 229.170 us |  0.4551 us |  8.87 | 0.9766 | 0.4883 |   14346 B |          NA |
|    LoopExpressionTreeEmbedded |             .NET 7.0 | 100000 |  55.844 us |  0.2229 us |  2.16 | 0.9155 | 0.8545 |   11911 B |          NA |
|                               |                      |        |            |            |       |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 | 100000 |  25.878 us |  0.0976 us |  1.00 |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 | 100000 |  25.838 us |  0.0247 us |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 | 100000 | 217.882 us |  0.2854 us |  8.42 |      - |      - |    1204 B |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 | 100000 | 141.166 us |  0.1973 us |  5.46 |      - |      - |    1212 B |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 | 100000 | 322.532 us |  7.3538 us |     ? | 0.9766 |      - |    6428 B |           ? |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 | 100000 | 295.384 us | 16.9754 us |     ? | 0.9766 | 0.2441 |    6700 B |           ? |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 | 100000 | 141.573 us |  1.1278 us |  5.47 | 0.7324 | 0.2441 |    5314 B |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 | 100000 | 243.151 us |  0.7500 us |  9.40 | 0.7324 | 0.2441 |    5040 B |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 | 100000 | 661.640 us |  1.4281 us | 25.57 |      - |      - |    2912 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 | 100000 | 587.697 us |  1.2416 us | 22.71 |      - |      - |    2928 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 | 100000 |  51.431 us |  0.1108 us |  1.99 | 0.2441 | 0.1221 |    1605 B |          NA |
|      LoopExpressionTreeStatic | .NET Framework 4.7.2 | 100000 | 673.718 us |  1.9624 us | 26.03 | 1.9531 | 0.9766 |   16520 B |          NA |
|    LoopExpressionTreeInstance | .NET Framework 4.7.2 | 100000 | 677.891 us |  2.4569 us | 26.20 | 1.9531 | 0.9766 |   16520 B |          NA |
|    LoopExpressionTreeEmbedded | .NET Framework 4.7.2 | 100000 |  77.112 us |  0.3262 us |  2.98 | 2.0752 | 0.9766 |   13462 B |          NA |

## Conclusions
Focusing on .NET 7.0 and the best case for each implementation:
(C + E = compilation + execution, E = execution)

### 10K loops
|                        Method | C + E Mean |   E Mean | C + E / E | Allocated |
|------------------------------ |-----------:|---------:|----------:|----------:|
|        NoFunction (base line) |   2.590 us |   2.6 us |         - |         - |
| DynamicMethodInstanceDelegate |  25.216 us |  15.5 us |       1.7 |    1167 B |
|   DynamicTypeInstanceDelegate | 233.241 us |  18.2 us |        13 |    5624 B |
|           ExpressionTreeBuilt |  26.043 us |  10.3 us |       2.6 |    4783 B |
|     LoopDynamicMethodEmbedded |  22.109 us |   2.6 us |       8.5 |    1467 B |
|    LoopExpressionTreeEmbedded |  32.250 us |   2.7 us |        12 |   11911 B |

LoopDynamicMethodEmbedded is the fastest although it allocates slightly more than the reference dynamic method (DynamicMethodInstanceDelegate).

### 50K loops
|                        Method |       Mean | Allocated |
|------------------------------ |-----------:|----------:|
|        NoFunction (base line) |  12.924 us |         - |
| DynamicMethodInstanceDelegate |  85.392 us |    1144 B |
|   DynamicTypeInstanceDelegate | 210.029 us |    5640 B |
|           ExpressionTreeBuilt |  76.373 us |    4783 B |
|     LoopDynamicMethodEmbedded |  30.786 us |    1461 B |
|    LoopExpressionTreeEmbedded |  32.250 us |   11911 B |

LoopDynamicMethodEmbedded is now 5* faster than DynamicMethodInstanceDelegate for the same allocation.

### 100K loops
|                        Method |       Mean |    E Mean | C + E / E | Allocated |
|------------------------------ |-----------:|----------:|----------:|----------:|
|        NoFunction (base line) |  25.835 us |   25.8 us |         - |         - |
| DynamicMethodInstanceDelegate | 165.332 us |  135.3 us |       1.2 |    1144 B |
|   DynamicTypeInstanceDelegate | 291.438 us |  134.6 us |       2.1 |    5640 B |
|           ExpressionTreeBuilt | 140.859 us |  127.9 us |       1.1 |    4776 B |
|     LoopDynamicMethodEmbedded |  43.700 us |   25.8 us |       1.7 |    1461 B |
|    LoopExpressionTreeEmbedded |  55.844 us |   25.9 us |       2.1 |   11911 B |

LoopDynamicMethodEmbedded is still 5* faster than DynamicMethodInstanceDelegate for the same allocation.

- Performance .NET 7.0 gets better vs .NET Framework 4.7.2 as loops increases
