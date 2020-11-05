using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{

    [Tooltip("How far away the item is from the person holding it")]
    public float distanceWhenHeld = 1.0f;

    public Rigidbody rb { get; private set; }
    private bool m_WasHeld, m_isHeld;
    private float m_ReleaseTime;
    public void NotifyHold(bool isHeld)
    {
        if (isHeld && !m_WasHeld) {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            m_WasHeld = true;
        } else {
            m_ReleaseTime = Time.fixedTime;
        }
        rb.useGravity = !isHeld;
        m_isHeld = isHeld;
    }

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (m_WasHeld && !m_isHeld && m_ReleaseTime + 1 < Time.fixedTime && rb.velocity.sqrMagnitude < 1f)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            m_WasHeld = false;
        }
    }
}
