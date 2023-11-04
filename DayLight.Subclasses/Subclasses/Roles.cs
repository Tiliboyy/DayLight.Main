using DayLight.Subclasses.Subclasses.Human.Chaos;
using DayLight.Subclasses.Subclasses.Human.ClassD;
using DayLight.Subclasses.Subclasses.Human.Guards;
using DayLight.Subclasses.Subclasses.Human.MTF;
using DayLight.Subclasses.Subclasses.Human.Scientist;
using DayLight.Subclasses.Subclasses.SCPs.SCP049_2;
using DayLight.Subclasses.Subclasses.SCPs.SCP173;

namespace DayLight.Subclasses.Subclasses;

public class RolesConfigs
{
    
    //Chaos
    public Kamikaze Kamikaze { get; set; } = new();

    //ClassD
    public Ausbruchsexperte Ausbruchsexperte { get; set; } = new();
    public Drogenschmuggler Drogenschmuggler { get; set; } = new();
    public Hausmeister Hausmeister { get; set; } = new();
    public Speedster Speedster { get; set; } = new();
    public StarkeClassD StarkeClassD { get; set; } = new();
    public Vampir Vampir { get; set; } = new();
    
    //Guard
    public ContainmentSpecialist ContainmentSpecialist { get; set; } = new();
    public LightGuard LightGuard { get; set; } = new();
    public Techniker Techniker { get; set; } = new();
    public Hacker Hacker { get; set; } = new();
    public WaffenExperte WaffenExperte { get; set; } = new();
    
    //Scientist
    public Berserker Berserker { get; set; } = new();
    public GuterLäufer GuterLäufer { get; set; } = new();
    public HeadScientist HeadScientist { get; set; } = new();
    public Trickster Trickster { get; set; } = new();

    //049-2
    public HungrigerZombie HungrigerZombie { get; set; } = new();
    public KamikazeZombie KamikazeZombie { get; set; } = new();
    //173
    public HolyPeanut HolyPeanut { get; set; } = new();
    
}