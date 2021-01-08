using System;
using System.IO;
using Newtonsoft.Json;


namespace FastProjector.MapGenerator.DevTools
{
    internal static class Logger
    {
        public static string LogPath {get;set;} = "/home/farid/Debug.log";

        public static void RemoveFile()
        {
            if(File.Exists(LogPath))
            {
                File.Delete(LogPath);
            }

        }
        public static void Log(string log)
        {
             if(!File.Exists(LogPath)){
                 File.Create(LogPath).Close();
                 
             }
            using var writer = File.AppendText(LogPath);
            writer.WriteLine("\n" + DateTime.Now.ToLocalTime());
            writer.WriteLine(log);
        }

        public static void Log(object log)
        {
              if(!File.Exists(LogPath)){
                 File.Create(LogPath).Close();
                 
             }
             using (var writer = File.AppendText(LogPath))
             {
                 writer.WriteLine("\n" + DateTime.Now.ToLocalTime());
                   writer.WriteLine(JsonConvert.SerializeObject(log));
                 //writer.WriteLine(ObjectDumper.Dump(log));
             }
        }
    }
}