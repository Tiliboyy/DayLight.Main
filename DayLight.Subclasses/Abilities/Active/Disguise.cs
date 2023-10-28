namespace DayLight.Subclasses.Abilities.Active;
/*
public class Disguise : ActiveAbility
{
    public override string Name { get; set; } = "Disguise";
    public override string Description { get; set; } = "Du kannst mit .056 deine gestalt ändern.";

    
    public void UseDisguiseAbility(Player player, RoleTypeId roleTypeId)
    {
        UseAbility(player);
        if(player.GetSubclass() == null && (Scp056)player.GetSubclass() != null)
            return;
        var scp056 = (Scp056)player.GetSubclass();
        scp056.ChangeApperace(player, roleTypeId);
    }
    protected override void ShowMessage(Player player)
    {
    }

    protected override void AbilityRemoved(Player player)
    {
        base.AbilityUsed(player);
    }

    public override float Duration { get; set; } = 0;
    public override float Cooldown { get; set; } = 60;
}
*/