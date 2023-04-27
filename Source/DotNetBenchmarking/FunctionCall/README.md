# Function call benchmarks
This folder contains benchmarks comparing different methods of calling a function. 
Here, function is used as a general concept which includes instance methods, static methods, delegates... in the .net world.
The functions tested can be either statically compiled (compiled at 'compile time', early binding) or dynamically compiled (compiled at execution time, late binding).

The runtimes the benchmarks are executed on are:
- .net Framework 4.7.2
- .net core 7

There are 2 cases, each focusing on a simple function:

## Addition
A function taking two integers in parameter and returning their sum, equivalent to:

```csharp
int sum = 1 + 1;
```

- Using statically compiled functions (instance, static, local, static method delegate, instance method delegate...)
- Using a dynamically compiled delegates (DynamicMethod, new type, expression tree...)

## Addition loop
A function executing the Addition function described above a certain number of times (loops), equivalent to:
```csharp
for (int i = 0; i < loops; i++)
{
	int sum = 1 + 1;
}
```

The sum implementation may be through a delegate, embedding IL code...
