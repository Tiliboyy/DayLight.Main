// -----------------------------------------------------------------------
// <copyright file="ErrorEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DayLight.Sync.Network.EventArgs.Network;

/// <summary>
///     Contains all informations after the network thrown an exception.
/// </summary>
public class ErrorEventArgs : System.EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ErrorEventArgs" /> class.
    /// </summary>
    /// <param name="exception">
    ///     <inheritdoc cref="Exception" />
    /// </param>
    public ErrorEventArgs(Exception exception)
    {
        Exception = exception;
    }

    /// <summary>
    ///     Gets the thrown exception.
    /// </summary>
    public Exception Exception { get; }
}
