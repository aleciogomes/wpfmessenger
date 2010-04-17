using System.Windows;
using WPFMessenger.Core;
using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace WPFMessenger.UI
{
    /// <summary>
    /// Interaction logic for TalkWindow.xaml
    /// </summary>
    public partial class TalkWindow : Window
    {

        private UDPConnection udp;

        private MSNUser destinyUser;

        //define como estático, para nao recriar toda vez que abrir uma janela de conversa
        private static Dictionary<string, Uri> EmoticonList { get; set; }

        internal MSNUser DestinyUser
        {
            get { return destinyUser; }
            set { 
                    destinyUser = value;
                    this.Title = String.Format("{0} - Conversa", destinyUser.UserName);
                    this.SetLabelText(lblDestinyUser, destinyUser.UserName);
                }
        }

        public TalkWindow(MSNUser destiny)
        {
            InitializeComponent();

            DestinyUser = destiny;

            Closing += Window_Closing;
            msgBox.PreviewKeyDown += MsgBox_KeyDown;

            udp = new UDPConnection();

            this.SetLabelText(lblCurrentUser, MSNSession.User.UserName);

            if (EmoticonList == null)
            {
                InitializeEmoticonList();
            }

        }

        private void InitializeEmoticonList()
        {
            EmoticonList = new Dictionary<string, Uri>();

            Image imgBt;
            foreach (Button b in TBEmoticon.Items)
            {
                imgBt = (Image)b.Content;
                EmoticonList.Add(imgBt.ToolTip.ToString(), new Uri(imgBt.Source.ToString()));
            }
        }

        private void SetLabelText(TextBlock lbl, string text)
        {
            int startIndex = (text.Length - 1 >= 15 ? 15 : text.Length - 1);

            string nome = String.Format("{0}...", text.Remove(startIndex));
            lbl.Text = nome;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(e != null)
                e.Cancel = true;

            Visibility = Visibility.Collapsed;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key.ToString().Equals("Escape"))
            {
                this.Window_Closing(sender, null);
            }


        }

        private void MsgBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key.ToString().Equals("Return"))
            {
                e.Handled = true;
                btEnviar.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void btEnviar_Click(object sender, RoutedEventArgs e)
        {
            string message = new TextRange(msgBox.Document.ContentStart, msgBox.Document.ContentEnd).Text.Trim(); 

            if (!message.Equals(String.Empty))
            {
                udp.SendMessage(this.destinyUser, message);
                msgBox.Document.Blocks.Clear();

                this.InsertMessage(MSNSession.User, message);
            }

        }

        public void InsertMessage(MSNUser user, string newMessage)
        {
            Paragraph p = new Paragraph();

            //nome do usuário
            p.Inlines.Add(FormatRun(user, String.Format("({0}) {1} diz:", System.DateTime.Now, user.UserName)));
            p.Inlines.Add(System.Environment.NewLine);

            //tab
            p.Inlines.Add("\t");

            //mensagem enviada
            FormatParagraph(ref p, user, newMessage);

            p.Inlines.Add(System.Environment.NewLine);

            textBoardContent.Blocks.Add(p);

            //movimenta o scroll automaticamente par ao fim do texto
            textBoard.ScrollToEnd();

        }

        private Run FormatRun(MSNUser user, string text)
        {
            Run run = new Run(text);

            if (user.UserID == destinyUser.UserID)
            {
                run.Foreground = new SolidColorBrush(Colors.LimeGreen);
            }

            return run;
        }

        private void FormatParagraph(ref Paragraph p, MSNUser user, string text)
        {

            string canditateToIcon = null;
            string availableText = text;
            string flushText = String.Empty;

            if (availableText.Length > 2)
            {

                while (availableText.Length > 0)
                {
                    if (availableText.Length > 1)
                    {
                        canditateToIcon = availableText.Substring(0, 2);
                    }
                    else
                    {
                        canditateToIcon = string.Empty;
                    }

                    if (EmoticonList.ContainsKey(canditateToIcon))
                    {

                        if (!flushText.Equals(String.Empty))
                        {
                            p.Inlines.Add(FormatRun(user, flushText));
                            flushText = String.Empty;
                        }

                        availableText = availableText.Substring(2);

                        p.Inlines.Add(GetImageFromEmoticon(EmoticonList[canditateToIcon]));
                    }
                    else
                    {
                        flushText+= availableText[0];

                        availableText = availableText.Substring(1);
                    }
                }


                if (!flushText.Equals(String.Empty))
                {
                    p.Inlines.Add(FormatRun(user, flushText));
                }

            }

        }

        private void FormataParagraph(ref Paragraph p, MSNUser user, string text)
        {

            string modifiedText = text;
            string flushText = null;
            int index;

            /*
            Dictionary<string, List<int>> listIndexEmoticon = new Dictionary<string, List<int>>();
            List<int> listIndex = null;
            
            bool achouIndex = false;
            */

            bool achouIcon = true;

            while (!modifiedText.Equals(String.Empty) && achouIcon)
            {
                achouIcon = false;

                //encontra os icones
                foreach (string emoticonKey in EmoticonList.Keys)
                {

                    if (!modifiedText.Equals(String.Empty))
                    {

                        index = modifiedText.IndexOf(emoticonKey);

                        //achouIndex = false;
                        //listIndex = new List<int>();

                        //pode ter informado o mesmo emoticon várias vezes
                        if (index >= 0)
                        {
                            achouIcon = true;
                            //listIndex.Add(index);

                            modifiedText = modifiedText.Substring(index + 2);
                            flushText = modifiedText.Remove(index);

                            if (!flushText.Equals(String.Empty))
                            {
                                p.Inlines.Add(FormatRun(user, flushText));
                            }

                            modifiedText = modifiedText.Substring(index + 2);

                            //adiciona a imagem no texto
                            p.Inlines.Add(GetImageFromEmoticon(EmoticonList[emoticonKey]));

                            index = modifiedText.IndexOf(emoticonKey);
                        }

                        /*
                        if (achouIndex)
                        {
                            listIndexEmoticon.Add(emoticonKey, listIndex);
                        }
                        */
                    }
                }
            }

            /*
            modifiedText = text;

            //Faz os replaces
            foreach (string emoticonKey in listIndexEmoticon.Keys)
            {
                foreach (int indexIco in listIndexEmoticon[emoticonKey])
                {
                    flushText = modifiedText.Remove(indexIco);

                    if (!flushText.Equals(String.Empty))
                    {
                        p.Inlines.Add(FormatRun(user, flushText));
                    }

                    modifiedText = modifiedText.Substring(indexIco + 2);

                    //adiciona a imagem no texto
                    p.Inlines.Add(GetImageFromEmoticon(EmoticonList[emoticonKey]));
                }

            }*/

        }

        private Image GetImageFromEmoticon(Uri adress)
        {
            BitmapImage bitmap = new BitmapImage(adress);
            Image image = new Image();
            image.Source = bitmap;
            image.Width = 14;

            return image;
        }

        private void Icon_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
