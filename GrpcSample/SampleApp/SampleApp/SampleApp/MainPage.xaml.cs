using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
            MessagingCenter.Subscribe<object, XamarinGrpcSampleServer.HelloRequest>(this, "receivedmsg", (o, h) =>
            {
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() => 
                {
                    var viewModel = BindingContext as MainViewModel;
                    viewModel.MsgList.Add(new MsgModel { Msg = h.Name });
                });
            });
        }

        Channel channel;

        private void SendBtn_Clicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as MainViewModel;

            string user = MsgInput.Text;

            var client = new XamarinGrpcSampleClient.Greeter.GreeterClient(channel);

            var reply = client.SayHello(new XamarinGrpcSampleClient.HelloRequest { Name = user });

            if (!string.IsNullOrEmpty(reply.Message))
            {
                viewModel.MsgList.Add(new MsgModel { Msg = $"target had read." });
            }
            
        }

        private void ConnectBtn_Clicked(object sender, EventArgs e)
        {
            channel = new Channel(IpInput.Text, ChannelCredentials.Insecure);            
        }
    }
    public class MainViewModel
    {
        public MainViewModel()
        {
            MsgList = new ObservableCollection<MsgModel>();
        }

        public ObservableCollection<MsgModel> MsgList { get; set; }
    }

    public class MsgModel
    {
        public string Msg { get; set; }
    }

}
