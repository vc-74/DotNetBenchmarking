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

## Terminology
|                          Term |                               Definition |
|------------------------------ |----------------------------------------- |
|               Static delegate | Delegate instance with a null target     |
|             Instance delegate | Delegate instance with a non-null target |

Note that an instance delegate can be created for a static method. 
This may seem confusing but is quite useful since instance delegates execute faster than static delegates ([more details](https://stackoverflow.com/a/42187448/446279)).

## TL;DR
- JIT inlining of static and non-virtual instance methods is very effective
- When creating delegates, prefer instance delegates (with a target) which are faster to execute ([more details](https://stackoverflow.com/a/42187448/446279))
- When building dynamic functions, embedding code is much faster than invoking a delegate

## Benchmarks detail
The different functions tested can be either:
- [Statically compiled](StaticallyCompiled): compiled at 'compile time', early binding. 
The benchmarks for such cases measure only the execution since there no compilation is needed. 
The different cases cover:
	- static, instance and virtual methods
	- static, instance and capturing local functions
	- non-capturing and capturing lambdas
	- delegates on static, instance and virtual methods
- [Dynamically compiled](DynamicallyCompiled): when the implementation is not known at compile time: compiled at execution time, late binding. 
The benchmark for such cases measure [compilation](DynamicallyCompiled/Compilation), [execution](DynamicallyCompiled/Execution) and [compilation + execution](DynamicallyCompiled/CompilationAndExecution).
The different cases cover:
	- static and instance delegates created from a DynamicMethod (IL)
	- static and instance delegates created from a dynamically created type's method (IL)
	- delegates created from an expression tree created explicitly or using a lambda function
	- delegates created from a DynamicMethod implementating the loop and calling a static delegate, and instance delegate or embedding the addition
