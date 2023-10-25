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
//using System.Text.Json;
//using System.Text.Json.Serialization;
using System.IO;
using System.IO.Ports;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Media.Effects;
using System.Net.Http.Json;
using Newtonsoft.Json;
using static TactTongue.MainWindow;

namespace TactTongue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class TactTongueStimuli
    {
        static int NUM_ELECTRODES = 18;
        public string stimulli_name;
        public int[] ele_array = new int[NUM_ELECTRODES];
        public int PP;
        public int Pp;
        public int IBN;
        public int IBP;
        public int OBN;
        public string direction_preset;
        public string taste_preset;
    }
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

        int[] activeElectrodeArray = new int[NUM_ELECTRODES];
        int[] activePulseWidthArray = new int[NUM_ELECTRODES];
        int[] activePulsePeriodArray = new int[NUM_ELECTRODES];
        int[] activeInnerBurstNumber = new int[NUM_ELECTRODES];
        int[] activeInnerBurstPeriod = new int[NUM_ELECTRODES];
        int[] activeOuterBurstNumber = new int[NUM_ELECTRODES];
        int[] innerChannelPeriod = new int[NUM_ELECTRODES];

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

        bool square_pattern_button_on = false;
        bool vibration_button_on = false;

       
        bool[] presets_activated = new bool[NUM_SENSATION_PRESETS];

        static int NUM_TACTTONGUE_STIMULI = 30;
        static int NUM_ELECTRODES = 18;
        List<TactTongueStimuli> tacttongue_stimuli_json;

    
        public MainWindow()
        {
            InitializeComponent();
            string[] portNames = SerialPort.GetPortNames();

            for (int i = 0; i < portNames.Length; i++)
            {
                port_names.Items.Add(portNames[i]);
            }
            initializeElectrodeArray();
            load_tacttongue_stimuli();
            for(int i = 0; i < NUM_TACTTONGUE_STIMULI; i++)
            {
                tacttongue_stimuli.Items.Add("Stimuli " + (i+1));
            }
        }
        public void initializeElectrodeArray()
        {
            for(int i = 0; i < NUM_ELECTRODES; i++)
            {
                activeElectrodeArray[i] = 1;
                activePulsePeriodArray[i] = (int) pp_slider.Value;
                activeInnerBurstNumber[i] = (int)ibn_slider.Value;
                activeInnerBurstPeriod[i] = (int)ibp_slider.Value;
                activeOuterBurstNumber[i] = (int)obn_slider.Value;
            }

            color_scale = (int)((1 - (intensity_slider1.Value) / (intensity_slider1.Maximum)) * 255);
            channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider2.Value) / (intensity_slider2.Maximum)) * 255);
            channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider3.Value) / (intensity_slider3.Maximum)) * 255);
            channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider4.Value) / (intensity_slider4.Maximum)) * 255);
            channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider5.Value) / (intensity_slider5.Maximum)) * 255);
            channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider6.Value) / (intensity_slider6.Maximum)) * 255);
            channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider7.Value) / (intensity_slider7.Maximum)) * 255);
            channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider8.Value) / (intensity_slider8.Maximum)) * 255);
            channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider9.Value) / (intensity_slider9.Maximum)) * 255);
            channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider10.Value) / (intensity_slider10.Maximum)) * 255);
            channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider11.Value) / (intensity_slider11.Maximum)) * 255);
            channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider12.Value) / (intensity_slider12.Maximum)) * 255);
            channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider13.Value) / (intensity_slider13.Maximum)) * 255);
            channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider14.Value) / (intensity_slider14.Maximum)) * 255);
            channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider15.Value) / (intensity_slider15.Maximum)) * 255);
            channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider16.Value) / (intensity_slider16.Maximum)) * 255);
            channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider17.Value) / (intensity_slider17.Maximum)) * 255);
            channel17_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            color_scale = (int)((1 - (intensity_slider18.Value) / (intensity_slider18.Maximum)) * 255);
            channel18_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
        }
        public void load_tacttongue_stimuli()
        {
            using (StreamReader r = new StreamReader("../../../tactongue-stimuli.json"))
            {
                string json = r.ReadToEnd();

                tacttongue_stimuli_json = JsonConvert.DeserializeObject<List<TactTongueStimuli>>(json);
               // System.Diagnostics.Debug.WriteLine(tacttongue_stimuli_json[0].IBN + " ");
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
                Console.WriteLine(e.Message);
                return;
            }
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

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            string data = sp.ReadLine();
            int data_length = data.Length;
            System.Diagnostics.Debug.WriteLine(data);

        }

        private void connect_button_Click(object sender, RoutedEventArgs e)
        {
            string selectedPort = port_names.SelectedItem.ToString();

            status_label.Text = "Connected";
            connectToCOMPort(selectedPort);
        }


        private void send_command_Click(object sender, RoutedEventArgs e)
        {

        }

        private void intensity_slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(intensity_value1!= null)
            {
                intensity_value1.Content = intensity_slider1.Value;
                if (intensity_slider1.Value == 0)
                {
                    channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                    return;
                }

                color_scale = (int)((1 - (intensity_slider1.Value) / (intensity_slider1.Maximum)) * 255);
                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
          
        }

        private void intensity_slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(intensity_value2 != null)
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

        public int[] getPulsePeriodValues()
        {
            for (int i = 0; i < 18; i++)
            {
                activePulsePeriodArray[i] = (int)pp_slider.Value;
            }
            return activePulsePeriodArray;
        }

        public int[] getIBNValues()
        {
            for (int i = 0; i < NUM_ELECTRODES; i++)
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
            if (electrod1_on == false)
            {
                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod1_on = true;
                activeElectrodeArray[0] = 1;
            }
            else if (electrod1_on == true)
            {
                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod1_on = false;
                activeElectrodeArray[0] = 0;
            }
        }

        private void electrode2_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod2_on == false)
            {
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod2_on = true;
                activeElectrodeArray[1] = 1;
            }
            else if (electrod2_on == true)
            {
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod2_on = false;
                activeElectrodeArray[1] = 0;
            }
        }

        

        private void pp_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            waveform_pp_value.Content = pp_slider.Value;

            if( (intensity_slider1 != null) && (ibp_slider != null) && (ibn_slider != null) && (obn_slider != null))
            {
                StimuliParameters stimuliParameters = new StimuliParameters();
                stimuliParameters.msg_type = 0;
                stimuliParameters.PP = (int)pp_slider.Value;
                stimuliParameters.Pp = (int)intensity_slider1.Value;
                stimuliParameters.IBP = (int)ibp_slider.Value;
                stimuliParameters.IBN = (int)ibn_slider.Value;
                stimuliParameters.OBN = (int)obn_slider.Value;


                string stimuli_parameters_command = JsonConvert.SerializeObject(stimuliParameters);
                System.Diagnostics.Debug.WriteLine("Sending Command: ");
                System.Diagnostics.Debug.WriteLine(stimuli_parameters_command);
                if (checkStimulationParameterValidity() == 0)
                {
                    sp.Write(stimuli_parameters_command);

                }
            }
            

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

      
        private void electrode3_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod3_on == false)
            {
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod3_on = true;
                activeElectrodeArray[2] = 1;
            }
            else if (electrod3_on == true)
            {
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod3_on = false;
                activeElectrodeArray[2] = 0;
            }
        }

        private void electrode4_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod4_on == false)
            {
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod4_on = true;
                activeElectrodeArray[3] = 1;
            }
            else if (electrod4_on == true)
            {
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod4_on = false;
                activeElectrodeArray[3] = 0;
            }
        }

        private void electrode5_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod5_on == false)
            {
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod5_on = true;
                activeElectrodeArray[4] = 1;
            }
            else if (electrod5_on == true)
            {
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod5_on = false;
                activeElectrodeArray[4] = 0;
            }
        }

        private void electrode6_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod6_on == false)
            {
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod6_on = true;
                activeElectrodeArray[5] = 1;
            }
            else if (electrod6_on == true)
            {
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod6_on = false;
                activeElectrodeArray[5] = 0;
            }

        }

        private void electrode7_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod7_on == false)
            {
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod7_on = true;
                activeElectrodeArray[6] = 1;
            }
            else if (electrod7_on == true)
            {
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod7_on = false;
                activeElectrodeArray[6] = 0;
            }
        }

        private void electrode8_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod8_on == false)
            {
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod8_on = true;
                activeElectrodeArray[7] = 1;
            }
            else if (electrod8_on == true)
            {
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod8_on = false;
                activeElectrodeArray[7] = 0;
            }
        }

        private void electrode9_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod9_on == false)
            {
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod9_on = true;
                activeElectrodeArray[8] = 1;
            }
            else if (electrod9_on == true)
            {
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod9_on = false;
                activeElectrodeArray[8] = 0;
            }
        }

        private void electrode10_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod10_on == false)
            {
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod10_on = true;
                activeElectrodeArray[9] = 1;
            }
            else if (electrod10_on == true)
            {
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod10_on = false;
                activeElectrodeArray[9] = 0;
            }
        }

        private void electrode11_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod11_on == false)
            {
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod11_on = true;
                activeElectrodeArray[10] = 1;
            }
            else if (electrod11_on == true)
            {
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod11_on = false;
                activeElectrodeArray[10] = 0;
            }
        }

        private void electrode12_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod12_on == false)
            {
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod12_on = true;
                activeElectrodeArray[11] = 1;
            }
            else if (electrod12_on == true)
            {
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod12_on = false;
                activeElectrodeArray[11] = 0;
            }
        }

        private void electrode13_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod13_on == false)
            {
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod13_on = true;
                activeElectrodeArray[12] = 1;
            }
            else if (electrod13_on == true)
            {
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod13_on = false;
                activeElectrodeArray[12] = 0;
            }
        }


        private void electrode14_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod14_on == false)
            {
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod14_on = true;
                activeElectrodeArray[13] = 1;
            }
            else if (electrod14_on == true)
            {
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod14_on = false;
                activeElectrodeArray[13] = 0;
            }
        }

        private void electrode15_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod15_on == false)
            {
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod15_on = true;
                activeElectrodeArray[14] = 1;
            }
            else if (electrod15_on == true)
            {
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod15_on = false;
                activeElectrodeArray[14] = 0;
            }
        }

        private void electrode16_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod16_on == false)
            {
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod16_on = true;
                activeElectrodeArray[15] = 1;
            }
            else if (electrod16_on == true)
            {
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod16_on = false;
                activeElectrodeArray[15] = 0;
            }
        }

        private void electrode17_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod17_on == false)
            {
                channel17_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod17_on = true;
                activeElectrodeArray[16] = 1;
            }
            else if (electrod17_on == true)
            {
                channel17_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod17_on = false;
                activeElectrodeArray[16] = 0;
            }
        }

        private void electrode18_button_Click(object sender, RoutedEventArgs e)
        {
            if (electrod18_on == false)
            {
                channel18_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                electrod18_on = true;
                activeElectrodeArray[17] = 1;
            }
            else if (electrod18_on == true)
            {
                channel18_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                electrod18_on = false;
                activeElectrodeArray[17] = 0;
            }
        }

        private void vibration_button_Click(object sender, RoutedEventArgs e)
        {
            if (vibration_button_on == false)
            {
                vibration_button.Opacity = 0.2;
                vibration_button_on = true;
               // pw_slider.Value = 9;
                pp_slider.Value = 10;
                ibn_slider.Value = 9;
                ibp_slider.Value = 50;
                obn_slider.Value = 19;




            }
            else if (square_pattern_button_on == true)
            {
                //square_pattern_button.Opacity = 0;
               // square_pattern_button_on = false;
            }

        }
        public int checkStimulationParameterValidity()
        {
            for (int i = 0; i < activeElectrodeArray.Length; i++)
            {
                if (activeElectrodeArray[i] == 1)
                {
                    if (activePulsePeriodArray[i] < activePulseWidthArray[i])
                    {
                        System.Diagnostics.Debug.WriteLine("Pulse period must be less than Pulse Width.   ");
                        MessageBox.Show("Check the Stimuli: Pulse period must be less than Pulse Width.");
                        return 1;
                    }
                    if ((activePulseWidthArray[i] * activeInnerBurstNumber[i]) > activeInnerBurstPeriod[i])
                    {
                        System.Diagnostics.Debug.WriteLine("(PP*IN) must be less than IP. ");
                        MessageBox.Show("Check the Stimuli: (PP*IBN) must be less than IBP.");
                        return 1;
                    }

                    if (activeInnerBurstNumber[i] * activeInnerBurstPeriod[i] >= 2000)
                    {
                        System.Diagnostics.Debug.WriteLine("IP*IN must be less than 2000 microseconds.");
                        MessageBox.Show("Check the Stimuli: IP*IN must be less than 2000 microseconds.");

                        return 1;
                    }
                }
            }

            return 0;
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

        public class StimuliParameters
        {
            public int msg_type { get; set; }
            public int PP { get; set; }

            public int Pp { get; set; }
            public int IBN { get; set; }
            public int IBP { get; set; }
            public int OBN { get; set; }
        }
        public class IntensitySliderParamters
        {
            public int msg_type { get; set; }
            public int[] Pp { get; set; }
        }


        public class ElectrodeParameters
        {
            public int[] ele { get; set; }
            public int msg_type { get; set; }
        }

        public class DirectionCommand
        {
            public int direction_command { get; set; }
            public int msg_type { get; set; }
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

        private void send_command_Click_1(object sender, RoutedEventArgs e)
        {
            ElectrodeParameters electrodeParameters = new ElectrodeParameters();
            electrodeParameters.ele = getActiveElectrodeArray();
            electrodeParameters.msg_type = 1;

            StimuliParameters stimuliParameters = new StimuliParameters();
            stimuliParameters.msg_type = 0;
            stimuliParameters.OBN = (int)obn_slider.Value;
            stimuliParameters.IBN = (int)ibn_slider.Value;
            stimuliParameters.IBP = (int)ibp_slider.Value;
            // stimuliParameters.Pp = getIntensityValues();
            stimuliParameters.PP = getPulsePeriodValues()[0];
           // stimuliParameters.direction_preset = "";
            //stimuliParameters.taste_preset = "";



            string electrode_command = JsonConvert.SerializeObject(electrodeParameters);
            string stimuli_parameters_command = JsonConvert.SerializeObject(stimuliParameters);
            System.Diagnostics.Debug.WriteLine(electrode_command);
            System.Diagnostics.Debug.WriteLine(stimuli_parameters_command);
            if (checkStimulationParameterValidity() == 0)
            {
                sp.Write(electrode_command);

            }

        }
        static int NUM_DIRECTIONAL_PRESETS = 14;
        static int NUM_SENSATION_PRESETS = 20;
        int left_image_enabled = 0;
        int right_image_enabled = 0;
        int up_image_enabled = 0;
        int down_image_enabled = 0;
        int left_diagnol_down_image_enabled = 0;
        int right_diagnol_down_image_enabled = 0;
        int right_diagnol_up_image_enabled = 0;
        int left_diagnol_up_image_enabled = 0;
        int hexagon_image_enabled = 0;
        int triangle_image_enabled = 0;
        int square_image_enabled = 0;
        int zigzag_image_enabled = 0;
        int waterfall_up_image_enabled = 0;
        int waterfall_down_image_enabled = 0;
        int[] directional_presets = new int[NUM_DIRECTIONAL_PRESETS];
        int[] sensation_presets = new int[NUM_SENSATION_PRESETS]; 
        
        public void initialize_directional_presets()
        {
            for(int i =0; i< NUM_DIRECTIONAL_PRESETS; i++)
            {
                directional_presets[i] = 0;
            }
        }

        public void initialize_sensation_presets()
        {
            for(int i = 0; i < NUM_SENSATION_PRESETS; i++)
            {
                sensation_presets[i] = 0;
            }
        }

        public void updateElectrodeUI()
        {
            if (activeElectrodeArray[0] == 1)
            {
                color_scale = (int)((1 - (intensity_slider1.Value) / (intensity_slider1.Maximum)) * 255);
                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }


            if (activeElectrodeArray[1] == 1)
            {
                color_scale = (int)((1 - (intensity_slider2.Value) / (intensity_slider2.Maximum)) * 255);
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[2] == 1)
            {
                color_scale = (int)((1 - (intensity_slider3.Value) / (intensity_slider3.Maximum)) * 255);
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[3] == 1)
            {
                color_scale = (int)((1 - (intensity_slider4.Value) / (intensity_slider4.Maximum)) * 255);
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[4] == 1)
            {
                color_scale = (int)((1 - (intensity_slider5.Value) / (intensity_slider5.Maximum)) * 255);
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[5] == 1)
            {
                color_scale = (int)((1 - (intensity_slider6.Value) / (intensity_slider6.Maximum)) * 255);
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[6] == 1)
            {
                color_scale = (int)((1 - (intensity_slider7.Value) / (intensity_slider7.Maximum)) * 255);
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[7] == 1)
            {
                color_scale = (int)((1 - (intensity_slider8.Value) / (intensity_slider8.Maximum)) * 255);
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[8] == 1)
            {
                color_scale = (int)((1 - (intensity_slider9.Value) / (intensity_slider9.Maximum)) * 255);
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[9] == 1)
            {
                color_scale = (int)((1 - (intensity_slider10.Value) / (intensity_slider10.Maximum)) * 255);
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[10] == 1)
            {
                color_scale = (int)((1 - (intensity_slider11.Value) / (intensity_slider11.Maximum)) * 255);
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[11] == 1)
            {
                color_scale = (int)((1 - (intensity_slider12.Value) / (intensity_slider12.Maximum)) * 255);
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[12] == 1)
            {
                color_scale = (int)((1 - (intensity_slider13.Value) / (intensity_slider13.Maximum)) * 255);
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[13] == 1)
            {
                color_scale = (int)((1 - (intensity_slider14.Value) / (intensity_slider14.Maximum)) * 255);
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }


            if (activeElectrodeArray[14] == 1)
            {
                color_scale = (int)((1 - (intensity_slider15.Value) / (intensity_slider15.Maximum)) * 255);
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[15] == 1)
            {
                color_scale = (int)((1 - (intensity_slider16.Value) / (intensity_slider16.Maximum)) * 255);
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[16] == 1)
            {
                color_scale = (int)((1 - (intensity_slider17.Value) / (intensity_slider17.Maximum)) * 255);
                channel17_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel17_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }

            if (activeElectrodeArray[17] == 1)
            {
                color_scale = (int)((1 - (intensity_slider18.Value) / (intensity_slider18.Maximum)) * 255);
                channel18_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));
            }
            else
            {
                channel18_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
            }




        }
        
        private void left_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(left_image_enabled == 0)
            {
                initialize_directional_presets();
                left_image_enabled = 1;
                left_image_border.BorderThickness = new Thickness(3, 3, 3, 3);

               /* channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));


                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));*/


                on[1, 0] = 1; on[2, 0] = 1; on[3, 0] = 1; on[4, 0] = 1;
                on[1, 1] = 1; on[2, 1] = 1; on[3, 1] = 1; on[4, 1] = 1;
                updatepattern();
                directional_presets[0] = 1;

                System.Diagnostics.Debug.WriteLine(getArrayString(getActiveElectrodeArray()));

                DirectionCommand directionCommand = new DirectionCommand();
                directionCommand.msg_type = 3;
                directionCommand.direction_command = 0;

                string stimuli_parameters_command = JsonConvert.SerializeObject(directionCommand);
                System.Diagnostics.Debug.WriteLine("Sending Command: ");
                System.Diagnostics.Debug.WriteLine(stimuli_parameters_command);
                updateElectrodeUI();

                if (checkStimulationParameterValidity() == 0)
                {
                    sp.Write(stimuli_parameters_command);

                }
            }
            else
            {
                left_image_enabled = 0;
                left_image_border.BorderThickness = new Thickness(1, 1, 1, 1);

               /* channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));


                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));*/

                on[1, 0] = 0; on[2, 0] = 0; on[3, 0] = 0; on[4, 0] = 0;
                on[1, 1] = 0; on[2, 1] = 0; on[3, 1] = 0; on[4, 1] = 0;
                updatepattern();
                directional_presets[0] = 0;
                updateElectrodeUI();

                System.Diagnostics.Debug.WriteLine(getArrayString(getActiveElectrodeArray()));
            }
            
        }

        private void right_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (right_image_enabled == 0)
            {
                initialize_directional_presets();
                right_image_enabled = 1;
                right_image_border.BorderThickness = new Thickness(3, 3, 3, 3);

                /*channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));


                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));*/

                on[1, 3] = 1; on[2, 3] = 1; on[3, 3] = 1; on[4, 3] = 1;
                on[1, 2] = 1; on[2, 2] = 1; on[3, 2] = 1; on[4, 2] = 1;
                updatepattern();
                directional_presets[1] = 1;
                updateElectrodeUI();

                System.Diagnostics.Debug.WriteLine(getArrayString(getActiveElectrodeArray()));
            }
            else
            {
                right_image_enabled = 0;
                right_image_border.BorderThickness = new Thickness(1, 1, 1, 1);

               /* channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));


                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));*/

                on[1, 3] = 0; on[2, 3] = 0; on[3, 3] = 0; on[4, 3] = 0;
                on[1, 2] = 0; on[2, 2] = 0; on[3, 2] = 0; on[4, 2] = 0;
                updatepattern();
                directional_presets[1] = 0;
                updateElectrodeUI();

                System.Diagnostics.Debug.WriteLine(getArrayString(getActiveElectrodeArray()));
            }
        }

        private void up_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (up_image_enabled == 0)
            {
                initialize_directional_presets();
                up_image_enabled = 1;
                up_image_border.BorderThickness = new Thickness(3, 3, 3, 3);

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                on[1, 0] = 1; on[1, 2] = 1; on[2, 0] = 1; on[2, 2] = 1;
                on[1, 1] = 1; on[1, 3] = 1; on[2, 1] = 1; on[2, 3] = 1;
                updatepattern();
                directional_presets[2] = 1;

                System.Diagnostics.Debug.WriteLine(getArrayString(getActiveElectrodeArray()));
            }
            else
            {
                up_image_enabled = 0;
                up_image_border.BorderThickness = new Thickness(1, 1, 1, 1);

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                on[1, 0] = 0; on[1, 2] = 0; on[2, 0] = 0; on[2, 2] = 0;
                on[1, 1] = 0; on[1, 3] = 0; on[2, 1] = 0; on[2, 3] = 0;
                updatepattern();
                directional_presets[2] = 0;

                System.Diagnostics.Debug.WriteLine(getArrayString(getActiveElectrodeArray()));
            }

        }

        private void down_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (down_image_enabled == 0)
            {
                initialize_directional_presets();
                down_image_enabled = 1;
                down_image_border.BorderThickness = new Thickness(3, 3, 3, 3);

                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                on[3, 0] = 1; on[3, 2] = 1; on[4, 0] = 1; on[4, 2] = 1;
                on[3, 1] = 1; on[3, 3] = 1; on[4, 1] = 1; on[4, 3] = 1;
                updatepattern();
                directional_presets[3] = 1;

                System.Diagnostics.Debug.WriteLine(getArrayString(getActiveElectrodeArray()));
            }
            else
            {
                down_image_enabled = 0;
                down_image_border.BorderThickness = new Thickness(1, 1, 1, 1);

                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                on[3, 0] = 0; on[3, 2] = 0; on[4, 0] = 0; on[4, 2] = 0;
                on[3, 1] = 0; on[3, 3] = 0; on[4, 1] = 0; on[4, 3] = 0;
                updatepattern();
                directional_presets[3] = 1;

                System.Diagnostics.Debug.WriteLine(getArrayString(getActiveElectrodeArray()));
            }

        }

        private void left_diagnol_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (left_diagnol_down_image_enabled == 0)
            {
                initialize_directional_presets();
                left_diagnol_down_image_enabled = 1;
                left_diagnol_image_border.BorderThickness = new Thickness(3, 3, 3, 3);

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                on[3, 0] = 0; on[3, 2] = 0; on[4, 0] = 0; on[4, 2] = 0;
                on[3, 1] = 0; on[3, 3] = 0; on[4, 1] = 0; on[4, 3] = 0;
                updatepattern();
                directional_presets[4] = 1;

                System.Diagnostics.Debug.WriteLine(getArrayString(getActiveElectrodeArray()));




            }
            else
            {
                left_diagnol_down_image_enabled = 0;
                left_diagnol_image_border.BorderThickness = new Thickness(1, 1, 1, 1);

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                updatepattern();
                directional_presets[4] = 0;

                System.Diagnostics.Debug.WriteLine(getArrayString(getActiveElectrodeArray()));

            }
        }

      

        private void right_diagnol_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (right_diagnol_down_image_enabled == 0)
            {
                initialize_directional_presets();
                right_diagnol_down_image_enabled = 1;
                right_diagnol_image_border.BorderThickness = new Thickness(3, 3, 3, 3);

                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                directional_presets[5] = 1;

            }
            else
            {
                right_diagnol_down_image_enabled = 0;
                right_diagnol_image_border.BorderThickness = new Thickness(1, 1, 1, 1);

                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                directional_presets[5] = 0;
            }
        }

        private void right_diagnol_up_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (right_diagnol_up_image_enabled == 0)
            {
                initialize_directional_presets();
                right_diagnol_up_image_enabled = 1;
                right_diagnol_up_image_border.BorderThickness = new Thickness(3, 3, 3, 3);

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                directional_presets[6] = 1;

            }
            else
            {
                right_diagnol_up_image_enabled = 0;
                right_diagnol_up_image_border.BorderThickness = new Thickness(1, 1, 1, 1);

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                directional_presets[6] = 0;


            }
        }

        private void left_diagnol_up_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (left_diagnol_up_image_enabled == 0)
            {
                initialize_directional_presets();
                left_diagnol_up_image_enabled = 1;
                left_diagnol_up_image_border.BorderThickness = new Thickness(3, 3, 3, 3);

                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                directional_presets[7] = 1;
            }
            else
            {
                left_diagnol_up_image_enabled = 0;
                left_diagnol_up_image_border.BorderThickness = new Thickness(1, 1, 1, 1);

                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                directional_presets[7] = 0;
            }
        }

        private void hexagon_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (hexagon_image_enabled == 0)
            {
                initialize_directional_presets();
                hexagon_image_enabled = 1;
                hexagon_image_border.BorderThickness = new Thickness(3, 3, 3, 3);

                
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                

                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                directional_presets[11] = 1;
            }
            else
            {
                hexagon_image_enabled = 0;
                hexagon_image_border.BorderThickness = new Thickness(1, 1, 1, 1);

                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));


                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                directional_presets[11] = 0;
            }
        }

        private void triangle_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (triangle_image_enabled == 0)
            {
                initialize_directional_presets();
                triangle_image_enabled = 1;
                triangle_image_border.BorderThickness = new Thickness(3, 3, 3, 3); ;
                
                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                directional_presets[9] = 1;

            }
            else
            {
                triangle_image_enabled = 0;
                triangle_image_border.BorderThickness = new Thickness(1, 1, 1, 1); ;

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                directional_presets[9] = 0;
            }
        }

        private void zigzag_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (zigzag_image_enabled == 0)
            {
                initialize_directional_presets();
                zigzag_image_enabled = 1;
                zigzag_image_border.BorderThickness = new Thickness(3, 3, 3, 3); ;

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));

                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                directional_presets[8] = 1;
            }
            else
            {
                zigzag_image_enabled = 0;
                zigzag_image_border.BorderThickness = new Thickness(1, 1, 1, 1);

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                
                
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                directional_presets[8] = 0;
            }
        }

        private void square_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (square_image_enabled == 0)
            {
                initialize_directional_presets();
                square_image_enabled = 1;
                square_image_border.BorderThickness = new Thickness(3, 3, 3, 3); ;

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
                directional_presets[10] = 1;
            }
            else
            {
                square_image_enabled = 0;
                square_image_border.BorderThickness = new Thickness(1, 1, 1, 1); ;

                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));

                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 244, 244));
                directional_presets[10] = 0;
            }
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
            using (StreamReader r = new StreamReader(openFileDialog.FileName))
            {
                string json = r.ReadToEnd();

                tacttongue_stimuli_json = JsonConvert.DeserializeObject<List<TactTongueStimuli>>(json);
                System.Diagnostics.Debug.WriteLine(tacttongue_stimuli_json[0].IBN + " ");
                updateUIForStimuli(tacttongue_stimuli_json[0]);
                updatePWSliders(tacttongue_stimuli_json[0].Pp);

            }
            
            
        }


        private void waterfall_up_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (waterfall_up_image_enabled == 0)
            {
                initialize_directional_presets();
                waterfall_up_image_enabled = 1;
                waterfall_up_image_border.BorderThickness = new Thickness(3, 3, 3, 3);
                directional_presets[12] = 1;
            }
            else
            {
                waterfall_up_image_enabled = 0;
                waterfall_up_image_border.BorderThickness = new Thickness(1, 1, 1, 1);
                directional_presets[12] = 0;
            }
        }

        private void waterfall_down_image_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (waterfall_down_image_enabled == 0)
            {
                initialize_directional_presets();
                waterfall_down_image_enabled = 1;
                waterfall_down_image_border.BorderThickness = new Thickness(3, 3, 3, 3);
                directional_presets[13] = 1;
            }
            else
            {
                waterfall_down_image_enabled = 0;
                waterfall_down_image_border.BorderThickness = new Thickness(1, 1, 1, 1);
                directional_presets[13] = 0;
            }
        }

       

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            TactTongueStimuli stimuli = new TactTongueStimuli
            {
                ele_array = getActiveElectrodeArray(),
                PP = getPulsePeriodValues()[0],
                Pp = getIntensityValues()[0],
                IBN = getIBNValues()[0],
                IBP = getIBPValues()[0],
                OBN = getOBNValues()[0],
                direction_preset = getArrayString(directional_presets)


            };
            string fileName = "TactTongue-Parameters.json";
            string jsonString = JsonConvert.SerializeObject(stimuli);
            File.WriteAllText(fileName, jsonString);
        }

        public void reset_activated_presets()
        {
            for(int i = 0; i< NUM_SENSATION_PRESETS; i++)
            {
                presets_activated[i] = false;
            }
        }
        public int get_activated_preset()
        {
            for(int i =0; i < NUM_SENSATION_PRESETS; i++)
            {
                if (presets_activated[i] == true)
                {
                    return i;
                }
            }
            return -1;
        }
        
        public void render_default_presets()
        {
            vibration.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            pulsating.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            itching.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            strong.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            prickling.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            gentle.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            faint.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            tickling.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            localized.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            diffuse.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            soothing.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            twitching.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            calming.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            squeezing.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            energizing.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            salty.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            sour.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            bitter.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            umami.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            sweet.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));

        }
        public void render_active_presets()
        {
            int get_active_sensation_preset = get_activated_preset();
            if (get_active_sensation_preset == -1)
            {

            }
            else
            {
                if (get_active_sensation_preset == 0)
                {
                    vibration.Background = new SolidColorBrush(Color.FromArgb(128,255, 0, 128));
                }
                else if (get_active_sensation_preset == 1)
                {
                    pulsating.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 2)
                {
                    itching.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 3)
                {
                    strong.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 4)
                {
                    prickling.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 5)
                {
                    gentle.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 6)
                {
                    faint.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 7)
                {
                    tickling.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 8)
                {
                    localized.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 9)
                {
                    diffuse.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 10)
                {
                    soothing.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 11)
                {
                    twitching.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 12)
                {
                    calming.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 13)
                {
                    squeezing.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 14)
                {
                    energizing.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 15)
                {
                    salty.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 16)
                {
                    sour.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 17)
                {
                    bitter.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 18)
                {
                    umami.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }
                else if (get_active_sensation_preset == 19)
                {
                    sweet.Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 128));
                }

            }
        }

        private void vibration_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[0] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[0] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[0] = false;
                render_default_presets();
            } 
            
        }

        private void pulsating_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[1] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[1] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[1] = false;
                render_default_presets();
            }

        }

        private void itching_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[2] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[2] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[2] = false;
                render_default_presets();
            }

        }

        private void strong_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[3] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[3] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[3] = false;
                render_default_presets();
            }
        }

        private void prickling_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[4] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[4] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[4] = false;
                render_default_presets();
            }
        }

        private void gentle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[5] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[5] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[5] = false;
                render_default_presets();
            }
        }

        private void faint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[6] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[6] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[6] = false;
                render_default_presets();
            }
        }

        private void tickling_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[7] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[7] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[7] = false;
                render_default_presets();
            }
        }

        private void localized_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[8] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[8] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[8] = false;
                render_default_presets();
            }
        }

        private void diffuse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[9] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[9] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[9] = false;
                render_default_presets();
            }
        }

        private void soothing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[10] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[10] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[10] = false;
                render_default_presets();
            }
        }

        private void twitching_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[11] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[11] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[11] = false;
                render_default_presets();
            }
        }

        private void calming_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[12] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[12] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[12] = false;
                render_default_presets();
            }
        }

        private void squeezing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[13] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[13] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[13] = false;
                render_default_presets();
            }
        }

        private void energizing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[14] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[14] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[14] = false;
                render_default_presets();
            }
        }

        private void salty_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[15] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[15] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[15] = false;
                render_default_presets();
            }
        }

        private void sour_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[16] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[16] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[16] = false;
                render_default_presets();
            }
        }

        private void bitter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[17] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[17] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[17] = false;
                render_default_presets();
            }
        }

        private void umami_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[18] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[18] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[18] = false;
                render_default_presets();
            }
        }

        private void sweet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (presets_activated[19] == false)
            {
                reset_activated_presets();
                render_default_presets();
                presets_activated[19] = true;
                render_active_presets();
            }
            else
            {
                presets_activated[19] = false;
                render_default_presets();
            }
        }
        public void updatePWSliders(int value)
        {
            intensity_slider1.Value = value;
            intensity_slider2.Value = value;
            intensity_slider3.Value = value;
            intensity_slider4.Value = value;
            intensity_slider5.Value = value;
            intensity_slider6.Value = value;
            intensity_slider7.Value = value;
            intensity_slider8.Value = value;
            intensity_slider9.Value = value;
            intensity_slider10.Value = value;
            intensity_slider11.Value = value;
            intensity_slider12.Value = value;
            intensity_slider13.Value = value;
            intensity_slider14.Value = value;
            intensity_slider15.Value = value;
            intensity_slider16.Value = value;
            intensity_slider17.Value = value;
            intensity_slider18.Value = value;
        }
        public void updateUIForStimuli(TactTongueStimuli item)
        {
            pp_slider.Value = item.PP;
            ibn_slider.Value = item.IBN;
            ibp_slider.Value = item.IBP;
            obn_slider.Value = item.OBN;
            updatePWSliders(item.Pp);

        }

        private void tacttongue_stimuli_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
           // System.Diagnostics.Debug.WriteLine(text);
            var stimuli_item = tacttongue_stimuli_json.Find(x => x.stimulli_name == text);
            //System.Diagnostics.Debug.WriteLine(stimuli_item.IBP + " ");
            updateUIForStimuli(stimuli_item);

            StimuliParameters stimuliParameters = new StimuliParameters();
            stimuliParameters.msg_type = 0;
            stimuliParameters.PP = stimuli_item.PP;
            stimuliParameters.Pp = stimuli_item.Pp;
            stimuliParameters.IBP = stimuli_item .IBP;
            stimuliParameters.IBN = stimuli_item.IBN;
            stimuliParameters.OBN = stimuli_item.OBN;


            string stimuli_parameters_command = JsonConvert.SerializeObject(stimuliParameters);
            System.Diagnostics.Debug.WriteLine("Sending Command: ");
            System.Diagnostics.Debug.WriteLine(stimuli_parameters_command);
            if (checkStimulationParameterValidity() == 0)
            {
                sp.Write(stimuli_parameters_command);

            }



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IntensitySliderParamters intensitySliderParamters = new IntensitySliderParamters();
            intensitySliderParamters.msg_type = 2;
            intensitySliderParamters.Pp = getIntensityValues();

            string stimuli_parameters_command = JsonConvert.SerializeObject(intensitySliderParamters);
            System.Diagnostics.Debug.WriteLine("Sending Command: ");
            System.Diagnostics.Debug.WriteLine(stimuli_parameters_command);

            if (checkStimulationParameterValidity() == 0)
            {
                sp.Write(stimuli_parameters_command);

            }
        }

        private void select_all_electrodes_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            if(select_all_electrodes_checkbox.IsChecked == true)
            {
                initializeElectrodeArray();

                color_scale = (int)((1 - (intensity_slider1.Value) / (intensity_slider1.Maximum)) * 255);
                channel1_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider2.Value) / (intensity_slider2.Maximum)) * 255);
                channel2_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider3.Value) / (intensity_slider3.Maximum)) * 255);
                channel3_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider4.Value) / (intensity_slider4.Maximum)) * 255);
                channel4_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider5.Value) / (intensity_slider5.Maximum)) * 255);
                channel5_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider6.Value) / (intensity_slider6.Maximum)) * 255);
                channel6_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider7.Value) / (intensity_slider7.Maximum)) * 255);
                channel7_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider8.Value) / (intensity_slider8.Maximum)) * 255);
                channel8_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider9.Value) / (intensity_slider9.Maximum)) * 255);
                channel9_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider10.Value) / (intensity_slider10.Maximum)) * 255);
                channel10_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider11.Value) / (intensity_slider11.Maximum)) * 255);
                channel11_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider12.Value) / (intensity_slider12.Maximum)) * 255);
                channel12_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider13.Value) / (intensity_slider13.Maximum)) * 255);
                channel13_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider14.Value) / (intensity_slider14.Maximum)) * 255);
                channel14_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider15.Value) / (intensity_slider15.Maximum)) * 255);
                channel15_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider16.Value) / (intensity_slider16.Maximum)) * 255);
                channel16_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider17.Value) / (intensity_slider17.Maximum)) * 255);
                channel17_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

                color_scale = (int)((1 - (intensity_slider18.Value) / (intensity_slider18.Maximum)) * 255);
                channel18_ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, (byte)color_scale, 0));

            }
        }

        private void set_parameters_Click(object sender, RoutedEventArgs e)
        {
            StimuliParameters stimuliParameters = new StimuliParameters();
            stimuliParameters.msg_type = 4;
            stimuliParameters.PP =  (int)pp_slider.Value;
            stimuliParameters.Pp =  (int)intensity_slider1.Value;
            stimuliParameters.IBN = (int)ibn_slider.Value;
            stimuliParameters.IBP = (int)ibp_slider.Value;
            stimuliParameters.OBN = (int)obn_slider.Value;


            string stimuli_parameters_command = JsonConvert.SerializeObject(stimuliParameters);
            System.Diagnostics.Debug.WriteLine("Sending Command: ");
            System.Diagnostics.Debug.WriteLine(stimuli_parameters_command);
            if (checkStimulationParameterValidity() == 0)
            {
                sp.Write(stimuli_parameters_command);

            }
        }
    }
}
