using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinBackgroundKit.Abstractions
{
	public interface IFocusableElement
	{
		event EventHandler<EventArgs> Pressed;
		event EventHandler<EventArgs> Released;
		event EventHandler<EventArgs> Cancelled;
		event EventHandler<EventArgs> ReleasedOrCancelled;

		bool IsFocusable { get; }

		ICommand PressedCommand { get; }
		ICommand ReleasedCommand { get; }
		ICommand CancelledCommand { get; }
		ICommand ReleasedOrCancelledCommand { get; }

		object PressedCommandParameter { get; }
		object ReleasedCommandParameter { get; }
		object CancelledCommandParameter { get; }
		object ReleasedOrCancelledCommandParameter { get; }

		void OnPressed();
		void OnReleased();
		void OnCancelled();
		void OnReleasedOrCancelled();

		void OnIsFocusablePropertyChanged(bool oldValue, bool newValue);
	}
}
