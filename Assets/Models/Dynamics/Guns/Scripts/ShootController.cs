using System.Linq;
using UnityEngine;

public class ShootController : MonoBehaviour 
{
	public AudioSource ShootSound;
	public float FireRate;
    public GameObject DefaultShotImpact;
    [Range(0, 100)] public float Accuracy;

    private BurstAnimation _burstAnimation;
    private float _fireTimer;
	private Animator _animator;    

	void Awake()
	{
		_animator = GetComponent<Animator> ();
		_burstAnimation = GetComponentInChildren<BurstAnimation>();
	}

	void LateUpdate () 
	{
		_fireTimer += Time.deltaTime;

		if (CanShoot ()) 
		{
			_fireTimer = 0.0f;
			_animator.Play("Shooting");
		}
	}

	private bool CanShoot() 
	{
		return (Input.GetButton ("Fire1") || Input.GetAxis("Fire1") < 0) && 
            _fireTimer >= FireRate;
	}

	public void Shoot()
	{
		ShootSound.Play();
		_burstAnimation.Activate();
		ProjectBullet ();
	}

	private void ProjectBullet()
	{
	    var i = 0;
	    var bulletStopped = false;
	    var ray = GetShootRay();
		var hits = Physics.SphereCastAll(ray, 0.000001f, 1000.0f, LayerMask.GetMask("Shootable"))
            .OrderBy(hit => hit.distance)
            .ToArray();

	    while (!bulletStopped && i < hits.Length)
	    {
	        var hit = hits[i];
            if (hit.collider.gameObject)
            {
                var shootable = hit.collider.gameObject.GetComponent<Shootable>();
                var shot = shootable != null ? shootable.ShotImpact : DefaultShotImpact;
                var instance = (GameObject)Instantiate(shot, hit.point, Quaternion.LookRotation(hit.normal));
                instance.transform.SetParent(hit.collider.gameObject.transform);
                bulletStopped = shootable == null || !shootable.GoThrough;
            }
	        i++;
	    }
	}

    private Ray GetShootRay()
    {
        var errorPercent = 1 - Accuracy / 100;
        var screenHalfWidth = Screen.width / 2.0f;
        var screenHalfHeight = Screen.height / 2.0f;
        var minAxis = Mathf.Min(screenHalfWidth, screenHalfHeight);
        var screenCenter = new Vector3(
            screenHalfWidth + Random.Range(-minAxis, minAxis) * errorPercent,
            screenHalfHeight + Random.Range(-minAxis, minAxis) * errorPercent, 
            Camera.main.farClipPlane);
        return Camera.main.ScreenPointToRay(screenCenter);
    }
}