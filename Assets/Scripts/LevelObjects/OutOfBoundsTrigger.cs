using UnityEngine;

public class OutOfBoundsTrigger : LevelTrigger
{
	public void OnTriggerEnter(Collider other)
	{
		InputManager.instance.PlayerFellOutOfBounds();
	}
}
