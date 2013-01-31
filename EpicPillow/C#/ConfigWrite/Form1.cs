using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO; 
namespace ConfigWrite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null && textBox2.Text != null && textBox3.Text != null)
            {
                string writeConfig = "irc_server=" + textBox1.Text + Environment.NewLine + "irc_port=" + textBox2.Text + Environment.NewLine + "irc_channel=" + textBox3.Text;
                StreamWriter sw = new StreamWriter("config.txt");
                sw.Write(writeConfig);
                sw.Close();
                MessageBox.Show("Config Settings Written to config.txt");
                System.Diagnostics.Process.Start("config.txt");
                System.Diagnostics.Process.Start("EpicPillow.exe");
                Environment.Exit(0); 
            }
            else
            {
                MessageBox.Show("You have an empty field!"); 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("config.txt"))
            {
                readConfig();
            }
        }
        public string irc_server;
        public string irc_port_string;
        public string irc_channel;
        public void readConfig()
        {
            try
            {
                StreamReader sr = new StreamReader("config.txt");
                string readConfig = sr.ReadToEnd();
                sr.Close();
                string[] configSplit = readConfig.Split('\n');
                for (int i = 0; i < configSplit.Length; i++)
                {
                    if (configSplit[i].StartsWith("irc_server="))
                    {
                        irc_server = configSplit[i].Substring(11);
                    }
                    if (configSplit[i].StartsWith("irc_port="))
                    {
                        irc_port_string = configSplit[i].Substring(9);
                    }
                    if (configSplit[i].StartsWith("irc_channel="))
                    {
                        irc_channel = configSplit[i].Substring(12);
                    }
                }
                textBox1.Text = irc_server;
                textBox2.Text = irc_port_string;
                textBox3.Text = irc_channel; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
