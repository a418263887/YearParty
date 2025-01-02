using Microsoft.AspNetCore.SignalR.Client;
using System.Drawing.Drawing2D;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using Util.Ext;
using WinApp.QuartzScheduler;
using static WinApp.Program;

namespace WinApp
{
    public partial class MainForm : Form
    {

        [DllImport("user32.dll")]

        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]

        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int IParam);

        public const int WM_SYSCOMMAND = 0x0112;

        public const int SC_MOVE = 0xF010;

        public const int HTCAPTION = 0x0002;

        public bool IsDebug = true, IsManager = true;

        #region 设置圆角

        public void SetWindowRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            FormPath = WinGetRoundedRectPath(rect, 15);
            this.Region = new Region(FormPath);
        }
        private GraphicsPath WinGetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();
            //   左上角   
            path.AddArc(arcRect, 180, 90);
            //   右上角   
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            //   右下角   
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            //   左下角   
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();
            return path;
        }

        #endregion
        public MainForm()
        {
            this.MaximizeBox = false;//使最大化窗口失效
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;//禁止改变窗体大小        
            InitializeComponent();
            //将 ListBox 的 ScrollAlwaysVisible 属性设置为 true
            lb_msg.ScrollAlwaysVisible = true;


        }


        private void MainForm_Load(object sender, EventArgs e)
        {

            ulog = ViewLog;
            ulog2 = ViewLogSignalRAsync;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;//设置ssl的版本未tls 1.2
                ServicePointManager.DefaultConnectionLimit = 500;//设置ServicePoint的连接数为500 默认值只有2 
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((x, y, z, w) => true);

                Init();
            }
            catch (Exception ex)
            {

                Serilog.Log.Error(ex.Message);
                ViewLog(2, ex.Message);
            }

        }
        async void Init()
        {
            try
            {
                await RebateScheduler.Init();
                await FincialRebateScheduler.Init();
            }
            catch (Exception ex)
            {
                ViewLog(2, "任务启动出错" + ex.Message);
            }
        }

        async Task Stop()
        {
            try
            {
                await RebateScheduler.Stop();
                await FincialRebateScheduler.Stop();
            }
            catch (Exception ex)
            {
                ViewLog(2, "任务停止出错" + ex.Message);
            }

        }
        public async void ViewLogSignalRAsync(int type, string msg)
        {
            await SignalRHelper.SendSignalR("RebateJob", msg);
            //HubConnection connection = new HubConnectionBuilder().WithUrl(new Uri("http://192.168.100.22:8008/rebateHub")).Build();
            //try
            //{

            //    // 手动启动连接
            //    await connection.StartAsync();
            //    // 发送消息到服务端
            //    await connection.InvokeAsync("SendMessageUser", "RebateJob", msg);
            //    // 关闭连接
            //    await connection.StopAsync();
            //}
            //catch (Exception ex)
            //{
            //    ViewLog(2, "SignalR报错" + ex.Message);
            //}
            //finally
            //{
            //    // 释放连接资源
            //    await connection.DisposeAsync();
            //}

            string color = "信息";
            //Black  //Red  //Yellow //Purple  
            switch (type)
            {

                case 1: color = "信息"; break;
                case 2: color = "异常"; break;
                case 3: color = "警告"; break;
            }
            Serilog.Log.Information($"{color} {msg}");

            if (lb_msg.InvokeRequired)
            {
                // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                Action<string> actionDelegate = (x) =>
                {
                    if (this.lb_msg.Items.Count < 5000)
                    {
                        this.lb_msg.Items.Add($"{color}_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : {x}");
                    }
                    else
                    {
                        this.lb_msg.Items.Clear();
                        this.lb_msg.Items.Add($"{color}_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : {x}");
                    }
                };
                // 或者
                // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                this.lb_msg.Invoke(actionDelegate, msg);

                Action top = () =>
                {
                    if (lb_msg.Items.Count - 3 - lb_msg.ClientSize.Height / lb_msg.ItemHeight < lb_msg.TopIndex)
                    {
                        this.lb_msg.TopIndex = (this.lb_msg.Items.Count - (int)(this.lb_msg.Height / this.lb_msg.ItemHeight)) >= 0 ? (this.lb_msg.Items.Count - (int)(this.lb_msg.Height / this.lb_msg.ItemHeight)) : 0;
                    }
                };
                this.lb_msg.Invoke(top);
            }
            else
            {
                if (lb_msg.Items.Count < 5000)
                {
                    this.lb_msg.Items.Add($"{color}_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : {msg}");
                }
                else
                {
                    lb_msg.Items.Clear();
                    this.lb_msg.Items.Add($"{color}_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : {msg}");
                }
                if (lb_msg.Items.Count - 3 - lb_msg.ClientSize.Height / lb_msg.ItemHeight < lb_msg.TopIndex)
                {
                    this.lb_msg.TopIndex = (this.lb_msg.Items.Count - (int)(this.lb_msg.Height / this.lb_msg.ItemHeight)) >= 0 ? (this.lb_msg.Items.Count - (int)(this.lb_msg.Height / this.lb_msg.ItemHeight)) : 0;
                }
            }
        }
        public void ViewLog(int type, string msg)
        {


            string color = "信息";
            //Black  //Red  //Yellow //Purple  
            switch (type)
            {

                case 1: color = "信息"; break;
                case 2: color = "异常"; break;
                case 3: color = "警告"; break;
            }
            Serilog.Log.Information($"{color} {msg}");

            if (lb_msg.InvokeRequired)
            {
                // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                Action<string> actionDelegate = (x) =>
                {
                    if (this.lb_msg.Items.Count < 5000)
                    {
                        this.lb_msg.Items.Add($"{color}_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : {x}");
                    }
                    else
                    {
                        this.lb_msg.Items.Clear();
                        this.lb_msg.Items.Add($"{color}_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : {x}");
                    }
                };
                // 或者
                // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                this.lb_msg.Invoke(actionDelegate, msg);

                Action top = () =>
                {
                    if (lb_msg.Items.Count - 3 - lb_msg.ClientSize.Height / lb_msg.ItemHeight < lb_msg.TopIndex)
                    {
                        this.lb_msg.TopIndex = (this.lb_msg.Items.Count - (int)(this.lb_msg.Height / this.lb_msg.ItemHeight)) >= 0 ? (this.lb_msg.Items.Count - (int)(this.lb_msg.Height / this.lb_msg.ItemHeight)) : 0;
                    }
                };
                this.lb_msg.Invoke(top);
            }
            else
            {
                if (lb_msg.Items.Count < 5000)
                {
                    this.lb_msg.Items.Add($"{color}_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : {msg}");
                }
                else
                {
                    lb_msg.Items.Clear();
                    this.lb_msg.Items.Add($"{color}_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : {msg}");
                }
                if (lb_msg.Items.Count - 3 - lb_msg.ClientSize.Height / lb_msg.ItemHeight < lb_msg.TopIndex)
                {
                    this.lb_msg.TopIndex = (this.lb_msg.Items.Count - (int)(this.lb_msg.Height / this.lb_msg.ItemHeight)) >= 0 ? (this.lb_msg.Items.Count - (int)(this.lb_msg.Height / this.lb_msg.ItemHeight)) : 0;
                }
            }
        }

        //控制区域    用MouseDown控件
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            //拖动窗体
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            //改变鼠标样式
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        // 窗体size发生改变时重新设置Region属性
        private void MainForm_Resize(object sender, EventArgs e)
        {
            SetWindowRegion();
        }


        private void lb_msg_DrawItem_1(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index >= 0 && e.Index < lb_msg.Items.Count)
                {
                    if (e.Index < 0)
                    {
                        return;
                    }
                    e.DrawBackground();

                    string itemText = lb_msg.Items[e.Index].ToString();
                    Color textColor = Color.Black; // 默认颜色

                    // 根据文字内容或其他条件设置不同的颜色
                    if (itemText.StartsWith("异常"))
                    {
                        textColor = Color.Red;
                    }
                    else if (itemText.StartsWith("警告"))
                    {
                        textColor = Color.Orange;
                    }
                    using (Brush brush = new SolidBrush(textColor))
                    {
                        e.Graphics.DrawString(lb_msg.Items[e.Index].ToString(), e.Font, brush, e.Bounds);
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        //启动
        private async void btn_test_ClickAsync(object sender, EventArgs e)
        {
            ViewLog(1, "停止调度");
            btn_test.Enabled = false;
            await Stop();
            ViewLog(1, "全部任务停止完成");
            btn_test.Enabled = true;
        }

        //关闭
        private void labClose_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要退出程序吗？", "温馨提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                // 彻底关闭进程
                System.Environment.Exit(0);
            }
        }

        //恢复窗体
        //private void notifyIcon_DoubleClick(object sender, EventArgs e)
        //{
        //    if (this.WindowState == FormWindowState.Minimized)
        //    {
        //        //还原窗体
        //        this.WindowState = FormWindowState.Normal;
        //        //任务显示
        //        this.ShowInTaskbar = true;
        //    }
        //    //激活窗体
        //    this.Activate();
        //}

        //最小化到右下角
        private void labelMini_Click(object sender, EventArgs e)
        {
            //最小化主窗口
            this.WindowState = FormWindowState.Minimized;
            //任务栏取消图标
            ///this.ShowInTaskbar = false;
        }

    }
}
