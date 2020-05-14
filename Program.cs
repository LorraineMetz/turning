using System;
using System.IO;
using Turning.Services;
using WebWindows.Blazor;

namespace Turning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);


            ComponentsDesktop.Run<Startup>("Turning v1.0", "wwwroot/_Host.html");

            NginxService.Stop();
        }
    }
}
