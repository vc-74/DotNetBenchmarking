// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

#if NET7_0
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmark methods must not be static", Scope = "type", Target = "~T:DotNetBenchmarking.ExpressionException")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmark methods must not be static", Scope = "type", Target = "~T:DotNetBenchmarking.MultipleOutputParameters")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmark methods must not be static", Scope = "type", Target = "~T:DotNetBenchmarking.HashCode.HashCode")]
#endif

[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmark methods must not be static", Scope = "type", Target = "~T:DotNetBenchmarking.Regex.Regex")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmark methods must not be static", Scope = "type", Target = "~T:DotNetBenchmarking.PropertyVsField")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmark methods must not be static", Scope = "type", Target = "~T:DotNetBenchmarking.FunctionCall.StaticallyCompiled.Execution")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmark methods must not be static", Scope = "type", Target = "~T:DotNetBenchmarking.FunctionCall.DynamicallyCompiled.Compilation")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmark methods must not be static", Scope = "type", Target = "~T:DotNetBenchmarking.FunctionCall.DynamicallyCompiled.Execution")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Benchmark methods must not be static", Scope = "type", Target = "~T:DotNetBenchmarking.FunctionCall.DynamicallyCompiled.CompilationAndExecution")]
