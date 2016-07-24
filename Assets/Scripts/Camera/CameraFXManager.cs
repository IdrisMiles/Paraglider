using UnityEngine;
using System.Collections;

public class CameraFXManager : MonoBehaviour {

    UnityStandardAssets.ImageEffects.MotionBlur m_motionBlur;
    UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration m_chromaticAberration;

    public Rigidbody m_physicsBody;
    

	// Use this for initialization
	void Start ()
    {
        m_motionBlur = GetComponent<UnityStandardAssets.ImageEffects.MotionBlur>();
        m_motionBlur.enabled = true;

        m_chromaticAberration = GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>();
        m_chromaticAberration.enabled = true;
        

    }
	
	// Update is called once per frame
	void Update ()
    {
        float speedRatio = m_physicsBody.velocity.magnitude / 5.0f;

        m_motionBlur.blurAmount = 1.0f * speedRatio;

        m_chromaticAberration.chromaticAberration = 5.0f * speedRatio;
        m_chromaticAberration.blurDistance = 1.0f * speedRatio;
        m_chromaticAberration.blurSpread = 0.4f * speedRatio;
	
	}
}
