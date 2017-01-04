using UnityEngine;
using System.Collections;

public class MechanicDoor : BaseDoor
{
	private Vector3 _initialPosition;
	private float _translationOffset = 1.05f;
	private Vector3 _lastPosition;
	private bool _wasMoving;
	private bool _movementStarted = false;
	private bool _movementStopped = false;

	public AudioClip StartMovingSound;
	public AudioClip Moving;
	public AudioClip StopMovingSound;
	
	protected new void Start() 
	{
		base.Start ();
		_lastPosition = _initialPosition = transform.position;
	}
	
	public override void Open() 
	{
		var newPosition = _initialPosition + transform.TransformDirection(Vector3.back) * _translationOffset;
		transform.position = Vector3.MoveTowards (transform.position, 
		                                       newPosition, 
		                                       SwitchSpeed * Time.deltaTime);
	}
	
	public override void Close()
	{
		var newPosition = _initialPosition;
		transform.position = Vector3.MoveTowards (transform.position, 
		                                    newPosition, 
		                                    SwitchSpeed * Time.deltaTime);
	}

	protected override void OnSwitch ()
	{
		_movementStarted = false;
		_movementStopped = false;
	}

	protected override void AfterUpdate ()
	{
		CheckMovingState ();
	}

	private void CheckMovingState()
	{
		var offset = transform.position - _lastPosition;
		var moving = offset.magnitude > 0.0001f;
		_lastPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		if (!_wasMoving && moving && !_movementStarted) 
		{
			_movementStarted = true;
			StartMoving();
		}
        else
		if (_wasMoving && !moving && !_movementStopped) 
		{
			_movementStopped = true;
			StopMoving();
		}

		_wasMoving = moving;
	}

	private void StartMoving()
	{
		AudioSource.Stop ();
		AudioSource.PlayOneShot (StartMovingSound);
		Invoke ("PlayMovingSound", StartMovingSound.length/3.0f);
	}

	private void PlayMovingSound () 
	{
		if (!_movementStopped) {
			AudioSource.Stop ();
			AudioSource.clip = Moving;
			AudioSource.loop = true;
			AudioSource.Play ();
		}
	}

	private void StopMoving()
	{
		AudioSource.Stop ();
		AudioSource.PlayOneShot (StopMovingSound);
	}
}