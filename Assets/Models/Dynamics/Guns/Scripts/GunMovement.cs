using UnityEngine;

public class GunMovement : MonoBehaviour 
{
	public float StepInterval = 2.0f;
	public GunMovementCurve GunMovementCurve = new GunMovementCurve();

	private Quaternion _originalRotation;
	private Vector3 _originalPosition;
	private Vector3 _rotationOffset;
	private Vector3 _putDownRotation;
    private Vector3 _aimPosition = new Vector3(-0.005f, -0.165f, -0.02f);

    public void Start()
	{
		_rotationOffset = Vector3.zero;
		_originalRotation = transform.localRotation;
	    _originalPosition = transform.localPosition;
		GunMovementCurve.Setup(transform, StepInterval);
	}

	public void Update() 
	{
        Aim();
		TurnLeftOrRightMovement ();
		WalkAndBreathMovement ();
		PutDownOnObstacle ();
	}

	private void TurnLeftOrRightMovement()
	{
		_rotationOffset.y = Mathf.Lerp(_rotationOffset.y, Input.GetAxis ("Mouse X")*5.0f, Time.deltaTime*3.0f);
		transform.localRotation = _originalRotation;
		transform.Rotate (_rotationOffset);
	}

	private void WalkAndBreathMovement()
	{
	    if (IsNotAiming())
	    {
            transform.localPosition = GunMovementCurve.Move(GetGunMovementSpeed());
        }
	}

	private float GetGunMovementSpeed()
	{
		return Input.GetAxis("Vertical") == 0.0f ? 0.3f : 4.0f;
	}
	
	private void PutDownOnObstacle() 
	{
		RaycastHit hit;
		var targetRotation = Vector3.zero;
		var minObstacleDistanceToDraw = 0.6f;
		var ray = new Ray (transform.position, transform.TransformVector (Vector3.forward));

		if (Physics.SphereCast (ray, 0.1f, out hit, minObstacleDistanceToDraw) && 
		    hit.collider.gameObject.tag != "Player") 
		{
			var drawVelocity = 1.0f - (hit.distance / minObstacleDistanceToDraw);
			targetRotation = new Vector3(60.0f, 0.0f, 120.0f) * drawVelocity;
		}

		_putDownRotation = Vector3.Lerp(_putDownRotation, targetRotation, 10.0f * Time.deltaTime);
		transform.Rotate(_putDownRotation);
	}

    private void Aim()
    {
        var target = Input.GetButton("Aim")
            ? _aimPosition
            : _originalPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, 10.0f*Time.deltaTime);
    }

    private bool IsNotAiming()
    {
        return Vector3.Distance(_originalPosition, transform.localPosition) <
            (GunMovementCurve.VerticalRange + GunMovementCurve.HorizontalRange);
    }
}