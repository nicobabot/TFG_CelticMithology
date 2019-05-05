public class Banshee_Blackboard : Blackboard
{
    public ParameterInt life;
    public ParameterInt total_life;
    public ParameterBool is_enemy_hit;
    public ParameterBool want_to_hit;
    public ParameterBool is_enemy_stunned;
    public ParameterGameObject player;
    public ParameterEnumDirection direction;
    public ParameterBool playerIsInsideRoom;
    public ParameterSpriteRend mySpriteRend;
    public ParameterAnimator myAnimator;

    // Use this for initialization
    override public void Start()
    {
        base.Start();

        list.Add(life);
        list.Add(total_life);
        list.Add(is_enemy_hit);
        list.Add(want_to_hit);
        list.Add(is_enemy_stunned);
        if (ProceduralDungeonGenerator.mapGenerator != null)
            player.myValue = ProceduralDungeonGenerator.mapGenerator.Player;
        list.Add(player);
        list.Add(direction);
        list.Add(playerIsInsideRoom);
        list.Add(mySpriteRend);
        list.Add(myAnimator);
    }
}