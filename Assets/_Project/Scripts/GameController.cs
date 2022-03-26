using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace spintap
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject scoreDisplay; // used to disable score display
        [SerializeField] private GameObject endDisplay;
        [SerializeField] private GameObject coin;
        [SerializeField] private GameObject pointerPivot;

        private float previousAngle = 90f;

        public TMP_Text scoreText;
        public TMP_Text finalScore;
        private int score;
        
        void Awake()
    	{
            spawnCoin();

            scoreDisplay.SetActive(true);
		    score = 0;
    	}

        #region SCORE/GAME CONTROLLER
        public void updateScore()
        {
            score += 10;
            scoreText.SetText(score.ToString());
        }

        public void endGame()
        {
            Time.timeScale = 0f;
            finalScore.SetText(score.ToString());

            scoreDisplay.SetActive(false);
            endDisplay.SetActive(true);
        }

        public void restartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        }
        #endregion

        #region COIN SPAWNING
        public void spawnCoin()
        {
            float targetAngle = chooseAngle(previousAngle) * Mathf.Deg2Rad;

            float xAngle = 3f * Mathf.Sin(targetAngle);
            float yAngle = 3f * Mathf.Cos(targetAngle);

            GameObject temporaryCoin = Instantiate(coin, new Vector3(xAngle, yAngle, 0f), Quaternion.identity) as GameObject;
        }

        private float chooseAngle(float angle)
        {
            float min = angle;
            float max = angle + 270f;

            float targetAngle = Random.Range(min, max) % 360f;

            if(targetAngle >= 0f && targetAngle <= 90f)
            {
                previousAngle = 90f;
            }
            else if(targetAngle > 90f && targetAngle <= 180f)
            {
                previousAngle = 180f;
            }
            else if(targetAngle > 180f && targetAngle <= 270f)
            {
                previousAngle = 270f;
            }
            else if(targetAngle > 270f && targetAngle <= 360f)
            {
                previousAngle = 360f;
            }

            return targetAngle;
        }
        #endregion
    }
}
