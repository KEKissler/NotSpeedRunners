using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InputSettings : ScriptableObject {
    public KeyCode jump;
    public KeyCode walkLeft;
    public KeyCode walkRight;
    public KeyCode grapple;
    public KeyCode respawn;
}
