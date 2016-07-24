using UnityEngine;
using System.Collections;

public enum PilotSide { Left, Right};

public class ParagliderToggles : GenericHoldable
{

    public AirFoil m_canopy;
    private Vector3 m_defaultPositionRelative;
    private Quaternion m_defaultOrientationRelative;
    public PilotSide m_side;


    //--------------------------------------------------------------------------

    void Awake()
    {
        SetLayer();        
    }


	// Use this for initialization
	void Start ()
    {
        m_defaultPositionRelative = transform.localPosition;
        m_defaultOrientationRelative = transform.localRotation;

        if(m_canopy == null)
        {
            m_canopy = GameObject.FindObjectOfType<AirFoil>();
        }

        IsHeld = false;
        Hand = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float togglePull = m_defaultPositionRelative.y - transform.localPosition.y;

        // Send difference in current and default transform to paraglider
        switch (m_side)
        {
            case PilotSide.Left:
                m_canopy.m_brakeLeft = togglePull;
                break;

            case PilotSide.Right:
                m_canopy.m_brakeRight = togglePull;
                break; 
        }

	
	}



    //--------------------------------------------------------------------------
    // Implentation of IHoldable

    public override bool IsHeld { get; set; }
    public override GameObject Hand { get; set; }

    public override string HoldableLayerName { get; set; }

    public override int HoldableLayerMask { get; set; }

    public override void SetLayer()
    {
        HoldableLayerName = "Holdable";
        HoldableLayerMask = LayerMask.GetMask(new string[] { HoldableLayerName });
        gameObject.layer = LayerMask.NameToLayer(HoldableLayerName);
    }

    public override void OnHeld(GameObject _holder)
    {

        if (_holder == null) { return; }

        Hand = _holder;
        IsHeld = true;

        StartCoroutine("Hold");
    }

    public override IEnumerator Hold()
    {
        while (IsHeld && Hand != null)
        {
            transform.position = Hand.transform.position;
            transform.rotation = Hand.transform.rotation;

            yield return null;
        }

        transform.localPosition = m_defaultPositionRelative;
        transform.localRotation = m_defaultOrientationRelative;
    }

    public override void OnReleased()
    {
        IsHeld = false;
        Hand = null;

        transform.localPosition = m_defaultPositionRelative;
        transform.localRotation = m_defaultOrientationRelative;
    }
}
