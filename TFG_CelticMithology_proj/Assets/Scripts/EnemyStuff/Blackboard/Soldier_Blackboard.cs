public class Soldier_Blackboard : Blackboard
{
    public ParameterInt life;
    public ParameterInt total_life;
    public ParameterBool is_enemy_hit;
    public ParameterGameObject player;
    public ParameterEnumDirection direction;
    public ParameterBool playerIsInsideRoom;

    // Use this for initialization
    override public void Start()
    {
        base.Start();

        list.Add(life);
        list.Add(total_life);
        list.Add(is_enemy_hit);
        player.myValue = ProceduralDungeonGenerator.mapGenerator.Player;
        list.Add(player);
        list.Add(direction);
        playerIsInsideRoom.myValue = false;
        list.Add(playerIsInsideRoom);
    }
}