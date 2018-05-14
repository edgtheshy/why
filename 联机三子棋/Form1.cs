using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 联机三子棋
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        bool canStart = false;
        static bool isPlayer1 = true;//是否是玩家一，玩家一红色棋，玩家二是蓝色棋子
        bool CanAddChess = true;//允不允许走棋
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        class Cell
        {
            bool isCellRed;//有红色的棋子
            public bool IsCellRed
            {
                get { return isCellRed; }
                set { isCellRed = value; }
            }
            bool isCellBlue;//有蓝色的棋子
            public bool IsCellBlue
            {
                get { return isCellBlue; }
                set { isCellBlue = value; }
            }
        }
        Cell[] cell = new Cell[9];

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = 400;
            this.Height = 400;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            btnConInternet.Location = new Point(320, 10);
            lblState.Location = new Point(320, 50);
            lblCanStart.Location = new Point(320, 80);
            DrawChessBoard();
            ClassCellInitialize();
            
        }

        /**
         * 画棋盘
         * */
        private void DrawChessBoard()
        {
            MessageBox.Show("新游戏开始");
            Graphics graphics = this.CreateGraphics();
            Pen pen = new Pen(Color.Blue, 2);
            //竖线
            graphics.DrawLine(pen, 10, 10, 10, 310);
            graphics.DrawLine(pen, 110, 10, 110, 310);
            graphics.DrawLine(pen, 210, 10, 210, 310);
            graphics.DrawLine(pen, 310, 10, 310, 310);

            //横线
            graphics.DrawLine(pen, 10, 10, 310, 10);
            graphics.DrawLine(pen, 10, 110, 310, 110);
            graphics.DrawLine(pen, 10, 210, 310, 210);
            graphics.DrawLine(pen, 10, 310, 310, 310);
        }

        /**
         * 初始化格子类
         * */
        private void ClassCellInitialize()
        {
            for (int i = 0; i < 9; i++)//cell类初始化
            {
                cell[i] = new Cell
                {
                    IsCellRed = false,
                    IsCellBlue = false
                };
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (canStart == true)
            {
                if (CanAddChess == true)
                {
                    int x = e.X;
                    int y = e.Y;
                    AddChess(x, y);
                    if (lblState.Text.Trim() == "连接成功")
                    {
                        string sendStr = x.ToString() + "+" + y.ToString();
                        byte[] bs = Encoding.ASCII.GetBytes(sendStr);
                        socket.Send(bs, bs.Length, 0);//发送测试信息               
                    }
                }
            }
        }

        /**
         * 接受服务器消息
         * */
        private void ReceiveServer()
        {
            while (true)
            {
                try
                {
                    string recvStr = "";
                    byte[] recvBytes = new byte[1024];
                    int bytes;
                    bytes = socket.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受信息
                    recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);

                    if (recvStr != "1" && recvStr != "2" && recvStr != "start")
                    {
                        string[] sArray = recvStr.Split('+');
                        int x = int.Parse(sArray[0]);
                        int y = int.Parse(sArray[1]);
                        AddChess(x, y);
                    }

                    if (recvStr == "1")
                    {
                        MessageBox.Show("你是玩家一");
                        CanAddChess = true;
                    }
                    if (recvStr == "2")
                    {
                        MessageBox.Show("你是玩家二");
                        CanAddChess = false;
                    }

                    if (recvStr == "start")
                    {
                        canStart = true;
                        lblCanStart.Text = "游戏开始";
                    }
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                    MessageBox.Show("出了个问题，请重启或者关闭游戏");
                }
            }
        }
        /**
        * 增加棋子
        * */
        static int count = 0;//几个格子有棋子了
        private void AddChess(int x, int y)
        {
            Graphics graphics = this.CreateGraphics();
            Brush brushRed = new SolidBrush(Color.Red);//填充的颜色红色
            Brush brushBlue = new SolidBrush(Color.Blue);//填充的颜色蓝色
            int result=2;
            //九个格子
            if ((x > 10 && x < 110) && (y > 10 && y < 110))
            {
                if (cell[0].IsCellRed == false && cell[0].IsCellBlue == false)
                {
                    if (isPlayer1)
                    {
                        graphics.FillEllipse(brushRed, 10, 10, 100, 100);
                        cell[0].IsCellRed = true;
                        result=IsGameWin();
                        count++;//添加了一个棋
                        CanAddChess =!CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    else
                    {
                        graphics.FillEllipse(brushBlue, 10, 10, 100, 100);
                        cell[0].IsCellBlue = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    isPlayer1 = !isPlayer1;//取反
                }
            }
            if ((x > 110 && x < 210) && (y > 10 && y < 110))
            {
                if (cell[1].IsCellRed == false && cell[1].IsCellBlue == false)
                {
                    if (isPlayer1)
                    {
                        graphics.FillEllipse(brushRed, 110, 10, 100, 100);
                        cell[1].IsCellRed = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    else
                    {
                        graphics.FillEllipse(brushBlue, 110, 10, 100, 100);
                        cell[1].IsCellBlue = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }

                    isPlayer1 = !isPlayer1;//取反
                }
            }
            if ((x > 210 && x < 310) && (y > 10 && y < 110))
            {
                if (cell[2].IsCellRed == false && cell[2].IsCellBlue == false)
                {
                    if (isPlayer1)
                    {
                        graphics.FillEllipse(brushRed, 210, 10, 100, 100);
                        cell[2].IsCellRed = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    else
                    {
                        graphics.FillEllipse(brushBlue, 210, 10, 100, 100);
                        cell[2].IsCellBlue = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }

                    isPlayer1 = !isPlayer1;//取反
                }
            }

            if ((x > 10 && x < 110) && (y > 110 && y < 210))
            {
                if (cell[3].IsCellRed == false && cell[3].IsCellBlue == false)
                {
                    if (isPlayer1)
                    {
                        graphics.FillEllipse(brushRed, 10, 110, 100, 100);
                        cell[3].IsCellRed = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    else
                    {
                        graphics.FillEllipse(brushBlue, 10, 110, 100, 100);
                        cell[3].IsCellBlue = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }

                    isPlayer1 = !isPlayer1;//取反
                }
            }
            if ((x > 110 && x < 210) && (y > 110 && y < 210))
            {
                if (cell[4].IsCellRed == false && cell[4].IsCellBlue == false)
                {
                    if (isPlayer1)
                    {
                        graphics.FillEllipse(brushRed, 110, 110, 100, 100);
                        cell[4].IsCellRed = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    else
                    {
                        graphics.FillEllipse(brushBlue, 110, 110, 100, 100);
                        cell[4].IsCellBlue = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }

                    isPlayer1 = !isPlayer1;//取反
                }
            }
            if ((x > 210 && x < 310) && (y > 110 && y < 210))
            {
                if (cell[5].IsCellRed == false && cell[5].IsCellBlue == false)
                {
                    if (isPlayer1)
                    {
                        graphics.FillEllipse(brushRed, 210, 110, 100, 100);
                        cell[5].IsCellRed = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    else
                    {
                        graphics.FillEllipse(brushBlue, 210, 110, 100, 100);
                        cell[5].IsCellBlue = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }

                    isPlayer1 = !isPlayer1;//取反
                }
            }

            if ((x > 10 && x < 110) && (y > 210 && y < 310))
            {
                if (cell[6].IsCellRed == false && cell[6].IsCellBlue == false)
                {
                    if (isPlayer1)
                    {
                        graphics.FillEllipse(brushRed, 10, 210, 100, 100);
                        cell[6].IsCellRed = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    else
                    {
                        graphics.FillEllipse(brushBlue, 10, 210, 100, 100);
                        cell[6].IsCellBlue = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }

                    isPlayer1 = !isPlayer1;//取反
                }
            }
            if ((x > 110 && x < 210) && (y > 210 && y < 310))
            {
                if (cell[7].IsCellRed == false && cell[7].IsCellBlue == false)
                {
                    if (isPlayer1)
                    {
                        graphics.FillEllipse(brushRed, 110, 210, 100, 100);
                        cell[7].IsCellRed = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    else
                    {
                        graphics.FillEllipse(brushBlue, 110, 210, 100, 100);
                        cell[7].IsCellBlue = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }

                    isPlayer1 = !isPlayer1;//取反
                }
            }
            if ((x > 210 && x < 310) && (y > 210 && y < 310))
            {
                if (cell[8].IsCellRed == false && cell[8].IsCellBlue == false)
                {
                    if (isPlayer1)
                    {
                        graphics.FillEllipse(brushRed, 210, 210, 100, 100);
                        cell[8].IsCellRed = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    else
                    {
                        graphics.FillEllipse(brushBlue, 210, 210, 100, 100);
                        cell[8].IsCellBlue = true;
                        result = IsGameWin();
                        count++;
                        CanAddChess = !CanAddChess;//走完后不许走，等另一个玩家走
                    }
                    isPlayer1 = !isPlayer1;//取反
                }
            }

           

            if (count == 9 && result == 2)
            {
                MessageBox.Show("平局，新的一局即将开始");

                graphics.Clear(this.BackColor);
                DrawChessBoard();
                ClassCellInitialize();
                count = 0;
            }
            if (result == 0)
            {
                MessageBox.Show("玩家二赢了");

                graphics.Clear(this.BackColor);
                DrawChessBoard();
                ClassCellInitialize();
                count = 0;
            }
            else if(result==1)
            {
                MessageBox.Show("玩家一赢了");

                graphics.Clear(this.BackColor);
                DrawChessBoard();
                ClassCellInitialize();
                count = 0;
            }
        }

        /**
         *判断是否结束
         * */
        private int IsGameWin()
        {           
            if ((cell[0].IsCellBlue == true && cell[1].IsCellBlue == true && cell[2].IsCellBlue == true) || (cell[3].IsCellBlue == true && cell[4].IsCellBlue == true && cell[5].IsCellBlue == true)|| (cell[6].IsCellBlue == true && cell[7].IsCellBlue == true && cell[8].IsCellBlue == true)||(cell[0].IsCellBlue == true && cell[3].IsCellBlue == true && cell[6].IsCellBlue == true)|| (cell[1].IsCellBlue == true && cell[4].IsCellBlue == true && cell[7].IsCellBlue == true)|| (cell[2].IsCellBlue == true && cell[5].IsCellBlue == true && cell[8].IsCellBlue == true)||(cell[0].IsCellBlue == true && cell[4].IsCellBlue == true && cell[8].IsCellBlue == true)||(cell[2].IsCellBlue == true && cell[4].IsCellBlue == true && cell[6].IsCellBlue == true))
            {
                return 0;//蓝色的赢
            }
            if ((cell[0].IsCellRed == true && cell[1].IsCellRed == true && cell[2].IsCellRed == true) || (cell[3].IsCellRed == true && cell[4].IsCellRed == true && cell[5].IsCellRed == true) || (cell[6].IsCellRed == true && cell[7].IsCellRed == true && cell[8].IsCellRed == true) || (cell[0].IsCellRed == true && cell[3].IsCellRed == true && cell[6].IsCellRed == true) || (cell[1].IsCellRed == true && cell[4].IsCellRed == true && cell[7].IsCellRed == true) || (cell[2].IsCellRed == true && cell[5].IsCellRed == true && cell[8].IsCellRed == true) || (cell[0].IsCellRed == true && cell[4].IsCellRed == true && cell[8].IsCellRed == true) || (cell[2].IsCellRed == true && cell[4].IsCellRed == true && cell[6].IsCellRed == true))
            {
                return 1;//红色的赢
            }
            else
                return 2;//未分胜负               
        }

        /**
        *客户端连接服务器
        * */
        private void button1_Click(object sender, EventArgs e)
        {
            if (lblState.Text.Trim() != "连接成功")
            {
                try
                {
                    int port = 2000;
                    string host = "127.0.0.1";
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint iPEnd = new IPEndPoint(ip, port);
                   
                    socket.Connect(iPEnd);
                    lblState.Text = "连接成功";

                    Thread ReceiveThread = new Thread(ReceiveServer);
                    ReceiveThread.IsBackground = true;
                    ReceiveThread.Start();
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                }
            }
            //try
            //{
            //    int port = 2000;
            //    string host = "127.0.0.1";
            //    IPAddress ip = IPAddress.Parse(host);
            //    IPEndPoint iPEnd = new IPEndPoint(ip, port);
            //    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    socket.Bind(iPEnd);
            //    socket.Listen(10);
            //    lblState.Text = "等待连接";
            //    Socket temp = socket.Accept();
            //    lblState.Text = "连接成功";
            //}
            //catch (SocketException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    MessageBox.Show(ex.StackTrace);
            //}
        }

        /**
         * 释放资源
         * */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            socket.Close();
        }
    }
}