namespace ICAN.SIC.Plugin.ChatInterface
{
    public interface IChatInterface
    {
        void PushUserResponse(string message);
        void PushMachineMessage(string message);
    }
}