using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServerSentEventsWebAPI.Models;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
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

        #endregion /Properties

        #region Constructors

        static ChatController()
        {
            clients = new ConcurrentBag<StreamWriter>();
        }

        #endregion

        #region Methods

        //[HttpGet]
        //public async Task GetAsync()
        //{
        //Response.Headers.Add("Content-Type", "text/event-stream");
        //    Response.Headers["Cache-Control"] = "no-cache";
        //    Response.Headers["X-Accel-Buffering"] = "no";
        //    Response.ContentType = "text/event-stream";

        //string[] data = new string[] {
        //    "Hello World!",
        //    "Hello Galaxy!",
        //    "Hello Universe!"
        //};
        //for (int i = 0; i < data.Length; i++)
        //{
        //    await Task.Delay(TimeSpan.FromSeconds(5));
        //    string dataItem = $"data: {data[i]}\n\n";
        //   // byte[] dataItemBytes = ASCIIEncoding.ASCII.GetBytes(dataItem);
        //    //await Response.Body.WriteAsync(dataItemBytes, 0, dataItemBytes.Length);
        //    await Response.WriteAsync(dataItem);
        //    await Response.Body.FlushAsync();
        //}

        //    for (var i = 0; true; ++i)
        //    {
        //        await Response.WriteAsync($"data: Controller {i} at {DateTime.Now}\r\r");
        //        //await Response.Body.FlushAsync();
        //        await Task.Delay(5 * 1000);
        //    }
        //}

        [HttpGet]
        public async Task SubscribeAsync()
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
        }

        [HttpPost]
        public async void Post(ChatMessage model)
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

            // await SendMessageAsync(model);
        }

        private async Task SendMessageAsync(ChatMessage chatMessage)
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

        #endregion /Methods

    }
}
