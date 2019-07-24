using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        int height = 10;
        int width = 10;
        Button[,] board;
        int[,] numboard;
        int[,] ezer1;
        Random rnd = new Random();
        int restart = 0;
        int time = 0;
        int minute = 0;

        Image flag = Properties.Resources.MineSweeper;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (restart == 1)
            {
                for (int shura = 0; shura < height; shura++)
                {
                    for (int amuda = 0; amuda < width; amuda++)
                    {
                        panel1.Controls.Remove(board[shura, amuda]);
                        board[shura, amuda].Dispose();
                    }
                }
                time = 0;
                minute = 0;
                textBox5.Text = " 00 : 00";
                timer1.Stop();
            }
            else
            {
                restart = 1;
            }
            timer1.Interval = 1000;
            try
            {
                height = int.Parse(textBox1.Text);
            }
            catch { };
            try
            { 
                width = int.Parse(textBox2.Text);
            }
            catch { };
            board = new Button[height, width];
            numboard = new int[height, width];

            int btnsize = 30;
            int i = 0;

            for (int shura = 0; shura < height; shura++)
                for (int amuda = 0; amuda < width; amuda++)
                {
                    numboard[shura, amuda] = -1;

                    board[shura, amuda] = new Button();
                    board[shura, amuda].Size = new Size(btnsize, btnsize);
                    board[shura, amuda].Location = new Point(amuda * btnsize, shura * btnsize);
                    board[shura, amuda].BackColor = Color.LightGray;
                    board[shura, amuda].Font = new Font("Arial", 14, FontStyle.Bold);
                    board[shura, amuda].Tag = i.ToString();
                    board[shura, amuda].BackgroundImageLayout = ImageLayout.Stretch;
                    board[shura, amuda].MouseDown += Form1_MouseDown;
                    panel1.Controls.Add(board[shura, amuda]);

                    i++;
                }
            panel1.Width = btnsize * board.GetLength(1) + 5;
            panel1.Height = btnsize * board.GetLength(0) + 5;
            this.Height = btnsize * board.GetLength(0) + 115;

            if (btnsize * board.GetLength(1) + 35 >= 288)
            {
                this.Width = btnsize * board.GetLength(1) + 35;
            }
            else
            {
                this.Width = 310;
                panel1.Location = new Point((288 - btnsize * board.GetLength(1)) / 2, 70);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            string n = (((Button)(sender)).Tag).ToString();
            int place = int.Parse(n);

            int Yplace = place / width;
            int Xplace = place % width;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (numboard[Yplace, Xplace] == 8)
                    numboard[Yplace, Xplace] = -1;
                else if (numboard[Yplace, Xplace] != -10)
                    numboard[Yplace, Xplace]++;

                if (numboard[Yplace, Xplace] != 0 && numboard[Yplace, Xplace] != -1 && numboard[Yplace,Xplace] != -10)
                    board[Yplace, Xplace].Text = numboard[Yplace, Xplace].ToString();
                else
                    board[Yplace, Xplace].Text = "";

                if (board[Yplace,Xplace].BackColor == Color.LightGreen)
                {
                    board[Yplace, Xplace].BackColor = Color.LightGray;
                }
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (numboard[Yplace, Xplace] != -10)
                {
                    numboard[Yplace, Xplace] = -1;
                    board[Yplace, Xplace].Text = "";
                }

            }

            #region color
            if (numboard[Yplace, Xplace] >= 0 && numboard[Yplace, Xplace] <= 8)
            {
                board[Yplace, Xplace].BackColor = Color.FromArgb(190, 190, 190);
                board[Yplace, Xplace].FlatStyle = FlatStyle.Flat;
                board[Yplace, Xplace].FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
                if (numboard[Yplace, Xplace] == 1)
                {
                    board[Yplace, Xplace].ForeColor = Color.Blue;
                }
                else if (numboard[Yplace, Xplace] == 2)
                {
                    board[Yplace, Xplace].ForeColor = Color.Green;
                }
                else if (numboard[Yplace, Xplace] == 3)
                {
                    board[Yplace, Xplace].ForeColor = Color.Red;
                }
                else if (numboard[Yplace, Xplace] == 4)
                {
                    board[Yplace, Xplace].ForeColor = Color.DarkBlue;
                }
                else if (numboard[Yplace, Xplace] == 5)
                {
                    board[Yplace, Xplace].ForeColor = Color.Brown;
                }
                else if (numboard[Yplace, Xplace] == 6)
                {
                    board[Yplace, Xplace].ForeColor = Color.FromArgb(20, 140, 160);
                }
                else if (numboard[Yplace, Xplace] == 7)
                {
                    board[Yplace, Xplace].ForeColor = Color.Black;
                }
                else if (numboard[Yplace, Xplace] == 8)
                {
                    board[Yplace, Xplace].ForeColor = Color.Gray;
                }
            }
            else if (numboard[Yplace, Xplace] == -1)
            {
                board[Yplace, Xplace].BackColor = Color.LightGray;
                board[Yplace, Xplace].FlatStyle = FlatStyle.Standard;
            }
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
            int didturn = 0;
            for (int shura = 0; shura < height; shura++)
            {
                for (int amuda = 0; amuda < width; amuda++)
                {
                    if (numboard[shura, amuda] >= 0)
                    {
                        #region PutFlag
                        int around = 0;

                        if (shura != height - 1 && amuda != width - 1 && numboard[shura + 1, amuda + 1] >= 0
                            && numboard[shura + 1, amuda + 1] <= 8)
                            around++;
                        if (shura != 0 && amuda != width - 1 && numboard[shura - 1, amuda + 1] >= 0
                            && numboard[shura - 1, amuda + 1] <= 8)
                            around++;
                        if (shura != height - 1 && amuda != 0 && numboard[shura + 1, amuda - 1] >= 0
                            && numboard[shura + 1, amuda - 1] <= 8)
                            around++;
                        if (shura != 0 && amuda != 0 && numboard[shura - 1, amuda - 1] >= 0
                            && numboard[shura - 1, amuda - 1] <= 8)
                            around++;
                        if (shura != height - 1 && numboard[shura + 1, amuda] >= 0 && numboard[shura + 1, amuda] <= 8)
                            around++;
                        if (shura != 0 && numboard[shura - 1, amuda] >= 0 && numboard[shura - 1, amuda] <= 8)
                            around++;
                        if (amuda != width - 1 && numboard[shura, amuda + 1] >= 0 && numboard[shura, amuda + 1] <= 8)
                            around++;
                        if (amuda != 0 && numboard[shura, amuda - 1] >= 0 && numboard[shura, amuda - 1] <= 8)
                            around++;

                        if (shura == height - 1 && amuda == width - 1)
                            around -= 1;
                        if (shura == 0 && amuda == width - 1)
                            around -= 1;
                        if (shura == height - 1 && amuda == 0)
                            around -= 1;
                        if (shura == 0 && amuda == 0)
                            around -= 1;
                        if (shura == height - 1)
                            around += 3;
                        if (shura == 0)
                            around += 3;
                        if (amuda == width - 1)
                            around += 3;
                        if (amuda == 0)
                            around += 3;

                        if (numboard[shura, amuda] == 8 - around)
                        {
                            if (shura != height - 1 && amuda != width - 1 && numboard[shura + 1, amuda + 1] == -1)
                            {
                                numboard[shura + 1, amuda + 1] = -10;
                                board[shura + 1, amuda + 1].BackgroundImage = flag;
                                didturn = 1;
                            }
                            if (shura != 0 && amuda != width - 1 && numboard[shura - 1, amuda + 1] == -1)
                            {
                                numboard[shura - 1, amuda + 1] = -10;
                                board[shura - 1, amuda + 1].BackgroundImage = flag;
                                didturn = 1;
                            }
                            if (shura != height - 1 && amuda != 0 && numboard[shura + 1, amuda - 1] == -1)
                            {
                                numboard[shura + 1, amuda - 1] = -10;
                                board[shura + 1, amuda - 1].BackgroundImage = flag;
                                didturn = 1;
                            }
                            if (shura != 0 && amuda != 0 && numboard[shura - 1, amuda - 1] == -1)
                            {
                                numboard[shura - 1, amuda - 1] = -10;
                                board[shura - 1, amuda - 1].BackgroundImage = flag;
                                didturn = 1;
                            }
                            if (shura != height - 1 && numboard[shura + 1, amuda] == -1)
                            {
                                numboard[shura + 1, amuda] = -10;
                                board[shura + 1, amuda].BackgroundImage = flag;
                                didturn = 1;
                            }
                            if (shura != 0 && numboard[shura - 1, amuda] == -1)
                            {
                                numboard[shura - 1, amuda] = -10;
                                board[shura - 1, amuda].BackgroundImage = flag;
                                didturn = 1;
                            }
                            if (amuda != width - 1 && numboard[shura, amuda + 1] == -1)
                            {
                                numboard[shura, amuda + 1] = -10;
                                board[shura, amuda + 1].BackgroundImage = flag;
                                didturn = 1;
                            }
                            if (amuda != 0 && numboard[shura, amuda - 1] == -1)
                            {
                                numboard[shura, amuda - 1] = -10;
                                board[shura, amuda - 1].BackgroundImage = flag;
                                didturn = 1;
                            }
                        }
                        #endregion

                        #region PutNumber
                        int flagsAround = 0;

                        if (shura != height - 1 && amuda != width - 1 && numboard[shura + 1, amuda + 1] == -10)
                            flagsAround++;
                        if (shura != 0 && amuda != width - 1 && numboard[shura - 1, amuda + 1] == -10)
                            flagsAround++;
                        if (shura != height - 1 && amuda != 0 && numboard[shura + 1, amuda - 1] == -10)
                            flagsAround++;
                        if (shura != 0 && amuda != 0 && numboard[shura - 1, amuda - 1] == -10)
                            flagsAround++;
                        if (shura != height - 1 && numboard[shura + 1, amuda] == -10)
                            flagsAround++;
                        if (shura != 0 && numboard[shura - 1, amuda] == -10)
                            flagsAround++;
                        if (amuda != width - 1 && numboard[shura, amuda + 1] == -10)
                            flagsAround++;
                        if (amuda != 0 && numboard[shura, amuda - 1] == -10)
                            flagsAround++;

                        if (numboard[shura, amuda] != 0 && numboard[shura, amuda] == flagsAround)
                        {
                            if (shura != height - 1 && amuda != width - 1 && numboard[shura + 1, amuda + 1] == -1)
                            {
                                board[shura + 1, amuda + 1].BackColor = Color.LightGreen;
                                didturn = 1;
                            }
                            if (shura != 0 && amuda != width - 1 && numboard[shura - 1, amuda + 1] == -1)
                            {
                                board[shura - 1, amuda + 1].BackColor = Color.LightGreen;
                                didturn = 1;
                            }
                            if (shura != height - 1 && amuda != 0 && numboard[shura + 1, amuda - 1] == -1)
                            {
                                board[shura + 1, amuda - 1].BackColor = Color.LightGreen;
                                didturn = 1;
                            }
                            if (shura != 0 && amuda != 0 && numboard[shura - 1, amuda - 1] == -1)
                            {
                                board[shura - 1, amuda - 1].BackColor = Color.LightGreen;
                                didturn = 1;
                            }
                            if (shura != height - 1 && numboard[shura + 1, amuda] == -1)
                            {
                                board[shura + 1, amuda].BackColor = Color.LightGreen;
                                didturn = 1;
                            }
                            if (shura != 0 && numboard[shura - 1, amuda] == -1)
                            {
                                board[shura - 1, amuda].BackColor = Color.LightGreen;
                                didturn = 1;
                            }
                            if (amuda != width - 1 && numboard[shura, amuda + 1] == -1)
                            {
                                board[shura, amuda + 1].BackColor = Color.LightGreen;
                                didturn = 1;
                            }
                            if (amuda != 0 && numboard[shura, amuda - 1] == -1)
                            {
                                board[shura, amuda - 1].BackColor = Color.LightGreen;
                                didturn = 1;
                            }
                        }
                        #endregion
                    }
                }
            }

            if (didturn == 0)
            {
                ezer1 = new int[height, width];
                for (int shura = 0; shura < height; shura++)
                {
                    for (int amuda = 0; amuda < width; amuda++)
                    {
                        #region Around
                        int around = 0;

                        if (numboard[shura, amuda] == -1)
                        {
                            if (shura != height - 1 && amuda != width - 1 && numboard[shura + 1, amuda + 1] >= 1
                                && numboard[shura + 1, amuda + 1] <= 8)
                                around++;
                            if (shura != 0 && amuda != width - 1 && numboard[shura - 1, amuda + 1] >= 1
                                && numboard[shura - 1, amuda + 1] <= 8)
                                around++;
                            if (shura != height - 1 && amuda != 0 && numboard[shura + 1, amuda - 1] >= 1
                                && numboard[shura + 1, amuda - 1] <= 8)
                                around++;
                            if (shura != 0 && amuda != 0 && numboard[shura - 1, amuda - 1] >= 1
                                && numboard[shura - 1, amuda - 1] <= 8)
                                around++;
                            if (shura != height - 1 && numboard[shura + 1, amuda] >= 1 && numboard[shura + 1, amuda] <= 8)
                                around++;
                            if (shura != 0 && numboard[shura - 1, amuda] >= 1 && numboard[shura - 1, amuda] <= 8)
                                around++;
                            if (amuda != width - 1 && numboard[shura, amuda + 1] >= 1 && numboard[shura, amuda + 1] <= 8)
                                around++;
                            if (amuda != 0 && numboard[shura, amuda - 1] >= 1 && numboard[shura, amuda - 1] <= 8)
                                around++;
                        }
                        #endregion

                        if (around > 0)
                        {
                            #region Number does not work - Put Flag
                            for (int row = 0; row < height; row++)
                                for (int col = 0; col < width; col++)
                                    ezer1[row, col] = numboard[row, col];

                            ezer1[shura, amuda] = 9;

                            int putflag = 0, BreakWhile2 = 0;
                            while (true)
                            {
                                if (BreakWhile2 == 1)
                                {
                                    break;
                                }
                                int works = 0;
                                for (int row = 0; row < height; row++)
                                {
                                    if (BreakWhile2 == 1)
                                    {
                                        break;
                                    }
                                    for (int col = 0; col < width; col++)
                                    {
                                        if (ezer1[row, col] >= 0 && ezer1[row, col] != 9)
                                        {
                                            #region CheckNumber
                                            int Around = 0;

                                            if (row != height - 1 && col != width - 1 && ezer1[row + 1, col + 1] >= 0 && ezer1[row + 1, col + 1] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (row != 0 && col != width - 1 && ezer1[row - 1, col + 1] >= 0 && ezer1[row - 1, col + 1] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (row != height - 1 && col != 0 && ezer1[row + 1, col - 1] >= 0 && ezer1[row + 1, col - 1] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (row != 0 && col != 0 && ezer1[row - 1, col - 1] >= 0 && ezer1[row - 1, col - 1] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (row != height - 1 && ezer1[row + 1, col] >= 0 && ezer1[row + 1, col] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (row != 0 && ezer1[row - 1, col] >= 0 && ezer1[row - 1, col] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (col != width - 1 && ezer1[row, col + 1] >= 0 && ezer1[row, col + 1] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (col != 0 && ezer1[row, col - 1] >= 0 && ezer1[row, col - 1] <= 9)
                                            {
                                                Around++;
                                            }

                                            if (row == height - 1 && col == width - 1)
                                            {
                                                Around -= 1;
                                            }
                                            if (row == 0 && col == width - 1)
                                            {
                                                Around -= 1;
                                            }
                                            if (row == height - 1 && col == 0)
                                            {
                                                Around -= 1;
                                            }
                                            if (row == 0 && col == 0)
                                            {
                                                Around -= 1;
                                            }
                                            if (row == height - 1)
                                            {
                                                Around += 3;
                                            }
                                            if (row == 0)
                                            {
                                                Around += 3;
                                            }
                                            if (col == width - 1)
                                            {
                                                Around += 3;
                                            }
                                            if (col == 0)
                                            {
                                                Around += 3;
                                            }
                                            #endregion

                                            #region CheckFlags
                                            int flagsAround = 0;

                                            if (row != height - 1 && col != width - 1 && ezer1[row + 1, col + 1] == -10)
                                                flagsAround++;
                                            if (row != 0 && col != width - 1 && ezer1[row - 1, col + 1] == -10)
                                                flagsAround++;
                                            if (row != height - 1 && col != 0 && ezer1[row + 1, col - 1] == -10)
                                                flagsAround++;
                                            if (row != 0 && col != 0 && ezer1[row - 1, col - 1] == -10)
                                                flagsAround++;
                                            if (row != height - 1 && ezer1[row + 1, col] == -10)
                                                flagsAround++;
                                            if (row != 0 && ezer1[row - 1, col] == -10)
                                                flagsAround++;
                                            if (col != width - 1 && ezer1[row, col + 1] == -10)
                                                flagsAround++;
                                            if (col != 0 && ezer1[row, col - 1] == -10)
                                                flagsAround++;
                                            #endregion

                                            if (flagsAround > ezer1[row, col] || 8 - Around < ezer1[row, col])
                                            {
                                                putflag = 1;
                                                BreakWhile2 = 1;
                                                break;
                                            }

                                            #region PutFlag
                                            if (ezer1[row, col] == 8 - Around)
                                            {
                                                if (row != height - 1 && col != width - 1 && ezer1[row + 1, col + 1] == -1)
                                                {
                                                    ezer1[row + 1, col + 1] = -10;
                                                    works = 1;
                                                }
                                                if (row != 0 && col != width - 1 && ezer1[row - 1, col + 1] == -1)
                                                {
                                                    ezer1[row - 1, col + 1] = -10;
                                                    works = 1;
                                                }
                                                if (row != height - 1 && col != 0 && ezer1[row + 1, col - 1] == -1)
                                                {
                                                    ezer1[row + 1, col - 1] = -10;
                                                    works = 1;
                                                }
                                                if (row != 0 && col != 0 && ezer1[row - 1, col - 1] == -1)
                                                {
                                                    ezer1[row - 1, col - 1] = -10;
                                                    works = 1;
                                                }
                                                if (row != height - 1 && ezer1[row + 1, col] == -1)
                                                {
                                                    ezer1[row + 1, col] = -10;
                                                    works = 1;
                                                }
                                                if (row != 0 && ezer1[row - 1, col] == -1)
                                                {
                                                    ezer1[row - 1, col] = -10;
                                                    works = 1;
                                                }
                                                if (col != width - 1 && ezer1[row, col + 1] == -1)
                                                {
                                                    ezer1[row, col + 1] = -10;
                                                    works = 1;
                                                }
                                                if (col != 0 && ezer1[row, col - 1] == -1)
                                                {
                                                    ezer1[row, col - 1] = -10;
                                                    works = 1;
                                                }
                                            }
                                            #endregion

                                            #region PutNumber
                                            if (ezer1[row, col] != 0 && ezer1[row, col] == flagsAround)
                                            {
                                                if (row != height - 1 && col != width - 1 && ezer1[row + 1, col + 1] == -1)
                                                {
                                                    ezer1[row + 1, col + 1] = 9;
                                                    works = 1;
                                                }
                                                if (row != 0 && col != width - 1 && ezer1[row - 1, col + 1] == -1)
                                                {
                                                    ezer1[row - 1, col + 1] = 9;
                                                    works = 1;
                                                }
                                                if (row != height - 1 && col != 0 && ezer1[row + 1, col - 1] == -1)
                                                {
                                                    ezer1[row + 1, col - 1] = 9;
                                                    works = 1;
                                                }
                                                if (row != 0 && col != 0 && ezer1[row - 1, col - 1] == -1)
                                                {
                                                    ezer1[row - 1, col - 1] = 9;
                                                    works = 1;
                                                }
                                                if (row != height - 1 && ezer1[row + 1, col] == -1)
                                                {
                                                    ezer1[row + 1, col] = 9;
                                                    works = 1;
                                                }
                                                if (row != 0 && ezer1[row - 1, col] == -1)
                                                {
                                                    ezer1[row - 1, col] = 9;
                                                    works = 1;
                                                }
                                                if (col != width - 1 && ezer1[row, col + 1] == -1)
                                                {
                                                    ezer1[row, col + 1] = 9;
                                                    works = 1;
                                                }
                                                if (col != 0 && ezer1[row, col - 1] == -1)
                                                {
                                                    ezer1[row, col - 1] = 9;
                                                    works = 1;
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                if (works == 0)
                                {
                                    break;
                                }
                            }
                            if (putflag == 1)
                            {
                                board[shura, amuda].BackgroundImage = flag;
                                numboard[shura, amuda] = -10;
                                didturn = 1;
                            }
                            #endregion

                            #region Flag does not work - Put number
                            for (int row = 0; row < height; row++)
                                for (int col = 0; col < width; col++)
                                    ezer1[row, col] = numboard[row, col];


                            ezer1[shura, amuda] = -10;

                            int putnumber = 0, BreakWhile = 0;
                            while (true)
                            {
                                if (BreakWhile == 1)
                                {
                                    break;
                                }
                                int works = 0;
                                for (int row = 0; row < height; row++)
                                {
                                    if (BreakWhile == 1)
                                    {
                                        break;
                                    }
                                    for (int col = 0; col < width; col++)
                                    {
                                        if (ezer1[row, col] >= 0 && ezer1[row, col] != 9)
                                        {
                                            #region CheckNumber
                                            int Around = 0;

                                            if (row != height - 1 && col != width - 1 && ezer1[row + 1, col + 1] >= 0 && ezer1[row + 1, col + 1] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (row != 0 && col != width - 1 && ezer1[row - 1, col + 1] >= 0 && ezer1[row - 1, col + 1] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (row != height - 1 && col != 0 && ezer1[row + 1, col - 1] >= 0 && ezer1[row + 1, col - 1] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (row != 0 && col != 0 && ezer1[row - 1, col - 1] >= 0 && ezer1[row - 1, col - 1] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (row != height - 1 && ezer1[row + 1, col] >= 0 && ezer1[row + 1, col] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (row != 0 && ezer1[row - 1, col] >= 0 && ezer1[row - 1, col] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (col != width - 1 && ezer1[row, col + 1] >= 0 && ezer1[row, col + 1] <= 9)
                                            {
                                                Around++;
                                            }
                                            if (col != 0 && ezer1[row, col - 1] >= 0 && ezer1[row, col - 1] <= 9)
                                            {
                                                Around++;
                                            }

                                            if (row == height - 1 && col == width - 1)
                                            {
                                                Around -= 1;
                                            }
                                            if (row == 0 && col == width - 1)
                                            {
                                                Around -= 1;
                                            }
                                            if (row == height - 1 && col == 0)
                                            {
                                                Around -= 1;
                                            }
                                            if (row == 0 && col == 0)
                                            {
                                                Around -= 1;
                                            }
                                            if (row == height - 1)
                                            {
                                                Around += 3;
                                            }
                                            if (row == 0)
                                            {
                                                Around += 3;
                                            }
                                            if (col == width - 1)
                                            {
                                                Around += 3;
                                            }
                                            if (col == 0)
                                            {
                                                Around += 3;
                                            }
                                            #endregion

                                            #region CheckFlags
                                            int flagsAround = 0;

                                            if (row != height - 1 && col != width - 1 && ezer1[row + 1, col + 1] == -10)
                                                flagsAround++;
                                            if (row != 0 && col != width - 1 && ezer1[row - 1, col + 1] == -10)
                                                flagsAround++;
                                            if (row != height - 1 && col != 0 && ezer1[row + 1, col - 1] == -10)
                                                flagsAround++;
                                            if (row != 0 && col != 0 && ezer1[row - 1, col - 1] == -10)
                                                flagsAround++;
                                            if (row != height - 1 && ezer1[row + 1, col] == -10)
                                                flagsAround++;
                                            if (row != 0 && ezer1[row - 1, col] == -10)
                                                flagsAround++;
                                            if (col != width - 1 && ezer1[row, col + 1] == -10)
                                                flagsAround++;
                                            if (col != 0 && ezer1[row, col - 1] == -10)
                                                flagsAround++;
                                            #endregion

                                            if (flagsAround > ezer1[row, col] || 8 - Around < ezer1[row, col])
                                            {
                                                putnumber = 1;
                                                BreakWhile = 1;
                                                break;
                                            }

                                            #region PutFlag
                                            if (ezer1[row, col] == 8 - Around)
                                            {
                                                if (row != height - 1 && col != width - 1 && ezer1[row + 1, col + 1] == -1)
                                                {
                                                    ezer1[row + 1, col + 1] = -10;
                                                    works = 1;
                                                }
                                                if (row != 0 && col != width - 1 && ezer1[row - 1, col + 1] == -1)
                                                {
                                                    ezer1[row - 1, col + 1] = -10;
                                                    works = 1;
                                                }
                                                if (row != height - 1 && col != 0 && ezer1[row + 1, col - 1] == -1)
                                                {
                                                    ezer1[row + 1, col - 1] = -10;
                                                    works = 1;
                                                }
                                                if (row != 0 && col != 0 && ezer1[row - 1, col - 1] == -1)
                                                {
                                                    ezer1[row - 1, col - 1] = -10;
                                                    works = 1;
                                                }
                                                if (row != height - 1 && ezer1[row + 1, col] == -1)
                                                {
                                                    ezer1[row + 1, col] = -10;
                                                    works = 1;
                                                }
                                                if (row != 0 && ezer1[row - 1, col] == -1)
                                                {
                                                    ezer1[row - 1, col] = -10;
                                                    works = 1;
                                                }
                                                if (col != width - 1 && ezer1[row, col + 1] == -1)
                                                {
                                                    ezer1[row, col + 1] = -10;
                                                    works = 1;
                                                }
                                                if (col != 0 && ezer1[row, col - 1] == -1)
                                                {
                                                    ezer1[row, col - 1] = -10;
                                                    works = 1;
                                                }
                                            }
                                            #endregion

                                            #region PutNumber
                                            if (ezer1[row, col] != 0 && ezer1[row, col] == flagsAround)
                                            {
                                                if (row != height - 1 && col != width - 1 && ezer1[row + 1, col + 1] == -1)
                                                {
                                                    ezer1[row + 1, col + 1] = 9;
                                                    works = 1;
                                                }
                                                if (row != 0 && col != width - 1 && ezer1[row - 1, col + 1] == -1)
                                                {
                                                    ezer1[row - 1, col + 1] = 9;
                                                    works = 1;
                                                }
                                                if (row != height - 1 && col != 0 && ezer1[row + 1, col - 1] == -1)
                                                {
                                                    ezer1[row + 1, col - 1] = 9;
                                                    works = 1;
                                                }
                                                if (row != 0 && col != 0 && ezer1[row - 1, col - 1] == -1)
                                                {
                                                    ezer1[row - 1, col - 1] = 9;
                                                    works = 1;
                                                }
                                                if (row != height - 1 && ezer1[row + 1, col] == -1)
                                                {
                                                    ezer1[row + 1, col] = 9;
                                                    works = 1;
                                                }
                                                if (row != 0 && ezer1[row - 1, col] == -1)
                                                {
                                                    ezer1[row - 1, col] = 9;
                                                    works = 1;
                                                }
                                                if (col != width - 1 && ezer1[row, col + 1] == -1)
                                                {
                                                    ezer1[row, col + 1] = 9;
                                                    works = 1;
                                                }
                                                if (col != 0 && ezer1[row, col - 1] == -1)
                                                {
                                                    ezer1[row, col - 1] = 9;
                                                    works = 1;
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                if (works == 0)
                                {
                                    break;
                                }
                            }
                            if (putnumber == 1)
                            {
                                board[shura, amuda].BackColor = Color.LightGreen;
                                didturn = 1;
                            }
                            #endregion
                        }
                    }
                }
            }

            if (didturn == 0) //Random Part
            {
                while (true)
                {
                    int boii = rnd.Next(0, height * width);
                    if (numboard[boii / width, boii % width] == -1)
                    {
                        board[boii / width, boii % width].BackColor = Color.LightGreen;
                        break;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
            if (time < 60)
            {
                if (minute >= 10)
                {
                    if (time >= 10)
                    {
                        textBox5.Text = " " + minute.ToString() + " : " + time.ToString();
                    }
                    else
                    {
                        textBox5.Text = " " + minute.ToString() + " : 0" + time.ToString();
                    }
                }
                else
                {
                    if (time >= 10)
                    {
                        textBox5.Text = " 0" + minute.ToString() + " : " + time.ToString();
                    }
                    else
                    {
                        textBox5.Text = " 0" + minute.ToString() + " : 0" + time.ToString();
                    }
                }
            }
            else
            {
                time = 0;
                minute++;
                if (minute >= 10)
                    textBox5.Text = " " + minute.ToString() + " : 0" + time.ToString();
                else
                    textBox5.Text = " 0" + minute.ToString() + " : 0" + time.ToString();

            }
        }
    }
}
