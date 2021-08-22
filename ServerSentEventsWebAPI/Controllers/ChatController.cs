using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServerSentEventsWebAPI.Models;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ServerSentEventsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        #region Properties

        private static ConcurrentBag<StreamWriter> clients;

        private static ConcurrentBag<ChatMessage> chatMessages;

        #endregion /Properties

        #region Constructors

        static ChatController()
        {
            clients = new ConcurrentBag<StreamWriter>();
            chatMessages = new ConcurrentBag<ChatMessage>();
        }

        #endregion

        #region Methods

        //[HttpGet]
        //public async Task Get()
        //{
        //    Response.Headers["Cache-Control"] = "no-cache";
        //    Response.Headers["X-Accel-Buffering"] = "no";
        //    Response.ContentType = "text/event-stream";
        //    for (var i = 0; true; ++i)
        //    {
        //        await Response.WriteAsync($"data: Controller {i} at {DateTime.Now}\r\r");
        //        //await Response.Body.FlushAsync();
        //        await Task.Delay(5 * 1000);
        //    }
        //}
        [HttpGet]
        public async Task GetAsync()
        {
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["X-Accel-Buffering"] = "no";
            Response.ContentType = "text/event-stream";
            using (var client = new StreamWriter(Response.Body))
            {
                lock (clients)
                {
                    clients.Add(client);
                }
                await client.WriteAsync("event: connected\ndata:\n\n");
                await client.FlushAsync();
                try
                {
                    await Task.Delay(Timeout.Infinite, HttpContext.RequestAborted);
                }
                catch (TaskCanceledException ex)
                {

                }
                lock (clients)
                {
                    StreamWriter ignore;
                    clients.TryTake(out ignore);
                    client.DisposeAsync();
                }
            }

            //Response.Headers.Add("Content-Type", "text/event-stream");
            ////string[] data = new string[] {
            ////    "Hello World!",
            ////    "Hello Galaxy!",
            ////    "Hello Universe!"
            ////};
            ////for (int i = 0; i < data.Length; i++)
            ////{
            ////    await Task.Delay(TimeSpan.FromSeconds(5));
            ////    string dataItem = $"data: {data[i]}\n\n";
            ////   // byte[] dataItemBytes = ASCIIEncoding.ASCII.GetBytes(dataItem);
            ////    //await Response.Body.WriteAsync(dataItemBytes, 0, dataItemBytes.Length);
            ////    await Response.WriteAsync(dataItem);
            ////    await Response.Body.FlushAsync();
            ////}
            ////for (var i = 0; true; ++i)
            ////{                
            ////    await Response.WriteAsync($"data: Controller {i} at {DateTime.Now}\r\r");
            ////    await Response.Body.FlushAsync();
            ////    await Task.Delay(TimeSpan.FromSeconds(5));
            ////}

            //while (chatMessages.TryPeek(out ChatMessage chatMessage))
            //{
            //    var chatMessageJson = JsonConvert.SerializeObject(chatMessage);
            //    await Response.WriteAsync($"data: {chatMessageJson}\n\n");
            //    //await Task.Delay(TimeSpan.FromSeconds(5));
            //    await Response.Body.FlushAsync();
            //    await Task.Delay(TimeSpan.FromSeconds(5));
            //}

            //Response.Headers.Add("Content-Type", "text/event-stream");
            //EventHandler<ChatMessage> onMessageCreated = async (sender, eventArgs) =>
            //{
            //    try
            //    {
            //        var message = eventArgs.Message;
            //        var messageJson = JsonConvert.SerializeObject(message, jsonSettings);
            //        await Response.WriteAsync($"data:{messageJson}\n\n");
            //        await Response.Body.FlushAsync();
            //    }
            //    catch (Exception)
            //    {
            //        // TODO: log error
            //    }
            //};

            //var response = Response;
            //response.ContentType = "text/event-stream";
            //var sourceStream = new PushStreamContent((stream, headers, context) =>
            //{
            //    OnStreamAvailable(stream, headers, context);
            //}, "text/event-stream");
            //await sourceStream.CopyToAsync(response.Body);      

            //string reponseType = "text/event-stream";
            //var response = new HttpResponseMessage(HttpStatusCode.Accepted)
            //{
            //    Content = new PushStreamContent((a, b, c) =>
            //    { OnStreamAvailable(a, b, c); }, reponseType)
            //};
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue(reponseType);
        }

        //[HttpGet]
        //public HttpResponseMessage Message()
        ////public void Get()
        //{
        //    //Response.ContentType = "text/event-stream";

        //    //DateTime startDate = DateTime.Now;
        //    //while (startDate.AddMinutes(1) > DateTime.Now)
        //    //{
        //    //    Response.WriteAsync(string.Format("data: {0}\n\n", DateTime.Now.ToString()));
        //    //    //Response.Body.FlushAsync();

        //    //    System.Threading.Thread.Sleep(1000);
        //    //}
        //    // return Response;

        //    //var obj = new { id = 1, name = "sdf" };
        //    //Response.ContentType = "text/event-stream";
        //    //string jsonCustomer = JsonConvert.SerializeObject(obj);
        //    //string data = $"data: {jsonCustomer}\n\n";
        //    //System.Threading.Thread.Sleep(5000);
        //    //HttpContext.Response.WriteAsync(data).Wait();
        //    //HttpContext.Response.Body.FlushAsync();
        //    //Response.Body.Close(); 

        [HttpPost]
        public async void PostAsync(ChatMessage model)
        {
            async Task Send(StreamWriter client)
            {
                try
                {
                    var data = JsonConvert.SerializeObject(model);
                    await client.WriteAsync($"data: {data}\n\n");
                    await client.FlushAsync();
                }
                catch (ObjectDisposedException ex)
                {
                    
                    lock (clients)
                    {
                        //StreamWriter ignore;
                        //clients.TryTake(out ignore);
                        //client.Dispose();
                    }
                }
            }

            await Task.WhenAll(clients.Select(Send));

            // await SendMessage(model);
        }

        private async Task SendMessage(ChatMessage chatMessage)
        {
            foreach (var client in clients)
            {
                try
                {
                    var data = JsonConvert.SerializeObject(chatMessage);
                    await client.WriteAsync($"data: {data}\n\n");
                    await client.FlushAsync();
                }
                catch (ObjectDisposedException ex)
                {
                    lock (clients)
                    {
                        StreamWriter ignore;
                        clients.TryTake(out ignore);
                        //client.Dispose();
                    }
                }
            }
        }

        //private async Task ChatCallbackMsg(ChatMessage chatMessage)
        //{
        //    foreach (var client in clients)
        //    {
        //        try
        //        {
        //            //var data = string.Format("data:{0}|{1}|{2}\n\n", model.Username, model.Message, model.SentDateTime);
        //            //await client.WriteAsync(data);
        //            //await client.FlushAsync();
        //            //await client.DisposeAsync();
        //            var data = JsonConvert.SerializeObject(chatMessage);
        //            await client.WriteAsync($"data: {data}\n\n");
        //            await client.FlushAsync();
        //            await client.DisposeAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            StreamWriter ignore;
        //            clients.TryTake(out ignore);
        //        }
        //    }
        //}

        private void OnStreamAvailable(Stream stream, HttpContent content, TransportContext context)
        {
            var client = new StreamWriter(stream);
            clients.Add(client);
        }

        #endregion /Methods

    }
}
