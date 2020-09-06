using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineController : MonoBehaviour
{
	public Transform associatedTargets;
	public float timeToBeatInSeconds;
	public float standstillTimeRequiredInSeconds;
	[Header("Animation")]
	public Transform MovingVisual;
	public Vector3 DeactivatedPosition;
	public float DeactivatedScale;
	public float DeactivatedRotationSpeed;
	
	public Vector3 ActivatedPosition;
	public float ActivateScale;
	public float ActivatedRotationSpeed;
	
	public Gradient ActivationColors;
	public ParticleSystem.MinMaxCurve activatingCurve;
	public ParticleSystem.MinMaxCurve deactivatingCurve;

	private const float STANDSTILL_SPEED_THRESHOLD = 0.1f;
	
	private Vector3 relativeDeactivatedPosition;
	private Vector3 relativeActivatedPosition;
	private Vector3 deactivatedLocalScale;
	private Vector3 activatedLocalScale;
	private bool activated;
	private Coroutine checkingForStandstill;
	private float toggleProgress;
	private SpriteRenderer spriteRenderer;
	private ConstantRotationController constantRotationController;
	private TimeManager TimeManager;
	private TargetManager TargetManager;
	private bool beatenShrine;

	public void Start()
	{
		relativeActivatedPosition = ActivatedPosition + transform.position;
		relativeDeactivatedPosition = DeactivatedPosition + transform.position;
		activatedLocalScale = new Vector3(ActivateScale, ActivateScale, 0);
		deactivatedLocalScale = new Vector3(DeactivatedScale, DeactivatedScale, 0);
		spriteRenderer = MovingVisual.GetComponent<SpriteRenderer>();
		constantRotationController = MovingVisual.GetComponent<ConstantRotationController>();
		TimeManager = FindObjectOfType<TimeManager>();
		TargetManager = FindObjectOfType<TargetManager>();
		if(associatedTargets != null)
		{
			//targets associated with a shrine start out disabled
			foreach (Transform target in associatedTargets)
			{
				target.gameObject.SetActive(false);
			}
		}
	}

	public void Update () {
		UpdateVisualProgress();
	}

	private void UpdateVisualProgress(float forceProgress = -1)
	{
		if (checkingForStandstill == null && forceProgress == -1)
		{
			return;
		}

		var curvedProgress = activated ? 
			deactivatingCurve.Evaluate(toggleProgress): 
			activatingCurve.Evaluate(toggleProgress);

		if (forceProgress != -1)
		{
			curvedProgress = Mathf.Clamp01(forceProgress);
		}
		
		MovingVisual.transform.position = activated ? 
			Vector3.Lerp(relativeActivatedPosition, relativeDeactivatedPosition, curvedProgress): 
			Vector3.Lerp(relativeDeactivatedPosition, relativeActivatedPosition, curvedProgress);
		
		MovingVisual.transform.localScale = activated ? 
			Vector3.Lerp(activatedLocalScale, deactivatedLocalScale, curvedProgress): 
			Vector3.Lerp(deactivatedLocalScale, activatedLocalScale, curvedProgress);

		spriteRenderer.color = ActivationColors.Evaluate(activated ? 1 - curvedProgress : curvedProgress);
		
		constantRotationController.speedMultiplier = activated ? 
			Mathf.Lerp(ActivatedRotationSpeed, DeactivatedRotationSpeed, curvedProgress): 
			Mathf.Lerp(DeactivatedRotationSpeed, ActivatedRotationSpeed, curvedProgress);
	}
	
	public void OnTriggerEnter(Collider other)
	{
		if (beatenShrine)
		{
			return;
		}
		if (other.GetComponent<PlayerController>() != null)
		{
			checkingForStandstill = StartCoroutine(CheckForStandstill(other.GetComponent<Rigidbody>()));
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (checkingForStandstill == null)
		{
			return;
		}
		StopCoroutine(checkingForStandstill);
		checkingForStandstill = null;
	}

	private IEnumerator CheckForStandstill(Rigidbody playerRigidbody)
	{
		var timePassedWhileStandstill = 0f;
		while (timePassedWhileStandstill < standstillTimeRequiredInSeconds)
		{
			yield return new WaitForFixedUpdate();
			if (playerRigidbody.velocity.magnitude < STANDSTILL_SPEED_THRESHOLD)
			{
				timePassedWhileStandstill += Time.fixedDeltaTime;
			}
			else
			{
				timePassedWhileStandstill = 0;
			}

			toggleProgress = timePassedWhileStandstill / standstillTimeRequiredInSeconds;
		}

		toggleProgress = 1;
		UpdateVisualProgress();
		ToggleShrineActivated();
		checkingForStandstill = null;
		toggleProgress = 0;
	}

	public void ResetPlayer()
	{
		if (!activated || beatenShrine)
		{
			TimeManager.Hide();
			return;
		}
		TimeManager.ResetCurrentTime();
		TargetManager.ResetAllTargets();
	}

	private void ToggleShrineActivated()
	{
		if (beatenShrine)
		{
			return;
		}
		activated = !activated;
		Debug.Log("Shrine " + (activated ? "Activated!" : "Deactivated."));
		if (activated)
		{
			var currentTargetParent = TargetManager.instance.targets;
			if (currentTargetParent != associatedTargets)
			{
				TargetManager.HideAllTargets();
				TargetManager.instance.targets = associatedTargets;
			}
			TargetManager.ResetAllTargets();
			TimeManager.Show(timeToBeatInSeconds, () =>
			{
				//spawn the small key here
				Debug.Log("Beat the shrine!");
				TimeManager.Hide();
				UpdateVisualProgress(1);
				beatenShrine = true;
			});
		}
		else
		{
			TimeManager.Hide();
			TargetManager.HideAllTargets();
		}
	}
}
