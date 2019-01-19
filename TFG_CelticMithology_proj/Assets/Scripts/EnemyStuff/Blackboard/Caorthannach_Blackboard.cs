public class Caorthannach_Blackboard : Blackboard {

    public ParameterInt life;
    public ParameterBool is_enemy_hit;
    public ParameterGameObject player;
    public ParameterEnumDirection direction;

    // Use this for initialization
    override public void Start()
    {
        base.Start();

        list.Add(life);
        list.Add(is_enemy_hit);
        list.Add(player);
        list.Add(direction);
    }
}
