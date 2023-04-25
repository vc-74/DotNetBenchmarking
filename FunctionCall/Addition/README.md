# Add function call benchmarks
This folder contains benchmarks comparing different methods of calling a function that takes 2 integers in parameter an returns their sum, equivalent to:

```csharp
int loops = Loops;
for (int a = 0; a < loops; a++)
{
    const int b = 2;
    int c = a + b;
}
```

The functions tested can be either statically compiled (compiled at compile time, early binding) or dynamically compiled (compiled at execution time, late binding).

The runtimes the benchmarks are executed on are:
- .net Framework 4.7.2
- .net core 7
