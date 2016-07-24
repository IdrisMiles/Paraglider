using UnityEngine;
using System.Collections;

public class GenericHolder<T> : IHolder where T : MonoBehaviour, IHoldable
{

    public GenericHolder(GameObject _holder, float _graspRadius = 1.0f)
    {
        Holder = _holder;
        GraspRadius = _graspRadius;
    }


    //--------------------------------------------------------------------------
    // Implentation of IHolder

    public bool IsHolding { get; set; }

    public float GraspRadius { get; set; }

    public Collider[] HeldObjects { get; set; }

    public GameObject Holder { get; set; }

    public void Hold()
    {
        if (IsHolding)
        {
            return;
        }

        GameObject holdableTypeOject = new GameObject();
        try
        {
            T holdableTypeComponent = holdableTypeOject.AddComponent<T>();
            HeldObjects = Physics.OverlapSphere(Holder.transform.position, GraspRadius, holdableTypeComponent.HoldableLayerMask);

            if (HeldObjects.Length != 0)
            {
                IsHolding = true;

                foreach (var item in HeldObjects)
                {
                    try
                    {
                        item.gameObject.GetComponent<T>().OnHeld(Holder.gameObject);
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e);
                        Debug.Log("Item does not have a ParagliderToggle component");
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Object.Destroy(holdableTypeOject);
        }
        Object.Destroy(holdableTypeOject);

    }

    public void Release()
    {
        IsHolding = false;

        if (HeldObjects != null)
        {
            foreach (Collider item in HeldObjects)
            {

                try
                {
                    item.gameObject.GetComponent<T>().OnReleased();
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                    Debug.Log("Item does not have a ParagliderToggle component");
                }
            }

            System.Array.Clear(HeldObjects, 0, HeldObjects.Length);
        }

    }
}
