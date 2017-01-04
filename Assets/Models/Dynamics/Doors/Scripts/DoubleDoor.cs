using UnityEngine;
using System.Collections;

public class DoubleDoor : Door {
	public DoubleDoor pair;

	protected override void OnSwitch()
	{
		base.OnSwitch ();
		if ((pair.IsClosed() && !IsClosed()) ||
		    (!pair.IsClosed() && IsClosed()))
		{
			pair.Switch ();
		}
	}
}