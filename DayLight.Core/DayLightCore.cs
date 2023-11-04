using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using DayLight.Core.API.Database;
using DayLight.Core.API.Events.EventArgs;
using DayLight.Core.API.Subclasses.EventHandlers;
using Exiled.Events.Handlers;
using Neuron.Core;
using Neuron.Core.Events;
using Neuron.Core.Logging;
using Neuron.Core.Meta;
using Neuron.Core.Modules;
using Neuron.Core.Plugins;
using Neuron.Modules.Patcher;
using Neuron.Modules.Reload;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using EventHandler = DayLight.Core.EventHandlers.EventHandler;
using PluginManager = Neuron.Core.Plugins.PluginManager;

namespace DayLight.Core
{
    [Module(Name = "DayLight.Core", Author = "Tiliboyy", Dependencies = new[] { typeof(PatcherModule) })]
    public class DayLightCore : ReloadableModule<Config, Translation>
    {
        public static ILogger NeuronLogger;
        public static DayLightCore Instance;
        
        [Inject]
        public NeuronBase Base {get;set;}

        public override void EnableModule()
        {
            NeuronLogger = Logger;
            if(!Config.Enabled) return;
            Player.Verified += EventHandler.OnVerified; 
            Server.RespawningTeam += SubclassEventHandlers.OnRespawningTeam;
            Server.RoundEnded += SubclassEventHandlers.OnRoundEnd;
            foreach (var commandBinding in ModuleCommandBindingQueue)
            {
                CustomCommand.RegisterCommand(commandBinding.Type);
            }
            ModuleCommandBindingQueue.Clear();
            
            Instance = this;
        }
        public override void OnReload()
        {
            if(!Config.Enabled) return;
        }
        public override void Disable()
        {
            if(!Config.Enabled) return;
            Instance = null;
            Player.Verified -= EventHandler.OnVerified;

            base.Disable();
        }

        public override void LateEnable()
        {
            DayLightDatabase.CreateCollection();
        }
        
        public override void Load(IKernel kernel)
        {
            kernel.Get<MetaManager>().MetaGenerateBindings.Subscribe(GenerateCommandBinding);
            kernel.Get<PluginManager>().PluginLoadLate.Subscribe(OnPluginLoadLate);
            kernel.Get<ModuleManager>().ModuleLoadLate.Subscribe(OnModuleLoadLate);
        }
        private void GenerateCommandBinding(MetaGenerateBindingsEvent args)
        {
            if(!args.MetaType.TryGetAttribute<AutomaticAttribute>(out _)) return;
            if(!args.MetaType.TryGetAttribute<CommandAttribute>(out _)) return;
            if(!args.MetaType.Is<CustomCommand>() && !args.MetaType.Is<ParentCommand>()) return;
            Core.Logger.Debug(args.MetaType.Type.FullName);
            args.Outputs.Add(new CommandBinding()
            {
                Type = args.MetaType.Type
            });
        }
        private void OnPluginLoadLate(PluginLoadEvent args)
        {
            args.Context.MetaBindings.OfType<CommandBinding>().ToList()
                .ForEach(x => CustomCommand.RegisterCommand(x.Type));
        }

        readonly internal Queue<CommandBinding> ModuleCommandBindingQueue = new();

        private void OnModuleLoadLate(ModuleLoadEvent args)
        {
            args.Context.MetaBindings.OfType<CommandBinding>().ToList()
                .ForEach(x => ModuleCommandBindingQueue.Enqueue(x));
        }
    
        public class CommandBinding : IMetaBinding
        {
            public Type Type { get; set; }

            public IEnumerable<Type> PromisedServices => new Type[] { };
        }
    }
}
