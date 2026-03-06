using UnityEngine;

public class MouvementPlanete : MonoBehaviour
{
    [Header("Vitesse de rotation sur elle-même")]
    public float vitesseRotation = 20f;

    [Header("Orbite (Le centre et la vitesse)")]
    public Transform centreOrbite; 
    public float vitesseOrbite = 10f;

    void Update()
    {
        // 1. Fait tourner la planète sur elle-même (Axe Y)
        transform.Rotate(Vector3.up * vitesseRotation * Time.deltaTime);

        // 2. Fait tourner la planète autour de son centre (Le Soleil, ou la Terre pour la Lune)
        if (centreOrbite != null)
        {
            transform.RotateAround(centreOrbite.position, Vector3.up, vitesseOrbite * Time.deltaTime);
        }
    }
}