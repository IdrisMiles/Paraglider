using UnityEngine;
using System.Collections;

public class PhysicsState
{
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 position;



    public PhysicsState(Vector3 _acceleration,
                        Vector3 _velocity,
                        Vector3 _position)
    {
        acceleration = _acceleration;
        velocity = _velocity;
        position = _position;
    }

}
