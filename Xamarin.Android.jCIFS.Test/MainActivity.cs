using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jcifs;
using Jcifs.Smb;
using Jcifs.Netbios;
using Java.Net;
namespace samba
{
	[Activity (Label = "samba", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			// Get our button from the layout resource,
			// and attach an event to it
			Button button1 = FindViewById<Button> (Resource.Id.myButton);
            button1.Click += delegate {
                Button button = FindViewById<Button>(Resource.Id.myButton);
                Config.RegisterSmbURLHandler();
                Config.SetProperty("jcifs.smb.client.lport", "8137");
                Config.SetProperty("jcifs.encoding", "Cp1252");
                Config.SetProperty("jcifs.smb.lmCompatibility", "0");
                Config.SetProperty("jcifs.netbios.hostname", "AndroidPhone");
                getFileContents2();
                //getFileContents();
            };
		}

        // This is NOT best-practice code, just showing a demo Jcifs api call
        public async Task getFileContents2()
        {
            await Task.Run(() => {
                var lan = new SmbFile("smb://", NtlmPasswordAuthentication.Anonymous);
               // var workgroups = lan.ListFiles();
                UniAddress u = UniAddress.GetByName("ALEXEY-PC");
                Button button = FindViewById<Button>(Resource.Id.myButton);
                RunOnUiThread(() => {
                     button.Text = u.ToString();
                });
            }
            ).ContinueWith((Task arg) => {
                Console.WriteLine(arg.Status);
                if (arg.Status == TaskStatus.Faulted)
                    Console.WriteLine(arg.Exception);
            }
            );
        }

        // This is NOT best-practice code, just showing a demo Jcifs api call
        public async Task getFileContents ()
		{
			await Task.Run (() => {
                var smbStream = new SmbFileInputStream ("smb://user:1@file_server//d.txt");
				byte[] b = new byte[8192];
				int n;
				while ((n = smbStream.Read (b)) > 0) {
					Console.Write (Encoding.UTF8.GetString (b).ToCharArray (), 0, n);
				}
				Button button = FindViewById<Button> (Resource.Id.myButton);
				RunOnUiThread(() => {
					button.Text = Encoding.UTF8.GetString (b);
				});
			}
			).ContinueWith ((Task arg) => {
				Console.WriteLine (arg.Status);
				if (arg.Status == TaskStatus.Faulted)
					Console.WriteLine (arg.Exception);
			}
			);
		}
	}
}


