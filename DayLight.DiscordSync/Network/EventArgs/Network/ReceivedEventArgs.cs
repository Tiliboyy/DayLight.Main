// -----------------------------------------------------------------------
// <copyright file="ReceivedEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using DayLight.Dependencys.Communication;
using DayLight.Dependencys.Communication.Enums;

namespace DiscordSync.Plugin.Network.EventArgs.Network;

/// <summary>
///     Contains all informations after the network received data.
/// </summary>
public class ReceivedEventArgs : System.EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ReceivedEventArgs" /> class.
    /// </summary>
    /// <param name="data">
    ///     <inheritdoc cref="SerilzedData" />
    /// </param>
    /// <param name="length">
    ///     <inheritdoc cref="Length" />
    /// </param>
    public ReceivedEventArgs(BotRequester data, int length)
    {
        SerilzedData = data.DataBR;
        UserID = data.UserID;
        Type = data.Type;
        BotRequester = data;
        Length = length;
    }
    public BotRequester BotRequester { get; set; }
    public MessageType Type { get; set; }
    public ulong UserID { get; set; }
    public string SerilzedData { get; private set; }

    
    public T? GetData<T>()
    {
        try
        {
            return BotRequester.GetData<T>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return default;
        }
    }
    public int Length { get; }
}
