using UnityEngine;
using System.Collections;

public class AirVolume : MonoBehaviour {

    public Vector3 m_volumeSize = new Vector3(1024.0f,1024.0f,1024.0f);
    public Vector3 m_volumeCentre;
    public int m_gridResolution = 8;
    public Vector3 m_voxelSize;

    public FluidElement[] m_voxels;
    public FluidElement[,,] Voxels;

    void Awake()
    {
        m_volumeCentre = 0.5f * m_volumeSize;

        // work out Volume and Voxel bounds 
        int voxelSize = (int)m_volumeSize.x;
        for (int i = 0; i < m_gridResolution && voxelSize > 0; i++)
        {
            voxelSize /= 2;
        }
        m_voxelSize = new Vector3(voxelSize, voxelSize, voxelSize);

        // initiaalise each cell as a FluidElement
        int numPerAxis = (int) (m_volumeSize.x / m_voxelSize.x);
        m_voxels = new FluidElement[(numPerAxis) * (numPerAxis) * (numPerAxis)];
        for (int i = 0; i < m_voxels.Length; i++)
        {
            m_voxels [i] = new FluidElement();
        }
    }

	// Use this for initialization
	void Start ()
    {
        //------------------------------------------------------------------------------------------------

        Random.InitState((int)Time.time);
        for (int i = 0; i < m_voxels.Length; i++)
        {
            float x = Random.Range(-50.0f, 50.0f);
            float y = Random.Range(0.0f, 500.0f);
            float z = Random.Range(-50.0f, 50.0f);
            m_voxels[i].m_wind = new Vector3(x, y, z);
        }


        //------------------------------------------------------------------------------------------------

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        
        int numPerAxis = (int)(m_volumeSize.x / m_voxelSize.x);
        int maxPoints = numPerAxis * numPerAxis * numPerAxis;
        maxPoints = maxPoints > 65000 ? 65000 : maxPoints;

        Vector3[] verts = new Vector3[2*maxPoints];
        int[] indices = new int[2*maxPoints];

        for(int k=0; k< numPerAxis; k++)
        {
            for (int j = 0; j < numPerAxis; j++)
            {
                for (int i = 0; i < numPerAxis; i++)
                {
                    int id = (i + (j * numPerAxis) + (k * numPerAxis * numPerAxis));
                    if(id>=65000)
                    {
                        break;
                    }
                    
                    // voxel centre
                    Vector3 voxelCentre = new Vector3(i * m_voxelSize.x, j * m_voxelSize.y, k * m_voxelSize.z) + (0.5f*m_voxelSize) - (0.5f * m_volumeSize);
                    Vector3 voxelWind = voxelCentre + (5.0f*m_voxels[id].m_wind.normalized);

                    verts[2*id] = voxelCentre;
                    indices[2*id] = 2*id;

                    verts[(2 * id) +1] = voxelWind;
                    indices[(2 * id) + 1] = (2 * id) + 1;
                }
            }
        }

        mesh.vertices = verts;
        mesh.SetIndices(indices,MeshTopology.Lines,0);
        
        mesh.RecalculateBounds();
        mesh.UploadMeshData(true);
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
       
	
	}

    // encode position vector into array index
    public int Hash(Vector3 _pos)
    {
        int x = (int)(_pos.x / m_voxelSize.x);
        int y = (int)(_pos.y / m_voxelSize.y);
        int z = (int)(_pos.z / m_voxelSize.z);

        int hash = (int)((z * m_voxelSize.x * m_voxelSize.y) + (y * m_voxelSize.x) + x);

        return hash < 0 ? 0 : (hash >= m_voxels.Length ? m_voxels.Length-1 : hash);

        //return new Vector3(_pos.x / m_voxelSize.x, _pos.y / m_voxelSize.y, _pos.z / m_voxelSize.z);
    }
}
