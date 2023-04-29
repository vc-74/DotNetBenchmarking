# Function call benchmarks
This folder contains benchmarks comparing different methods of calling a function. 
Here, function is used as a general concept which includes instance methods, static methods, delegates... in the .net world.

The runtimes the benchmarks are executed on are:
- .NET Framework 4.7.2
- .NET 7

## The case
These tests are based on a very simple case equivalent to:

```csharp
int a = 2;

for (int i = 0; i < loops; i++)
{
	int sum = a + i;
}
```

## TL;DR
- JIT inlining of static and non-virtual instance methods is very effective
- When creating delegates, prefer instance delegates over static delegates if they are going to be intensively invoked ([more details](https://stackoverflow.com/a/42187448/446279))
For statically compiled code, this can easily be achieved by calling the static method from a lambda:
```csharp
static int Add(int a, int b) => a + b;

delegate int Adder(int a, int b);

Adder adder = (int a, int b) => Add(a, b);
```

For dynamically compiled code, although `DynamicMethod` builds static methods, the delegate created by `DynamicMethod.CreateDelegate` can be an instance delegate:

```csharp
private static readonly object _delegateTarget = new();

delegate int TakesTwoIntsReturnsInt(int a, int b);

DynamicMethod addDynamicMethod = new("Add",
    MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard,
    returnType: typeof(int), parameterTypes: new Type[] { typeof(object), typeof(int), typeof(int) },
	owner: ..., skipVisibility: true;

...

return (TakesTwoIntsReturnsInt)addDynamicMethod.CreateDelegate(typeof(TakesTwoIntsReturnsInt), _delegateTarget);
```

- When building dynamic functions, embedding code is much faster than invoking a delegate

## Terminology
The terminology around delegates is not always clear, especially the distinction between delegate type and delegate instance. 
In this document, unless explicitly stated otherwise, delegate refers to delegate instance. Also, the following terms will be used:

|                          Term |                               Definition |
|------------------------------ |----------------------------------------- |
|               Static delegate | Delegate instance with a null target     |
|             Instance delegate | Delegate instance with a non-null target |

## Benchmarks detail
The different functions tested can be either:

- [Statically compiled](StaticallyCompiled): compiled at 'compile time', early binding. 
The benchmarks for such cases measure only the execution since no compilation is needed. 
The different cases cover:
	- Class methods: static, instance and virtual
	- Local functions: static, instance and capturing
	- Lambdas: non-capturing and capturing
	- Delegates: on static, instance and virtual methods

- [Dynamically compiled](DynamicallyCompiled): compiled at execution time, late binding. 
The benchmark for such cases measure [compilation](DynamicallyCompiled/Compilation), [execution](DynamicallyCompiled/Execution) and [compilation + execution](DynamicallyCompiled/CompilationAndExecution).
The different cases cover:
	- DynamicMethod: static and instance delegates
	- TyperBuilder: static and instance delegates on emitted methods
	- Expresion tree: explicit creation and compiler generated from a lambda function
	- DynamicMethod implementating the loop: calling a static delegate, and instance delegate or embedding the addition

## Delegates performance
Static delegates execution require some arguments reshuffling ([details](https://stackoverflow.com/a/42187448/446279)) and are slower, however, their instances can be cached during the first invocation and reused during the next invocation.
Instance delegates do not require arguments reshuffling and are faster than static delegates, however, instances cannot be cached and have to be recreated.
Lambdas are the best of both worlds, the compiler generates a class with a delegate field, instantiates an instance during the first invocation and caches it. 
This instance is used as the delegate's target. Lambdas are therefore fast and don't allocate.

| Delegate type | Instantiation                    | Target                                       |
|-------------- |--------------------------------- |--------------------------------------------- |
| Instance      | For each invocation              | Not null (instance)                          |
| Static        | Only during the first invocation | Null                                         |
| Lambda        | Only during the first invocation | Not null (compiler generated class instance) |

### Details
In the following example:

```csharp
public delegate int TakesTwoIntsReturnsInt(int a, int b);

public void CallStatic()
{
    TakesTwoIntsReturnsInt add = AddStatic;
    add(1, 2);
}

public static int AddStatic(int a, int b) => a + b;
```

`CallStatic` compiles to:

```IL
.method public hidebysig instance void  CallStatic() cil managed
{
  // Code size       36 (0x24)
  .maxstack  8

  // Get the singleton instance
  IL_0000:  ldsfld     class IL.Program/TakesTwoIntsReturnsInt IL.Program/'<>O'::'<0>__AddStatic'
  IL_0005:  dup
  IL_0006:  brtrue.s   IL_001b
  // The singleton instance was null, instantiate it...
  IL_0008:  pop
  IL_0009:  ldnull
  IL_000a:  ldftn      int32 IL.Program::AddStatic(int32,
                                                   int32)
  IL_0010:  newobj     instance void IL.Program/TakesTwoIntsReturnsInt::.ctor(object,
                                                                              native int)
  IL_0015:  dup
  // and cache it
  IL_0016:  stsfld     class IL.Program/TakesTwoIntsReturnsInt IL.Program/'<>O'::'<0>__AddStatic'
  // The singleton instance is on the top of the stack
  IL_001b:  ldc.i4.1
  IL_001c:  ldc.i4.2
  IL_001d:  callvirt   instance int32 IL.Program/TakesTwoIntsReturnsInt::Invoke(int32,
                                                                                int32)
  IL_0022:  pop
  IL_0023:  ret
}
```

whereas

```csharp
public delegate int TakesTwoIntsReturnsInt(int a, int b);

public void CallInstance()
{
    TakesTwoIntsReturnsInt add = AddInstance;
    add(1, 2);
}

public int AddInstance(int a, int b) => a + b;
```

compiles to:

```IL
.method public hidebysig instance void  CallInstance() cil managed
{
  // Code size       21 (0x15)
  .maxstack  8
  // Always instantiate the delegate instance
  IL_0000:  ldarg.0
  IL_0001:  ldftn      instance int32 IL.Program::AddInstance(int32,
                                                              int32)
  IL_0007:  newobj     instance void IL.Program/TakesTwoIntsReturnsInt::.ctor(object,
                                                                              native int)
  IL_000c:  ldc.i4.1
  IL_000d:  ldc.i4.2
  IL_000e:  callvirt   instance int32 IL.Program/TakesTwoIntsReturnsInt::Invoke(int32,
                                                                                int32)
  IL_0013:  pop
  IL_0014:  ret
} // end of method Program::CallInstance
```
