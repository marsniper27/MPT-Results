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
        public List<Result> results = new List<Result>();
        public List<Result> failedTest = new List<Result>();
        public List<Result> passedTest = new List<Result>();
        public string fileName;
        public string filePath;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text = "";
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
                fileName = Path.GetFileNameWithoutExtension(textBox1.Text);
                filePath = Path.GetDirectoryName(textBox1.Text);
                

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            if (folderBrowser.ShowDialog() == DialogResult.OK)

            {

                textBox2.Text = folderBrowser.SelectedPath;

            }
        }

        private void Run_Click(object sender, EventArgs e)
        {
            label3.Text = "Reading file";

            using (StreamReader r = File.OpenText(textBox1.Text))
            {
                string json = r.ReadToEnd();
                results = JsonConvert.DeserializeObject<List<Result>>(json);
            }


            label3.Text = "Varifying results";
            foreach (Result x in results)
            {
                if(x.TotalExpectedWin != x.GameTotalWin)
                {
                    failedTest.Add(x);
                }
                else if(x.TotalExpectedWin == x.GameTotalWin)
                {
                    passedTest.Add(x);
                }
            }


            label3.Text = "Writing output";
            try
            {
                if(File.Exists(filePath + fileName + "_Results.txt"))
                {
                    label3.Text = "";
                    MessageBox.Show("Output folder already contains a results file");
                    results.Clear();
                    fileName = "";
                    return;
                }
                using (StreamWriter sw = File.CreateText(textBox2.Text + @"\Output.txt"))
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
            label3.Text = "COMPLETE";
            if (checkBox1.Checked == true)
            {
                System.Diagnostics.Process.Start(filePath + fileName + "_Results.txt");
            }
            results.Clear();
            fileName = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {

            label6.Text = "";
            label7.Text = "";
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = folderBrowser.SelectedPath;

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            label6.Text = "";
            label7.Text = "";
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            if (folderBrowser.ShowDialog() == DialogResult.OK)

            {

                textBox4.Text = folderBrowser.SelectedPath;

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool complete = false;
            int count = 0;
            List <string> fileEntries = new List<string> (Directory.GetFiles(textBox3.Text));
            while (!complete)
            {
                foreach (string filePath in fileEntries)
                {
                    if (Path.GetExtension(filePath) != ".testResults")
                    {
                        fileEntries.RemoveAt(count);
                        count= 0;
                        break;
                    }
                    count++;
                }
                if(count >= fileEntries.Count())
                { complete = true; }
            }

            var numFiles = fileEntries.Count();
            count = 1;
            foreach (string filePath in fileEntries)
            {
                failedTest.Clear();
                passedTest.Clear();
                results.Clear();
                fileName = Path.GetFileNameWithoutExtension(filePath);
                

                label6.Text = "Reading file";
                label7.Text = "File" + count + "of" + numFiles;

                using (StreamReader r = File.OpenText(filePath))
                {
                    string json = r.ReadToEnd();
                    results = JsonConvert.DeserializeObject<List<Result>>(json);
                }


                label3.Text = "Varifying results";
                foreach (Result x in results)
                {
                    if (x.TotalExpectedWin != x.GameTotalWin)
                    {
                        failedTest.Add(x);
                    }
                    else if (x.TotalExpectedWin == x.GameTotalWin)
                    {
                        passedTest.Add(x);
                    }
                }


                label3.Text = "Writing output";
                try
                {
                    if (File.Exists(textBox4.Text+@"\" + fileName + "_Results.txt"))
                    {
                        label3.Text = "";
                        MessageBox.Show("Output folder already contains a results file");
                        results.Clear();
                    }
                    using (StreamWriter sw = File.CreateText(textBox4.Text + @"\" + fileName + "_Results.txt"))
                    {
                        if (passedTest.Count() != 0)
                        {
                            if (passedTest.Count() == results.Count())
                            {
                                sw.WriteLine("All Tests Passed!");
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
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
                count++;
                results.Clear();
            }
            label6.Text = "COMPLETE";
            results.Clear();
        }
    }
}
