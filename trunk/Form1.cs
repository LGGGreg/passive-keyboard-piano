using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;

namespace KeyboardPiano
{
    public partial class Form1 : Form
    {
        private OutputDevice outDevice;

        private int outDeviceID = 0;

        private OutputDeviceDialog outDialog = new OutputDeviceDialog();
        private ChannelMessageBuilder builder = new ChannelMessageBuilder();
        

        public Form1()
        {
            InitializeComponent();            
        }
        protected override void OnResize(EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            if(OutputDevice.DeviceCount == 0)
            {
                MessageBox.Show("No MIDI output devices available.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);

                Close();
            }
            else
            {
                try
                {
                    outDevice = new OutputDevice(outDeviceID);
                    sendMsg(true, 900, 0, 0);
                

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    Close();
                }
            }

            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            
            if(outDevice != null)
            {
                outDevice.Dispose();
            }

            outDialog.Dispose();

            base.OnClosed(e);
        }

        public void changeInstrument(int instru)
        {

            outDevice.Send(new ChannelMessage(
                ChannelCommand.ProgramChange, 0, instru, 0));
        }
        public void sendMsg(bool on,int id,int volume,int chanel)
        {
            ChannelCommand c = ChannelCommand.NoteOff;
            
            int param = 0;
            if(on)
            {
                param = volume;//volume!
                //param = 20;
                c = ChannelCommand.NoteOn;
            }
            if (id > 127)
            {
                //space
                if (on)
                {
                    //return;
                    id = 31;
                    id = 0;
                    volume = 80;
                }
                else
                {
                    id = 31;
                    id = 0;
                    volume = 77;
                }
                c = ChannelCommand.PitchWheel;
            }
            outDevice.Send(new ChannelMessage(
                c, chanel, id, param));
        }



        private void HandleSysExMessagePlayed(object sender, SysExMessageEventArgs e)
        {
       //     outDevice.Send(e.Message); Sometimes causes an exception to be thrown because the output device is overloaded.
        }



        private void pianoControl1_PianoKeyDown(object sender, PianoKeyEventArgs e)
        {
            outDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, e.NoteID, 127));
        }

        private void pianoControl1_PianoKeyUp(object sender, PianoKeyEventArgs e)
        {
            outDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, e.NoteID, 0));
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AboutDialog dlg = new AboutDialog();

            dlg.ShowDialog();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            outDialog.ShowDialog();
            if(outDeviceID==outDialog.OutputDeviceID)return;
            outDeviceID = outDialog.OutputDeviceID;
            outDevice = new OutputDevice(outDeviceID);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (outDevice != null)
            {
                changeInstrument(comboBox1.SelectedIndex);
            }
        }        
    }
}