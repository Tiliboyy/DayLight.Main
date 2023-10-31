using DayLight.Core.API.Subclasses.Features;
using DayLight.Subclasses.Subclasses;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Loader.Features.Configs;
using Neuron.Core;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using Ninject;
using System;
using System.IO;
using YamlDotNet.Serialization;
using Server = Exiled.Events.Handlers.Server;


namespace DayLight.Subclasses;
[Plugin(Name = "DayLight.Subclasses", Author = "Tiliboyy")]
public class SubclassesPlugin : ReloadablePlugin<SubclassConfig, SubclassTranslation>
{
    public static SubclassesPlugin Instance;
    public static RolesConfigs RolesConfig = new RolesConfigs();
    [Inject]
    public NeuronBase Base {get;set;}

    
    public override void EnablePlugin()
    {
        try
        {
            Instance = this;

//if (!Directory.Exists(Base.RelativePath("Subclasses")))
           // {
           //     Directory.CreateDirectory(Base.RelativePath("Subclasses"));
            //}


            
            //File.WriteAllText(Path.Combine(Base.RelativePath("Subclasses"), "global.yml"), new Serializer().Serialize(RolesConfig));

            //RolesConfig = new Deserializer().Deserialize<RolesConfigs>(File.ReadAllText(Path.Combine(Base.RelativePath("Subclasses"), "global.yml")));
            Subclass.RegisterSubclasses(overrideClass: RolesConfig);

        }
        catch (Exception error)
        {
            Logger.Error(error);
        }
    }

    public override void Disable()
    {
        Exiled.CustomRoles.API.Features.CustomRole.UnregisterRoles();
        Instance = null;
    }
}