using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

// -------------------------------------------------------------------
// <copyright file="HybridOrientationChangesFrame.cs" company="Ivan Vacula">
//   Copyright (c) Ivan Vacula. All rights reserved.
// </copyright>
// <author>Ivan Vacula</author>
// <email>ivan@vacula.net</email>
// <date>2011-12-31</date>
// <summary>Contains HybridOrientationChangesFrame class.</summary>
// -------------------------------------------------------------------

// This is a modified version of HybridOrientationChangesFrame by David Anson.
// http://blogs.msdn.com/b/delay/archive/2010/09/28/this-one-s-for-you-gregor-mendel-code-to-animate-and-fade-windows-phone-orientation-changes-now-supports-a-new-mode-hybrid.aspx
// Copyright (C) Microsoft Corporation. All Rights Reserved.
// This code released under the terms of the Microsoft Public License
// (Ms-PL, http://opensource.org/licenses/ms-pl.html).
namespace CompreAqui.Transition
{
/// <summary>
	/// TransitionFrame subclass that animates and fades between device orientation changes.
	/// </summary>
	public class HybridOrientationChangesFrame : TransitionFrame
	{
		/// <summary>
		/// Identifies the IsAnimationEnabled DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty IsAnimationEnabledProperty =
			DependencyProperty.Register("IsAnimationEnabled", typeof(bool), typeof(HybridOrientationChangesFrame), new PropertyMetadata(true));

		/// <summary>
		/// Identifies the Duration DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty DurationProperty =
			DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(HybridOrientationChangesFrame), new PropertyMetadata(TimeSpan.FromSeconds(0.4)));

