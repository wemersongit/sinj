using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace util.BRLight
{
    public class MemoryManagement
    {
      //  [DllImport("kernel32.dll")]
     //   private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        public void FlushMemory()
        {
            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.SuppressFinalize(this); 
          //  if (Environment.OSVersion.Platform == PlatformID.Win32NT)
           //     SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }
    }
}
