using UnityEngine;
using System.Collections;

public class AirVolume : MonoBehaviour {

    public Vector3 m_volumeSize = new Vector3(1024.0f,1024.0f,1024.0f);
    public Vector3 m_volumeCentre;
    public int m_gridResolution = 8;
    public Vector3 m_voxelSize;

    public FluidElement[] m_voxels;

    void Awake()
    {
        m_volumeCentre = 0.5f * m_volumeSize;

        int voxelSize = (int)m_volumeSize.x;
        for (int i = 0; i < m_gridResolution && voxelSize > 0; i++)
        {
            voxelSize /= 2;
        }
        m_voxelSize = new Vector3(voxelSize, voxelSize, voxelSize);

        m_voxels = new FluidElement[(m_gridResolution+1) * (m_gridResolution+1) * (m_gridResolution+1)];
        for (int i = 0; i < m_voxels.Length; i++)
        {

            m_voxels [i] = new FluidElement();

        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        for (int i = 0; i < m_voxels.Length; i++)
        {
            m_voxels [i].m_wind = new Vector3(5.0f, 0.0f, -10.0f);
        }
	
	}

    // encode position vector into array index
    public int Hash(Vector3 _pos)
    {
        int x = (int)(_pos.x / m_voxelSize.x);
        int y = (int)(_pos.y / m_voxelSize.y);
        int z = (int)(_pos.z / m_voxelSize.z);

        int hash = (int)((x * m_voxelSize.x * m_voxelSize.y) + (y * m_voxelSize.x) + z);

        return hash < 0 ? 0 : (hash > m_voxels.Length ? m_voxels.Length-1 : hash);

        //return new Vector3(_pos.x / m_voxelSize.x, _pos.y / m_voxelSize.y, _pos.z / m_voxelSize.z);
    }
}
