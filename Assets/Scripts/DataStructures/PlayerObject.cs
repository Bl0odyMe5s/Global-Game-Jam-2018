using UnityEngine;

public class PlayerObject
{
    private int score;
    private string name;
    private float speed;
    private GameObject _player;

    public PlayerObject(string name, GameObject player)
    {
        score = 0;
        this.name = name;
        speed = 1;
        Player = player;
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

    public GameObject Player
    {
        get { return _player; }
        set { _player = value; }
    }
}
