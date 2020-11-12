using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    [Tooltip("How far away the item is from the person holding it")]
    public float distanceWhenHeld = 1.0f;
    
    [Tooltip("Should the object rotate to match the holders rotation?")]
    public bool shouldAutoRotate = false;
    public Rigidbody rb { get; protected set; }
    
    public bool isPushable {get; protected set; }

    protected bool m_WasHeld, m_isHeld;
    protected float m_ReleaseTime;
    virtual public void NotifyHold(bool isHeld)
    {
        if (isHeld && !m_WasHeld) {
            m_WasHeld = true;
        } else {
            m_ReleaseTime = Time.fixedTime;
        }
        rb.useGravity = !isHeld;
        m_isHeld = isHeld;
    }

    virtual protected void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        isPushable = false;
    }

    void Update()
    {
        if (m_WasHeld && !m_isHeld && m_ReleaseTime + 1 < Time.fixedTime && rb.velocity.sqrMagnitude < 1f)
        {
            m_WasHeld = false;
        }
    }
}
