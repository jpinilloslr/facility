using UnityEngine;

public class ShotImpact : MonoBehaviour
{
    public AudioClip[] HitSounds;
    public float LifeTime = 10;
    private float timer;

    void Awake()
    {
        timer = 0.0f;
        var soundClip = HitSounds[Random.Range(0, HitSounds.Length-1)];
        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(soundClip);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > LifeTime && !IsOnCamera())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOnCamera()
    {
        var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return screenPosition.x > 0 && 
            screenPosition.x < Screen.width &&
            screenPosition.y > 0 &&
            screenPosition.y < Screen.height;
    }
}