[System.Serializable]

public class SaveData
{
    public int level;
    public float[] position = new float[3]; 

    public SaveData(Player player, GameController game){
        level == game.level;
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
