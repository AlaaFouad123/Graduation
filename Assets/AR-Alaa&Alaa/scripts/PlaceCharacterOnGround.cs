using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaceRobotOnGround : MonoBehaviour
{
    public GameObject robotPrefab;
    private ARPlaneManager arPlaneManager;
    private bool isRobotPlaced = false;


    void Start()
    {
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
    }

    void Update()
    {
        if (!isRobotPlaced && arPlaneManager.trackables.count > 0)
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                PlaceRobot(plane.transform.position);
                break; // Place the robot on the first detected plane
            }
        }
    }

    void PlaceRobot(Vector3 position)
    {
        // Get the camera's position and forward direction
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        // Calculate the direction from the camera to the placement position
        Vector3 directionToCamera = (cameraPosition - position).normalized;

        // Ensure the robot faces the camera but remains upright
        Quaternion rotation = Quaternion.LookRotation(new Vector3(directionToCamera.x, 0, directionToCamera.z));

        // Instantiate the robot at the plane's position and make it face the camera
        GameObject robot = Instantiate(robotPrefab, position, rotation);
        isRobotPlaced = true;

        // Optionally, disable plane visualization after placing the robot
        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        arPlaneManager.enabled = false; // Disable plane detection
    }


    //fixed
    //void PlaceRobot(Vector3 position)
    //{
    //    // Get the camera's position and forward direction
    //    Vector3 cameraPosition = Camera.main.transform.position;
    //    Vector3 cameraForward = Camera.main.transform.forward;

    //    // Calculate the direction from the camera to the placement position
    //    Vector3 directionToCamera = (cameraPosition - position).normalized;

    //    // Ensure the robot faces the camera but remains upright
    //    Quaternion rotation = Quaternion.LookRotation(new Vector3(directionToCamera.x, 0, directionToCamera.z));

    //    // Calculate the right direction relative to the camera
    //    Vector3 rightDirection = Vector3.Cross(Vector3.up, directionToCamera).normalized;

    //    // Offset the robot's position to the right
    //    float offsetDistance = 0.5f; // Adjust this value to set the distance to the right
    //    Vector3 newPosition = position + (rightDirection * offsetDistance);

    //    // Instantiate the robot at the new position
    //    GameObject robot = Instantiate(robotPrefab, newPosition, rotation);
    //    isRobotPlaced = true;

    //    // Optionally, disable plane visualization after placing the robot
    //    foreach (var plane in arPlaneManager.trackables)
    //    {
    //        plane.gameObject.SetActive(false);
    //    }
    //    arPlaneManager.enabled = false; // Disable plane detection
    //}


}