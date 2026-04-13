using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using TMPro; // Pour le texte
using UnityEngine.UI; // Pour le slider

public class PlaceOnPlane : MonoBehaviour
{
    [Header("AR et Prefab")]
    public GameObject objetAPlacer; 
    public ARRaycastManager raycastManager;
    
    [Header("Interface Utilisateur (UI)")]
    public GameObject panneauInfos; 
    public TextMeshProUGUI texteInfos; 

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject objetDejaPlace; 

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Sécurité interface
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return; 

            if (touch.phase == TouchPhase.Began)
            {
                // 1. TENTATIVE : Est-ce qu'on touche une planète 3D ?
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit3D;
                
                if (Physics.Raycast(ray, out hit3D))
                {
                    AfficherInfosPlanete(hit3D.collider.gameObject.name);
                    return; // On a touché une planète, on arrête le code ici !
                }

                // 2. SINON : On place/déplace le système sur le sol
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
                        panneauInfos.SetActive(false); // Cache les infos quand on déplace
                    }
                }
            }
        }
    }

    // Fonction pour le bouton "Effacer"
    public void EffacerPlanetes()
    {
        if (objetDejaPlace != null) Destroy(objetDejaPlace);
        panneauInfos.SetActive(false);
    }

    // Fonction pour le Slider de taille
    public void ChangerTaille(float nouvelleTaille)
    {
        if (objetDejaPlace != null)
        {
            // On multiplie la taille de base par la valeur du slider
            objetDejaPlace.transform.localScale = Vector3.one * nouvelleTaille;
        }
    }

    // L'encyclopédie
    void AfficherInfosPlanete(string nomPlanete)
    {
        panneauInfos.SetActive(true);
        
        if (nomPlanete == "Soleil") texteInfos.text = "<b>Le Soleil</b>\nÉtoile au centre du système. 5500°C en surface.";
        else if (nomPlanete == "Terre") texteInfos.text = "<b>La Terre</b>\nNotre maison. Seule planète connue abritant la vie.";
        else if (nomPlanete == "Mars") texteInfos.text = "<b>Mars</b>\nLa planète rouge. Abrite le plus haut volcan du système.";
        else if (nomPlanete == "Saturne") texteInfos.text = "<b>Saturne</b>\nGéante gazeuse avec de magnifiques anneaux de glace.";
        else if (nomPlanete == "Jupiter") texteInfos.text = "<b>Jupiter</b>\nLa plus grande planète du système solaire.";
        else texteInfos.text = "<b>" + nomPlanete + "</b>\nUn astre fascinant de notre système.";
    }
}