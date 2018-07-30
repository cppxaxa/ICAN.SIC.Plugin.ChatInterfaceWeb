namespace ICAN.SIC.Plugin.ChatInterfaceWeb
{
    enum Color
    {
        Bot,
        User
    }

    internal interface IUIMessage
    {
        Color Color { get; }
        string Message { get; }
    }
}