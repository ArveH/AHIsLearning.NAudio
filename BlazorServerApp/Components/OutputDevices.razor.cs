using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NAudio.CoreAudioApi;

namespace BlazorServerApp.Components
{
    public class OutputDevicesBase : ComponentBase
    {
        protected List<MMDevice> _loopbackDevices;
        protected bool _show;

        [Parameter]
        public string CurrentDeviceName { get; set; }

        [Parameter]
        public MMDevice CurrentDevice { get; set; }

        protected override Task OnInitializedAsync()
        {
            try
            {
                var deviceEnum = new MMDeviceEnumerator();
                _loopbackDevices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
                CurrentDeviceName = _loopbackDevices.FirstOrDefault()?.FriendlyName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _loopbackDevices = new List<MMDevice>();
            }

            return Task.CompletedTask;
        }

        protected void OnSelect(MMDevice device)
        {
            CurrentDeviceName = device.FriendlyName;
            CurrentDevice = device;
            _show = false;
        }
    }
}
