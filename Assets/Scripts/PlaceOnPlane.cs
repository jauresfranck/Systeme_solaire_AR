using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems; // <-- NOUVEAU : Pour gérer les boutons (UI)

public class PlaceOnPlane : MonoBehaviour
{
    [Header("Ce qu'on veut faire apparaître (Le Prefab)")]
    public GameObject objetAPlacer; 
    
    [Header("Le scanner de clic")]
    public ARRaycastManager raycastManager;
    
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject objetDejaPlace; 

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // NOUVEAU : Sécurité ! Si le doigt touche l'interface (un bouton), on bloque le code ici.
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return; 
            }

            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;

                    if (objetDejaPlace == null)
                    {
                        objetDejaPlace = Instantiate(objetAPlacer, hitPose.position, hitPose.rotation);
                    }
                    else
                    {
                        objetDejaPlace.transform.position = hitPose.position;
                    }
                }
            }
        }
    }

    // NOUVEAU : La fonction magique qui sera activée par notre futur bouton
    public void EffacerPlanetes()
    {
        if (objetDejaPlace != null)
        {
            Destroy(objetDejaPlace); // On détruit le clone du système solaire
            objetDejaPlace = null;   // On vide la mémoire pour pouvoir recommencer
        }
    }
}