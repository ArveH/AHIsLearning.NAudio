using NAudio.CoreAudioApi;
using System;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            var deviceEnum = new MMDeviceEnumerator();
            var loopbackDevices = deviceEnum.EnumerateAudioEndPoints(
                DataFlow.Render, DeviceState.Active).ToList();

            comboLoopback.DisplayMember = nameof(MMDevice.FriendlyName);
            comboLoopback.ValueMember = nameof(MMDevice.ID);
            comboLoopback.DataSource = new BindingSource(loopbackDevices, null);

            base.OnLoad(e);
        }
    }
}
