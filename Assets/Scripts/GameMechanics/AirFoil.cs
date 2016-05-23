using UnityEngine;
using System.Collections;

public class AirFoil : MonoBehaviour {

    public AirVolume m_air;
    public FluidElement m_relativeAir;

    // projected surface area along flightpath
    public float m_surfaceArea;
    public float m_surfaceAreaMax;
    public float m_surfaceAreaMin;

    // 
    public GameObject m_AngleOfAttackTransform;
    public float m_angleOfAttack;
    public float m_angleOfAttackMax;
    public float m_angleOfAttackMin;

    // coefficients
    public float m_dragCoefficient;
    public float m_liftCoefficient;

    // brake input, positive difference = turn right
    // also changes flight path direction/orientation, 
    // rotate Lift along 'Forward' vector
    public float m_brakeLeft;
    public float m_brakeRight;
    public float m_brakeDifference;
    public float m_brakeAverage;

    public GameObject m_BankingAngleTransform;
    public float m_bankingAngle;

    public GameObject m_TurnAngleTransform;

    // Aerodynamic things
    public float m_mass;
    public float m_dynamicPressure;

    // Aeroydynamic forces
    public Vector3 m_lift = new Vector3();
    public Vector3 m_drag = new Vector3();
    public Vector3 m_weight = new Vector3();
    public Vector3 m_flightDirection = new Vector3();

    private Rigidbody m_physicsBody;

	// Use this for initialization
	void Start () {

        m_physicsBody = GetComponent<Rigidbody>();
        m_weight = new Vector3(0.0f, -m_mass * 10, 0.0f);

        m_TurnAngleTransform = this.transform.FindChild("Transform_TurnAngle").gameObject;
        if (m_TurnAngleTransform == null)
        {
            // Handle here
        }

        // Get GameObject representing Banking angle
        m_BankingAngleTransform = m_TurnAngleTransform.transform.FindChild("Transform_BankingAngle").gameObject;
        if (m_BankingAngleTransform == null)
        {
            // Handle here
        }
	

        // Get GameObject representing angle of attack
        m_AngleOfAttackTransform = m_BankingAngleTransform.transform.FindChild("Transform_AngleOfAttack").gameObject;
        if (m_AngleOfAttackTransform == null)
        {
            // Handle here
        }


        // 
        m_air = GameObject.FindGameObjectWithTag("Air").GetComponent<AirVolume>();
        m_relativeAir = m_air.m_voxels [m_air.Hash(transform.position)];
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        m_relativeAir = m_air.m_voxels [m_air.Hash(transform.position)];

        ComputeDynamicPressure();
        ComputeTurn();
        ComputeSurfaceArea();
        ComputeDragForce();
        ComputeLiftForce();

        m_physicsBody.AddForce(m_lift + m_drag + m_weight + m_relativeAir.m_wind, ForceMode.Force);
	
	}

    void ComputeDynamicPressure()
    {
        m_dynamicPressure = 0.5f * m_relativeAir.m_density * m_physicsBody.velocity.sqrMagnitude;
    }


    void ComputeTurn()
    {
        m_brakeDifference = m_brakeLeft - m_brakeRight;
        m_brakeAverage = 0.5f * (m_brakeRight + m_brakeLeft);
        m_angleOfAttack = Mathf.Lerp(m_angleOfAttackMin, m_angleOfAttackMax, m_brakeAverage);
        m_bankingAngle = m_brakeDifference * 45.0f;

        // angle of attack
        Vector3 attackAng = m_AngleOfAttackTransform.transform.localRotation.eulerAngles;
        attackAng.x = m_angleOfAttackMax - m_angleOfAttack;
        m_AngleOfAttackTransform.transform.localRotation = Quaternion.Euler(attackAng);

        // banking angle
        Vector3 bankAng = m_BankingAngleTransform.transform.localRotation.eulerAngles;
        bankAng.z = m_bankingAngle;
        m_BankingAngleTransform.transform.localRotation = Quaternion.Euler(bankAng);
        m_physicsBody.AddTorque(0.0f, -m_brakeDifference*2.0f, 0.0f, ForceMode.Force);
    }


    /// <summary>
    /// Computes the projected surface area of the canopy.
    /// Also updates drag and lift coefficients
    /// </summary>
    void ComputeSurfaceArea()
    {
        float dotProdVel = Vector3.Dot(m_physicsBody.velocity.normalized, new Vector3(0, -1, 0));
        m_surfaceArea = m_surfaceAreaMin +  ( dotProdVel * (1 - Mathf.Abs(m_brakeDifference)) * (m_surfaceAreaMax - m_surfaceAreaMin));
        m_surfaceArea += 20 * Mathf.Sin(90.0f * (m_angleOfAttack / m_angleOfAttackMax));
    }


    void ComputeLiftForce()
    {

        float liftMagnitude = m_dynamicPressure * m_surfaceArea * m_liftCoefficient;

        // Work out Lift unit vector
        Vector3 localForward = m_physicsBody.velocity;// - m_relativeAir.m_wind;
        localForward.Normalize();

        //localForward = Quaternion.AngleAxis(m_angleOfAttackMax - m_angleOfAttack, transform.right) * localForward;
        Vector3 localUp = Vector3.Cross(localForward, transform.right);

        //m_lift = Quaternion.AngleAxis(m_bankingAngle, localForward) * localUp * liftMagnitude;
        m_lift = localUp * liftMagnitude;

        //Debug.Log("loacl forward:" + localForward);
        //Debug.Log("loacl up:" + localUp);
        //Debug.Log(m_lift);
    }

    void ComputeDragForce()
    {
        m_drag = -m_physicsBody.velocity.normalized * m_dynamicPressure * m_surfaceArea * m_dragCoefficient;
    }



    private void HandleInput()
    {
        // Left Brake
        if (Input.GetKey(KeyCode.A))
        {
            if(m_brakeLeft < 1.0f)
            {
                m_brakeLeft += 0.005f;
            }
        } 
        else if (m_brakeLeft > 0.0f)
        {
            m_brakeLeft -= 0.005f;
        }

        //
        if (Input.GetKey(KeyCode.D))
        {
            if(m_brakeRight < 1.0f)
            {
                m_brakeRight += 0.005f;
            }
        } 
        else if (m_brakeRight > 0.0f)
        {
            m_brakeRight -= 0.005f;
        }
    }



}
