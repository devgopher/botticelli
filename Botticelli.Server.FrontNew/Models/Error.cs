namespace Botticelli.Server.FrontNew.Models;

public class Error
{
    public int Code { get; set; } // code 0 = success, 1- err
    public string UserMessage { get; set; }
}