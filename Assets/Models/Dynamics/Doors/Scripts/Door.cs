using UnityEngine;
using System.Collections;

public class Door : BaseDoor {
    public AudioClip OpenAudio;
    public AudioClip CloseAudio;
    public bool openFromBothSides = true;

    private float _openAngle;
    private Quaternion _initialRotation;

    protected new void Start() 
	{
		base.Start ();
		_initialRotation = transform.rotation;
	}
	
	protected override void OnSwitch() 
	{
		_openAngle = 0;
		PlaySound ();
	}
	
	public override void Open() 
	{
		SetOpenAngle ();
		var eulerRotation = _initialRotation.eulerAngles + new Vector3 (0, _openAngle, 0);
		var newRotation = Quaternion.Euler (eulerRotation);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, 
		                                       newRotation, 
		                                       SwitchSpeed * Time.deltaTime);
	}
	
	public override void Close()
	{
		var eulerRotation = _initialRotation.eulerAngles;
		var newRotation = Quaternion.Euler (eulerRotation);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, 
		                                       newRotation, 
		                                       SwitchSpeed * Time.deltaTime);
	}

	private void SetOpenAngle()
	{
		if (_openAngle == 0) 
		{
			_openAngle = openFromBothSides && IsPlayerOutside () 
				? 90 : -90;
		}
	}

	private void PlaySound() 
	{
		var sound = IsClosed () ? CloseAudio : OpenAudio;
		AudioSource.PlayOneShot (sound);
	}
}