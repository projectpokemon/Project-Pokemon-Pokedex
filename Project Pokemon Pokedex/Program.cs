using DotNetNdsToolkit;
using IPS_Pages_Publisher.Interfaces;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TextTemplating;
using Newtonsoft.Json;
using pk3DS.Core;
using pk3DS.Core.Structures;
using pk3DS.Core.Structures.PersonalInfo;
using ProjectPokemon.Pokedex.Models;
using ProjectPokemon.Pokedex.Models.EOS;
using ProjectPokemon.Pokedex.Models.Gen7;
using ProjectPokemon.Pokedex.Models.PSMD;
using SkyEditor.Core.IO;
using SkyEditor.ROMEditor.MysteryDungeon.Explorers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPokemon.Pokedex
{
    partial class Program
    {

        public static void RunProgram(string program, string args)
        {
            var p = new Process();
            p.StartInfo.FileName = program;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(program);
            p.StartInfo.Arguments = args;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;

            p.Start();

            p.WaitForExit();
        }   

        static string BuildAndReturnTemplate<T>(object model) where T : class
        {
            // Verify the type is correct
            var templateType = typeof(T);
            var sessionProperty = templateType.GetProperty("Session");
            var initializeMethod = templateType.GetMethod("Initialize", new Type[] { });
            var transformMethod = templateType.GetMethod("TransformText", new Type[] { });

            if (sessionProperty == null || sessionProperty.PropertyType != typeof(IDictionary<string, object>))
            {
                throw new ArgumentException("Type parameter must have a Session property of type IDictionary<string, object>.", nameof(T));
            }
            if (initializeMethod == null || initializeMethod.GetParameters().Length > 0)
            {
                throw new ArgumentException("Type parameter must have a Initialize() method with no parameters.", nameof(T));
            }
            if (transformMethod == null || transformMethod.GetParameters().Length > 0)
            {
                throw new ArgumentException("Type parameter must have a TransformText() method with no parameters.", nameof(T));
            }

            // Set up the template
            var templateInstance = templateType.GetConstructor(new Type[] { }).Invoke(new object[] { });
            var session = new TextTemplatingSession();
            sessionProperty.SetValue(templateInstance, session);
            session["Model"] = model;
            initializeMethod.Invoke(templateInstance, new object[] { });

            // Run the template
            var resultText = transformMethod.Invoke(templateInstance, new object[] { }) as string;
            return resultText;
        }

        static async Task<DataCollection> BuildAllData(string eosFilename, string psmdFilename, string moonFilename, string ultraMoonFilename)
        {
            var data = new DataCollection();
            var provider = new PhysicalIOProvider();
            using (var eosROM = new NdsRom())
            {
                await eosROM.OpenFile(eosFilename, provider);
                Console.WriteLine("Building EOS data");
                data.EosData = await LoadEosData(eosROM);
            }

            Console.WriteLine("Building PSMD data");
            data.PsmdData = await LoadPsmdData(psmdFilename);

            Console.WriteLine("Building SM data");
            data.SMData = LoadSunMoonData(moonFilename, false);

            Console.WriteLine("Building Ultra SM data");
            data.UltraSMData = LoadSunMoonData(ultraMoonFilename, true);

            data.EosData.ParentCollection = data;
            data.PsmdData.ParentCollection = data;
            data.SMData.ParentCollection = data;
            data.UltraSMData.ParentCollection = data;

            return data;
        }

        static void Main(string[] args)
        {
            var eosPath = args[0];
            var psmdPath = args[1];
            var smPath = args[2];
            var ultraSmPath = args[3];

            if (args.Length < 4)
            {
                Console.WriteLine("Usage: ProjectPokemonPokedex.exe <eosPath> <psmdPath> <smPath> <ultraSmPath> <OutputFilename>");
                return;
            }

            Console.WriteLine("Building data");
            var data = BuildAllData(eosPath, psmdPath, smPath, ultraSmPath).Result;

            Console.WriteLine("Generating EOS records");
            BuildEOS(data.EosData, "eos.ipsdb");

            Console.WriteLine("Generating PSMD records");
            BuildPSMD(data.PsmdData, "psmd.ipsdb");

            Console.WriteLine("Generating SM records");
            BuildSM(data.SMData, "sm.ipsdb", false);

            Console.WriteLine("Generating Ultra SM records");
            BuildSM(data.UltraSMData, "ultrasm.ipsdb", true);
        }
    }
}
