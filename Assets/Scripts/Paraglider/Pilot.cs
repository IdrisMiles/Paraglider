using UnityEngine;
using System.Collections;

public class Pilot : MonoBehaviour {

    GameObject m_canopyGO;
    AirFoil m_canopyAF;

    public PilotHand m_handLeft;
    public PilotHand m_handRight;

    // Use this for initialization
    void Start () 
    {
        m_canopyGO = GameObject.FindGameObjectWithTag("Canopy");
        if (m_canopyGO != null)
        {
            m_canopyAF = m_canopyGO.GetComponent<AirFoil>();
        }
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //HandleInput();	
	}


    private void HandleInput()
    {
        // Left Brake
        if (Input.GetKey(KeyCode.A))
        {
            if(m_canopyAF.m_brakeLeft < 1.0f)
            {
                m_canopyAF.m_brakeLeft += 0.005f;
            }
        } 
        else if (m_canopyAF.m_brakeLeft > 0.0f)
        {
            m_canopyAF.m_brakeLeft -= 0.005f;
        }

        //
        if (Input.GetKey(KeyCode.D))
        {
            if(m_canopyAF.m_brakeRight < 1.0f)
            {
                m_canopyAF.m_brakeRight += 0.005f;
            }
        } 
        else if (m_canopyAF.m_brakeRight > 0.0f)
        {
            m_canopyAF.m_brakeRight -= 0.005f;
        }
    }

}
