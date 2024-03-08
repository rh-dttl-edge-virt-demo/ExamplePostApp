using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fakeACSFramework
{
    public partial class ExamplePostApp : Form
    {
        public SerialPort mySerialPort = new SerialPort();
        public ExamplePostApp()
        {
            InitializeComponent();
            String[] serialports = SerialPort.GetPortNames();

            if (serialports.Length ==  0 )
            {
                MessageBox.Show("No COM ports for scanner found!");
            }
            else
            {
                foreach (string p in serialports)
                {
                    cmbSerialPorts.Items.Add(p);
                }
                cmbSerialPorts.SelectedIndex = 0;
                SetupSerialPort();

            }


            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;

            lblAppBanner2.Text = "New update is connected to POST database: " + Environment.GetEnvironmentVariable("POST_CD");
        }
               
                
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            
            string[] BarcodeValues = indata.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            lblFormTypeValue.Invoke((MethodInvoker)delegate {lblFormTypeValue.Text = BarcodeValues[0];});
            txtFirstName.Invoke((MethodInvoker)delegate { txtFirstName.Text = BarcodeValues[2]; });
            txtMiddleName.Invoke((MethodInvoker)delegate { txtMiddleName.Text = BarcodeValues[3]; });
            txtLastName.Invoke((MethodInvoker)delegate { txtLastName.Text = BarcodeValues[4]; });
            txtSex.Invoke((MethodInvoker)delegate { txtSex.Text = BarcodeValues[5]; });
            txtPlaceOfBirth.Invoke((MethodInvoker)delegate { txtPlaceOfBirth.Text = BarcodeValues[6]; });
            txtDOB.Invoke((MethodInvoker)delegate { txtDOB.Text = BarcodeValues[7]; });
            txtSSN.Invoke((MethodInvoker)delegate { txtSSN.Text = BarcodeValues[8]; });
            txtAddress.Invoke((MethodInvoker)delegate { txtAddress.Text = BarcodeValues[9]; });
            txtCity.Invoke((MethodInvoker)delegate { txtCity.Text = BarcodeValues[10]; });
            txtState.Invoke((MethodInvoker)delegate { txtState.Text = BarcodeValues[11]; });
            txtZipCode.Invoke((MethodInvoker)delegate { txtZipCode.Text = BarcodeValues[12]; });
            //File.WriteAllText("C:\\Users\\Seth\\Desktop\\barcoderead.txt", indata);
            //MessageBox.Show("All done!");
        }

        private void cmbSerialPorts_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SetupSerialPort();
        }

        private void SetupSerialPort()
        {
            if (mySerialPort.IsOpen)
            {
                mySerialPort.Close();
            }

            mySerialPort.PortName = cmbSerialPorts.SelectedItem.ToString();
            mySerialPort.Open();
        }
    }

}
