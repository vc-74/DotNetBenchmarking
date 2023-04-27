using System.Collections;
using System.Reflection;

namespace DotNetBenchmarking.BitCollection;

/// <summary>
/// Custom bit array implementation.
/// </summary>
internal class MyBitArray
{
    public MyBitArray(int count)
    {
        _bitArray = new(count);
    }
    private readonly BitArray _bitArray;

    public int SetBitCount => _setBitCount;
    private int _setBitCount = 0;

    public bool Get(int index) => _bitArray[index];
    public void Set(int index, bool value)
    {
        if (!_bitArray[index])
        {
            _bitArray[index] = value;
            _setBitCount++;
        }
    }
    public void SetOptimized(int index, bool value) => _bitArray[index] = value;

    public bool NoneSet => (_setBitCount == 0);

    public bool AllSet => (_setBitCount == _bitArray.Count);

    public bool AllSetHack
    {
        get
        {
            const int _bitsPerInteger = 32;

            int[] _bitArrayInternalArray = (int[])_mArrayInfo.GetValue(_bitArray)!;

            bool result = true;

            int fullBytesCount = Math.DivRem(_bitArray.Count, _bitsPerInteger, out int lastBitsCount);

            for (int i = 0; i < fullBytesCount; i++)
            {
                if (_bitArrayInternalArray[i] != -1)
                {
                    // The full bytes bits are not all 1
                    return false;
                }
            }

            if (lastBitsCount != 0)
            {
                // Check last bytes
                int trueLastValue = (-1 << lastBitsCount) | _bitArrayInternalArray[^1];
                result = (trueLastValue == -1);
            }

            return result;
        }
    }

    private static readonly FieldInfo _mArrayInfo = typeof(BitArray).GetField("m_array", BindingFlags.Instance | BindingFlags.NonPublic)!;
}
