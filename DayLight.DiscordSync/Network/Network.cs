using DayLight.DiscordSync.Dependencys.Communication;
using DiscordSync.Plugin.Network.EventArgs.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PluginAPI.Core;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DiscordSync.Plugin.Network;

/// <summary>
///     A client that sends JSON serialized strings to a connected server.
/// </summary>
public sealed class Network : IDisposable
{
    /// <summary>
    ///     The reception buffer length.
    /// </summary>
    public const int ReceptionBuffer = 256;

    private bool canSend;

    private bool isDisposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Network" /> class.
    /// </summary>
    /// <param name="ipAddress">The remote server IP address.</param>
    /// <param name="port">The remote server IP port.</param>
    /// <param name="reconnectionInterval">The reconnection interval.</param>
    public Network(string ipAddress, ushort port, TimeSpan reconnectionInterval)
        : this(new IPEndPoint(IPAddress.Parse(ipAddress), port), reconnectionInterval) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Network" /> class.
    /// </summary>
    /// <param name="ipEndPoint">The remote server IP address and port.</param>
    /// <param name="reconnectionInterval">
    ///     <inheritdoc cref="ReconnectionInterval" />
    /// </param>
    public Network(IPEndPoint ipEndPoint, TimeSpan reconnectionInterval)
    {
        IPEndPoint = ipEndPoint;
        ReconnectionInterval = reconnectionInterval;
    }

    /// <summary>
    ///     Gets the active <see cref="System.Net.Sockets.TcpClient" /> instance.
    /// </summary>
    public TcpClient? TcpClient { get; private set; }

    /// <summary>
    ///     Gets the IP end point to connect with.
    /// </summary>
    public IPEndPoint IPEndPoint { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether the <see cref="TcpClient" /> is connected or not.
    /// </summary>
    public bool IsConnected => TcpClient?.Connected ?? false;

    /// <summary>
    ///     Gets the reconnection interval.
    /// </summary>
    public TimeSpan ReconnectionInterval { get; private set; }
    public static JsonSerializerSettings JsonSerializerSettings { get; } = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        TypeNameHandling = TypeNameHandling.Objects
    };

    /// <summary>
    ///     Releases all resources.
    /// </summary>
    public void Dispose() => Dispose(true);

    /// <summary>
    ///     Finalizes an instance of the <see cref="Network" /> class.
    /// </summary>
    ~Network()
    {
        Dispose(false);
    }

    /// <summary>
    ///     Invoked after network received partial data.
    /// </summary>
    public event EventHandler<ReceivedPartialEventArgs> ReceivedPartial;

    /// <summary>
    ///     Invoked after network received full data.
    /// </summary>
    public event EventHandler<ReceivedFullEventArgs> ReceivedFull;

    /// <summary>
    ///     Invoked after the network thrown an exception while sending data.
    /// </summary>
    public event EventHandler<SendingErrorEventArgs> SendingError;

    /// <summary>
    ///     Invoked after the network thrown an exception while receiving data.
    /// </summary>
    public event EventHandler<ReceivingErrorEventArgs> ReceivingError;
    /// <summary>
    ///     Invoked after network sent data.
    /// </summary>
    public event EventHandler<SentEventArgs> Sent;

    /// <summary>
    ///     Invoked before the network connects to the server.
    /// </summary>
    public event EventHandler<ConnectingEventArgs> Connecting;

    /// <summary>
    ///     Invoked after successfully connecting to the server.
    /// </summary>
    public event EventHandler<ConnectingErrorEventArgs> ConnectingError;

    /// <summary>
    ///     Invoked after the network successfully connects to the server.
    /// </summary>
    public event EventHandler Connected;

    /// <summary>
    ///     Invoked after the network thrown an exception while updating the connection.
    /// </summary>
    public event EventHandler<UpdatingConnectionErrorEventArgs> UpdatingConnectionError;

    /// <summary>
    ///     Invoked after the network termination.
    /// </summary>
    public event EventHandler<TerminatedEventArgs> Terminated;

    /// <summary>
    ///     Starts the <see cref="TcpClient" />.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="Task" /> cancellation token.</param>
    /// <returns>Returns the <see cref="Network" /> <see cref="Task" />.</returns>
    public async Task Start(CancellationTokenSource cancellationToken)
    {
        if (isDisposed)
            throw new ObjectDisposedException(GetType().FullName);

        await Update(cancellationToken).ContinueWith(task => OnTerminated(this, new TerminatedEventArgs(task))).ConfigureAwait(false);
    }

    /// <summary>
    ///     Close the <see cref="TcpClient" /> and the <see cref="Network" />.
    /// </summary>
    public void Close() => Dispose();

    /// <summary>
    ///     Sends data async to the server.
    /// </summary>
    /// <typeparam name="T">he data type.</typeparam>
    /// <param name="data">The data to be sent.</param>
    /// <returns>Returns the <see cref="ValueTask" />.</returns>
    public async ValueTask SendAsync(PluginSender data)
    {
        await SendAsync(data, CancellationToken.None);
    }