		/// <summary>
		/// Identifies the EasingFunction DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty EasingFunctionProperty =
			DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(HybridOrientationChangesFrame), new PropertyMetadata(null));

		/// <summary>
		/// Stores the previous orientation.
		/// </summary>
		private PageOrientation previousOrientation = PageOrientation.PortraitUp;

		/// <summary>
		/// Stores the Popup for displaying the "before" content.
		/// </summary>
		private Popup popup = new Popup();

		/// <summary>
		/// Stores the Storyboard used to animate the transition.
		/// </summary>
		private Storyboard storyboard = new Storyboard();

		/// <summary>
		/// Stores the Timeline used to perform the "before" fade.
		/// </summary>
		private DoubleAnimation beforeOpacityAnimation = new DoubleAnimation { From = 1, To = 0 };

		/// <summary>
		/// Stores the Timeline used to perform the "after" fade.
		/// </summary>
		private DoubleAnimation afterOpacityAnimation = new DoubleAnimation { From = 0, To = 1 };

		/// <summary>
		/// Stores the Timeline used to perform the "before" rotation.
		/// </summary>
		private DoubleAnimation beforeRotationAnimation = new DoubleAnimation();

		/// <summary>
		/// Stores the Timeline used to perform the "before" rotation.
		/// </summary>
		private DoubleAnimation afterRotationAnimation = new DoubleAnimation();

		/// <summary>
		/// Stores the Transform used to create the "after" rotation.
		/// </summary>
		private RotateTransform afterRotateTransform = new RotateTransform();

		/// <summary>
		/// Initializes a new instance of the HybridOrientationChangesFrame class.
		/// </summary>
		/// <param name="duration">Animation duration.</param>
		public HybridOrientationChangesFrame(TimeSpan duration)
		{
			this.Duration = duration;

			// Set up animations
			Storyboard.SetTargetProperty(this.beforeOpacityAnimation, new PropertyPath(UIElement.OpacityProperty));
			this.storyboard.Children.Add(this.beforeOpacityAnimation);
			Storyboard.SetTargetProperty(this.afterOpacityAnimation, new PropertyPath(UIElement.OpacityProperty));
			this.storyboard.Children.Add(this.afterOpacityAnimation);
			Storyboard.SetTargetProperty(this.beforeRotationAnimation, new PropertyPath(RotateTransform.AngleProperty));
			this.storyboard.Children.Add(this.beforeRotationAnimation);
			Storyboard.SetTargetProperty(this.afterRotationAnimation, new PropertyPath(RotateTransform.AngleProperty));
			this.storyboard.Children.Add(this.afterRotationAnimation);
			this.storyboard.Completed += new EventHandler(this.HandleStoryboardCompleted);

			// Initialize variables
			this.EasingFunction = new QuadraticEase(); // Initialized here to avoid a single shared instance

			// Add custom transform to end of existing group
			var transformGroup = this.RenderTransform as TransformGroup;
			if (null != transformGroup)
			{
				transformGroup.Children.Add(this.afterRotateTransform);
			}

			// Hook events
			this.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(this.HandleOrientationChanged);
		}

		/// <summary>
		/// Gets or sets a value indicating whether animation is enabled.
		/// </summary>
		public bool IsAnimationEnabled
		{
			get { return (bool)GetValue(IsAnimationEnabledProperty); }
			set { SetValue(IsAnimationEnabledProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating the duration of the orientation change animation.
		/// </summary>
		public TimeSpan Duration
		{
			get { return (TimeSpan)GetValue(DurationProperty); }
			set { SetValue(DurationProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating the IEasingFunction to use for the orientation change animation.
		/// </summary>
		public IEasingFunction EasingFunction
		{
			get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
			set { SetValue(EasingFunctionProperty, value); }
		}

		/// <summary>
		/// Handles the OrientationChanged event.
		/// </summary>
		/// <param name="sender">Event source.</param>
		/// <param name="e">Event arguments.</param>
		private void HandleOrientationChanged(object sender, OrientationChangedEventArgs e)
		{
			// Stop/complete Storyboard in case it's active
			this.storyboard.Stop();
			this.HandleStoryboardCompleted(null, null);

			if (this.IsAnimationEnabled)
			{
				// Capture device width/height
				var actualWidth = ActualWidth;
				var actualHeight = ActualHeight;

				// Get "before" width/height
				bool normal = PageOrientation.Portrait == (PageOrientation.Portrait & this.previousOrientation);
				var width = normal ? actualWidth : actualHeight;
				var height = normal ? actualHeight : actualWidth;

				// Capture "before" visuals in a WriteableBitmap
				var writeableBitmap = new WriteableBitmap((int)width, (int)height);
				writeableBitmap.Render(this, null);
				writeableBitmap.Invalidate();

				// Create transforms for "before" content
				var beforeTranslateTransform = new TranslateTransform();
				var beforeRotateTransform = new RotateTransform { CenterX = actualWidth / 2, CenterY = actualHeight / 2 };
				var beforeTransforms = new TransformGroup();
				beforeTransforms.Children.Add(beforeTranslateTransform);
				beforeTransforms.Children.Add(beforeRotateTransform);

				// Configure transforms for "before" content
				var translateDelta = (actualHeight - actualWidth) / 2;
				var beforeAngle = 0.0;
				if (PageOrientation.LandscapeLeft == this.previousOrientation)
				{
					beforeAngle = -90;
					beforeTranslateTransform.X = -translateDelta;
					beforeTranslateTransform.Y = translateDelta;
				}
				else if (PageOrientation.LandscapeRight == this.previousOrientation)
				{
					beforeAngle = 90;
					beforeTranslateTransform.X = -translateDelta;
					beforeTranslateTransform.Y = translateDelta;
				}

				beforeRotateTransform.Angle = -beforeAngle;

				// Configure for "after" content
				var afterAngle = 0.0;
				if (PageOrientation.LandscapeLeft == e.Orientation)
				{
					afterAngle = -90;
				}
				else if (PageOrientation.LandscapeRight == e.Orientation)
				{
					afterAngle = 90;
				}

				this.afterRotateTransform.CenterX = actualWidth / 2;
				this.afterRotateTransform.CenterY = actualHeight / 2;

				// Create content with default background and WriteableBitmap overlay for "before"
				var container = new Grid
				{
					Width = width,
					Height = height,
					Background = (Brush)Application.Current.Resources["PhoneBackgroundBrush"],
					RenderTransform = beforeTransforms,
					IsHitTestVisible = false,
				};
				var content = new Rectangle
				{
					Fill = new ImageBrush
					{
						ImageSource = writeableBitmap,
						Stretch = Stretch.None,
					}
				};
				container.Children.Add(content);

				// Configure Popup for displaying "before" content
				this.popup.Child = container;
				this.popup.IsOpen = true;

				// Update animations to fade from "before" to "after"
				Storyboard.SetTarget(this.beforeOpacityAnimation, container);
				this.beforeOpacityAnimation.Duration = this.Duration;
				this.beforeOpacityAnimation.EasingFunction = this.EasingFunction;
				Storyboard.SetTarget(this.afterOpacityAnimation, this);
				this.afterOpacityAnimation.Duration = this.Duration;
				this.afterOpacityAnimation.EasingFunction = this.EasingFunction;

				// Update animations to rotate from "before" to "after"
				Storyboard.SetTarget(this.beforeRotationAnimation, beforeRotateTransform);
				this.beforeRotationAnimation.From = beforeRotateTransform.Angle;
				this.beforeRotationAnimation.To = this.beforeRotationAnimation.From + (beforeAngle - afterAngle);
				this.beforeRotationAnimation.Duration = this.Duration;
				this.beforeRotationAnimation.EasingFunction = this.EasingFunction;
				Storyboard.SetTarget(this.afterRotationAnimation, this.afterRotateTransform);
				this.afterRotationAnimation.From = -(beforeAngle - afterAngle);
				this.afterRotationAnimation.To = 0;
				this.afterRotationAnimation.Duration = this.Duration;
				this.afterRotationAnimation.EasingFunction = this.EasingFunction;

				// Play the animations
				this.storyboard.Begin();
			}

			// Save current orientation for next time
			this.previousOrientation = e.Orientation;
		}

		/// <summary>
		/// Handles the completion of the Storyboard.
		/// </summary>
		/// <param name="sender">Event source.</param>
		/// <param name="e">Event arguments.</param>
		private void HandleStoryboardCompleted(object sender, EventArgs e)
		{
			// Remove and clear Popup
			this.popup.IsOpen = false;
			this.popup.Child = null;
		}
	}
}
