using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;

public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button button;
    public float inputThreshold = 0.1f;
    public Transform movementSource;

    private bool isMoving = false;
    private List<Vector3> positionList = new List<Vector3>();

    public GameObject debugCubePrefab;
    public bool creationMode = true;

    public float recognitionThreshold;
    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string>{}

    public UnityStringEvent onRecognize;

    private List<Gesture> trainingSet = new List<Gesture>();
    public string newGestureName;
    public float newPositionThresholdDistance = 0.05f;
    
    // Start is called before the first frame update
    void Start()
    {
        string[] gestureNamesFromFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (var item in gestureNamesFromFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), button, out bool isPressed, inputThreshold);
        //Start Movement
        if (!isMoving && isPressed)
        {
            StartMovement();
        }
        //End Movement
        else if (isMoving && !isPressed)
        {
            EndMovement();
        }
        //Update Movement
        else if (isMoving && isPressed)
        {
            UpdateMovement();
        }

        void StartMovement()
        {
            isMoving = true;
            positionList.Clear();
            var position = movementSource.position;
            positionList.Add(position);
            if (debugCubePrefab)
            {
                Destroy(Instantiate(debugCubePrefab, position, Quaternion.identity),2);
            }
        }

        void EndMovement()
        {
            isMoving = false;
            //Create the gesture from the pos list
            Point[] pointArray = new Point[positionList.Count];
            for (int i = 0; i < positionList.Count; i++)
            {
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
                pointArray[i] = new Point(screenPoint.x,screenPoint.y, 0);
            }
            Gesture newGesture = new Gesture(pointArray);
            if (creationMode)
            {
                newGesture.Name = newGestureName;
                trainingSet.Add(newGesture);

                string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
                GestureIO.WriteGesture(pointArray,newGestureName,fileName);
            }
            else
            {
                Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
                Debug.Log(result.GestureClass + ": " + result.Score);
                if (result.Score > recognitionThreshold)
                {
                    onRecognize?.Invoke(result.GestureClass);
                }
            }
        }

        void UpdateMovement()
        {
            Vector3 lastPosition = positionList[positionList.Count - 1];
            if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
            {
                positionList.Add(movementSource.position);
                if (debugCubePrefab)
                {
                    Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity),2);
                }
            }
          
        }
    }
}
