using UnityEngine;
using System.Collections;

public class AirFoil : MonoBehaviour {

    public AirVolume m_air;
    public FluidElement m_localAir;

    // projected surface area along flightpath
    public float m_surfaceArea;
    public float m_surfaceAreaMax;
    public float m_surfaceAreaMin;

    // 
    public GameObject m_AngleOfAttackTransform;
    public float m_angleOfAttack;
    public float m_angleOfAttackMax;
    public float m_angleOfAttackMin;
    public float m_angleOfAttackCritical;

    // coefficients
    public float m_dragCoefficient;
    public float m_liftCoefficient;
    public float m_liftCoefficientMax;

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
    public float m_dynamicPressure;

    // Aeroydynamic forces
    public Vector3 m_lift = new Vector3();
    public Vector3 m_drag = new Vector3();
    public Vector3 m_flightDirection = new Vector3();
    public Vector3 m_relativeWind = new Vector3();

    public Rigidbody m_physicsBody;
    public GameObject m_Canopy;

	// Use this for initialization
	void Start () {

        //m_physicsBody = GetComponent<Rigidbody>();

        //m_TurnAngleTransform = this.transform.FindChild("Transform_TurnAngle").gameObject;
        if (m_TurnAngleTransform == null)
        {
            // Handle here
        }

        // Get GameObject representing Banking angle
        //m_BankingAngleTransform = m_TurnAngleTransform.transform.FindChild("Transform_BankingAngle").gameObject;
        if (m_BankingAngleTransform == null)
        {
            // Handle here
        }
	

        // Get GameObject representing angle of attack
        //m_AngleOfAttackTransform = m_BankingAngleTransform.transform.FindChild("Transform_AngleOfAttack").gameObject;
        if (m_AngleOfAttackTransform == null)
        {
            // Handle here
        }


        // 
        m_air = GameObject.FindGameObjectWithTag("Air").GetComponent<AirVolume>();
        m_localAir = m_air.m_voxels [m_air.Hash(transform.position)];
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        m_localAir = m_air.m_voxels [m_air.Hash(transform.position)];
        m_relativeWind = /*m_localAir.m_wind */- m_physicsBody.velocity;

        ComputeDynamicPressure();
        ComputeTurn();
        ComputeSurfaceArea();
        ComputeDragForce();
        ComputeLiftForce();

        //Debug.Log("Lift Drag ratio: " + m_liftCoefficient / m_dragCoefficient);
        //Debug.Log("Wind: " + m_localAir.m_wind);
        //Debug.Log("Force to apply: " + (m_lift + m_drag + m_localAir.m_wind));
        //Debug.Log("Speed: " + m_physicsBody.velocity.magnitude);
        m_physicsBody.AddForce((m_lift + m_drag + m_localAir.m_wind), ForceMode.Force);

	}


    void ComputeDynamicPressure()
    {
        m_dynamicPressure = 0.5f * m_localAir.m_density * m_physicsBody.velocity.sqrMagnitude;
    }


    /// <summary>
    /// Computes the turn angles for angle of attack and banking angle.
    /// Also applies the torque for rotation of the rigidbody.
    /// </summary>
    void ComputeTurn()
    {
        // determine which way we are turning
        m_brakeDifference = m_brakeLeft - m_brakeRight;
        m_bankingAngle = m_brakeDifference * 70.0f;

        // determine how much brakes are being applied
        m_brakeAverage = 0.5f * (m_brakeRight + m_brakeLeft);
        m_angleOfAttack = Mathf.Lerp(m_angleOfAttackMin, m_angleOfAttackMax, m_brakeAverage);

        // angle of attack
        Vector3 attackAng = m_AngleOfAttackTransform.transform.localRotation.eulerAngles;
        attackAng.x = m_angleOfAttackMax - m_angleOfAttack;
        m_AngleOfAttackTransform.transform.localRotation = Quaternion.Euler(attackAng);

        // banking angle
        Vector3 bankAng = m_BankingAngleTransform.transform.localRotation.eulerAngles;
        bankAng.z = m_bankingAngle;
        m_BankingAngleTransform.transform.localRotation = Quaternion.Euler(bankAng);
        m_physicsBody.AddTorque(0.0f, -m_brakeDifference, 0.0f, ForceMode.Acceleration);
    }


    /// <summary>
    /// Computes the projected surface area of the canopy and updates m_surfaceArea.
    /// </summary>
    void ComputeSurfaceArea()
    {
        float dotProdVel = Mathf.Abs(Vector3.Dot(-m_relativeWind.normalized, new Vector3(0, -1, 0)));
        float angleOfAttackRatio = (m_angleOfAttack / m_angleOfAttackMax);
        float surfaceAreaDiff = m_surfaceAreaMax - m_surfaceAreaMin;
        m_surfaceArea = (m_surfaceAreaMax * 0.9f) + (angleOfAttackRatio * 0.1f * m_surfaceAreaMax) - (dotProdVel * 0.9f * surfaceAreaDiff);
        m_surfaceArea *= (1.1f - Mathf.Abs(m_brakeDifference));

        //Debug.Log("Surface Area: " + m_surfaceArea);
        //Debug.Log("dot prod: " + dotProdVel);
    }


    void ComputeLiftCoefficient()
    {
        float angleOfAttackRatio = Mathf.PingPong(m_angleOfAttack, m_angleOfAttackCritical) / m_angleOfAttackCritical;
        m_liftCoefficient = angleOfAttackRatio * m_liftCoefficientMax;

    }

    void ComputeLiftForce()
    {
        ComputeLiftCoefficient();
        
        float liftMagnitude = m_dynamicPressure * m_surfaceArea * m_liftCoefficient;

        // Work out Lift unit vector
        Vector3 localForward = m_physicsBody.velocity;// - m_localAir.m_wind;
        localForward.Normalize();
        Vector3 localUp = Vector3.Cross(localForward, m_Canopy.transform.right);
        
        m_lift = localUp * liftMagnitude;

        //Debug.Log("Lift Magnitude:" + liftMagnitude);
        //Debug.Log("Lift Force: " + m_lift);
    }

    void ComputeDragForce()
    {
        m_drag = -m_physicsBody.velocity.normalized * m_dynamicPressure * m_surfaceArea * m_dragCoefficient;

        //Debug.Log("Drag Force: " + m_drag);
    }

}
