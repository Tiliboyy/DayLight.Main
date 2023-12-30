// -----------------------------------------------------------------------
// <copyright file="ReceivedFullEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using DayLight.Dependencys.Models.Communication;

namespace DiscordSync.Plugin.Network.EventArgs.Network;

/// <summary>
///     Contains all informations after the network received full data.
/// </summary>
public class ReceivedFullEventArgs : ReceivedEventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ReceivedFullEventArgs" /> class.
    /// </summary>
    /// <param name="data">
    ///     <inheritdoc cref="ReceivedEventArgs.SerilzedData" />
    /// </param>
    /// <param name="length">
    ///     <inheritdoc cref="ReceivedEventArgs.Length" />
    /// </param>
    public ReceivedFullEventArgs(BotMessage data, int length)
        : base(data, length) { }
}
