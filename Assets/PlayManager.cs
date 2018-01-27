using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private GameObject _ball;
    [SerializeField] private GameObject _player;
    private Manager _manager;
    
    private void Start()
    {
        _manager = FindObjectOfType<Manager>();
        
        SpawnBall();
    }
    
    private void SpawnBall()
    {
        var randomPlayer = _manager.PlayerObjects[(int) Mathf.Round(Random.Range(0, 1))].Player;
        
        var ball = Instantiate(_ball);
        ball.transform.position = randomPlayer.transform.position + randomPlayer.transform.forward * 2;
        _manager.Ball = ball;
    }
}
