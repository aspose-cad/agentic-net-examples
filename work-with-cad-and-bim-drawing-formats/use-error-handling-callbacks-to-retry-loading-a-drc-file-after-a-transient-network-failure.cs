using System;using System.IO;using System.Net;using System.Net.Http;using System.Threading.Tasks;using Aspose.CAD;
class Program {
 static bool Transient(HttpStatusCode code)=>code==HttpStatusCode.RequestTimeout||code==(HttpStatusCode)429||(int)code>=500;
 static async Task<string> DownloadWithRetry(Uri uri,Action<int,string> onRetry) {
  if(uri.Scheme!="https"&&!(uri.IsLoopback&&uri.Scheme=="http"))throw new ArgumentException("Use HTTPS, or loopback HTTP for a local retry test.");
  Directory.CreateDirectory("output");string path="output/downloaded.drc";
  using(var client=new HttpClient{Timeout=TimeSpan.FromSeconds(10)}) {
   for(int attempt=1;attempt<=3;attempt++) {
    try {using(var response=await client.GetAsync(uri)){response.EnsureSuccessStatusCode();byte[] bytes=await response.Content.ReadAsByteArrayAsync();if(bytes.Length<5||bytes.Length>16*1024*1024||System.Text.Encoding.ASCII.GetString(bytes,0,5)!="DRACO")throw new InvalidDataException("The response is not a bounded DRACO file.");File.WriteAllBytes(path,bytes);return path;}}
    catch(HttpRequestException ex)when(!ex.StatusCode.HasValue||Transient(ex.StatusCode.Value)){onRetry(attempt,ex.Message);if(attempt==3)throw;await Task.Delay(50);}
    catch(TaskCanceledException ex){onRetry(attempt,ex.Message);if(attempt==3)throw;await Task.Delay(50);}
   }
  }
  throw new InvalidOperationException("Retry attempts exhausted.");
 }
 static async Task<int> Main(string[] args){try{if(args.Length>1)throw new ArgumentException("Usage: example [https://host/model.drc]");string input=args.Length>0?await DownloadWithRetry(new Uri(args[0]),(attempt,message)=>Console.Error.WriteLine("Application retry callback "+attempt+": "+message)):"input.drc";if(!File.Exists(input)){Console.Error.WriteLine("Input file not found: "+input);return args.Length==0?0:1;}using(Image image=Image.Load(input)){Console.WriteLine("Loaded DRACO model: "+image.GetType().FullName);}Console.WriteLine("Verified task 44118");return 0;}catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}}
}
