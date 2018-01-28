using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour {

    [SerializeField] private Text timer, scoreText1, scoreText2;
    [SerializeField] private Ball ball;
    [SerializeField] private Image timerPanel;

    public List<Color> playerColors;

    private float timerValue, score1, score2;

	// Use this for initialization
	void Start () {

        timerValue = Manager.manager.SecondsUntilRoundStop;
        timer.text = timerValue.ToString("0:00");
        score1 = Manager.manager.PlayerScores[0];
        score2 = Manager.manager.PlayerScores[1];

        scoreText1.text = score1.ToString();
        scoreText2.text = score2.ToString();
    }

    void Update()
    {
        if (Manager.manager.State == GameStates.Playing)
        {
            timerValue -= Time.deltaTime;
            timer.text = timerValue.ToString("0:00");
        }
    }

    public void AddScore(int shooter)
    {
        switch (shooter)
        {
            case 0:
                score1 = Manager.manager.PlayerScores[0];
                scoreText1.text = score1.ToString();
                break;

            case 1:
                score2 = Manager.manager.PlayerScores[1];
                scoreText2.text = score2.ToString();
                break;
        }
    }

    public float TimerValue
    {
        get { return timerValue; }
        set { timerValue = value; }
    }

    public Image TimerPanel
    {
        get { return timerPanel; }
        set { timerPanel = value; }
    }

    public List<Color> PlayerColors
    {
        get { return playerColors; }
        set { playerColors = value; }
    }
}
