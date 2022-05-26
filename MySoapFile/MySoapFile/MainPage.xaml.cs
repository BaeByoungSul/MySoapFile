using BBS;
using MySoapFile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace MySoapFile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            btnDownload.Clicked += BtnDownload_Clicked;
            btnSaveFile.Clicked += BtnSaveFile_Clicked;
            btnReadFile.Clicked += BtnReadFile_Clicked;
        }

        private async void BtnReadFile_Clicked(object sender, EventArgs e)
        {
            int iCnt = await  ReadCountAsync();
            Console.WriteLine(iCnt);
        }

        private async void BtnSaveFile_Clicked(object sender, EventArgs e)
        {
            var folderPath = DependencyService.Get<IExternalStorage>().GetPath();
            Console.WriteLine(folderPath);


            await SaveCountAsync(10);
        }

        public async Task SaveCountAsync(int count)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "count.txt");
            using (var writer = File.CreateText(backingFile))
            {
                await writer.WriteLineAsync(count.ToString());
            }
        }

        public async Task<int> ReadCountAsync()
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "count.txt");

            if (backingFile == null || !File.Exists(backingFile))
            {
                return 0;
            }

            var count = 0;
            using (var reader = new StreamReader(backingFile, true))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (int.TryParse(line, out var newcount))
                    {
                        count = newcount;
                    }
                }
            }

            return count;
        }

        private void BtnDownload_Clicked(object sender, EventArgs e)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxBufferSize = 65536;
            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = 2147483647;
            
            binding.Security.Mode = BasicHttpSecurityMode.None;


            //EndpointAddress address = new EndpointAddress("http://20.227.136.125:9099/FileService");

            EndpointAddress address = new EndpointAddress("http://172.20.105.36:9099/FileService");

            ChannelFactory<IFileService> factory =
                new ChannelFactory<IFileService>(binding, address);

            IFileService channel = factory.CreateChannel();

            DownloadRequest request = new DownloadRequest()
            {
                FileName= "com.nakdong.mysoapdb.apk",
                FileVersion="1.1",
                CheckVersion=false

            };
            DownloadResponse response = channel.Download(request);

            CustomStream customStream = new CustomStream(response.MyStream, response.FileLength);
            customStream.ProgressChanged += DownloadProgressChanged;


            //            string documents = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            var folderPath = DependencyService.Get<IExternalStorage>().GetPath();
            string targetFilePath = Path.Combine(folderPath, request.FileName);

            FileStream targetStream = File.Create(targetFilePath);

            
            //FileStream targetStream = File.Create(@"d:\");
            customStream.CopyTo(targetStream);
            targetStream.Close();

            ((IClientChannel)channel).Close();
            factory.Close();

        }

        private void DownloadProgressChanged(object sender, Models.ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
