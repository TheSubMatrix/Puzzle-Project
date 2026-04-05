using UnityEngine;

public static class FloatExtensions
{
    /// <summary>
    /// Maps a value from one range to another
    /// </summary>
    /// <param name="value">The value you want to remap</param>
    /// <param name="from1">The lower bound of the starting range</param>
    /// <param name="to1">The upper bound of the starting range</param>
    /// <param name="from2">The lower bound of the ending range</param>
    /// <param name="to2">The upper bound of the ending range</param>
    /// <returns>The value remapped from one range to another as a float</returns>
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    /// <summary>
    /// Truncates a float to a certain number of decimal places
    /// </summary>
    /// <param name="value">The value to truncate</param>
    /// <param name="digits">The number of digits to truncate this value to</param>
    /// <returns>The truncated value</returns>
    public static float Truncate(this float value, int digits)
    {
        return Mathf.Round(value * Mathf.Pow(10, digits)) / Mathf.Pow(10, digits);
    }
}
