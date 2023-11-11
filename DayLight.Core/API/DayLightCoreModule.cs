using Exiled.Events.Handlers;
using Neuron.Core;
using Neuron.Modules.Configs.Localization;
using Neuron.Modules.Reload;
using System.Reflection;

namespace DayLight.Core.API;


public class DayLightCoreModule<TConfig, TTranslation> : ReloadablePlugin<TConfig, TTranslation>
    where TConfig : IConfig
    where TTranslation : Translations<TTranslation>, new(){
    
    
    
    public override sealed void EnablePlugin()
    {
        

        if (!StaticUtils.GetOrCreate<TConfig>().Enabled)
        {
            return;
        }
        Server.WaitingForPlayers += LateLoad;
        Enabled();
        API.Logger.Info($"Loaded plugin {Assembly.GetAssembly(this.GetType()).GetName().Name}");

    }

    public virtual void LateLoad()
    {
        
    }
    public override void OnReload()
    {
        if (!StaticUtils.GetOrCreate<TConfig>().Enabled)
        {
            Disable();
            return;
        }
        Reloading();
    }
    public virtual void Enabled()
    {
        
    }
    protected virtual void Reloading()
    {
        
    }
}
