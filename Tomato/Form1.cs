using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tomato
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            notifyIcon1.Icon = Resource1.a1;


        }

        private enum TomatoStatus
        {
            None,
            EatingTomato,
            HaveRest,
            HaveLongRest

        }
        TomatoStatus DoingStatus;
        private int RestCount = 0;
        private int goodTomato = 0;
        private int badTomato = 0;
        
        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Step = 1;
            switch (DoingStatus)
            {
                case TomatoStatus.None:

                    switchTomato();
                    timer1.Start();
                    break;

                case TomatoStatus.EatingTomato:
                    {
                        
                        timer1.Start();
                    }
                    break;

                case TomatoStatus.HaveRest:
                    {
                        RestCount += 1;
                        timer1.Start();
                    }
                    break;

                case TomatoStatus.HaveLongRest:
                    {
                        timer1.Start();
                    }
                    break;

                default:
                    break;
             }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DoingStatus != TomatoStatus.None) {
                if (progressBar1.Value < progressBar1.Maximum)
                {
                    progressBar1.Value += progressBar1.Step;
                    label3.Text = string.Format("{0:D2}", ((progressBar1.Maximum - progressBar1.Value) / 60).ToString()) + ":" + string.Format("{0:D2}", ((progressBar1.Maximum - progressBar1.Value) % 60));
                }
                else 
                {
                    timer1.Stop();
                    if (DoingStatus == TomatoStatus.EatingTomato)
                    {
                        UpdateLog("完成一个番茄时间");
                        notifyIcon1.ShowBalloonTip(1000, "Tomato", "完成一个番茄时间", ToolTipIcon.Info);
                      
                        goodTomato++;
                        if (RestCount % 2 == 0 && RestCount != 0)
                        {
                            switchLongRest();
                        }
                        else
                        {
                            switchShortRest();
                        }
                    }
                    else if (DoingStatus == TomatoStatus.HaveRest)
                    {
                        UpdateLog("完成5分钟休息");
                        notifyIcon1.ShowBalloonTip(1000, "Tomato", "完成5分钟休息", ToolTipIcon.Info);
                        switchTomato();
                    }
                    else if (DoingStatus == TomatoStatus.HaveLongRest)
                    {
                        UpdateLog("完成15分钟休息");
                        notifyIcon1.ShowBalloonTip(1000, "Tomato", "完成15分钟休息", ToolTipIcon.Info);
                        switchTomato();
                    }
                }
            }
            UpdateStatistic();
        }
        private void UpdateStatistic()
        {
            goodTomato_counts.Text = goodTomato.ToString();
            badTomato_counts.Text = badTomato.ToString();

        }

        private void switchShortRest()
        {
            progressBar1.Maximum = 5*60 ;

            progressBar1.Value = 0;
            label3.Text = "05:00";
            timer1.Interval = 1000;
            button1.Text = "休息5分钟";
            DoingStatus = TomatoStatus.HaveRest;
            UpdateLog(button1.Text);
        }

        private void switchLongRest()
        {
            progressBar1.Maximum = 15*60 ;

            progressBar1.Value = 0;
            label3.Text = "15:00";
            timer1.Interval = 1000;
            button1.Text = "休息15分钟";
            DoingStatus = TomatoStatus.HaveLongRest;
            UpdateLog(button1.Text);
        }
        private void switchNone()
        {
            progressBar1.Value = 0;
            label3.Text = "";
            timer1.Interval = 1000;
            button1.Text = "开始";
            DoingStatus = TomatoStatus.None;
            UpdateLog("放弃");
        }
        private void switchTomato()
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = 25*60;
 
            progressBar1.Value = 0;
            label3.Text = "25:00";
            timer1.Interval = 1000;

            button1.Text = "专注";
            RestCount = 0;
            DoingStatus = TomatoStatus.EatingTomato;
            UpdateLog(button1.Text);

        }
        private void switchFinishTask()
        {
            if (DoingStatus == TomatoStatus.EatingTomato)
            {
                goodTomato++;
            }
            progressBar1.Value = 0;
            label3.Text = "";
            timer1.Interval = 1000;
            button1.Text = "开始";
            DoingStatus = TomatoStatus.None;
            UpdateLog("完成任务【" + textBox_task.Text + "】");
            notifyIcon1.ShowBalloonTip(1000, "Tomato", "完成任务【" + textBox_task.Text + "】", ToolTipIcon.Info);
            textBox_task.Text = "";
            
        }
        private void UpdateLog(string msg)
        {
            textBox_details.Text += DateTime.Now.ToShortTimeString() + " " + msg + "\n";
        }
        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            switchNone();
            badTomato++;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            switchFinishTask();
            
            
        }
    }
}
