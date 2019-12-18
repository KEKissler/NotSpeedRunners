using UnityEngine;

public class TimerAnimationController : MonoBehaviour {

    [HideInInspector]
    public float? ClipLength = null;
    [HideInInspector]
    public System.Action OnTimerEnd;
    [HideInInspector]
    public Transform ToFollow;
    public Color color;
    public SpriteRenderer rotater, leftBlocker, rightBlocker;

    private float duration = 0;
    private bool firstHalf = true;

	// Use this for initialization
	void Start () {
        rotater.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		if(ClipLength == null)
        {
            return;
        }
        if(duration > ClipLength.Value || ClipLength.Value <= 0)
        {
            if(OnTimerEnd != null)
            {
                OnTimerEnd.Invoke();
            }
            Destroy(gameObject);
            return;
        }
        if(ToFollow != null)
        {
            transform.position = ToFollow.position;
        }
        rotater.transform.eulerAngles = new Vector3(0, 0, duration / ClipLength.Value * -360);
        if (firstHalf && duration > ClipLength.Value / 2)
        {
            leftBlocker.sortingOrder = 0;
            rightBlocker.color = rotater.color;
            firstHalf = false;
        }
        duration += Time.deltaTime;

	}
}
