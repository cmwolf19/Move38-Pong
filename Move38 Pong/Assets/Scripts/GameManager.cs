/******************************************
 * File Name: GameManager.cs
 * Author: Connor Wolf
 * Date: 5/23/2020
 * Desc:
 *      Controls the functionality of the game including scoring and resetting.
 *      Singleton that is always assigned at beginning of the game. 
 ******************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
        /// <summary>
        /// Singleton reference variable.
        /// </summary>
        public static GameManager instance;

        private AudioSource audSource;

        /// <summary>
        /// Reference to the ball in play. NOT THE PREFAB.
        /// </summary>
        private BallBehaviour currentBall;
        private int leftScore;
        private int rightScore;
        public int targetScore;

        /// <summary>
        /// Prefab of game ball. Assigned in inspector.
        /// </summary>
        [Tooltip("Pong Ball Prefab")]
        public GameObject pongBall;

        /// <summary>
        /// Delay from spawning ball to having ball enter play. Assigned in inspector.
        /// </summary>
        [Tooltip("Delay between ball spawn to launch.")]
        public float launchDelay;

        public Text leftScoreText;
        public Text rightScoreText;
        public Text winText;
        
        public GameObject leftTrophy;
        public GameObject rightTrophy;
    #endregion
    
    #region Unity Callbacks
        //Setting instance variables
        private void Awake()
        {
            instance = this;
            audSource = GetComponent<AudioSource>();

            currentBall = Instantiate(pongBall, Vector3.zero, Quaternion.identity).GetComponent<BallBehaviour>();
            currentBall.gameObject.SetActive(false);
        }
    
        //Starting/Resetting the game.
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !currentBall.gameObject.activeSelf)
            {
                StartCoroutine(SpawnBall());
                winText.text = "";
                leftScore = 0;
                rightScore = 0;
                leftScoreText.text = "0";
                rightScoreText.text = "0";
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
            Application.Quit();
            }
        }
    #endregion

    #region Functions
        /// <summary>
        /// Handles giving points to the correct person. Pass in "LEFT" or "RIGHT".
        /// </summary>
        public void GivePoint(string winner)
        {
            audSource.Play();

            if (winner.Equals("LEFT")) leftScore++;
            else if (winner.Equals("RIGHT")) rightScore++;
            else Debug.LogWarning("No participant called \"" + winner + "\" exists.");

            leftScoreText.text = leftScore.ToString();
            rightScoreText.text = rightScore.ToString();

            if (leftScore >= targetScore || rightScore >= targetScore)
                EndGame(winner);
            else
                StartCoroutine(SpawnBall());
        }

        /// <summary>
        /// Sets the win text and unloads the ball. Also activates the trophy for the winner until the next winner is crowned.
        /// </summary>
        public void EndGame(string winner)
        {
            currentBall.gameObject.SetActive(false);
            winText.text = winner + " WINS\nSPACE TO PLAY AGAIN";
            if (winner.Equals("LEFT"))
            {
                leftTrophy.SetActive(true);
                rightTrophy.SetActive(false);
            } else
            {
                leftTrophy.SetActive(false);
                rightTrophy.SetActive(true);
            }
        }

        /// <summary>
        /// Coroutine that spawns the ball, waits for the launch delay, and then calls the launch.
        /// </summary>
        /// <returns></returns>
        public IEnumerator SpawnBall()
        {
            currentBall.gameObject.SetActive(true);
            currentBall.Respawn();
            yield return new WaitForSeconds(launchDelay);
            currentBall.Launch();
        }

        /// <summary>
        /// Checks the score to help see who is in the lead.
        /// </summary>
        /// <returns></returns>
        public int CheckScore()
        {
            if (leftScore > rightScore) return -1;
            if (rightScore > leftScore) return 1;
            return (int) Mathf.Sign(Random.Range(-1, 1));
        }
    #endregion
}
