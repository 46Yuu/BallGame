using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageController : MonoBehaviour
{
    GameController gameController;
    HUDController hudController;
    enum PlayerGoal { Goal_1,Goal_2};
    [SerializeField] PlayerGoal goal;

    private void Awake()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        hudController = gameController.hudController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if(goal == PlayerGoal.Goal_1)
            {
                hudController.textGoal.SetText($"{other.GetComponent<BallController>().latesPlayerHit.GetComponent<PlayerController>().playerName} scored a point for Player 2");
                gameController.scoreP2++;
            }
            else
            {
                hudController.textGoal.SetText($"{other.GetComponent<BallController>().latesPlayerHit.GetComponent<PlayerController>().playerName} scored a point for Player 1");
                gameController.scoreP1++;
            }
            hudController.textGoal.gameObject.SetActive(true);
            StartCoroutine(WaitRestart());
        }
    }

    private IEnumerator WaitRestart()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(3);
        Time.timeScale = 1;
        hudController.textGoal.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        GameController.GetInstance().RoundStart();
    }
}
