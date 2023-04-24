# Function call benchmarks
This folder contains benchmarks comparing different methods of calling a function.
The functions tested can be either statically compiled (compiled at compile time, early binding) or dynamically compiled (compiled at execution time, late binding).

The runtimes the benchmarks are executed on are:
- .net Framework 4.7.2
- .net core 7

There are 2 cases:

## Addition
A function taking two integers in parameter and returning their sum:
- Using statically compiled functions (instance, static, local...)
- Using a statically compiled delegate (static method, instance method...)
- Using a dynamically compiled delegate (IL, expression tree...)

## Addition loop
A function taking a number of loops in parameter and executing as many additions:
- Using a dynamically compiled delegate (IL, expression tree...)
