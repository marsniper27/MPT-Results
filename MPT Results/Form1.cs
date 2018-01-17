using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace MPT_Results
{
    public partial class Form1 : Form
    {
        public List<Result> results;
        public List<Result> failedTest = new List<Result>();
        public List<Result> passedTest = new List<Result>(); 

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();



            openFileDialog1.InitialDirectory = @"C:\";

            openFileDialog1.Title = "Select MPT results";



            openFileDialog1.CheckFileExists = true;

            openFileDialog1.CheckPathExists = true;



            openFileDialog1.DefaultExt = "testResults";

            openFileDialog1.Filter = "MPT Results (*.testResults)|*.testResults|All files (*.*)|*.*";

            openFileDialog1.FilterIndex = 1;

            openFileDialog1.RestoreDirectory = true;



            openFileDialog1.ReadOnlyChecked = true;

            openFileDialog1.ShowReadOnly = true;



            if (openFileDialog1.ShowDialog() == DialogResult.OK)

            {

                textBox1.Text = openFileDialog1.FileName;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            if (folderBrowser.ShowDialog() == DialogResult.OK)

            {

                textBox2.Text = folderBrowser.SelectedPath;

            }
        }

        private void Run_Click(object sender, EventArgs e)
        {
            using (StreamReader r = File.OpenText(textBox1.Text))
            {
                string json = r.ReadToEnd();
                results = JsonConvert.DeserializeObject<List<Result>>(json);
            }

            foreach (Result x in results)
            {
                if(x.TotalExpectedWin == x.GameTotalWin)
                {
                    failedTest.Add(x);
                }
                else if(x.TotalExpectedWin == x.GameTotalWin)
                {
                    passedTest.Add(x);
                }
            }

            try
            {
                if(File.Exists(textBox2.Text + "Output.txt"))
                {
                    MessageBox.Show("Output folder already contains a results file");
                    return;
                }
                using (StreamWriter sw = File.CreateText(textBox2.Text + "\Output.txt"))
                {
                    if (passedTest.Count() != 0)
                    {
                        if (passedTest.Count() == results.Count())
                        {
                            sw.WriteLine("All Tests Passed!");
                            return;
                        }
                        else
                        {
                            sw.WriteLine("----------Passed Tests----------");
                            sw.WriteLine();
                            sw.WriteLine("Number of Passed Tests: " + passedTest.Count());

                            foreach (Result pass in passedTest)
                            {
                                sw.WriteLine(pass.GamePlayName);
                            }
                        }
                    }
                    if (failedTest.Count() != 0)
                    {
                        sw.WriteLine();
                        sw.WriteLine("----------Failed Tests----------");
                        sw.WriteLine();
                        sw.WriteLine("Number of Failed Tests: " + failedTest.Count());
                        sw.WriteLine();
                        foreach (Result fail in failedTest)
                        {
                            sw.WriteLine(fail.GamePlayName);
                        }
                    }
                }
            }
            catch(Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }
        }
    }
}
