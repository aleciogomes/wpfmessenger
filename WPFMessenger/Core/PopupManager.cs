using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Windows.Controls;

namespace WPFMessenger
{
    class PopupManager
    {

        private readonly Timer balloonCloseTimer;

        public PopupManager()
        {
            //balloonCloseTimer = new Timer(null);
        }

        public void CreateBaloon()
        {

            Alerta balloon = new Alerta();
            balloon.BalloonText = "Custom Balloon";

            //show balloon and close it after 4 seconds
            ShowCustomBalloon(balloon, PopupAnimation.Slide, 4000);
        }

        /// <summary>
        /// Shows a custom control as a tooltip in the tray location.
        /// </summary>
        /// <param name="balloon"></param>
        /// <param name="animation">An optional animation for the popup.</param>
        /// <param name="timeout">The time after which the popup is being closed.
        /// Submit null in order to keep the balloon open inde
        /// </param>
        /// <exception cref="ArgumentNullException">If <paramref name="balloon"/>
        /// is a null reference.</exception>
        private void ShowCustomBalloon(UIElement balloon, PopupAnimation animation, int? timeout)
        {

            if (balloon == null) throw new ArgumentNullException("balloon");
            if (timeout.HasValue && timeout < 500)
            {
                string msg = "Invalid timeout of {0} milliseconds. Timeout must be at least 500 ms";
                msg = String.Format(msg, timeout);
                throw new ArgumentOutOfRangeException("timeout", msg);
            }

            //create an invisible popup that hosts the UIElement
            Popup popup = new Popup();
            popup.AllowsTransparency = true;

            //don't animate by default - devs can use attached
            //events or override
            popup.PopupAnimation = animation;

            popup.Child = balloon;

            popup.Placement = PlacementMode.AbsolutePoint;
            popup.StaysOpen = true;

            Point position = new Point() {X = 100, Y = 100 };
            popup.HorizontalOffset =0;
            popup.VerticalOffset = 0;

            //fire attached event
            //RaiseBalloonShowingEvent(balloon);

            //display item
            popup.IsOpen = true;

        }


        public RoutedEventArgs RaiseBalloonShowingEvent(DependencyObject target)
        {
            if (target == null) return null;

            RoutedEventArgs args = new RoutedEventArgs(Alerta.BalloonShowingEvent, this);

            if (target is UIElement)
            {
                (target as UIElement).RaiseEvent(args);
            }
            else if (target is ContentElement)
            {
                (target as ContentElement).RaiseEvent(args);
            }

            return args;
        }
    }
}
