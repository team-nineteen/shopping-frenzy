﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHold : MonoBehaviour
{

    [Tooltip("Reference to the player crosshair")]
    public Crosshair crosshair;

    [Tooltip("The maximum height above the player you can pick items from")]
    public float interactYRange = .25f;

    [Tooltip("The distance from the camera that you can pickup items from")]
    public float interactRange = 1.85f;

    [Tooltip("The distance from the holding position required such that the item becomes uninterested in you")]
    public float loseItemRange = 2.5f;

    [Tooltip("How fast an item will gravitate towards the center of the screen")]
    public float itemGravitationSharpness = 1.0f;

    [Tooltip("How strong the rotation force should be for auto-rotating (aligning) objects.")]
    public float rotationTorque = 100f;

    [Tooltip("How quickly an item will settle in the center of the screen.")]
    public float itemGravitationFriction = 0.5f;
    [Tooltip("What Layers can be held")]
    public LayerMask solidCheckLayers = -1;
    public Holdable m_HeldItem { get; private set; }
    private PlayerInputHandler m_Input;
    private Camera m_Camera;
    private Holdable m_PointingAtItem;
    private bool m_previouslyHeld;
    private Vector3 gravitationPoint;
    void Start()
    {
        m_Input = GetComponent<PlayerInputHandler>();
        m_Camera = GetComponentInChildren<Camera>();
    }

    void UpdateCrosshair()
    {
        // Ray that intersects through center of screen.
        m_PointingAtItem = null;

        // Cast a ray through the center of the screen.
        Ray ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, solidCheckLayers, QueryTriggerInteraction.Ignore))
        {
            Holdable h = hit.collider.GetComponentInParent<Holdable>();
            if (h && hit.point.y - m_Camera.transform.position.y <= interactYRange)
                m_PointingAtItem = h;
        }

        // If we are currently not holding an item already.
        if (!m_HeldItem)
        {
            if (m_PointingAtItem)
            {
                // HoverCrosshair when pointing at.
                crosshair.ChangeTo(Crosshair.Aim.Hover);
            }
            else
            {
                // Regular when not pointing at anything.
                crosshair.ChangeTo(Crosshair.Aim.Default);
            }
        }

    }

    private float mod(float a, float n) {
        return ((a % n) + n) % n;
    }

    // Try to center the item to the screen as best as possible while retaining physics.
    void centerGameObject(Holdable obj)
    {
        // Calculate the point where the object should be relative to the player.
        gravitationPoint = m_Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, m_Camera.nearClipPlane + obj.distanceWhenHeld));

        // Pushable objects should gravitate much faster.
        float effectiveSharpness = (obj.isPushable ? 10 : 1) * itemGravitationSharpness;
        Vector3 difference = (gravitationPoint - obj.transform.position) * effectiveSharpness;

        // Pushable objects should not change y position, but should rotate towards the player.
        if (obj.isPushable)
        {
            difference.y = 0f;
        }
        if (obj.shouldAutoRotate)
        {
            // Apply a rotation force (Torque) to rotate the cart to align with the player rotation.
            float diff = Mathf.Acos(Quaternion.Dot(obj.transform.rotation, transform.rotation));
            if (diff > 0.01f) {
                float cw = mod(transform.rotation.eulerAngles.y - obj.transform.rotation.eulerAngles.y, 360);
                float ccw = mod(obj.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y, 360);
                float dir = cw < ccw ? 1: -1;
                obj.rb.AddTorque(transform.up * rotationTorque * dir);
            }
        }

        // Apply the friction and force on holdable.
        obj.rb.velocity = obj.rb.velocity * itemGravitationFriction;
        obj.rb.AddForce(difference);
    }

    void HoldItem()
    {
        centerGameObject(m_HeldItem);
        if ((gravitationPoint - m_HeldItem.transform.position).magnitude > loseItemRange) StopHoldingItem();
    }

    void FixedUpdate()
    {
        UpdateCrosshair();

        if (m_Input.GetInteractInputHeld() && !m_previouslyHeld)
        {
            if (m_PointingAtItem)
            {
                StartHoldingItem();
            }
        }
        else if (!m_Input.GetInteractInputHeld())
        {
            if (m_HeldItem)
            {
                StopHoldingItem();
            }
        }
        if (m_HeldItem) HoldItem();
        m_Input.isInteractToggled = (m_HeldItem != null);
        m_previouslyHeld = m_Input.GetInteractInputHeld();
    }
    void StartHoldingItem()
    {
        m_HeldItem = m_PointingAtItem;
        m_HeldItem.NotifyHold(true);
        
        crosshair.ChangeTo(Crosshair.Aim.Hold);
    }
    void StopHoldingItem()
    {
        m_HeldItem.NotifyHold(false);
        m_HeldItem = null;

        crosshair.ChangeTo(Crosshair.Aim.Hover);
    }


}
