using UnityEngine;
using System.Collections;


public class FluidElement {

    public FluidElement(float _density = 1.225f)
    {
        m_density = _density;
        m_wind = new Vector3(0.0f, 0.0f, -10.0f);
    }

    public float m_density; //kg/m^3 - 1.225 average density at 15 Celcius at Sea Level (0 altitude)
    public Vector3 m_wind;
    public Vector3 m_position;
}
