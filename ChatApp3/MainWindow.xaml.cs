using ChatInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IChatService Server;
        private static DuplexChannelFactory<IChatService> _channelFactory;
        public MainWindow()
        {
            InitializeComponent();
            _channelFactory = new DuplexChannelFactory<IChatService>(new ClientCallback(), "ChatServiceEndpoint");
            Server = _channelFactory.CreateChannel();
        }
        public void TakeMessage(string message, string userName)
        {
            TBSent.Text += userName + ": " + message + "\n";
        }

        private void BTNSend_Click(object sender, RoutedEventArgs e)
        {
            Server.SendToAll(TBsend.Text, TBsignin.Text);
            TakeMessage(TBsend.Text, TBsignin.Text);
            TBsend.Text = "";
        }

        private void BTNsignin_Click(object sender, RoutedEventArgs e)
        {
            int returnValue = Server.Login(TBsignin.Text);
            if (returnValue == 1)
            {
                MessageBox.Show("You are already signed in!");
                
            }
            else if (returnValue == 0)
            {
                MessageBox.Show("You are signed in!");
                TBsignin.IsEnabled = false;
                BTNsignin.IsEnabled = false;
            }
        }
    }
}
