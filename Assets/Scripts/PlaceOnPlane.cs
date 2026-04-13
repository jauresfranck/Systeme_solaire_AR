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
        
        switch (nomPlanete)
        {
            case "Soleil":
                texteInfos.text = "<b>Le Soleil</b>\nRayon : 696 340 km\nVolume : 1.3 million de Terres\nTempérature : 5 500 °C";
                break;
            case "Mercure":
                texteInfos.text = "<b>Mercure</b>\nRayon : 2 439 km\nDistance : 58 millions km\nParticularité : La plus proche du Soleil.";
                break;
            case "Venus":
            case "Vénus": // Au cas où tu aies mis un accent
                texteInfos.text = "<b>Vénus</b>\nRayon : 6 051 km\nTempérature : 462 °C\nParticularité : La planète la plus chaude.";
                break;
            case "Terre":
                texteInfos.text = "<b>La Terre</b>\nRayon : 6 371 km\nDistance : 150 millions km\nParticularité : Notre maison !";
                break;
            case "Lune":
                texteInfos.text = "<b>La Lune</b>\nRayon : 1 737 km\nDistance Terre : 384 400 km\nParticularité : Notre satellite naturel.";
                break;
            case "Mars":
                texteInfos.text = "<b>Mars</b>\nRayon : 3 389 km\nVolume : 15% de la Terre\nParticularité : La planète rouge.";
                break;
            case "Jupiter":
                texteInfos.text = "<b>Jupiter</b>\nRayon : 69 911 km\nVolume : 1 321 Terres\nParticularité : La plus grande planète.";
                break;
            case "Saturne":
                texteInfos.text = "<b>Saturne</b>\nRayon : 58 232 km\nParticularité : Célèbre pour ses anneaux de glace.";
                break;
            case "Uranus":
                texteInfos.text = "<b>Uranus</b>\nRayon : 25 362 km\nTempérature : -224 °C\nParticularité : Elle tourne couchée sur le côté.";
                break;
            case "Neptune":
                texteInfos.text = "<b>Neptune</b>\nRayon : 24 622 km\nDistance : 4.5 milliards km\nParticularité : Vents violents à 2 000 km/h.";
                break;
            default:
                // Si on touche un objet qui n'est pas dans la liste (ex: l'anneau)
                texteInfos.text = "<b>" + nomPlanete + "</b>\nUn astre fascinant de notre système.";
                break;
        }
    }
}