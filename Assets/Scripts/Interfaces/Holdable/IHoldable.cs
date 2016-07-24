using UnityEngine;
using System.Collections;

public interface IHoldable
{
    /// <summary>
    /// Boolean property to determin if object is cyrrently being held.
    /// </summary>
    bool IsHeld { get; set; }

    /// <summary>
    /// Reference to the gameobject holding this
    /// </summary>
    GameObject Hand { get; set; }

    string HoldableLayerName { get; set; }

    int HoldableLayerMask { get; set; }

    /// <summary>
    /// Interface to be implemented, set the layer mask used by IHolder to locate this IHoldable
    /// </summary>
    void SetLayer();

    /// <summary>
    /// Interface to implement what happens when this object is held.
    /// </summary>
    /// <param name="_holder"></param>
    void OnHeld(GameObject _holder);

    /// <summary>
    /// Interface to implement what happens whilst this object is being held.
    /// </summary>
    /// <returns></returns>
    IEnumerator Hold();

    /// <summary>
    /// Interface to implement what happens when this object is released.
    /// </summary>
    void OnReleased();
}

