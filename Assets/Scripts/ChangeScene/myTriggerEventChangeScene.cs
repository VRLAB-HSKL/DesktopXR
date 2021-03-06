﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasse wird verwendet um zu erkennen, ob ein Objekt den Collider berührt.
/// </summary>
public class myTriggerEventChangeScene : MonoBehaviour
{
    /// <summary>
    /// Diese Methode wird aufgerufen, solange ein Objekt sich im Collider befindet.
    /// </summary>
    /// <param name="collider">Das Objekt, das mit dem Collider in Berührung kommt.</param>
    private void OnTriggerStay(Collider collider)
    {
        if (collider.attachedRigidbody.gameObject.name.Equals("Caster")) this.transform.GetComponentInParent<ClickButtonChangeScene>().myTriggerStay(collider, this.gameObject.name);
    }

    /// <summary>
    /// Diese Methode wird aufherufen, wenn der Collider das Objekt nicht mehr berührt.
    /// </summary>
    /// <param name="collider">Das Objekt, das mit dem Collider in Berührung kommt.</param>
    private void OnTriggerExit(Collider collider)
    {
        if (collider.attachedRigidbody.gameObject.name.Equals("Caster")) this.transform.GetComponentInParent<ClickButtonChangeScene>().myTriggerExit(collider, this.gameObject.name);
    }
}
