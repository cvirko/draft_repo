using Auth.Client.ConsoleApp.Interfaces;
using Auth.Client.ConsoleApp.Services.Api;
using Auth.Client.ConsoleApp.Services.SiganlR;
using Auth.Client.ConsoleApp.Services.UserActions;
using Auth.Domain.Core.Common.Consts;

const string LOCALURI = "https://localhost:7250";
string uri = "";

Console.WriteLine("Chose hub type: \n 1) Local \n 2) Server");
var type = Console.ReadLine();
switch (type)
{
    default:
        uri = LOCALURI; break;
}

SignalRClient _socket = null;
IUnitOfWorkServerApi _uowApi = null;
try
{
    _uowApi = new UnitOfWorkServerApi(uri);
    await _uowApi.Account().TryLoginAsync();
    _socket = new SignalRClient(string.Join("/", uri, AppConsts.HUBNAME), _uowApi);
    await _socket.ConnectToMessageListener();

    var _chat =  new ChatActionService(_uowApi);
    await _chat.AddActionsAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
finally
{
    _socket?.Dispose();
    _uowApi?.Dispose();
}



