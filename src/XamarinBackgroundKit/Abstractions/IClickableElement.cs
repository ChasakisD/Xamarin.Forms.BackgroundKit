using System;
using Xamarin.Forms;

namespace XamarinBackgroundKit.Abstractions
{
	public interface IClickableElement
	{
		event EventHandler<EventArgs> Clicked;

		bool IsClickable { get; }

		Command ClickedCommand { get; }

		object ClickedCommandParameter { get; }

		void OnClicked();

		void OnIsClickablePropertyChanged(bool oldValue, bool newValue);
	}
}
