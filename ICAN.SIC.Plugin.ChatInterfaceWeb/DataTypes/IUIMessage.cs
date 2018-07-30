namespace ICAN.SIC.Plugin.ChatInterface
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