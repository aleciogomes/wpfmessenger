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

        private void ShowCustomBalloon(UIElement balloon, PopupAnimation animation, int? timeout)
        {
            //create an invisible popup that hosts the UIElement
            Popup popup = new Popup();
            popup.AllowsTransparency = true;

            //don't animate by default - devs can use attached
            //events or override
            popup.PopupAnimation = animation;

            popup.Child = balloon;

            popup.Placement = PlacementMode.AbsolutePoint;
            popup.StaysOpen = true;

            Point position = new Point() {X = 1280, Y = 964 };
            popup.HorizontalOffset =1279;
            popup.VerticalOffset = 920;

            //display item
            popup.IsOpen = true;

        }

    }
}
