﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HTC.UnityPlugin.Vive;

/// <summary>
/// Diese Klasse wird dazu genutzt, ein Objekt zu laden und es in der Welt zu platzieren.
/// </summary>
public class LoadObject : MonoBehaviour
{
    private GameObject grabbableObjectContainer;
    private GameObject trackerObjectContainer;
    private GameObject turningPlateObjectContainer;

    private List<GameObject> objectList = new List<GameObject>();

    private int chosenObject;
    private int activatedObjectContainer;

    private bool objectLoaded;

    // Wird zur Initialisierung genutzt.
    void Start()
    {
        grabbableObjectContainer = GameObject.FindGameObjectWithTag("GrabbableObjectContainer");
        trackerObjectContainer = GameObject.FindGameObjectWithTag("TrackerObjectContainer");
        turningPlateObjectContainer = GameObject.FindGameObjectWithTag("TurningPlateObjectContainer");

        chosenObject = 1;
        activatedObjectContainer = 0;
        objectLoaded = false;

        GameObject[] tempObjects = Resources.LoadAll<GameObject>("Objects/ShowObjects");
        foreach (GameObject go in tempObjects) objectList.Add(go);

        Debug.Log("Listengröße: " + objectList.Count);
        activateGrabbableObjectContainer();
    }

    /// <summary>
    /// Diese Methode aktiviert den GrabbableObjectContainer und zeigt das Objekt darin an.
    /// Zusätzlich werden die beiden anderen Container deaktiviert.
    /// </summary>
    public void activateGrabbableObjectContainer()
    {
        grabbableObjectContainer.gameObject.SetActive(true);
        trackerObjectContainer.gameObject.SetActive(false);
        turningPlateObjectContainer.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// Diese Methode aktiviert den TrackerObjectContainer und zeigt das Objekt darin an.
    /// Zusätzlich werden die beiden anderen Container deaktiviert.
    /// </summary>
    public void activateTrackerObjectContainer()
    {
        grabbableObjectContainer.gameObject.SetActive(false);
        trackerObjectContainer.gameObject.SetActive(true);
        turningPlateObjectContainer.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// Diese Methode aktiviert den TurningPlateObjectContainer und zeigt das Objekt darin an.
    /// Zusätzlich werden die beiden anderen Container deaktiviert.
    /// </summary>
    public void activateTurningPlateObjectContainer()
    {
        grabbableObjectContainer.gameObject.SetActive(false);
        trackerObjectContainer.gameObject.SetActive(false);
        turningPlateObjectContainer.transform.parent.gameObject.SetActive(true);
    }

    /// <summary>
    /// Methode zur Aktivierung der verschiedenen Interaktionsmöglichkeiten.
    /// </summary>
    /// <param name="buttonName">Name des betätigten Knopfes.</param>
    public void activateContainer(string buttonName)
    {
        switch (buttonName)
        {
            case "TurnButton":
                activateTurningPlateObjectContainer();
                turningPlateObjectContainer.transform.parent.gameObject.GetComponent<TurningPlate>().toggle3D();
                unloadObjects();
                loadObject(1);
                break;
            case "TrackButton":
                activateTrackerObjectContainer();
                unloadObjects();
                loadObject(2);
                break;
            case "GrabButton":
                activateGrabbableObjectContainer();
                unloadObjects();
                loadObject(3);
                break;
            case "TeleportButton":
                unloadObjects();
                teleport();
                break;
        }
    }

    /// <summary>
    /// Methode zum Laden des ausgewählten Objektes und Einfügen an den richtigen Objekt Container.
    /// </summary>
    /// <param name="objContainer"></param>
    private void loadObject(int objContainer)
    {
        activatedObjectContainer = objContainer;
        if (!objectLoaded)
        {
            GameObject tempObj = objectList[chosenObject].transform.gameObject;
            
            if (objContainer == 1)
            {
                Vector3 turningPlatePos = turningPlateObjectContainer.transform.parent.transform.parent.transform.position;                
                GameObject clonedObject = Instantiate(tempObj, turningPlatePos, Quaternion.identity) as GameObject;
                clonedObject.transform.parent = turningPlateObjectContainer.transform;
            }
            else if (objContainer == 2)
            {
                Vector3 trackerPos = trackerObjectContainer.transform.parent.transform.position;
                GameObject clonedObject = Instantiate(tempObj, trackerPos, Quaternion.identity) as GameObject;
                clonedObject.transform.parent = trackerObjectContainer.transform;
            }
            else if (objContainer == 3)
            {
                GameObject clonedObject = Instantiate(tempObj, new Vector3(0.2f, 0.8f, 0.2f), Quaternion.identity) as GameObject;
                clonedObject.transform.parent = grabbableObjectContainer.transform;
                clonedObject.AddComponent<BasicGrabbable>();
            }
            else Debug.Log("Fehler bei der Auswahl des ObjectContainers!");
            objectLoaded = true;
        }
    }

    /// <summary>
    /// Methode zum teleportieren in eine andere Szene, je nach ausgewähltem Objekt.
    /// </summary>
    private void teleport()
    {
        if (chosenObject == 0) SceneManager.LoadScene("BikeShowRoom");
        else if (chosenObject == 1) SceneManager.LoadScene("CarShowRoom");
    }

    /// <summary>
    /// Methode zum Zerstören der geladenen Objekte um Platz für ein nächstes zu machen.
    /// </summary>
    private void unloadObjects()
    {
        objectLoaded = false;
        if (activatedObjectContainer == 1) Destroy(turningPlateObjectContainer.transform.GetChild(0).gameObject);
        else if (activatedObjectContainer == 2) Destroy(trackerObjectContainer.transform.GetChild(0).gameObject);
        else if (activatedObjectContainer == 3) Destroy(grabbableObjectContainer.transform.GetChild(0).gameObject);
    }

    // Update wird einmal pro Frame aufgerufen.
    void Update()
    {

    }
}
