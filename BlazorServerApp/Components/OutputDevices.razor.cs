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
        protected List<MMDevice> LoopbackDevices;
        protected bool Show;
        protected string CurrentDeviceName;

        private MMDevice _currentDevice;


        [Parameter]
        public EventCallback<MMDevice> OnChangedDevice { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var deviceEnum = new MMDeviceEnumerator();
                LoopbackDevices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
                var device = LoopbackDevices.FirstOrDefault();
                CurrentDeviceName = device?.FriendlyName;
                await OnSelect(device);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                LoopbackDevices = new List<MMDevice>();
            }
        }

        protected async Task OnSelect(MMDevice device)
        {
            _currentDevice = device;
            CurrentDeviceName = device.FriendlyName;

            Show = false;

            await OnChangedDevice.InvokeAsync(_currentDevice);
        }
    }
}
