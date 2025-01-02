using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Ext
{
    public class SignalRHelper
    {
        public static async Task SendSignalR(string Pagename, string msg)
        {
            HubConnection connection = new HubConnectionBuilder().WithUrl(new Uri("http://192.168.100.121:8008/rebateHub")).Build();
            //HubConnection connection = new HubConnectionBuilder().WithUrl(new Uri("http://localhost:8008/rebateHub")).Build();
            try
            {

                // 手动启动连接
                await connection.StartAsync();
                // 发送消息到服务端
                await connection.InvokeAsync("SendMessagePage", Pagename, msg);
                // 关闭连接
                await connection.StopAsync();
            }
            catch
            {
            }
            finally
            {
                // 释放连接资源
                await connection.DisposeAsync();
            }
        }
    }
}
