using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Net.Sockets;

namespace Tic_Tac_Toe_Game
{ 
    public partial class Game : Form
    {
        string SoundDir;
        private SoundPlayer click;
        private SoundPlayer winner;
        private SoundPlayer loser;
        public Game(bool isHost, string ip = null)
        {
            InitializeComponent();
            MessageReceiver.DoWork += MessageReceiver_DoWork;
            CheckForIllegalCrossThreadCalls = false;

            if(isHost)
            {
                PlayerChar = 'X';
                OpponentChar = 'O';
                    server = new TcpListener(System.Net.IPAddress.Any, 8080);
                    server.Start();
                    sock = server.AcceptSocket();
            }
            else
            {
                PlayerChar = 'O';
                OpponentChar = 'X';
                try
                {
                    client = new TcpClient(ip, 8080);
                    sock = client.Client;
                    MessageReceiver.RunWorkerAsync();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }

        private void MessageReceiver_DoWork(object sender, DoWorkEventArgs e)
        {
            if (CheckState())
                return;
            FreezeBoard();
            turn.Text = "Opponent's Turn!";
            ReceiveMove();
            turn.Text = "Your Trun!";
            if (!CheckState())
                UnfreezeBoard();
        }

        private char PlayerChar;
        private char OpponentChar;
        private Socket sock;
        private BackgroundWorker MessageReceiver = new BackgroundWorker();
        private TcpListener server = null;
        private TcpClient client;


        private void Won() {
            winner.Play();
            FreezeBoard();
            turn.Text = "You Won!";
            MessageBox.Show("You Won!");
            
        }

        private void Lost()
        {
            loser.Play();
            FreezeBoard();
            turn.Text = "You Lost!";
            MessageBox.Show("You Lost!");
            
        }

        private bool CheckState()
        {
            
            if(button1.Text == button2.Text && button2.Text == button3.Text && button3.Text != "")
            {
                if(button1.Text[0] == PlayerChar)
                {
                   Won();
                }
                else
                { 
                    Lost();
                }
                return true;
            }

            else if (button4.Text == button5.Text && button5.Text == button6.Text && button6.Text != "")
            {
                if (button4.Text[0] == PlayerChar)
                {
                   Won();
                }
                else
                {
                    Lost();
                }
                return true;
            }

            else if (button7.Text == button8.Text && button8.Text == button9.Text && button9.Text != "")
            {
                if (button7.Text[0] == PlayerChar)
                {
                   Won();
                }
                else
                {
                    Lost();
                }
                return true;
            }

            else if (button1.Text == button4.Text && button4.Text == button7.Text && button7.Text != "")
            {
                if (button1.Text[0] == PlayerChar)
                {
                   Won();
                }
                else
                {
                    Lost();
                }
                return true;
            }

            else if (button2.Text == button5.Text && button5.Text == button8.Text && button8.Text != "")
            {
                if (button2.Text[0] == PlayerChar)
                {
                   Won();
                }
                else
                {
                    Lost();
                }
                return true;
            }

            else if (button3.Text == button6.Text && button6.Text == button9.Text && button9.Text != "")
            {
                if (button3.Text[0] == PlayerChar)
                {
                   Won();
                }
                else
                {
                    Lost();
                }
                return true;
            }

            else if (button1.Text == button5.Text && button5.Text == button9.Text && button9.Text != "")
            {
                if (button1.Text[0] == PlayerChar)
                {
                   Won();
                }
                else
                {
                    Lost();
                }
                return true;
            }

            else if (button3.Text == button5.Text && button5.Text == button7.Text && button7.Text != "")
            {
                if (button3.Text[0] == PlayerChar)
                {
                    winner.Play();
                    this.playing.Text = "You Won!";
                    Won();
                }
                else
                {
                    this.playing.Text = "You Lost!";
                    Lost();
                }
                return true;
            }

            else if(button1.Text != "" && button2.Text != "" && button3.Text != "" && button4.Text != "" && button5.Text != "" && button6.Text != "" && button7.Text != "" && button8.Text != "" && button9.Text != "")
            {
                turn.Text = "It's a draw!";
                MessageBox.Show("It's a draw!");
                return true;
            }
            return false;
        }
        private void FreezeBoard()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
        }
        private void restart_click(object sender, EventArgs e)
        {
            this.button1.Enabled = true;
            this.button2.Enabled = true;
            this.button3.Enabled = true;
            this.button4.Enabled = true;
            this.button5.Enabled = true;
            this.button6.Enabled = true;
            this.button7.Enabled = true;
            this.button8.Enabled = true;
            this.button9.Enabled = true;
            this.button1.Text = "";
            this.button2.Text = "";
            this.button3.Text = "";
            this.button4.Text = "";
            this.button5.Text = "";
            this.button6.Text = "";
            this.button7.Text = "";
            this.button8.Text = "";
            this.button9.Text = "";
            this.playing.Text = "Winner: N/A";
            this.turn.Text = "Playing:";
        }

        private void UnfreezeBoard()
        {
            if (button1.Text == "")
                button1.Enabled = true;
            if (button2.Text == "")
                button2.Enabled = true;
            if (button3.Text == "")
                button3.Enabled = true;
            if (button4.Text == "")
                button4.Enabled = true;
            if (button5.Text == "")
                button5.Enabled = true;
            if (button6.Text == "")
                button6.Enabled = true;
            if (button7.Text == "")
                button7.Enabled = true;
            if (button8.Text == "")
                button8.Enabled = true;
            if (button9.Text == "")
                button9.Enabled = true;
        }

        private void ReceiveMove()
        {
            byte[] buffer = new byte[1];
            try
            {
                sock.Receive(buffer);
            }
            catch (Exception e) 
            { 
                //this will avoid crashing the programming incase it is closed when the client is closed while server is running
            }
            if (buffer[0] == 1)
                button1.Text = OpponentChar.ToString();
            if (buffer[0] == 2)
                button2.Text = OpponentChar.ToString();
            if (buffer[0] == 3)
                button3.Text = OpponentChar.ToString();
            if (buffer[0] == 4)
                button4.Text = OpponentChar.ToString();
            if (buffer[0] == 5)
                button5.Text = OpponentChar.ToString();
            if (buffer[0] == 6)
                button6.Text = OpponentChar.ToString();
            if (buffer[0] == 7)
                button7.Text = OpponentChar.ToString();
            if (buffer[0] == 8)
                button8.Text = OpponentChar.ToString();
            if (buffer[0] == 9)
                button9.Text = OpponentChar.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            click.Play();
            byte[] num = { 1 };
            sock.Send(num);
            button1.Text = PlayerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            click.Play();
            byte[] num = { 2 };
            sock.Send(num);
            button2.Text = PlayerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            click.Play();
            byte[] num = { 3 };
            sock.Send(num);
            button3.Text = PlayerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            click.Play();
            byte[] num = { 4 };
            sock.Send(num);
            button4.Text = PlayerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            click.Play();
            byte[] num = { 5 };
            sock.Send(num);
            button5.Text = PlayerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            click.Play();
            byte[] num = { 6 };
            sock.Send(num);
            button6.Text = PlayerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            click.Play();
            byte[] num = { 7 };
            sock.Send(num);
            button7.Text = PlayerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            click.Play();
            byte[] num = { 8 };
            sock.Send(num);
            button8.Text = PlayerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            click.Play();
            byte[] num = { 9 };
            sock.Send(num);
            button9.Text = PlayerChar.ToString();
            MessageReceiver.RunWorkerAsync();
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            click.Play();
            MessageReceiver.WorkerSupportsCancellation = true;
            MessageReceiver.CancelAsync();
            if (server != null)
                server.Stop();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            string workingDirectory = Environment.CurrentDirectory;
            for (int i =0; i <= workingDirectory.Length - 10; i++)
            {
                SoundDir += workingDirectory[i];        
            }
            click = new SoundPlayer(@SoundDir+@"\sounds\click.wav");
            winner = new SoundPlayer(@SoundDir + @"\sounds\winner.wav");
            loser = new SoundPlayer(@SoundDir + @"\sounds\loser.wav");
            


        }
    }
}
