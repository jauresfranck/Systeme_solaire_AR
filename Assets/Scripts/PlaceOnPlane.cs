using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceOnPlane : MonoBehaviour
{
    [Header("Ce qu'on veut faire apparaître (Le Prefab)")]
    public GameObject objetAPlacer; 
    
    [Header("Le scanner de clic")]
    public ARRaycastManager raycastManager;
    
    // Liste pour mémoriser où le rayon touche le sol
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    // Pour mémoriser si le système solaire est déjà dans la pièce
    private GameObject objetDejaPlace; 

    void Update()
    {
        // On vérifie si un doigt touche l'écran
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Si le doigt vient tout juste de se poser sur l'écran
            if (touch.phase == TouchPhase.Began)
            {
                // On lance un "rayon laser" depuis le doigt vers le sol réel
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    // On récupère les coordonnées exactes (position + rotation) de l'impact
                    Pose hitPose = hits[0].pose;

                    // Si on n'a pas encore fait apparaître le système solaire...
                    if (objetDejaPlace == null)
                    {
                        // On le crée à l'endroit touché !
                        objetDejaPlace = Instantiate(objetAPlacer, hitPose.position, hitPose.rotation);
                    }
                    else
                    {
                        // S'il existe déjà, on le déplace simplement au nouveau clic
                        objetDejaPlace.transform.position = hitPose.position;
                    }
                }
            }
        }
    }
}