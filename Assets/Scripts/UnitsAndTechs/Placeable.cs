using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
    public abstract Vector2 GridSize { get; set; }
    public virtual GridMode GridMode { get; set; }
}