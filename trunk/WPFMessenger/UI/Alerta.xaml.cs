using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace WPFMessenger
{
  public partial class Alerta : UserControl
  {

    public Alerta()
    {
      InitializeComponent();
    }

    #region BalloonText dependency property

    public static readonly DependencyProperty BalloonTextProperty =
        DependencyProperty.Register("BalloonText",
                                    typeof (string),
                                    typeof (Alerta),
                                    new FrameworkPropertyMetadata(""));

    public string BalloonText
    {
      get { return (string) GetValue(BalloonTextProperty); }
      set { SetValue(BalloonTextProperty, value); }
    }

    #endregion

    private void OnBalloonClosing(object sender, RoutedEventArgs e)
    {
      e.Handled = true;
    }

    private void OnFadeOutCompleted(object sender, EventArgs e)
    {
      Popup pp = (Popup)Parent;
      pp.IsOpen = false;
    }
  }
}
