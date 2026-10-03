using UnityEngine;

public class PlayerCameraBootstrap : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    private void Awake()
    {
        GameObject[] mainCameraObjects =
            GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject cameraObject in mainCameraObjects)
        {
            Camera cameraComponent =
                cameraObject.GetComponent<Camera>();

            if (cameraComponent == null)
                continue;

            // Don't deactivate our own Player Camera
            if (cameraComponent == playerCamera)
                continue;

            cameraComponent.enabled = false;

            AudioListener audioListener =
                cameraObject.GetComponent<AudioListener>();

            if (audioListener != null)
                audioListener.enabled = false;
        }

        // Manually activate our own Camera
        playerCamera.enabled = true;
        playerCamera.gameObject.tag = "MainCamera";

        AudioListener playerAudioListener =
            playerCamera.GetComponent<AudioListener>();

        if (playerAudioListener != null)
            playerAudioListener.enabled = true;
    }
}