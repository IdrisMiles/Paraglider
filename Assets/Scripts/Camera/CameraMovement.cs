using UnityEngine;
using System.Collections;

/// <summary>
/// Camera movement.
/// This class handles the camera movement from user input.
/// </summary>
public class CameraMovement : MonoBehaviour 
{

	private Transform m_trans;
	// Use this for initialization
	void Start () 
	{
		m_trans = this.transform;
	
	}
	
	// Update is called once per frame
	void Update () {

		//-----------------------------------------------------------------------------
		// Translation
        /*
		// Forward and Backwards
		if (Input.GetKey (KeyCode.W)) {
			m_trans.position += m_trans.TransformDirection(new Vector3(0.0f,0.0f,0.1f));
		}
		else if (Input.GetKey (KeyCode.S)) {
			m_trans.position += m_trans.TransformDirection(new Vector3(0.0f,0.0f,-0.1f));
		}

		// Left and Right
		if (Input.GetKey (KeyCode.A)) {
			m_trans.position += m_trans.TransformDirection(new Vector3(-0.1f,0.0f,0.0f));
		}
		else if (Input.GetKey (KeyCode.D)) {
			m_trans.position += m_trans.TransformDirection(new Vector3(0.1f,0.0f,0.0f));
		}
        */

        // Up and Down
        if (Input.GetKey (KeyCode.PageUp)) {
            m_trans.position += new Vector3(0.0f,0.5f,0.0f);
        }
        else if (Input.GetKey (KeyCode.PageDown)) {
            m_trans.position += new Vector3(0.0f,-0.5f,0.0f);
        }


		//------------------------------------------------------------------------------
		// Rotation

		// Forward and Backwards
		if (Input.GetKey (KeyCode.UpArrow)) {
			m_trans.Rotate(new Vector3(0.8f,0.0f,0.0f),Space.Self);
		}
		else if (Input.GetKey (KeyCode.DownArrow)) {
			m_trans.Rotate(new Vector3(-0.8f,0.0f,0.0f),Space.Self);
		}
		
		// Left and Right
		if (Input.GetKey (KeyCode.LeftArrow)) {
			m_trans.Rotate(new Vector3(0.0f,-0.5f,0.0f),Space.World);
		}
		else if (Input.GetKey (KeyCode.RightArrow)) {
			m_trans.Rotate(new Vector3(0.0f,0.5f,0.0f),Space.World);
		}
	

	}
}
