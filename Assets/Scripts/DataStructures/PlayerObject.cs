public class PlayerObject
{
    private int score;
    private string name;
    private float speed;

    public PlayerObject(string name)
    {
        score = 0;
        this.name = name;
        speed = 1;
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
}
