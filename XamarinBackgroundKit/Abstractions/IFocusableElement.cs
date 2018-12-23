using System;
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

		Command PressedCommand { get; }
		Command ReleasedCommand { get; }
		Command CancelledCommand { get; }
		Command ReleasedOrCancelledCommand { get; }

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
