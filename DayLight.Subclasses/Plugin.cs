using DayLight.Core.API;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.Subclasses.Features;
using DayLight.Subclasses.Subclasses;
using Neuron.Core;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using Ninject;
using System;


namespace DayLight.Subclasses;
[Plugin(Name = "DayLight.Subclasses", Author = "Tiliboyy")]
public class SubclassesPlugin : DayLightCorePlugin<SubclassConfig, SubclassTranslation>
{
    public static SubclassesPlugin Instance;
    public static RolesConfigs RolesConfig = new();
    [Inject]
    public NeuronBase Base {get;set;}


    protected override void Enabled()
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