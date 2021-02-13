using Microsoft.AspNetCore.Components;
using NAudio.CoreAudioApi;

namespace BlazorServerApp.Pages
{
    public class IndexBase : ComponentBase
    {
        public string CurrentDeviceName { get; set; }
        public MMDevice CurrentDevice;

        public void Record()
        {
            ;
        }
    }
}
