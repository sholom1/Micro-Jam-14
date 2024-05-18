using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {
	[SerializeField] TMP_Text scoreText;

	[SerializeField] private int normalPoints;
	[SerializeField] private int bonusPoints;

	private int score;

	private static ScoreManager instance;
	public static ScoreManager Instance {
		get {
			return instance;
		}
	}

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
	}

	public void AddNormalPoints() {
		score += normalPoints;
		UpdateScore();
	}

	public void AddBonusPoints() {
		score += bonusPoints;
		UpdateScore();
	}

	private void UpdateScore() {
		scoreText.text = "Score: " + score.ToString();
	}
}
