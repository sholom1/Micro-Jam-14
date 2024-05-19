using System.Linq;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
    [SerializeField] private AudioClip[] normalHitAudioClips;
    [SerializeField] private AudioClip bonusHitAudioClip;
    [SerializeField] private AudioSource audioSource;

    public void PlayNormalHitSFX() {
        int randomIndex = Random.Range(0, normalHitAudioClips.Length);
		AudioClip randomClip = normalHitAudioClips[randomIndex];
        audioSource.clip = randomClip;
        audioSource.Play();
	}

    public void PlayBonusHitSFX() {
		audioSource.clip = bonusHitAudioClip;
		audioSource.Play();
	}
}
