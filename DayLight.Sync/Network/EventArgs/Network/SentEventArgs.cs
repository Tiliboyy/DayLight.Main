// -----------------------------------------------------------------------
// <copyright file="SentEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DayLight.Sync.Network.EventArgs.Network;

/// <summary>
///     Contains all informations after the network sent data.
/// </summary>
public class SentEventArgs : System.EventArgs
{
    public SentEventArgs(string data, int length)
    {
        Data = data;
        Length = length;
    }
    /// <summary>
    ///     Initializes a new instance of the <see cref="SentEventArgs" /> class.
    /// </summary>
    /// <param name="data">
    ///     <inheritdoc cref="ReceivedEventArgs.SerilzedData" />
    /// </param>
    /// <param name="length">
    ///     <inheritdoc cref="ReceivedEventArgs.Length" />
    /// </param>
    public string Data { get; set; }
    public int Length { get; set; }
}
