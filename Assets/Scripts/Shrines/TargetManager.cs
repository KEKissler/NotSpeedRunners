using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for destroying and creating targets, as well as updating a live reference to the current active shrine, if any
/// </summary>
public class TargetManager : MonoBehaviour
{
	public int RemainingTargets
	{
		get { return numTargets; }
		set
		{
			numTargets = value;
			if (numTargets <= 0)
			{
				timeManager.OnEndCurrentTimer();
			}
		}
	}
	[HideInInspector] public Transform targets;

	private int numTargets;
	private static TargetManager instance;
	private TimeManager timeManager;
	
	public void Awake ()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void Initialize ()
	{
		timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
		targets = GameObject.Find("Targets").transform;
	}

	public void ResetAllTargets()
	{
		foreach (Transform t in targets)
		{
			t.gameObject.SetActive(true);
		}
		RemainingTargets = targets.childCount;
	}
}
