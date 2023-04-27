using System.Collections;

namespace DotNetBenchmarking.HashCode;

/// <summary>
/// Custom hash code calculator implementation.
/// </summary>
public struct MyHashCode
{
    private const int _seed = 5923;
    private const int _multiplier = 7481;

    /// <summary>
    /// Builds a new hash code.
    /// </summary>
    /// <returns>The built hash code.</returns>
    public static MyHashCode Build() => new(_seed);

    /// <summary>
    /// Constructor from a hash value.
    /// </summary>
    /// <param name="value">Hash value.</param>
    private MyHashCode(int value)
    {
        _value = value;
    }
    private readonly int _value;

    /// <summary>
    /// Builds a new hash code and initializes it from a hash code source.
    /// </summary>
    /// <param name="hashCodeSource">Item from which a hash code can be extracted (using GetHashCode).</param>
    public MyHashCode(object? hashCodeSource)
    {
        int sourceHashCode = GetHashCode(hashCodeSource);
        _value = AddValue(_seed, sourceHashCode);
    }

    /// <summary>
    /// Returns the hash code for a given hash code source (0 if the source is null).
    /// </summary>
    /// <param name="hashCodeSource">Item from which a hash code can be extracted (using GetHashCode).</param>
    /// <returns>The hash code.</returns>
    private static int GetHashCode(object? hashCodeSource) =>

        (hashCodeSource != null) ? hashCodeSource.GetHashCode()
                                 : 0;

    /// <summary>
    /// Adds a new hash value to a hash code.
    /// </summary>
    /// <param name="currentValue">Current hash value.</param>
    /// <param name="valueToAdd">Value to add.</param>
    /// <returns>The new hash value.</returns>
    private static int AddValue(int currentValue, int valueToAdd)
    {
        unchecked
        {
            return (currentValue * _multiplier) + valueToAdd;
        }
    }

    /// <summary>
    /// Builds a new hash code adding an object's hash code.
    /// </summary>
    /// <param name="hashCode">Hash code to which the object's hash code has to be added.</param>
    /// <param name="hashCodeSource">Item which hash code to add.</param>
    /// <returns>The new hash code instance.</returns>
    public static MyHashCode operator +(MyHashCode hashCode, object? hashCodeSource)
    {
        int sourceHashCode = GetHashCode(hashCodeSource);
        int newHashValue = AddValue(hashCode._value, sourceHashCode);

        return new MyHashCode(newHashValue);
    }

    /// <summary>
    /// Builds a new hash code adding multiple objects' hash code.
    /// </summary>
    /// <param name="collection">Collection of items which hash code to add.</param>
    /// <returns>The updated hash instance.</returns>
    public MyHashCode AddRange(IEnumerable collection)
    {
        int newHashValue = _value;

        foreach (object hashCodeSource in collection)
        {
            int sourceHashCode = GetHashCode(hashCodeSource);
            newHashValue = AddValue(_value, sourceHashCode);
        }

        return new MyHashCode(newHashValue);
    }

    /// <summary>
    /// Implicit cast operator to int.
    /// </summary>
    /// <param name="hashCode">Hash code to convert.</param>
    public static implicit operator int(MyHashCode hashCode) => hashCode._value;

    #region IEquatable implementation
    public bool Equals(MyHashCode? other) =>
        (other is not null) &&
        (((int)other) == ((int)this));

    public override bool Equals(object? other) => (other is MyHashCode) && Equals(other);

    public override int GetHashCode() => _value;

    public static bool operator ==(MyHashCode left, MyHashCode right) => left.Equals(right);

    public static bool operator !=(MyHashCode left, MyHashCode right) => !left.Equals(right);
    #endregion
}
