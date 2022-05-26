using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySoapFile.Droid;
using MySoapFile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Environment = Android.OS.Environment;

[assembly:Dependency(typeof(ExternalStorage_Droid))]
namespace MySoapFile.Droid
{
    public class ExternalStorage_Droid : IExternalStorage
    {
        public string GetPath()
        {
            Context context = Android.App.Application.Context;
            //var filePath = context.GetExternalFilesDir("");
            var filePath = context.GetExternalFilesDir(Environment.DirectoryDownloads);

            return filePath.Path;

        }
    }
}