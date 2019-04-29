# Xamarin.Forms-BackgroundKit
🎨 🔨 A powerful Kit for customizing the background of Xamarin.Forms views

📐 Corner Radius | 🎨 Background Gradients | 🍩 Borders | 🌈 Border Gradients | 🙏 Shadows

* [Screenshots](#Screenshots)
* [What is Supported](#What-is-supported)
* [Ripple Support](#Ripple-Support)
* [Setup](#Setup)
* [Usage](#Usage)


### ScreenShots

#### Android
<img src="https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/art/contentViewAndroid.gif" width="280" /> | 
<img src="https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/art/searchBarAndroid.gif" width="280" /> | 
<img src="https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/art/cornerRadiiElevation.png" width="280" /> | 
<img src="https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/art/corner_clipping_android.png" width="280" />

#### iOS
<img src="https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/art/cornerRadiiElevation-iOS.png" width="280" /> |
<img src="https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/art/corner_clipping_ios.png" width="280" />

### What was hard

Mixing Gradient on border with gradient on background was a really painful challenge. On Android I had to deal with custom Drawables and CALayers on iOS. Also, on Android, making elevation and clipping work for different radius on each corner was a trial and error pain. An outline provider has been made in order to support cornerRadii. GradientStrokeDrawable extends GradientDrawable and draws a custom paint(to replicate the border) into the shape of the view. On iOS, the GradientStrokeLayer also extends CAGradientLayer and it has a ShadowLayer and another one CAGradientLayer for the border. Clipping on iOS is done through a CAShapeLayer.

### What is used for the Background

BackgroundKit Provides a consistent way for adding Background to your views.  
What is supported out of the box: 

| Background | Android | iOS |
| ------ | ------ | ------ |
| Elevation | Android.Views.View.Elevation | MDCShadowLayer
| Ripple | Android.Graphics.Drawable.RippleDrawable | MDCInkViewController |
| CornerRadius | [GradientStrokeDrawable](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.Android/Renderers/GradientStrokeDrawable.cs) | [GradientStrokeLayer](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.iOS/Renderers/GradientStrokeLayer.cs) |
| Gradients | [GradientStrokeDrawable](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.Android/Renderers/GradientStrokeDrawable.cs) | [GradientStrokeLayer](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.iOS/Renderers/GradientStrokeLayer.cs) |
| Border | [GradientStrokeDrawable](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.Android/Renderers/GradientStrokeDrawable.cs) | [GradientStrokeLayer](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.iOS/Renderers/GradientStrokeLayer.cs) |
| Gradient Border | [GradientStrokeDrawable](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.Android/Renderers/GradientStrokeDrawable.cs) | [GradientStrokeLayer](https://github.com/ChasakisD/Xamarin.Forms-BackgroundKit/blob/master/src/XamarinBackgroundKit.iOS/Renderers/GradientStrokeLayer.cs) |

### Native Views (FastRenderers Pattern on Android)

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

#### Xamarin.Forms Layouts

| Xamarin.Forms Layouts | Android | iOS |
| ------ | ------ | ------ |
| AbsoluteLayout | Yes | Yes |
| CarouselView | Yes | Yes |
| CollectionView | Yes | Yes |
| ContentView | Yes | Yes |
| FlexLayout | Yes | Yes |
| Frame | Yes | Yes |
| Grid | Yes | Yes |
| ListView | Yes | Yes |
| RelativeLayout | Yes | Yes |
| ScrollView | Yes | Yes |
| StackLayout | Yes | Yes |

#### Xamarin.Forms Views

| Xamarin.Forms Views | Android | iOS |
| ------ | ------ | ------ |
| ActivityIndicator | Yes | Yes |
| BoxView | Yes | No at the moment |
| Button | Yes | Yes |
| DatePicker | Yes | Yes |
| Editor | Yes | Yes |
| Entry | Yes | Yes |
| Image | Yes | Yes |
| ImageButton | Yes | Yes |
| Label | Yes | Yes |
| Picker | Yes | Yes |
| ProgressBar | Yes | Yes |
| SearchBar | Yes | Yes |
| Slider | Yes | Yes |
| Stepper | Yes | Yes |
| Switch | Yes | Yes |
| TimePicker | Yes | Yes |
| WebView | Yes | Yes |

#### Custom Views from BackgroundKit

| BackgroundKit View | Android | iOS |
| ------ | ------ | ------ |
| MaterialCard | Yes | Yes |
| MaterialContentView | Yes | Yes |

PS.: Material Components on Android and iOS are not supporting changing the background, so gradients and gradient borders are not supported when Visual="Material" is used.

### Ripple Support

| BackgroundKit View | Android | iOS |
| ------ | ------ | ------ |
| MaterialCard | Yes through RippleDrawable | Yes through RippleDrawable |
| MaterialContentView | Yes through MDCInkViewController | Yes through MDCInkViewController |

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

## <a name="usage"></a>Usage
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
| `DashGap` | `double` | Adds dashed border to the background. Dash gap specifies the pixels that the gap of the dash will be |
| `DashWidth` | `double` | Adds dashed border to the background. Dash width specifies the pixels that the width of a filled line will be |
| `BorderAngle` | `double` | The gradient angle of the border of the background | The -360 to 360 angle of the gradient |
| `BorderGradientType` | `GradientType` | The type of gradient of the border of the background | Linear or Radial. Currently Linear is supported only |
| `BorderGradients` | `IList<GradientStop>` | The gradients that will be used for the border of the background |
| `IsRippleEnabled` | `bool` | Whether or not the ripple will be enabled |
| `RippleColor` | `Color` | The ripple color of the background |

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
