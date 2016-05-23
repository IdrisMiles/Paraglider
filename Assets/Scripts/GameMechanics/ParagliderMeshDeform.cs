using UnityEngine;
using System.Collections;

public class ParagliderMeshDeform : MonoBehaviour {

    string m_name;
	Transform m_trans;
    System.Collections.Generic.Dictionary<string,Transform> m_meshTransforms = new System.Collections.Generic.Dictionary<string, Transform>();


	// Use this for initialization
	void Start () 
    {
        m_name = this.name;
		m_trans = this.transform;

        // Get child objects
        for (int i=0; i<m_trans.childCount; i++)
        {
            Transform child = m_trans.GetChild(i);
            m_meshTransforms.Add(child.name,child);
        }
	
	}
	
	// Update is called once per frame
	void Update () 
    {

		// Move Paraglider Up and Down
		if (Input.GetKey (KeyCode.O)) 
        {
			//m_meshTransforms[m_name+"_M"].position += m_trans.TransformDirection(new Vector3(0.0f,0.1f,0.0f));
            m_meshTransforms[m_name+"_M"].GetComponent<Rigidbody>().AddForce(new Vector3(0.0f,30f,0.0f));
		}
		else if (Input.GetKey (KeyCode.P)) 
        {
			//m_meshTransforms[m_name+"_M"].position += m_trans.TransformDirection(new Vector3(0.0f,-0.1f,0.0f));
            m_meshTransforms[m_name+"_M"].GetComponent<Rigidbody>().AddForce(new Vector3(0.0f,-30f,0.0f));
		}

        // Cosntant wind force
        foreach (Transform o in m_meshTransforms.Values)
        {
            o.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f,4f,0.0f));

            float surfaceTension = o.position.x - m_meshTransforms[m_name+"_M"].position.x;
            Vector3 pressure = new Vector3(0.5f*surfaceTension, 0.0f,0.0f);//o.position - m_trans.position;
            //pressure.Normalize();
            o.GetComponent<Rigidbody>().AddForce(pressure);
        }
	
	}

    void Integrate()
    {

    }

}
