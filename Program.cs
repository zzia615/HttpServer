using System;
using System.Net.Http;
using System.Net;
using System.Text;
using System.IO;
namespace AspnetWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:8787/");
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            httpListener.Start();
            while(true){
                var context = httpListener.GetContext();
                var request = context.Request;
                var response = context.Response;
                try
                {
                    var fileName = request.Url.LocalPath;
                    if(fileName.Equals("/")){
                        fileName = "index.html";
                    }
                    else
                    {
                        fileName = fileName.Substring(1);
                        fileName = fileName.Replace("/","\\");
                    }
                    var rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"cpts");
                    var fullName = Path.Combine(rootPath,fileName);
                    var lastAttr = fullName.Substring(fullName.LastIndexOf(".")+1,fullName.Length -fullName.LastIndexOf(".")-1 );
                    var parm = "";
                    if(!string.IsNullOrEmpty(request.Url.Query))
                    {
                        parm=request.Url.Query.Substring(1);
                    }
                    
                    var bytes = File.ReadAllBytes(fullName);
                    response.ContentEncoding = Encoding.UTF8;
                    if(lastAttr.ToUpper().Equals("HTML")||lastAttr.ToUpper().Equals("HTM")){
                        response.ContentType = "text/html;charset=utf-8";
                    }else{
                        response.ContentType = "text/plain;charset=utf-8";
                    }
                    response.ContentLength64 = bytes.Length;
                    var stream = response.OutputStream;
                    stream.Write(bytes,0,bytes.Length);
                    stream.Close();
                }
                catch (System.Exception ex)
                {
                    var errMsg = ex.ToString();
                    var bytes = Encoding.UTF8.GetBytes(errMsg);
                    var stream = response.OutputStream;
                    stream.Write(bytes,0,bytes.Length);
                    stream.Close();
                }
                
            }
        }
    }
}