    /// <summary>
    ///     Sends data async to the server.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    /// <param name="data">The data to be sent.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the <see cref="ValueTask" />.</returns>
    public async ValueTask SendAsync(PluginSender data, CancellationToken cancellationToken)
    {
        try
        {

            if (!canSend)
                return;

            var counter = 0;
            while (!IsConnected)
            {
                canSend = false;
                counter++;
                await Task.Delay(500, cancellationToken);

                if (counter >= 50)
                {
                    Log.Info($"{nameof(SendAsync)}: Connection timed out.");
                    Dispose();

                    return;
                }
            }
            var serializedData = JsonConvert.SerializeObject(data, JsonSerializerSettings);
            var bytesToSend = Encoding.UTF8.GetBytes(serializedData + '\0');

            await TcpClient!.GetStream().WriteAsync(bytesToSend, 0, bytesToSend.Length, cancellationToken);

            OnSent(this, new SentEventArgs(serializedData, bytesToSend.Length));
        }
        catch (Exception exception) when (exception.GetType() != typeof(OperationCanceledException))
        {
            DiscordSyncPlugin.NetworkCancellationTokenSource.Cancel();
            Dispose();
            OnSendingError(this, new SendingErrorEventArgs(exception));
        }
    }

    /// <summary>
    ///     Releases unmanaged resources and, optionally, managed ones.
    /// </summary>
    /// <param name="shouldDisposeAllResources">Indicates whether all resources should be disposed or only unmanaged ones.</param>
    private void Dispose(bool shouldDisposeAllResources)
    {
        if (shouldDisposeAllResources)
        {
            TcpClient?.Dispose();
            TcpClient = null;

            IPEndPoint = null;
        }

        isDisposed = true;

        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Called after network received full data.
    /// </summary>
    /// <param name="sender">The sender instance.</param>
    /// <param name="ev">The <see cref="ReceivedFullEventArgs" /> instance.</param>
    private void OnReceivedFull(object sender, ReceivedFullEventArgs ev)
    {
        ReceivedFull?.Invoke(sender, ev);
    }

    /// <summary>
    ///     Called after network received partial data.
    /// </summary>
    /// <param name="sender">The sender instance.</param>
    /// <param name="ev">The <see cref="ReceivedPartialEventArgs" /> instance.</param>
    private void OnReceivedPartial(object sender, ReceivedPartialEventArgs ev) => ReceivedPartial?.Invoke(sender, ev);

    /// <summary>
    ///     Called after the network thrown an exception while sending data.
    /// </summary>
    /// <param name="sender">The sender instance.</param>
    /// <param name="ev">The <see cref="SendingErrorEventArgs" /> instance.</param>
    private void OnSendingError(object sender, SendingErrorEventArgs ev) => SendingError?.Invoke(sender, ev);

    /// <summary>
    ///     Called after the network thrown an exception while receiving data.
    /// </summary>
    /// <param name="sender">The sender instance.</param>
    /// <param name="ev">The <see cref="ReceivingErrorEventArgs" /> instance.</param>
    private void OnReceivingError(object sender, ReceivingErrorEventArgs ev) => ReceivingError?.Invoke(sender, ev);

    /// <summary>
    ///     Called after network sent data.
    /// </summary>
    /// <param name="sender">The sender instance.</param>
    /// <param name="ev">The <see cref="SentEventArgs" /> instance.</param>
    private void OnSent(object sender, SentEventArgs ev) => Sent?.Invoke(sender, ev);

    /// <summary>
    ///     Called before the network connects to the server.
    /// </summary>
    /// <param name="sender">The sender instance.</param>
    /// <param name="ev">The <see cref="ConnectingEventArgs" /> instance.</param>
    private void OnConnecting(object sender, ConnectingEventArgs ev) => Connecting?.Invoke(sender, ev);

    /// <summary>
    ///     Called after the network thrown an exception while sending data.
    /// </summary>
    /// <param name="sender">The sender instance.</param>
    /// <param name="ev">The <see cref="ConnectingErrorEventArgs" /> instance.</param>
    private void OnConnectingError(object sender, ConnectingErrorEventArgs ev) => ConnectingError?.Invoke(sender, ev);

    /// <summary>
    ///     Called after the network successfully connects to the server.
    /// </summary>
    /// <param name="sender">The sender instance.</param>
    /// <param name="ev">The <see cref="System.EventArgs" /> instance.</param>
    private void OnConnected(object sender, System.EventArgs ev) => Connected?.Invoke(sender, ev);

    /// <summary>
    ///     Called after the network thrown an exception while updating the connection.
    /// </summary>
    /// <param name="sender">The sender instance.</param>
    /// <param name="ev">The <see cref="UpdatingConnectionErrorEventArgs" /> instance.</param>
    private void OnUpdatingConnectionError(object sender, UpdatingConnectionErrorEventArgs ev) => UpdatingConnectionError?.Invoke(sender, ev);

    /// <summary>
    ///     Called after the network termination.
    /// </summary>
    /// <param name="sender">The sender instance.</param>
    /// <param name="ev">The <see cref="TerminatedEventArgs" /> instance.</param>
    private void OnTerminated(object sender, TerminatedEventArgs ev) => Terminated?.Invoke(sender, ev);

    private async Task ReceiveAsync(CancellationTokenSource cancellationToken)
    {
        StringBuilder totalReceivedData = new();
        var buffer = new byte[ReceptionBuffer];
        while (true)
        {
            try
            {
                if (TcpClient is null)
                    return;

                var readTask = TcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length, cancellationToken.Token);

                await Task.WhenAny(readTask, Task.Delay(10000, cancellationToken.Token)).ConfigureAwait(false);

                cancellationToken.Token.ThrowIfCancellationRequested();

                var bytesRead = await readTask;

                if (bytesRead > 0)
                {
                    var receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (receivedData.IndexOf('\0') != -1)
                    {
                        foreach (var splitData in receivedData.Split('\0'))
                        {

                            if (splitData is "\"heartbeat\"" or "heartbeat")
                                continue;
                            if (totalReceivedData.Length > 0)
                            {
                                BotRequester botRequester;
                                try
                                {
                                    
                                    botRequester = JsonConvert.DeserializeObject<BotRequester>(receivedData + splitData, JsonSerializerSettings)!; 
                                        
                                }
                                catch
                                {
                                    totalReceivedData.Clear();
                                    continue;
                                }
                                OnReceivedFull(this, new ReceivedFullEventArgs(botRequester, bytesRead));

                                totalReceivedData.Clear();
                            }
                            else if (!string.IsNullOrEmpty(splitData))
                            {
                                BotRequester botRequester;

                                try
                                {
                                    botRequester = JsonConvert.DeserializeObject<BotRequester>(receivedData + splitData, JsonSerializerSettings)!;

                                }
                                catch
                                {

                                    totalReceivedData.Append(splitData);
                                    continue;
                                }




                                OnReceivedFull(this, new ReceivedFullEventArgs(botRequester, bytesRead));

                                totalReceivedData.Clear();
                            }
                        }
                    }
                    else
                    {

                        //OnReceivedPartial(this, new ReceivedPartialEventArgs(receivedData, bytesRead));

                        totalReceivedData.Append(receivedData);
                    }
                }
            }
            catch
            {
                TcpClient?.Dispose();
            }
        }
    }

