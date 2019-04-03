# Xamarin.Forms-BackgroundKit
The way the UI customization in Xamarin.Forms should be

BackgroundKit Provides a consistent way for adding Background to your views.  
What is supported out of the box: 

### What is used for the Background

| Background | Android | iOS |
| ------ | ------ | ------ |
| Elevation | Android.Views.View.Elevation | MDCShadowLayer
| Gradients | [GradientStrokeDrawable](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.Android/Renderers/GradientStrokeDrawable.cs) | CAGradientLayer |
| Border | [GradientStrokeDrawable](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.Android/Renderers/GradientStrokeDrawable.cs) | CAGradientLayer |
| CornerRadius | [GradientStrokeDrawable](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.Android/Renderers/GradientStrokeDrawable.cs) | CAGradientLayer / Mask when not uniform radius |

### Native Views

| Parent Views | Android | iOS |
| ------ | ------ | ------ |
| MaterialContentView | Android.Views.View | UIView |
| MaterialCard | CardView* | MaterialComponents.Card |  

* I used MaterialCardView but there is [bug](https://github.com/material-components/material-components-android/commit/09673a5de798241860e5cecd051e0caa19397ca0) with the RippleEffect and it is not in the codebase yet.

### Native Background Managers
| Android | iOS |
| ------ | ------ |
| MaterialVisualElementTracker | MaterialVisualElementTracker |
| MaterialVisualElementTracker | MaterialVisualElementTracker |  

### What is supported
| Xamarin.Forms View | Android | iOS |
| ------ | ------ | ------ |
| StackLayout | Yes | Yes |
| AbsoluteLayout | Yes | Yes |
| Grid | Yes | Yes |
| RelativeLayout | Yes | Yes |
| ContentView | Yes | Yes |
| Button | Yes | Yes |
| Entry | Yes | Yes |
| Picker | Yes | Yes |
| Label | Yes | Yes |
| Switch | Yes | Yes |
| BoxView | Yes | Yes |
| ListView | Yes | Yes |
| CollectionView | Yes | Yes |

| BackgroundKit View | Android | iOS |
| ------ | ------ | ------ |
| MaterialCard | Yes | Yes |
| MaterialContentView | Yes | Yes |

### Ripple Support

| BackgroundKit View | Android | iOS |
| ------ | ------ | ------ |
| MaterialCard | Yes | Yes |
| MaterialContentView | No at the moment | No at the moment |

## Setup
#### Xamarin.Android
Initialize the renderer below the Xamarin.Forms.Init
```cs
XamarinBackgroundKit.Android.BackgroundKit.Init();
```

#### Xamarin.iOS
Initialize the renderer below the Xamarin.Forms.Init
```cs
XamarinBackgroundKit.Android.BackgroundKit.Init();
```

## Usage
### API Documentation
#### Background
| Property | Type | Description | Why do I need it? |
| ------ | ------ | ------ | ------ |
| `Elevation` | `double` | The elevation of the Background | It adds shadow to view that depends on the value of elevation |
| `TranslationZ` | `double` | The translation in Z axis of the background (only affects on Android) | In android you need to overlap views without adding shadows. TranslationZ is what you need! |
| `CornerRadius` | `CornerRadius` | The corner radius of the background | Exactly as Thickness. Sets the radius to each corner |
| `Angle` | `double` | The gradient angle of the background | The -360 to 360 angle of the gradient |
| `GradientType` | `GradientType` | The type of gradient of the background | Linear or Radial. Currently Linear is supported only |
| `Gradients` | `IList<GradientStop>` | The gradients that will be used for the background |
| `BorderColor` | `Color` | The border color of the background |
| `BorderWidth` | `double` | The border width of the background |

#### Material Content View
| Property | Type | Description |
| ------ | ------ | ------ |
| `Background` | `Background` | The background of the view | |
| `IsCircle` | `bool` | By using this property you do not have to set the same WidthRequest and HeightRequest and manually add the CornerRadius. It measures all the child views and then becomes a circle |
| `IsCornerRadiusHalfHeight` | `bool` | The corner radius will ALWAYS become the half of the height |

#### Material Card
Same as MaterialContentView

#### Background Effect
Background Effect has an attached property in order to add background to your views without the need of a custom renderer!

### Actual Usage
Everything you need to do is coming from XAML!

#### Material Card/ContentView
```xml
<controls:MaterialContentView HeightRequest="60">
	<controls:MaterialContentView.Background>
		<controls:Background
			Angle="0"
			BorderColor="Brown"
			BorderWidth="4"
			CornerRadius="30"
			GradientType="Linear">
			<controls:Background.Gradients>
				<controls:GradientStop Offset="0" Color="Blue" />
				<controls:GradientStop Offset="1" Color="DarkRed" />
			</controls:Background.Gradients>
		</controls:Background>
	</controls:MaterialContentView.Background>

	<Label Text="Material ContentView with Gradient and Offsets" />
</controls:MaterialContentView>
```

#### By using the Effect
```xml
<StackLayout
    Margin="16"
    Padding="16"
    HorizontalOptions="FillAndExpand">
    <effects:BackgroundEffect.Background>
        <controls:Background
            Angle="0"
            CornerRadius="20"
            Elevation="16"
            GradientType="Linear">
            <controls:Background.Gradients>
                <controls:GradientStop Offset="0" Color="Blue" />
                <controls:GradientStop Offset="1" Color="DarkRed" />
            </controls:Background.Gradients>
        </controls:Background>
    </effects:BackgroundEffect.Background>

    <Label Text="I am inside a stack layout" />
</StackLayout>
```

### ScreenShots

#### Android
<img src="https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/art/background_android.png" width="280" />

#### iOS (coming soon...)