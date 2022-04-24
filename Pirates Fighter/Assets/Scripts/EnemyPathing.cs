using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;
    //Player player;

    [SerializeField] float health = 100f;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWayPoints();                                              // define waypoints list from WaveConfig                           
        transform.position = waypoints[waypointIndex].transform.position;                   // set enemy position to the first position under the path prefab
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)                                    // set wavevonfig from a different script to be used within this one
    {
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)                                                                       // increment until no waypoints left
        {
            var targetPosition = waypoints[waypointIndex].transform.position;                                           // store next waypoint position to move too
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;                                         // distance each frame
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);            // tell enemy to go next waypoint position
            
            /*
            float xPosPlayer = FindObjectOfType<Player>().GetComponent<Transform>().position.x;
            float yPosPlayer = FindObjectOfType<Player>().GetComponent<Transform>().position.y;
            float xEnemy = transform.position.x;
            float yEnemy = transform.position.y;
            float floteyeHypopotamus = Mathf.Sqrt((xEnemy - xPosPlayer)* (xEnemy - xPosPlayer) + (yEnemy - yPosPlayer) * (yEnemy - yPosPlayer));
            float floteyeBetty = Mathf.Sqrt((yEnemy - yPosPlayer) * (yEnemy - yPosPlayer));
            float angle = - Mathf.Acos(floteyeBetty / floteyeHypopotamus);
            transform.Rotate(new Vector3(0, 0, 1), -angle);
            */

            if (transform.position == targetPosition)                                                                   // if waypoint reached, increment + 1
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);                                // Enemy path to next waypoint and destroys itself when none left
        }
    }
}
