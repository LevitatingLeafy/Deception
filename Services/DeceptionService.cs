using Models;
using Modules;
using Utils;

using System;
using System.IO;

namespace Services
{
    public static class DeceptionService
    {
        public static void Run(string file, Module mod)
        {
            WriteService.Progress(mod.Name);

            string sourceFile = null;
            // Generate
            sourceFile = GeneratorService.Generate(file, mod);

            // Compile
            if (sourceFile != null)
            {
                string compiledSource = CompilerService.Compile(sourceFile);
                //Console.WriteLine();

                if (compiledSource != null)
                {
                    // Suggested PS
                    CommandService.Suggest(compiledSource, mod.Type);
                }
                else
                {
                    WriteService.ErrorExit("Compilation Failed");
                }
                Console.WriteLine();
            }
        }
    }
}