using Xamarin.Forms;

namespace XamarinBackgroundKit.Extensions
{
    public static class CommandExtensions
    {
        public static void CheckAndExecute(this Command command, object param)
        {
            if (command == null || !command.CanExecute(param)) return;
            command.Execute(param);
        }
    }
}
