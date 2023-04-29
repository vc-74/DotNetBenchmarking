namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled;

/// <summary>
/// Type of delegate instance.
/// </summary>
public enum DelegateInstanceType
{
    /// <summary>
    /// Delegate instance with a null target.
    /// </summary>
    Static,

    /// <summary>
    /// Delegate instance with a non-null target.
    /// </summary>
    Instance
}
