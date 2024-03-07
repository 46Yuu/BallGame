using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CageController : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] HUDController hudController;
    enum PlayerGoal { Goal_1,Goal_2};
    [SerializeField] PlayerGoal goal;
    private bool canScore = true;

    private void Awake()
    {
        gameController = GameController.GetInstance();
        hudController = HUDController.GetInstance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && canScore)
        {
            hudController.textGoal.gameObject.SetActive(true);
            ScoreGoal(other.GetComponent<BallController>().latesPlayerHit);
            StartCoroutine(WaitRestart());
        }
    }

    private IEnumerator WaitRestart()
    {
        Time.timeScale = 0.6f;
        GetComponent<ParticleSystem>().Play();
        canScore = false;
        yield return new WaitForSeconds(2);
        GetComponent<ParticleSystem>().Stop();
        Time.timeScale = 1;
        hudController.textGoal.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        GameController.GetInstance().RoundStart();
        canScore = true;
    }

    private void ScoreGoal(GameObject player)
    {

        if (goal == PlayerGoal.Goal_1)
        {
            hudController.textGoal.GetComponent<TMPro.TMP_Text>().SetText(player.GetComponent<PlayerController>().playerName + "scored a point for Team 2");
            gameController.scoreP2++;
            hudController.scoreP2.GetComponent<TMPro.TMP_Text>().SetText(gameController.scoreP2.ToString());
        }
        else
        {
            hudController.textGoal.GetComponent<TMPro.TMP_Text>().SetText(player.GetComponent<PlayerController>().playerName + "scored a point for Team 1");
            gameController.scoreP1++;
            hudController.scoreP1.GetComponent<TMPro.TMP_Text>().SetText(gameController.scoreP1.ToString());
        }
    }
}
