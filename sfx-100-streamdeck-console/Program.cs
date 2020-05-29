using System;
using System.Collections.Generic;
using System.Reflection;
using sfx_100_streamdeck_plugin;

namespace sfx_100_streamdeck_console
{
    class Program
    {
        static void Main(string[] args)
        {
            var contractMethods = typeof(sfx_100_streamdeck_pipecontract.ISfxStreamDeckPipeContract).GetMethods();
            PipeServerConnection.Instance.RestartChannel();
            object result = Int32.MinValue;
            List<object> paramElems = new List<object>();

            if (args.Length == 0 || (args.Length == 1 && args[0] == "?"))
            {
                Console.WriteLine("Command and params missing - Usage:");
                Console.WriteLine("sfx-100-streamdeck-console.exe (CommandName) [arg1] [arg2] ...");
                Console.WriteLine("When using Effect names and Controller names (SFX, Heave, Pitch ...), these MUST EXACTLY match the ones defined in SimFeedback");
                Console.WriteLine("Responses are also provided as Exit Code (Converted to integer))");
                Console.WriteLine("");
                Console.WriteLine("List of possible Commands:");
                Console.WriteLine("");
                
                foreach (var contractMethod in contractMethods)
                {
                    Console.WriteLine(contractMethod);
                }
                Console.WriteLine("");
                Console.WriteLine("Examples:");
                Console.WriteLine("sfx-100-streamdeck-console.exe EnableAllEffects ");
                Console.WriteLine("sfx-100-streamdeck-console.exe SetOverallIntensity 50");
                Console.WriteLine("sfx-100-streamdeck-console.exe EffectIntensitySet Heave 20");
                Console.WriteLine("sfx-100-streamdeck-console.exe EffectIntensityIncrement Heave 1      (The 1 is steps to increment by)");
                Console.WriteLine("sfx-100-streamdeck-console.exe ControllerIntensityIncrement SFX 1    (The 1 is steps to increment by)");
                Console.WriteLine("");
            }
            else
            {
                // Prüfen ob das Kommando enthalten ist

                foreach (var contractMethod in contractMethods)
                {
                    if (args[0] != contractMethod.Name) continue;
                    MethodInfo mi = PipeServerConnection.Instance.Channel.GetType().GetMethod(contractMethod.Name);

                    if (mi != null)
                    {
                        Console.WriteLine("Method found: " + mi.Name);

                        var miParams = mi.GetParameters();

                        foreach (var paramx in miParams)
                        {
                            Console.WriteLine("Param found: " + paramx.Name);
                        }

                        Console.WriteLine("Command line arguments: " + (args.Length - 1));
                        Console.WriteLine("Method arguments: " + miParams.Length);

                        if (miParams.Length > 0 && (args.Length -1) == miParams.Length)
                        {
                            // Prüfen ob die Anzahl der Parameter mit der Anzahl der Argumente übereinstimmt
                            if (args.Length - 1 == miParams.Length)
                            {
                                Console.WriteLine("Args count correct");

                                // Argumente aufbauen
                                
                                for (int i = 1; i < args.Length; i++)
                                {
                                    Type paramType = miParams[i - 1].ParameterType;
                                    Console.WriteLine("Using arg: " + args[i] + " - converting to type: " +
                                                      miParams[i - 1].ParameterType);
                                    paramElems.Add(Convert.ChangeType(args[i], paramType));
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong Parameter count. Check Examples by calling \"sfx-100-streamdeck-console.exe ?\"");
                            Environment.Exit(Convert.ToInt32(result));
                        }

                        try
                        {
                            result = mi.Invoke(PipeServerConnection.Instance.Channel, paramElems.ToArray());
                            Console.WriteLine("Result: " + result);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: PipeServer not found, is SimFeedback running and Streamdeck Extension active? ");
                            Console.WriteLine(e.Message);
                        }
                        Environment.Exit(Convert.ToInt32(result));
                    }
                    else
                    {
                        Console.WriteLine("Error getting Method.");
                    }

                    Console.WriteLine(result);
                    Environment.Exit(Convert.ToInt32(result));
                }
                Console.WriteLine("Method not found. Check Spelling, available Methods and Examples by calling \"sfx-100-streamdeck-console.exe ?\"");
            }
        }
    }
}
