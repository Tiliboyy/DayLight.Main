namespace DayLight.Subclasses.Subclasses.SCPs;

/*
//[CustomRole(RoleTypeId.Tutorial)]
public class Scp056 : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 1;


    public override uint Id { get; set; } = 60;

    protected override bool Hidden { get; set; } = true;
    protected override RoleTypeId? SpawningRole { get; set; } = RoleTypeId.ClassD;

    public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
    public override string Name { get; set; } = "SCP-056";

    public override string Description { get; set; } =
        "Du kannst dich mit .056 als andere Rollen verkleiden";

    public override string CustomInfo { get; set; } = "SCP-056";

    public override bool KeepInventoryOnSpawn { get; set; } = true;
    public override Vector3 Scale { get; set; } = new(1, 1f, 1);

    public override List<CustomAbility> CustomAbilities { get; set; } = new();
    public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
    {
        RoleSpawnPoints = new List<RoleSpawnPoint>()
        {
            new RoleSpawnPoint() { Role = RoleTypeId.ClassD, Chance = 100 }
        },
    };

    
    public override List<string> Inventory { get; set; } = new();
    public Dictionary<Player, RoleTypeId> CurrentRoles = new();

    protected override void SubscribeEvents()
    {
        Exiled.Events.Handlers.Player.Died += OnDied;
        Exiled.Events.Handlers.Player.ChangingRole += OnChangingRoles;
        Exiled.Events.Handlers.Player.Verified += OnVerified;
        base.SubscribeEvents();
    }
    
    public void ChangeApperace(Player player,RoleTypeId roleTypeId)
    {
        CurrentRoles[player] = roleTypeId;
        player.ChangeAppearance(CurrentRoles[player]);
    }
    public void OnVerified(VerifiedEventArgs ev)
    {
        foreach (Player player in TrackedPlayers)
        {
            player.ChangeAppearance(CurrentRoles[player]);
        }
    }
    public void OnChangingRoles(ChangingRoleEventArgs ev)
    {
        foreach (Player player in TrackedPlayers)
        {
            player.ChangeAppearance(CurrentRoles[player]);
        }
    }
    public void OnDied(DiedEventArgs ev)
    {
        foreach (Player player in TrackedPlayers)
        {
            player.ChangeAppearance(CurrentRoles[player]);
        }
    }
    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        
        
        Exiled.Events.Handlers.Player.Died -= OnDied;
        Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRoles;
        Exiled.Events.Handlers.Player.Verified -= OnVerified;

    }
    protected override void RoleAdded(Player player)
    {
        if(CurrentRoles.ContainsKey(player))
        {
            CurrentRoles.Remove(player);
        }
        CurrentRoles.Add(player, RoleTypeId.ClassD);

        base.RoleAdded(player);
    }
}
*/