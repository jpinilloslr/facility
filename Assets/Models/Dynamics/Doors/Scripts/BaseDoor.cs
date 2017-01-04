using UnityEngine;
using System.Collections;

public abstract class BaseDoor : MonoBehaviour
{
    private const float MinInteractionDistance = 1.5f;

    private bool _closed = true;
    private float _switchTimer;
    private float _autoCloseTimer;
    private Plane _plane;
	private Collider _modelCollider;
	private Vector3 _normal;
	
	protected AudioSource AudioSource;
	
	public float SwitchMinTime = 0.3f;
	public float AutoCloseTime = 25.0f;
	public float SwitchSpeed = 300.0f;
	public readonly bool active = true;
	
	protected void Start() 
	{
		var normal = transform.TransformDirection (Vector3.left);
		_plane = new Plane (normal, transform.position);
		_modelCollider = GetComponentInChildren<Collider> ();
		AudioSource = GetComponentInChildren<AudioSource> ();
	}
	
	void Update () 
	{
		if (!active)
			return;

		_switchTimer += Time.deltaTime;
		_autoCloseTimer += Time.deltaTime;
		
		if (CanSwitch()) 
		{
			Switch ();
		}
		UpdateState();
		AutoClose();
		AfterUpdate ();
	}

	protected virtual void AfterUpdate()
	{
	}

	bool CanSwitch() 
	{
		return Input.GetButton ("Interact") && 
			IsPlayerInFrontOfTheDoor () && 
				_switchTimer >= SwitchMinTime;
	}
	
	protected void Switch() 
	{
		_switchTimer = 0;
		_autoCloseTimer = 0;
		_closed = !_closed;
		OnSwitch ();
	}

	protected virtual void OnSwitch ()
	{
	}
	
	void UpdateState() 
	{
		if (_closed) 
			Close ();
		else 
			Open ();
	}
		
	void AutoClose()
	{
		if (!_closed && _autoCloseTimer >= AutoCloseTime) 
		{
			Switch();
		}
	}
	
	bool IsPlayerInFrontOfTheDoor()
	{
		var ray = GetInFrontPlayerRay ();
		RaycastHit raycastHit;
		var contact = Physics.Raycast (ray, out raycastHit, MinInteractionDistance);
		return contact && raycastHit.collider.Equals(_modelCollider);
	}
	
	protected bool IsPlayerOutside() 
	{
		return _plane.GetSide (Camera.main.transform.position);
	}
	
	private Ray GetInFrontPlayerRay() 
	{
		return Camera.main.ScreenPointToRay(
			new Vector3(
			Camera.main.pixelWidth/2, 
			Camera.main.pixelHeight/2, 
			MinInteractionDistance));
	}

	public abstract void Open();
	public abstract void Close ();

	public bool IsClosed()
	{
		return _closed;
	}
}