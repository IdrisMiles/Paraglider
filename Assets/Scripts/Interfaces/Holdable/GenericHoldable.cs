using UnityEngine;
using System.Collections;

public class GenericHoldable : MonoBehaviour, IHoldable {
    
    void Awake()
    {
        HoldableLayerName = "Holdable";
        HoldableLayerMask = LayerMask.GetMask(new string[] { HoldableLayerName });
        gameObject.layer = LayerMask.NameToLayer(HoldableLayerName);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    //--------------------------------------------------------------------------
    // Implentation of IHoldable

    public virtual bool IsHeld { get; set; }
    public virtual GameObject Hand { get; set; }

    public virtual string HoldableLayerName { get; set; }

    public virtual int HoldableLayerMask { get; set; }

    public virtual void SetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Holdable");
    }

    public virtual void OnHeld(GameObject _holder)
    {

        if (_holder == null) { return; }

        Hand = _holder;
        IsHeld = true;

        StartCoroutine("Hold");
    }

    public virtual IEnumerator Hold()
    {
        while (IsHeld && Hand != null)
        {
            transform.position = Hand.transform.position;
            transform.rotation = Hand.transform.rotation;

            yield return null;
        }
        
    }

    public virtual void OnReleased()
    {
        IsHeld = false;
        Hand = null;
        
    }
}

