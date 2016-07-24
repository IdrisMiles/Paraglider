using UnityEngine;
using System.Collections;

public interface IHolder
{
    void Hold();

    void Release();

    bool IsHolding { get; set; }
    float GraspRadius { get; set; }
    Collider[] HeldObjects { get; set; }
    GameObject Holder { get; set; }
}
