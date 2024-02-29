using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageController : MonoBehaviour
{
    enum PlayerGoal { Goal_1,Goal_2};
    [SerializeField] PlayerGoal goal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if(goal == PlayerGoal.Goal_1)
            {
                Debug.Log($"{other.GetComponent<BallController>().latesPlayerHit.GetComponent<PlayerController>().playerName} scored a point for Player 2");
            }
            else
            {
                Debug.Log($"{other.GetComponent<BallController>().latesPlayerHit.GetComponent<PlayerController>().playerName} scored a point for Player 1");
            }

            StartCoroutine(WaitRestart());
        }
    }

    private IEnumerator WaitRestart()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(3);
        Time.timeScale = 1;
        yield return new WaitForSeconds(1);
        GameController.GetInstance().RoundStart();
    }
}
