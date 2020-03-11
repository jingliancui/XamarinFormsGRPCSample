using Grpc.Core;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinGrpcSampleServer;

namespace SampleApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try
            {
                var server = new Server
                {
                    Services = { XamarinGrpcSampleServer.Greeter.BindService(new GreeterImpl()) },

                    Ports = { new ServerPort("0.0.0.0", 54321, ServerCredentials.Insecure) }
                };

                server.Start();
            }
            catch (Exception ex)
            {
                var b = ex;
            }

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }

    public class GreeterImpl : XamarinGrpcSampleServer.Greeter.GreeterBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var currentPlatform = Xamarin.Essentials.DeviceInfo.Platform;
            var platform = "";
            if (currentPlatform == Xamarin.Essentials.DevicePlatform.Android)
            {
                platform = "android";
            }
            if (currentPlatform == Xamarin.Essentials.DevicePlatform.iOS)
            {
                platform = "ios";
            }
            MessagingCenter.Send(new object(), "receivedmsg", request);
            return Task.FromResult(new HelloReply { Message = platform + ": Hello " + request.Name });
        }
    }
}
