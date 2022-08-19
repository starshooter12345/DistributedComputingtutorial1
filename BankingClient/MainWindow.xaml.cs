using BankingServer;
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

namespace BankingClient
{
    
    
    public partial class MainWindow : Window
    {
        private ServerInterface.ServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<ServerInterface.ServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataService";
            //Below is creating the connection between the client and server
            try
            {
                foobFactory = new ChannelFactory<ServerInterface.ServerInterface>(tcp, URL);
                foob = foobFactory.CreateChannel();
                Total_Num.Text = foob.GetNumEntries().ToString();
            }
            catch
            {
                MessageBox.Show("Cannot connect to server, please restart the server and then start client again");
                return;
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            string fName = "", lName = "" , img="";
            int bal = 0;
            uint acct = 0, pin = 0;

            try
            {
                index = Int32.Parse(Index_Num.Text);
            }
            catch
            {
                MessageBox.Show("Enter a number!");
                return;
            }

            try
            {
                if (index > foob.GetNumEntries() || index <= 0)
                {
                    MessageBox.Show("Index is out of range!");
                    return;
                }
                else
                {
                    foob.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName, out img);
                }

                Image Image = new Image();
                Image.Width = 100;

                BitmapImage bmpImage = new BitmapImage();

                bmpImage.BeginInit();
                Console.WriteLine(img);
                bmpImage.UriSource = new Uri(img);
                bmpImage.DecodePixelWidth = 100;
                bmpImage.EndInit();
                Image.Source = bmpImage;


                FirstNameBox.Text = fName;
                LastNameBox.Text = lName;
                Balance_Box.Text = bal.ToString("C");
                AccountNoBox.Text = acct.ToString();
                PinBox.Text = pin.ToString("D4");
                image.Source = Image.Source;

            }
            catch
            {
                MessageBox.Show("Cannot connect to the server,please restart the server and then start the client again");
                return;
            }
        }
    }
}
