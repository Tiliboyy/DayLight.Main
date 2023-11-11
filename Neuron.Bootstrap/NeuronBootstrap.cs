using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Server = Exiled.Events.Handlers.Server;

namespace Neuron.Bootstrap;

public class NeuronBootstrap : Plugin<Configs>
{
    public override string Name { get; } = "Neuron.Bootstrap";
    public override string Author { get; } = "Dimenzio & Tiliboyy";
    public override PluginPriority Priority { get; } = PluginPriority.First;
    public static DateTime StartTime;
    public override void OnEnabled()
    {
        if(!Config.IsEnabled) return;
        StartTime = DateTime.Now;
        BootstrapNeuron();
        var bootTimeSpan = StartTime - DateTime.Now;
        Log.Info($"Neuron startet in  {bootTimeSpan}!");
        
        base.OnEnabled();
    }
    
    internal static void BootstrapNeuron()
    {
        try
        {
            if (StartupArgs.Args.Any(x => x.Equals("--noneuron", StringComparison.OrdinalIgnoreCase))) return;
            Log.Info("Bootstraping Neuron Platform");

            var assemblies = new List<Assembly>();
            var domain = AppDomain.CurrentDomain;
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SCP Secret Laboratory");

            foreach (var file in Directory.GetFiles(Path.Combine(path,"Neuron","Managed"), "*.dll"))
            {
                try
                {
                    var assembly = domain.Load(File.ReadAllBytes(file));
                    assemblies.Add(assembly);
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to load Assembly\n" + file + "\n" + ex);
                }
            }

            domain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                return assemblies.First(x => x.FullName == args.Name);
            };

            var coreAssembly = AppDomain.CurrentDomain.GetAssemblies().First(x => x.GetName().Name == "Neuron.Platform");
            var entryPoint = coreAssembly?.GetType("Neuron.Platform.Platform");
            if (entryPoint == null) throw new Exception("No Valid EntryPoint was found. Please Reinstall the Neuron Platform");
            var method = entryPoint.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);
            if (method == null) throw new Exception("No Valid Main Method was found. Please Reinstall the Neuron Platform");
            method.Invoke(null,Array.Empty<object>());
        }
        catch(Exception ex)
        {
            Log.Error("Loading of Neuron failed:\n" + ex);
        }
    }
}