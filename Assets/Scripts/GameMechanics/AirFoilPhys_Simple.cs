using UnityEngine;
using System.Collections;

public class AirFoilPhys_Simple : MonoBehaviour {

    Rigidbody m_rigidBody;
    GameObject m_angleOfAttackObject;

    //-----------------------------------------------------------
    // Angles

    // higher AoA increases Lift and Drag thus decreases Speed
    public float m_angleOfAttack = 10.0f;

    // angle between cord of wing and horizon
    public float m_attitudeAngle;

    // angle between horizon and flight direction
    public float m_flightAngle;



    //-----------------------------------------------------------
    // Forces

    // wind force
    public Vector3 m_externalWind = new Vector3(0.0f, 0.0f, 10.0f);

    // airflow created by forward motion, has same magnitude as forward motion
    public Vector3 m_relativeWind = new Vector3();

    // weight

    // lift

    // drag

    // thrust?

    public Vector3 m_velocity = new Vector3();


    //-----------------------------------------------------------
    // Speeds

    // relative motion through air
    public Vector3 m_airSpeed;

    // relative motion to ground - air speed + wind speed
    public Vector3 m_groundSpeed;

    //-----------------------------------------------------------
    // Coefficients and ratios

    // distance travelled / height lost
    public float m_glideRatio;

    // angle of motion and horizon
    public float m_angleOfDescent;
    // see above
    public float m_angleOfFlight;

    // average line between top and bottom of wing.
    public float m_meanCamberLine;


    public float m_dragCoef = 50.0f; // 50m^2 - 170ft^2 canopy
    public float m_liftCoef = 1.0f;

    // Lift force vector
    public float m_lift;

    // weight of cnopy and pilot, vector
    public float m_weight;


    public float m_thrust;
    public float m_drag;

    public float m_canopyArea = 50.0f;
    public float m_density = 1.0f;
    public float m_descentAngle = 90.0f;


    private Vector3 m_prevVel = new Vector3();



	// Use this for initialization
	void Start () {

        // Get Rigidbody component
        m_rigidBody = this.GetComponent<Rigidbody>();
        if (m_rigidBody == null)
        {
            m_rigidBody = new Rigidbody();
        }

        // Get GameObject representing angle of attack
        m_angleOfAttackObject = this.transform.FindChild("Transform_AngleOfAttack").gameObject;
        if (m_angleOfAttackObject == null)
        {
            // Handle here
        }
	
	}
	
	// Update is called once per frame
	void Update () {

        UpdateAngleOfAttack();

	}

    void FixedUpdate()
    {
        ComputeDescentAngle();
        ComputeLift();
        ComputeDrag();
        ComputeThrust();
        
        ApplyForce();

    }

    void ApplyForce()
    {
        Debug.Log("Lift: " + m_lift + " | Thrust: " + m_thrust);
        //m_rigidBody.AddRelativeForce(new Vector3(0.0f, 0.01f * m_lift, 0.01f * (m_thrust)));
        m_rigidBody.AddForce(new Vector3(0.0f, 0.01f * m_lift, 0.01f * (m_thrust)));

        Debug.Log("Velocity: " + m_rigidBody.velocity);
    }

    void UpdateAngleOfAttack()
    {
        // Update Angle of attack
        if (Input.GetKey (KeyCode.L)) 
        {
            m_angleOfAttack = m_angleOfAttack>19.9f ? 20.0f : m_angleOfAttack + 0.1f;
            Vector3 tmpAng = m_angleOfAttackObject.transform.rotation.eulerAngles;
            tmpAng.x = 20.0f-m_angleOfAttack;
            m_angleOfAttackObject.transform.rotation = Quaternion.Euler(tmpAng);
        }
        else if (Input.GetKey (KeyCode.K)) 
        {
            m_angleOfAttack = m_angleOfAttack<0.2f ? 0.1f : m_angleOfAttack - 0.1f;
            Vector3 tmpAng = m_angleOfAttackObject.transform.rotation.eulerAngles;
            tmpAng.x = 20.0f-m_angleOfAttack;
            m_angleOfAttackObject.transform.rotation = Quaternion.Euler(tmpAng);
        }

        m_liftCoef = 10*m_angleOfAttack / m_canopyArea;

        float changeInVelSqr = m_rigidBody.velocity.sqrMagnitude - m_prevVel.sqrMagnitude;
        //m_angleOfAttack = (2.0f*m_lift)/(m_density*m_canopyArea*m_liftCoef*m_rigidBody.velocity.sqrMagnitude);

        m_prevVel = m_rigidBody.velocity;
    }

    void ComputeLift()
    {
        // Lift ~=~ AngleOfAttack * Velocit^2
        m_lift = m_angleOfAttack * Mathf.Pow(m_rigidBody.velocity.y,2.0f);
        //Vector3 vel = m_rigidBody.transform.TransformDirection(new Vector3(0.0f, 0.0f, m_rigidBody.velocity.magnitude));

        // Lift = 0.5 * Density * Velocity^2 * LiftCoefficient * Area
        m_lift = 0.5f * m_density * m_canopyArea * m_liftCoef * Mathf.Pow(m_rigidBody.velocity.y,2.0f);
        
        // Lift*Sin(DescentAngle) = Drag*Cos(DescentAngle)
        // DescentAngle - Descent direction relative to Horizon
        // A dot B = |A|*|B|*Cos(theta)
        // Lift / Drag = Cos(DescentAngle) / Sin(DesccentAngle)
        //             = HorizontalDistGained / VerticleHeightLost

    }

    void ComputeThrust()
    {
        // lift = Force * sin(angle of attack)
        // o / tan^-1() = a
        m_thrust = m_lift / (Mathf.Rad2Deg * Mathf.Tan(m_angleOfAttack));

        m_thrust = (m_lift * -m_rigidBody.velocity.y) / m_drag;
        
    }

    void ComputeDrag()
    {
        // Drag = 0.5 * Density * Velocity^2 * DragCoefficient * Area
        m_drag = 0.5f * m_density * m_canopyArea * m_rigidBody.velocity.sqrMagnitude;
    }

    void ComputeDescentAngle()
    {
        m_descentAngle = Mathf.Rad2Deg * Mathf.Asin(m_rigidBody.velocity.y / m_rigidBody.velocity.magnitude);
        Debug.Log("Descent Angle: " + m_descentAngle);
    }
}