    private async Task Update(CancellationTokenSource cancellationToken)
    {
        while (true)
        {
            try
            {
                ConnectingEventArgs ev = new(IPEndPoint.Address, (ushort)IPEndPoint.Port, ReconnectionInterval);

                OnConnecting(this, ev);

                IPEndPoint.Address = ev.IPAddress;
                IPEndPoint.Port = ev.Port;
                ReconnectionInterval = ev.ReconnectionInterval;

                TcpClient = new TcpClient();

                await TcpClient.ConnectAsync(IPEndPoint.Address, IPEndPoint.Port);

                canSend = true;
                OnConnected(this, System.EventArgs.Empty);

                await ReceiveAsync(cancellationToken);
            }
            catch (IOException ioException)
            {
                OnReceivingError(this, new ReceivingErrorEventArgs(ioException));
            }
            catch (SocketException socketException) when (socketException.ErrorCode == 10061)
            {
                OnConnectingError(this, new ConnectingErrorEventArgs(socketException));
            }
            catch (Exception exception) when (exception.GetType() != typeof(OperationCanceledException))
            {
                OnUpdatingConnectionError(this, new UpdatingConnectionErrorEventArgs(exception));
            }

            cancellationToken.Token.ThrowIfCancellationRequested();

            await Task.Delay(ReconnectionInterval, cancellationToken.Token);
        }
    }
    
    public async Task<BotRequester> Request(MessageType messageType, object data = null, string nickname = "")
    {
        return await WriteLineAndGetReply(new PluginSender(messageType, data, nickname), TimeSpan.FromSeconds(2f));
    }
    public async Task<BotRequester> WriteLineAndGetReply(PluginSender data, TimeSpan timeout)
    {
        var Reply = new BotRequester();
        ReceivedFull += (s, e) => Reply = e.BotRequester;
        await SendAsync(data);
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (Reply.Type == MessageType.None && stopwatch.Elapsed < timeout)
            Thread.Sleep(10);
        Log.Debug("Recieved Reply: " + Reply.Type + ": " + Reply.DataBR);
        return Reply;
    }


    private async Task ReplyLine(PluginSender data)
    {
        await DiscordSyncPlugin.Instance.Network.SendAsync(data);
    }
    public async Task ReplyLine(MessageType requestType, object data = null, string nickname = "")
    {
        var pluginSender = new PluginSender(requestType, data, nickname);

        await DiscordSyncPlugin.Instance.Network.SendAsync(pluginSender);
    }
}
