using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		InputManager.instance.PlayerFellOutOfBounds();
	}
}
