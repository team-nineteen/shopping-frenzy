using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : Holdable
{
    override public void NotifyHold(bool isHeld)
    {
        if (isHeld && !m_WasHeld) {
            m_WasHeld = true;
        } else {
            m_ReleaseTime = Time.fixedTime;
        }
        //rb.useGravity = !isHeld;
        m_isHeld = isHeld;
    }

    override protected void Start() {
        base.Start();
        isPushable = true;
    }
}
