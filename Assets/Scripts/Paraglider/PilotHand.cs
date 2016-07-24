using UnityEngine;
using System;
using System.Collections;

public class PilotHand : MonoBehaviour {

    public PilotSide m_side;
    private Vector3 m_defaultPositionRelative;
    public LayerMask m_holableLayer;

    IHolder m_holder;

    //--------------------------------------------------------------------------

    // Use this for initialization
    void Start ()
    {
        m_holder = new GenericHolder<ParagliderToggles>(gameObject, 0.5f);
        m_defaultPositionRelative = transform.localPosition;

}
	
	// Update is called once per frame
	void Update ()
    {
        HandleInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_holder.Hold();
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            m_holder.Release();
        }
	
	}

    private void HandleInput()
    {
        // Left Brake
        if ((Input.GetKey(KeyCode.A) && m_side == PilotSide.Left) || (Input.GetKey(KeyCode.D) && m_side == PilotSide.Right))
        {
            if (transform.localPosition.y > m_defaultPositionRelative.y - 1.0f )
            {
                transform.localPosition += new Vector3(0.0f, -0.005f, 0.0f);
            }
        }
        else if (transform.localPosition.y < m_defaultPositionRelative.y)
        {
            transform.localPosition += new Vector3(0.0f, 0.005f, 0.0f);
        }
        
    }
    
    
}
