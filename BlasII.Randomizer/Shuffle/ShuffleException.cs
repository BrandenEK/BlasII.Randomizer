using System;

namespace BlasII.Randomizer.Shuffle;

/// <summary>
/// An error that occurs during the Shuffle process
/// </summary>
public class ShuffleException(string message) : Exception(message) { }
