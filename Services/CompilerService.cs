using System;
using System.IO;
using System.Diagnostics;

namespace Services
{
    public static class CompilerService
    {
        public static string Compile(string source)
        {
            string result = null;

            int extPos = source.LastIndexOf(".");
            if (extPos >= 0)            
            {
                // Project dir
                string curDir = Directory.GetCurrentDirectory();
                // Console.WriteLine("curDir: " + curDir);                

                // Payload Dir
                string payloadDir = curDir + "/Payloads/";

                // Source File
                string sourcePath = payloadDir + source;

                // Target File
                string target = source.Substring(0, extPos);
                target += ".exe";
                string targetPath = payloadDir + target;

                // Dependencies: Kernel32.cs
                string kernel32Path = curDir + "/Utils/Kernel32.cs";                

                // Set Process args
                // Ex. mcs -out:test.exe -pkg:dotnet ../Util/Kernel32.cs a4aaef12-2613-4a84-b1fa-517df814d4ba.cs
                string filename = "mcs";
                string args = "-out:" + targetPath + " -pkg:dotnet " + kernel32Path + " " + sourcePath;
                string stdout = ""; 
                string stderr = "";

                // Console.WriteLine("sourcePath: " + sourcePath);
                // Console.WriteLine("targetPath: " + targetPath);
                // Console.WriteLine("kernel32Path: " + kernel32Path);
                // Console.WriteLine("args: " + args);

                try
                {
                    Process proc = new Process();
                    proc.StartInfo = new ProcessStartInfo();
                    proc.StartInfo.FileName = filename;
                    proc.StartInfo.Arguments = args;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.Start();
                    stdout = proc.StandardOutput.ReadToEnd();
                    stderr = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();
                    result = target;

                    // Console.ForegroundColor = ConsoleColor.Green;
                    // Console.WriteLine("[+] Compile payload to: " + "./Paylods/" + target);
                    // Console.ResetColor();
                    // WriteService.Success("Compile payload:  " + "./Paylods/" + target);
                }
                catch (Exception e)
                {
                    WriteService.Error("Compiler error: " +  e);
                    //Console.WriteLine("[!] Compiler error: " +  e);                    
                    result = null;
                }

                // Console.WriteLine("stdout: " + stdout);
                // Console.WriteLine("stderr: " + stderr);

                if (stderr == "")
                {
                    // WriteService.Success("Compiled payload: " + "./Payloads/" + target);
                    WriteService.Success("Compiled payload: " + targetPath);
                }
                else 
                {                    
                    WriteService.Error("Compiler stdout: " + stdout);
                    WriteService.Error("Compiler stderr: " + stderr);
                    result = null;
                }
            }
            else
            {
                WriteService.Error("Cannot parse generated filename");
            }

            return result;
        }
    }
}