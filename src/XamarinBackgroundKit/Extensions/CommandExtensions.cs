using System.Windows.Input;

namespace XamarinBackgroundKit.Extensions
{
    public static class CommandExtensions
    {
        public static void CheckAndExecute(this ICommand command, object param)
        {
            if (command == null || !command.CanExecute(param)) return;
            command.Execute(param);
        }
    }
}
