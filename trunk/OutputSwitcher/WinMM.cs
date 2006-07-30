using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace OutputSwitcher
{
    class WinMM
    {
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        public struct WaveOutCaps { 
            public ushort wMid;
            public ushort wPid;
            public uint vDriverVersion; 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
            public string szProductName; 
            public uint dwFormats; 
            public ushort wChannels; 
            public ushort wReserved1; 
            public uint dwSupport; 
        } 

        [DllImport("winmm.dll", CharSet=CharSet.Auto)]
        static extern uint waveOutGetNumDevs();
        
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        static extern uint waveOutGetDevCaps(IntPtr uDeviceID, out WaveOutCaps pWoc, uint cbwoc);

        // Gets the number of installed wave Output devices
        private static uint GetNumDevices()
        {
            return waveOutGetNumDevs();
        }

        // Gets the device name assosicated with the deviceID
        private static String GetDeviceName(int deviceID)
        {
            WaveOutCaps waveCaps = new WaveOutCaps();
            waveOutGetDevCaps((IntPtr)deviceID, out waveCaps, (uint)Marshal.SizeOf(waveCaps));
            return waveCaps.szProductName;
        }

        private static string registryRoot = "HKEY_CURRENT_USER\\Software\\Microsoft\\Multimedia\\Sound Mapper";
        
        // Gets the Default Playback Device from the Registry
        private static string GetCurrentPlaybackDevice()
        {
            return (string)Registry.GetValue(registryRoot, "Playback", "");
        }

        // Sets the Default Playback Sound Mapper device in the registry
        private static void SetCurrentPlaybackDevice(string deviceName)
        {
            Registry.SetValue(registryRoot, "Playback", deviceName, RegistryValueKind.String);
        }

        // Switches the Ouput Device
        public static void switchOuputDevice()
        {
            for(int i=0; i<GetNumDevices(); i++)
            {
                if (GetCurrentPlaybackDevice() != GetDeviceName(i))
                {
                    SetCurrentPlaybackDevice(GetDeviceName(i));
                }
            }
        }
    }
}
