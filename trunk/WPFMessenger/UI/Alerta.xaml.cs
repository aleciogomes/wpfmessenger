using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace WPFMessenger
{
  /// <summary>
  /// Interaction logic for FancyBalloon.xaml
  /// </summary>
  public partial class Alerta : UserControl
  {
    private bool isClosing = false;

    public Alerta()
    {
      InitializeComponent();
    }

    #region BalloonText dependency property



    public static readonly RoutedEvent BalloonShowingEvent = EventManager.RegisterRoutedEvent("BalloonShowing",
            RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(Alerta));

    public event RoutedEventHandler BalloonShowing
    {
        add { AddHandler(BalloonShowingEvent, value); }
        remove { RemoveHandler(BalloonShowingEvent, value); }
    }

    private void RaiseBalloonShowingEvent()
    {
        RoutedEventArgs newEventArgs = new RoutedEventArgs(Alerta.BalloonShowingEvent);
        RaiseEvent(newEventArgs);
    }

    public static readonly DependencyProperty BalloonTextProperty =
        DependencyProperty.Register("BalloonText",
                                    typeof (string),
                                    typeof (Alerta),
                                    new FrameworkPropertyMetadata(""));

    /// <summary>
    /// A property wrapper for the <see cref="BalloonTextProperty"/>
    /// dependency property:<br/>
    /// Description
    /// </summary>
    public string BalloonText
    {
      get { return (string) GetValue(BalloonTextProperty); }
      set { SetValue(BalloonTextProperty, value); }
    }

    #endregion

    private void OnBalloonClosing(object sender, RoutedEventArgs e)
    {
      e.Handled = true;
      isClosing = true;
    }

    private void OnFadeOutCompleted(object sender, EventArgs e)
    {
      Popup pp = (Popup)Parent;
      pp.IsOpen = false;
    }
  }
}
