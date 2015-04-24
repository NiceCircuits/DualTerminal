using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;


namespace DoubleTerminal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            toolStripComboBoxBaud.SelectedIndex = 4;
            int nPorts = refreshPorts();
            if (nPorts >= 2)
            {
                toolStripComboBoxPort1.SelectedIndex = 0;
                toolStripComboBoxPort2.SelectedIndex = 1;
            }
        }

         // Append text of the given color.
        private void AppendText(Color color, string text)
        {
            int start = richTextBox1.TextLength;
            richTextBox1.AppendText(text);
            int end = richTextBox1.TextLength;
            
            // Textbox may transform chars, so (end-start) != text.Length
            richTextBox1.Select(start, end - start);
            {
                richTextBox1.SelectionColor = color;
                // could set box.SelectionBackColor, box.SelectionFont too.
            }
            richTextBox1.SelectionLength = 0; // clear
        }

        private int refreshPorts()
        {
            string[] ArrayComPortsNames = null;
            ArrayComPortsNames = SerialPort.GetPortNames();
            toolStripComboBoxPort1.Items.Clear();
            toolStripComboBoxPort2.Items.Clear();
            toolStripComboBoxPort1.Items.AddRange(ArrayComPortsNames);
            toolStripComboBoxPort2.Items.AddRange(ArrayComPortsNames);
            return ArrayComPortsNames.Length;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.AppendText(Color.Blue,"dupa");
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshPorts();
        }

        private void status()
        {
            if (serialPort1.IsOpen)
            {
                toolStripStatusLabel1.Text = serialPort1.PortName;
                toolStripStatusLabel1.ForeColor = Color.Red;
            }
            else
            {
                toolStripStatusLabel1.Text = "---";
                toolStripStatusLabel1.ForeColor = Color.Black;
            }
            if (serialPort2.IsOpen)
            {
                toolStripStatusLabel2.Text = serialPort2.PortName;
                toolStripStatusLabel2.ForeColor = Color.Blue;
            }
            else
            {
                toolStripStatusLabel2.Text = "---";
                toolStripStatusLabel2.ForeColor = Color.Black;
            }
        }

        private void openToolStripMenuItem_Click(object sender=null, EventArgs e=null)
        {
            try
            {
                serialPort1.PortName = toolStripComboBoxPort1.SelectedItem.ToString();
                serialPort2.PortName = toolStripComboBoxPort2.SelectedItem.ToString();
                serialPort1.Open();
                serialPort2.Open();
                openToolStripMenuItem.Enabled = false;
                closeToolStripMenuItem.Enabled = true;
            }
            catch (Exception ex)
            {
                closeToolStripMenuItem_Click();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                status();
            }
        }

        private void closeToolStripMenuItem_Click(object sender=null, EventArgs e=null)
        {
            serialPort1.Close();
            serialPort2.Close();
            openToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = false;
            status();
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate() { AppendText(Color.Red, serialPort1.ReadExisting()); }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeToolStripMenuItem_Click();
        }

        private void serialPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate() { AppendText(Color.Blue, serialPort2.ReadExisting()); }));
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}
