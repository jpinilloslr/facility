using System;
using UnityEngine;

[Serializable]
public class GunMovementCurve
{
	public float HorizontalRange = 0.01f;
	public float VerticalRange = 0.01f;
	public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f),
														new Keyframe(1f, 0f), new Keyframe(1.5f, -1f),
														new Keyframe(2f, 0f));
	public float VerticaltoHorizontalRatio = 1f;

	private float cyclePositionX;
	private float cyclePositionY;
	private float baseInterval;
	private Vector3 originalPosition;
	private float time;

	public void Setup(Transform gun, float bobBaseInterval)
	{
		baseInterval = bobBaseInterval;
		originalPosition = gun.transform.localPosition;
		time = curve[curve.length - 1].time;
	}

	public Vector3 Move(float speed)
	{
		float xPos = originalPosition.x + (curve.Evaluate(cyclePositionX)*HorizontalRange);
		float yPos = originalPosition.y + (curve.Evaluate(cyclePositionY)*VerticalRange);

		cyclePositionX += (speed*Time.deltaTime)/baseInterval;
		cyclePositionY += ((speed*Time.deltaTime)/baseInterval)*VerticaltoHorizontalRatio;

		if (cyclePositionX > time)
		{
			cyclePositionX = cyclePositionX - time;
		}
		if (cyclePositionY > time)
		{
			cyclePositionY = cyclePositionY - time;
		}

		return new Vector3(xPos, yPos, 0f);
	}
}