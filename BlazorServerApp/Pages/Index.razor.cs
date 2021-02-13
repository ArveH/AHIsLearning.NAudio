using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace BlazorServerApp.Pages
{
    public class IndexBase : ComponentBase
    {
        public string CurrentDeviceName { get; set; }
        public MMDevice CurrentDevice;

        private WaveFileWriter _writer;
        private WasapiLoopbackCapture _captureDevice;

        public void DeviceChanged(MMDevice device)
        {
            CurrentDevice = device;
        }

        public void StartRecording()
        {
            _captureDevice = new WasapiLoopbackCapture(CurrentDevice);
            _captureDevice.DataAvailable += OnDataAvailable;
            _captureDevice.RecordingStopped += OnRecordingStopped;

            _writer = new WaveFileWriter(GetFileName(), _captureDevice.WaveFormat);
            _captureDevice.StartRecording();
        }

        public void StopRecording()
        {
            Debug.WriteLine("StopRecording");
            _captureDevice?.StopRecording();
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            FinalizeWaveFile();
            if (e.Exception != null)
            {
                Debug.WriteLine(e.Exception.Message);
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            _writer.Write(e.Buffer, 0, e.BytesRecorded);
            int secondsRecorded = (int)(_writer.Length / _writer.WaveFormat.AverageBytesPerSecond);
            if (secondsRecorded >= 30)
            {
                StopRecording();
            }
        }

        private string GetFileName()
        {
            var deviceName = _captureDevice.GetType().Name;
            var sampleRate = $"{_captureDevice.WaveFormat.SampleRate / 1000}kHz";
            var channels = _captureDevice.WaveFormat.Channels == 1 ? "mono" : "stereo";

            return $"{deviceName} {sampleRate} {channels} {DateTime.Now:yyy-MM-dd HH-mm-ss}.wav";
        }

        private void FinalizeWaveFile()
        {
            _writer?.Dispose();
            _writer = null;
        }
    }
}
