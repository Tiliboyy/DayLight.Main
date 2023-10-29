﻿// -----------------------------------------------------------------------
// <copyright file="ConnectingErrorEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordSync.Plugin.Network.EventArgs.Network;

/// <summary>
///     Contains all informations after the network thrown an exception while connecting data.
/// </summary>
public class ConnectingErrorEventArgs : ErrorEventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ConnectingErrorEventArgs" /> class.
    /// </summary>
    /// <param name="exception">
    ///     <inheritdoc cref="ErrorEventArgs.Exception" />
    /// </param>
    public ConnectingErrorEventArgs(Exception exception)
        : base(exception) { }
}
