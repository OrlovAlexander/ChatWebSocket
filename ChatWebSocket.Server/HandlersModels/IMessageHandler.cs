namespace ChatWebSocket.Server.HandlersModels;

internal interface IMessageHandler
{
    Task IncomingHandler(IRequestMessage requestMessage);
    Task OutgoingHandler(IResponseMessage responseMessage);
}