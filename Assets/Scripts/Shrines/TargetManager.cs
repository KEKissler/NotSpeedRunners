using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for destroying and creating targets, as well as updating a live reference to the current active shrine, if any
/// </summary>
public class TargetManager : MonoBehaviour
{
	public TimeManager TimeManager;
	public int RemainingTargets
	{
		get { return numTargets; }
		set
		{
			numTargets = value;
			if (numTargets <= 0)
			{
				TimeManager.OnEndCurrentTimer();
			}
		}
	}
	[HideInInspector] public Transform targets;

	private int numTargets;
	private static TargetManager instance;
	
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

	public void Start()
	{
		Initialize();
	}

	public void Initialize ()
	{
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
