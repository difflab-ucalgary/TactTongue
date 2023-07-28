


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Text.Json;
using System.IO;
using System.IO.Ports;
using System.Windows.Threading;
using System.Threading;

namespace TactileTongue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int color_scale;
        SerialPort sp = new SerialPort();
        bool serial_port_connection_status = false;
        int baudRate = 9600;



        int[] electrode_array = new int[18];
        int[] pulse_width = new int[18];
        int[] pulse_period = new int[18];
        int[] inner_burst_number = new int[18];
        int[] inner_burst_period = new int[18];
        int[] outer_burst_number = new int[18];
        int[] inter_channel_period = new int[18];
        int[,] cElectrodeMap = new int[5, 4] {  {0, 0, 1, 0},
                                                {2, 3, 4, 5},
                                                {6, 7, 8, 9},
                                                {10,11,12,13},
                                                {14,15,16,17}
                                               };

        int[,] on = new int[5, 4] { {0, 0, 0, 0},
                                    {0, 0, 0, 0},
                                    {0, 0, 0, 0},
                                    {0, 0, 0, 0},
                                    {0, 0, 0, 0}
                                   };

        public MainWindow()
        {
            InitializeComponent();

            string[] portNames = SerialPort.GetPortNames();

            for (int i = 0; i < portNames.Length; i++)
            {
                port_names.Items.Add(portNames[i]);
            }
        }

        public void connectToCOMPort(string portName)
        {
            try
            {
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.Open();
                serial_port_connection_status = true;
                sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived); ;

            }
            catch (Exception e)
            {
                Console.WriteLine("Could not Connect to the Serial Port \n");
                status_label.Text = "Disconnected";
                serial_port_connection_status = false;
                return;
            }
        }
        string ReceivedData;
        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //var serialDevice = sender as SerialPort;
            //var buffer = new byte[serialDevice.BytesToRead];
            // serialDevice.Read(buffer, 0, buffer.Length);
          //  string volts = sp.ReadExisting();
            string data = sp.ReadLine(); 
            int volt_length = data.Length;
          //  ReceivedData = sp.ReadLine();
            Console.WriteLine(data);
            
        }

        private void connect_button_Click(object sender, RoutedEventArgs e)
        {
            string selectedPort = port_names.SelectedItem.ToString();

            status_label.Text = "Connected";
            connectToCOMPort(selectedPort);
        }

 

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        public class StimuliParameters
        {
            public string ele { get; set; }
            public string pp { get; set; }
            public string Pp { get; set; }
            public string IBN { get; set; }
            public string IBP { get; set; }
            public string OBN { get; set; }
        }

        string getArrayString(int[] inputArray)
        {
            string outputString = "[";
            for (int i = 0; i < inputArray.Length; i++)
            {
                if (i == inputArray.Length - 1)
                {
                    outputString += inputArray[i];
                }
                else
                {
                    outputString += inputArray[i] + ",";
                }
            }
            outputString += "]";
            return outputString;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StimuliParameters stimuli = new StimuliParameters
            {
                ele = getArrayString(getActiveElectrodeArray()),
                pp = getArrayString(getIntensityValues()),
                Pp = getArrayString(getPulsePeriodValues()),
                IBN = getArrayString(getIBPValues()),
                IBP = getArrayString(getIBPValues()),
                OBN = getArrayString(getOBNValues())

            };
            /* if(serial_port_connection_status == false)
             {
                 MessageBox.Show("Please connect the TactTongue Control Board");
                 System.Windows.Application.Current.Shutdown();
             }*/
            // Console.WriteLine("Hello");

            // string commandString = "[1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]:[10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10]";
            string commandString = stimuli.ele + ":" + stimuli.pp;
            Console.WriteLine(commandString);

            if(down_pattern_on == true)
            {
                sp.Write("down");
            }


            //Console.WriteLine(stimuli.ele + ":" + stimuli.pp + ":" + stimuli.Pp + ":" + stimuli.IBN + ":" + stimuli.IBP + ":" + stimuli.OBN);
           // sp.Write(stimuli.ele + ":" + stimuli.pp + ":" + stimuli.Pp + ":" + stimuli.IBN + ":" + stimuli.IBP + ":" + stimuli.OBN);
            // sp.Write(commandString);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            channel1_ellipse.Fill = new SolidColorBrush(Colors.Green);
        }

        private void intensity_slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value1.Content = intensity_slider1.Value;
            if (intensity_slider1.Value == 0)
            {
                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }

            color_scale =  (int)((1 -(intensity_slider1.Value) / (intensity_slider1.Maximum)) * 255);
            channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value2.Content = intensity_slider2.Value;
            if (intensity_slider2.Value == 0)
            {
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider2.Value) / (intensity_slider2.Maximum)) * 255);
            channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value3.Content = intensity_slider3.Value;
            if (intensity_slider3.Value == 0)
            {
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider3.Value) / (intensity_slider3.Maximum)) * 255);
            channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value4.Content = intensity_slider4.Value;
            if (intensity_slider4.Value == 0)
            {
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider4.Value) / (intensity_slider4.Maximum)) * 255);
            channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider5_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value5.Content = intensity_slider5.Value;
            if (intensity_slider5.Value == 0)
            {
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider5.Value) / (intensity_slider5.Maximum)) * 255);
            channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider6_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value6.Content = intensity_slider6.Value;
            if (intensity_slider6.Value == 0)
            {
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider6.Value) / (intensity_slider6.Maximum)) * 255);
            channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider7_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value7.Content = intensity_slider7.Value;
            if (intensity_slider7.Value == 0)
            {
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider7.Value) / (intensity_slider7.Maximum)) * 255);
            channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider8_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value8.Content = intensity_slider8.Value;
            if (intensity_slider8.Value == 0)
            {
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider8.Value) / (intensity_slider8.Maximum)) * 255);
            channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider9_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value9.Content = intensity_slider9.Value;
            if (intensity_slider9.Value == 0)
            {
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider9.Value) / (intensity_slider9.Maximum)) * 255);
            channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider10_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value10.Content = intensity_slider10.Value;
            if (intensity_slider10.Value == 0)
            {
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider10.Value) / (intensity_slider10.Maximum)) * 255);
            channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider11_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value11.Content = intensity_slider11.Value;
            if (intensity_slider11.Value == 0)
            {
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider11.Value) / (intensity_slider11.Maximum)) * 255);
            channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider12_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value12.Content = intensity_slider12.Value;
            if (intensity_slider12.Value == 0)
            {
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider12.Value) / (intensity_slider12.Maximum)) * 255);
            channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider13_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value13.Content = intensity_slider13.Value;
            if (intensity_slider13.Value == 0)
            {
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider13.Value) / (intensity_slider13.Maximum)) * 255);
            channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider14_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value14.Content = intensity_slider14.Value;
            if (intensity_slider14.Value == 0)
            {
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider14.Value) / (intensity_slider14.Maximum)) * 255);
            channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider15_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value15.Content = intensity_slider15.Value;
            if (intensity_slider15.Value == 0)
            {
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider15.Value) / (intensity_slider15.Maximum)) * 255);
            channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider16_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value16.Content = intensity_slider16.Value;
            if (intensity_slider16.Value == 0)
            {
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider16.Value) / (intensity_slider16.Maximum)) * 255);
            channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider17_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value17.Content = intensity_slider17.Value;
            if (intensity_slider17.Value == 0)
            {
                channel17_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider17.Value) / (intensity_slider17.Maximum)) * 255);
            channel17_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }

        private void intensity_slider18_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            intensity_value18.Content = intensity_slider18.Value;
            if (intensity_slider18.Value == 0)
            {
                channel18_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                return;
            }
            color_scale = (int)((1 - (intensity_slider18.Value) / (intensity_slider18.Maximum)) * 255);
            channel18_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }
        private void left_icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            left_icon.Focus();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        }

        bool square_pattern_button_on = false;
        bool up_pattern_button_on = false;
        bool down_pattern_on = false;
        bool vibration_button_on = false;
        public void resetOrder()
        {
            direction_ch1.Text = "";
            direction_ch2.Text = "";
            direction_ch3.Text = "";
            direction_ch4.Text = "";
            direction_ch5.Text = "";
            direction_ch6.Text = "";
            direction_ch7.Text = "";
            direction_ch8.Text = "";
            direction_ch9.Text = "";
            direction_ch10.Text = "";
            direction_ch11.Text = "";
            direction_ch12.Text = "";
            direction_ch13.Text = "";
            direction_ch14.Text = "";
            direction_ch15.Text = "";
            direction_ch16.Text = "";
            direction_ch17.Text = "";
            direction_ch18.Text = "";

        }

        public void setOrderForSquare()
        {
            direction_ch1.Text = "1";
            direction_ch2.Text = "2";
            direction_ch3.Text = "3";
            direction_ch4.Text = "4";
            direction_ch5.Text = "12";
            direction_ch6.Text = "";
            direction_ch7.Text = "";
            direction_ch8.Text = "5";
            direction_ch9.Text = "11";
            direction_ch10.Text = "";
            direction_ch11.Text = "";
            direction_ch12.Text = "6";
            direction_ch13.Text = "10";
            direction_ch14.Text = "9";
            direction_ch15.Text = "8";
            direction_ch16.Text = "7";
            direction_ch17.Text = "";
            direction_ch18.Text = "";
        }

        public void setOrderForUp()
        {
            direction_ch1.Text = "1";
            direction_ch2.Text = "2";
            direction_ch3.Text = "3";
            direction_ch4.Text = "4";
            direction_ch5.Text = "5";
            direction_ch6.Text = "6";
            direction_ch7.Text = "7";
            direction_ch8.Text = "8";
            direction_ch9.Text = "";
            direction_ch10.Text = "";
            direction_ch11.Text = "";
            direction_ch12.Text = "";
            direction_ch13.Text = "";
            direction_ch14.Text = "";
            direction_ch15.Text = "";
            direction_ch16.Text = "";
            direction_ch17.Text = "";
            direction_ch18.Text = "";
        }

        public void setOrderForDown()
        {
            direction_ch1.Text = "";
            direction_ch2.Text = "";
            direction_ch3.Text = "";
            direction_ch4.Text = "";
            direction_ch5.Text = "";
            direction_ch6.Text = "";
            direction_ch7.Text = "";
            direction_ch8.Text = "";
            direction_ch9.Text = "1";
            direction_ch10.Text = "2";
            direction_ch11.Text = "3";
            direction_ch12.Text = "4";
            direction_ch13.Text = "5";
            direction_ch14.Text = "6";
            direction_ch15.Text = "7";
            direction_ch16.Text = "8";
            direction_ch17.Text = "";
            direction_ch18.Text = "";
        }

        public void setSquareIntensitySliders()
        {
            intensity_slider1.Value = 2;
            intensity_slider2.Value = 2;
            intensity_slider3.Value = 2;
            intensity_slider4.Value = 2;
            intensity_slider5.Value = 2;
            intensity_slider6.Value = 0;
            intensity_slider7.Value = 0;
            intensity_slider8.Value = 2;
            intensity_slider9.Value = 2;
            intensity_slider10.Value = 0;
            intensity_slider11.Value = 0;
            intensity_slider12.Value = 2;
            intensity_slider13.Value = 2;
            intensity_slider14.Value = 2;
            intensity_slider15.Value = 2;
            intensity_slider16.Value = 2;
            intensity_slider17.Value = 0;
            intensity_slider18.Value = 0;


        }

        public void setUpIntensitySliders()
        {
            intensity_slider1.Value = 2;
            intensity_slider2.Value = 2;
            intensity_slider3.Value = 2;
            intensity_slider4.Value = 2;
            intensity_slider5.Value = 2;
            intensity_slider6.Value = 2;
            intensity_slider7.Value = 2;
            intensity_slider8.Value = 2;
            intensity_slider9.Value = 0;
            intensity_slider10.Value = 0;
            intensity_slider11.Value = 0;
            intensity_slider12.Value = 0;
            intensity_slider13.Value = 0;
            intensity_slider14.Value = 0;
            intensity_slider15.Value = 0;
            intensity_slider16.Value = 0;
            intensity_slider17.Value = 0;
            intensity_slider18.Value = 0;


        }

        public void setDownIntensitySliders()
        {
            intensity_slider1.Value = 0;
            intensity_slider2.Value = 0;
            intensity_slider3.Value = 0;
            intensity_slider4.Value = 0;
            intensity_slider5.Value = 0;
            intensity_slider6.Value = 0;
            intensity_slider7.Value = 0;
            intensity_slider8.Value = 0;
            intensity_slider9.Value = 2;
            intensity_slider10.Value = 2;
            intensity_slider11.Value = 2;
            intensity_slider12.Value = 2;
            intensity_slider13.Value = 2;
            intensity_slider14.Value = 2;
            intensity_slider15.Value = 2;
            intensity_slider16.Value = 2;
            intensity_slider17.Value = 0;
            intensity_slider18.Value = 0;


        }
        void updatepattern()
        {
            //Take care of the first two elements outside the main square
            int two_electrode_row1 = cElectrodeMap[0, 1]; //two electrode row
            int two_electrode_row2 = cElectrodeMap[0, 2];
            int temp;
            activeElectrodeArray[two_electrode_row1] = on[0, 1];
            activeElectrodeArray[two_electrode_row2] = on[0, 2];

            //if the On array is ative, set the corresponding electrode active.
            for (int j = 0; j < 4; j++)
            {
                for (int i = 1; i < 5; i++)
                {
                    temp = cElectrodeMap[i, j];
                    activeElectrodeArray[temp] = on[i, j];
                }
            }
        }

        public void setActiveElectrodeForSquare()
        {
            on[1, 0] = 1; on[1, 2] = 1; on[2, 0] = 1; on[2, 3] = 1; on[4, 0] = 1; on[4, 2] = 1;
            on[1, 1] = 1; on[1, 3] = 1; on[3, 0] = 1; on[3, 3] = 1; on[4, 1] = 1; on[4, 3] = 1;
            updatepattern();

        }

        public void setActiveElectrodeForDown()
        {
            on[1, 0] = 1; on[1, 2] = 1; on[2, 0] = 1; on[2, 2] = 1; 
            on[1, 1] = 1; on[1, 3] = 1; on[2, 1] = 1; on[2, 3] = 1;
            updatepattern();

        }

        public void setActiveElectrodeForUp()
        {
            on[3, 0] = 1; on[3, 2] = 1; on[4, 0] = 1; on[4, 2] = 1;
            on[3, 1] = 1; on[3, 3] = 1; on[4, 1] = 1; on[4, 3] = 1;
            updatepattern();

        }
        private void square_pattern_click(object sender, RoutedEventArgs e)
        {
            resetOrder();
            
            if (square_pattern_button_on == false)
            {
                
                square_pattern_button.Opacity = 0.2;

                square_pattern_button_on = true;

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                setOrderForSquare();
                setActiveElectrodeForSquare();
                setSquareIntensitySliders();


            }
           else if(square_pattern_button_on == true)
            {
                square_pattern_button.Opacity = 0;
                square_pattern_button_on = false;
            }
        }

        private void up_pattern_button_Click(object sender, RoutedEventArgs e)
        {
            resetOrder();
            if (up_pattern_button_on == false)
            {
                up_pattern_button.Opacity = 0.2;

                square_pattern_button_on = true;

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
               

                setOrderForUp();
                setActiveElectrodeForUp();
                setUpIntensitySliders();
            }
            else if (up_pattern_button_on == true)
            {
                up_pattern_button.Opacity = 0;
                up_pattern_button_on = false;
            }

        }

        private void pw_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            waveform_pw_value.Content = pw_slider.Value;
        }

        private void pp_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            waveform_pp_value.Content = pp_slider.Value;
        }

        private void ibn_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            waveform_ibn_value.Content = ibn_slider.Value;
        }

        private void ibp_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            waveform_ibp_value.Content = ibp_slider.Value;
        }

        private void obn_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            waveform_obn_value.Content = obn_slider.Value;
        }

        private void icp_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            waveform_icp_value.Content = icp_slider.Value;
        }

        private void setSlidersForVibration()
        {

        }

        private void vibration_button_Click(object sender, RoutedEventArgs e)
        {
            if (vibration_button_on == false)
            {
                vibration_button.Opacity = 0.2;
                vibration_button_on = true;
                pw_slider.Value = 9;
                pp_slider.Value = 10;
                ibn_slider.Value = 9;
                ibp_slider.Value = 50;
                obn_slider.Value = 19;


             

            }
            else if (square_pattern_button_on == true)
            {
                square_pattern_button.Opacity = 0;
                square_pattern_button_on = false;
            }

        }
        bool electrod1_on = false;
        bool electrod2_on = false;
        bool electrod3_on = false;
        bool electrod4_on = false;
        bool electrod5_on = false;
        bool electrod6_on = false;
        bool electrod7_on = false;
        bool electrod8_on = false;
        bool electrod9_on = false;
        bool electrod10_on = false;
        bool electrod11_on = false;
        bool electrod12_on = false;
        bool electrod13_on = false;
        bool electrod14_on = false;
        bool electrod15_on = false;
        bool electrod16_on = false;
        bool electrod17_on = false;
        bool electrod18_on = false;

        int[] activeElectrodeArray = new int[18];
        int[] activePulseWidthArray = new int[18];
        int[] activePulsePeriodArray = new int[18];
        int[] activeInnerBurstNumber = new int[18];
        int[] activeInnerBurstPeriod = new int[18];
        int[] activeOuterBurstNumber = new int[18];
        int[] innerChannelPeriod = new int[18];


        public int checkStimulationParameterValidity()
        {
            for(int i = 0; i < activeElectrodeArray.Length; i++)
            {
                if(activeElectrodeArray[i] == 1)
                {
                    if(activePulsePeriodArray[i] > activePulseWidthArray[i])
                    {
                        Console.WriteLine("Pulse period must be less than Pulse Width.   ");
                        return 1;
                    }
                    if((activePulseWidthArray[i] * activeInnerBurstNumber[i]) > activeInnerBurstPeriod[i])
                    {
                        Console.WriteLine("(PP*IN) must be less than IP. ");
                        return 1;
                    }

                    if(activeInnerBurstNumber[i] *activeInnerBurstPeriod[i] >= 2000)
                    {
                        Console.WriteLine("IP*IN must be less than 2000 microseconds.");

                        return 1;
                    }
                }
            }

            return 0;
        }

        public int[] getIntensityValues()
        {
            activePulseWidthArray[0] = (int)intensity_slider1.Value;
            activePulseWidthArray[1] = (int)intensity_slider2.Value;
            activePulseWidthArray[2] = (int)intensity_slider3.Value;
            activePulseWidthArray[3] = (int)intensity_slider4.Value;
            activePulseWidthArray[4] = (int)intensity_slider5.Value;
            activePulseWidthArray[5] = (int)intensity_slider6.Value;
            activePulseWidthArray[6] = (int)intensity_slider7.Value;
            activePulseWidthArray[7] = (int)intensity_slider8.Value;
            activePulseWidthArray[8] = (int)intensity_slider9.Value;
            activePulseWidthArray[9] = (int)intensity_slider10.Value;
            activePulseWidthArray[10] = (int)intensity_slider11.Value;
            activePulseWidthArray[11] = (int)intensity_slider12.Value;
            activePulseWidthArray[12] = (int)intensity_slider13.Value;
            activePulseWidthArray[13] = (int)intensity_slider14.Value;
            activePulseWidthArray[14] = (int)intensity_slider15.Value;
            activePulseWidthArray[15] = (int)intensity_slider16.Value;
            activePulseWidthArray[16] = (int)intensity_slider17.Value;
            activePulseWidthArray[17] = (int)intensity_slider18.Value;
            return activePulseWidthArray;

        }

        public int[] getPulsePeriodValues()
        {
           for(int i =0; i < 18; i++)
            {
                activePulsePeriodArray[i] = (int)pp_slider.Value;
            }
            return activePulsePeriodArray;
        }

        public int[] geIBNValues()
        {
            for (int i = 0; i < 18; i++)
            {
                activeInnerBurstNumber[i] = (int)ibn_slider.Value;
            }
            return activeInnerBurstNumber;

        }

        public int[] getIBPValues()
        {
            for (int i = 0; i < 18; i++)
            {
                activeInnerBurstPeriod[i] = (int)ibp_slider.Value;
            }
            return activeInnerBurstPeriod;
        }

        public int[] getOBNValues()
        {
            for (int i = 0; i < 18; i++)
            {
                activeOuterBurstNumber[i] = (int)obn_slider.Value;
            }
            return activeOuterBurstNumber;

        }

        public int[] getICPValues()
        {
            for (int i = 0; i < 18; i++)
            {
                innerChannelPeriod[i] = (int)icp_slider.Value;
            }
            return innerChannelPeriod;

        }
        public int[] getActiveElectrodeArray()
        {
            if (electrod1_on == true)
            {
                activeElectrodeArray[0] = 1;
            }

            if (electrod2_on == true)
            {
                activeElectrodeArray[1] = 1;
            }


            if (electrod3_on == true)
            {
                activeElectrodeArray[2] = 1;
            }

            if (electrod4_on == true)
            {
                activeElectrodeArray[3] = 1;
            }

            if (electrod5_on == true)
            {
                activeElectrodeArray[4] = 1;
            }

            if (electrod6_on == true)
            {
                activeElectrodeArray[5] = 1;
            }

            if (electrod7_on == true)
            {
                activeElectrodeArray[6] = 1;
            }

            if (electrod8_on == true)
            {
                activeElectrodeArray[7] = 1;
            }


            if (electrod9_on == true)
            {
                activeElectrodeArray[8] = 1;
            }

            if (electrod10_on == true)
            {
                activeElectrodeArray[9] = 1;
            }

            if (electrod11_on == true)
            {
                activeElectrodeArray[10] = 1;
            }

            if (electrod12_on == true)
            {
                activeElectrodeArray[11] = 1;
            }

            if (electrod13_on == true)
            {
                activeElectrodeArray[12] = 1;
            }

            if (electrod14_on == true)
            {
                activeElectrodeArray[13] = 1;
            }

            if (electrod15_on == true)
            {
                activeElectrodeArray[14] = 1;
            }


            if (electrod16_on == true)
            {
                activeElectrodeArray[15] = 1;
            }

            if (electrod17_on == true)
            {
                activeElectrodeArray[16] = 1;
            }

            if (electrod18_on == true)
            {
                activeElectrodeArray[17] = 1;
            }

            return activeElectrodeArray;

        }
        private void electrode1_button_Click(object sender, RoutedEventArgs e)
        {
            if(electrod1_on == false)
            {
                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod1_on = true;
            }
            else if(electrod1_on == true)
            {
                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod1_on = false;
            }
        }

        private void electrode2_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod2_on == false)
            {
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod2_on = true;
            }
            else if (electrod2_on == true)
            {
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod2_on = false;
            }
        }

        private void electrode3_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod3_on == false)
            {
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod3_on = true;
            }
            else if (electrod3_on == true)
            {
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod3_on = false;
            }
        }

        private void electrode4_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod4_on == false)
            {
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod4_on = true;
            }
            else if (electrod4_on == true)
            {
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod4_on = false;
            }
        }

        private void electrode5_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod5_on == false)
            {
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod5_on = true;
            }
            else if (electrod5_on == true)
            {
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod5_on = false;
            }
        }

        private void electrode6_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod6_on == false)
            {
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod6_on = true;
            }
            else if (electrod6_on == true)
            {
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod6_on = false;
            }
        }

        private void electrode7_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod7_on == false)
            {
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod7_on = true;
            }
            else if (electrod7_on == true)
            {
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod7_on = false;
            }
        }

        private void electrode8_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod8_on == false)
            {
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod8_on = true;
            }
            else if (electrod8_on == true)
            {
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod8_on = false;
            }
        }

        private void electrode9_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod9_on == false)
            {
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod9_on = true;
            }
            else if (electrod9_on == true)
            {
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod9_on = false;
            }
        }

        private void electrode10_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod10_on == false)
            {
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod10_on = true;
            }
            else if (electrod10_on == true)
            {
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod10_on = false;
            }
        }

        private void electrode11_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod11_on == false)
            {
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod11_on = true;
            }
            else if (electrod11_on == true)
            {
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod11_on = false;
            }
        }

        private void electrode12_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod12_on == false)
            {
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod12_on = true;
            }
            else if (electrod12_on == true)
            {
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod12_on = false;
            }
        }

        private void electrode13_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod13_on == false)
            {
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod13_on = true;
            }
            else if (electrod13_on == true)
            {
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod13_on = false;
            }
        }


        private void electrode14_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod14_on == false)
            {
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod14_on = true;
            }
            else if (electrod14_on == true)
            {
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod14_on = false;
            }
        }

        private void electrode15_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod15_on == false)
            {
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod15_on = true;
            }
            else if (electrod15_on == true)
            {
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod15_on = false;
            }
        }

        private void electrode16_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod16_on == false)
            {
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod16_on = true;
            }
            else if (electrod16_on == true)
            {
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod16_on = false;
            }
        }

        private void electrode17_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod17_on == false)
            {
                channel17_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod17_on = true;
            }
            else if (electrod17_on == true)
            {
                channel17_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod17_on = false;
            }
        }

        private void electrode18_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod18_on == false)
            {
                channel18_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod18_on = true;
            }
            else if (electrod18_on == true)
            {
                channel18_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod18_on = false;
            }
        }

        private void down_pattern_button_Click(object sender, RoutedEventArgs e)
        {
            resetOrder();
            if (down_pattern_on == false)
            {
                down_pattern_button.Opacity = 0.2;

                down_pattern_on = true;

                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));


                setOrderForDown();
                setActiveElectrodeForDown();
                setDownIntensitySliders();
            }
            else if (down_pattern_on == true)
            {
                down_pattern_button.Opacity = 0;
                down_pattern_on = false;
            }
        }
    }
}
