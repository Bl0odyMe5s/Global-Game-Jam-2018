using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For camera-on-rail movement.
/// </summary>
[ExecuteInEditMode] //Turn on to update points
public class CameraRailScript : MonoBehaviour
{
    enum RailMode { SingleRun, Loop, BackAndForth }
    RailMode railMode = RailMode.SingleRun;
    enum InterPolationMode { Linear, Hermite } //Hermite interpolation needs 2 extra points on each end of the rail.
    InterPolationMode interpolationMode = InterPolationMode.Hermite;

    bool interpolateTime = true;

    CameraRailPointScript[] points;
    int index = 0;
    public static Color railColour = Color.blue;
    public static Color railDirectionColour = Color.red;
    public Camera cam;
    private IEnumerator railClock;
    private bool direction = true;

    private float distanceFactor;
    private float distanceToNeighbour;

    void Start () {
        points = new CameraRailPointScript[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            points[i] = transform.GetChild(i).GetComponent<CameraRailPointScript>();
            if (i > 0)
            {

                points[i - 1].nextNeighbourPosition = points[i].transform.position;
                points[i - 1].GetComponent<CameraRailPointScript>().distanceToNextNeighbour = (points[i - 1].transform.position - points[i].transform.position).magnitude;
                points[i - 1].GetComponent<CameraRailPointScript>().speedInNode = points[i - 1].GetComponent<CameraRailPointScript>().distanceToNextNeighbour / points[i - 1].GetComponent<CameraRailPointScript>().timeToNeighbour;

            }
        }
        if (Application.isPlaying)
        {
            StartRail();
        }
        index = 1;
	}
	
    
    /// <summary>
    /// Start the camera movement along the rail.
    /// </summary>
    public void StartRail()
    {
        distanceToNeighbour = points[index].distanceToNextNeighbour;
        if (interpolateTime)
        {
            railClock = CamPointInterpolation(points[index].distanceToNextNeighbour, 0);
        }
        else
        {
            railClock = CamPointInterpolation(points[index].timeToNeighbour, 0);
        }
        StartCoroutine(railClock);
    }

    /// <summary>
    /// Stop the camera movement.
    /// </summary>
    public void StopRail()
    {
        if (railClock != null) {
            StopCoroutine(railClock);
        }
    } 

    /// <summary>
    /// The technical part oF the camera movement along the rail
    /// </summary>
    /// <param name="time">How long it should Take to move to the next point.</param>
    /// <param name="timePassed">How much time has passed yet.</param>
    /// <returns></returns>
    private IEnumerator CamPointInterpolation(float time, float timePassed, float distancePassed = 0)
    {
        yield return new WaitForEndOfFrame();

        timePassed += Time.deltaTime;
        float factor;

        if (!interpolateTime)
        {
            factor = 1.0f / time * timePassed;
        }
        else
        {
            float speedAtPosition = 0;

            factor = 1.0f / distanceToNeighbour * distancePassed;
            //speedAtPosition = Mathf.Lerp(points[index].speedInNode, points[index + 1].speedInNode, factor);
            if (interpolationMode == InterPolationMode.Linear)
            {
                speedAtPosition = Mathf.Lerp(points[index].speedInNode, points[index + 1].speedInNode, factor);
            }
            else if (interpolationMode == InterPolationMode.Hermite)
            {
                if (index > 0 && index < transform.childCount - 2)
                {
                    speedAtPosition = HermiteInterpolate(points[index - 1].speedInNode, points[index].speedInNode, points[index + 1].speedInNode, points[index + 2].speedInNode, factor, 0, 0);
                }
               // print(speedAtPosition + "  " + points[index].speedInNode + "  " + points[index + 1].speedInNode +  "  " + distanceToNeighbour + "   " + distancePassed + "  " +  cam.transform.position);
            }
            distancePassed += speedAtPosition * Time.deltaTime;
            factor = 1.0f / distanceToNeighbour * distancePassed;
            //print(factor);

        }
        if (interpolationMode == InterPolationMode.Linear)
        {
            if ((index < transform.childCount - 1 && direction) || (index > 0 && !direction)) //Only interpolate if not at the end point.
            {
                if (direction) //If going forward
                {
                    cam.transform.position = points[index].transform.position + (points[index + 1].transform.position - points[index].transform.position) * factor;
                    cam.transform.rotation = Quaternion.Slerp(points[index].transform.rotation, points[index + 1].transform.rotation, factor);
                    cam.fieldOfView = points[index].fov + (points[index + 1].fov - points[index].fov) * factor;
                }
                else //If going backwards.
                {
                    cam.transform.position = points[index].transform.position + (points[index - 1].transform.position - points[index].transform.position) * factor;
                    cam.transform.rotation = Quaternion.Slerp(points[index].transform.rotation, points[index - 1].transform.rotation, factor);
                    cam.fieldOfView = points[index].fov + (points[index - 1].fov - points[index].fov) * factor;
                }
            }
        }
        else if (interpolationMode == InterPolationMode.Hermite) 
        {
            if ((index < transform.childCount - 2 && direction && index > 0) || (index > 1 && !direction && index < transform.childCount - 1))//Only interpolate if not at the end point.
            {
                if (direction) //If going forward
                {
                    cam.transform.position = HermiteInterpolate(points[index - 1].transform.position, points[index].transform.position, points[index + 1].transform.position, points[index + 2].transform.position,
                        factor, 0, 0);
                    cam.transform.rotation = HermiteInterpolate(points[index - 1].transform.rotation, points[index].transform.rotation, points[index + 1].transform.rotation, points[index + 2].transform.rotation,
                         factor, 0, 0);

                    cam.fieldOfView = HermiteInterpolate(points[index - 1].fov, points[index].fov, points[index + 1].fov, points[index + 2].fov, factor, 0, 0);
                    //Also interpolate time in this case.
                    time = HermiteInterpolate(points[index - 1].timeToNeighbour, points[index].timeToNeighbour, points[index + 1].timeToNeighbour, points[index + 2].timeToNeighbour, factor, 0, 0);
                }
                else //If going backwards
                {
                    cam.transform.position = HermiteInterpolate(points[index + 1].transform.position, points[index].transform.position, points[index - 1].transform.position, points[index - 2].transform.position,
                        factor, 0, 0);
                    cam.transform.rotation = HermiteInterpolate(points[index + 1].transform.rotation, points[index].transform.rotation, points[index - 1].transform.rotation, points[index - 2].transform.rotation,
                         factor, 0, 0);

                    cam.fieldOfView = HermiteInterpolate(points[index + 1].fov, points[index].fov, points[index - 1].fov, points[index - 2].fov, factor, 0, 0);
                    //Also interpolate time in this case.
                    time = HermiteInterpolate(points[index + 1].timeToNeighbour, points[index].timeToNeighbour, points[index - 1].timeToNeighbour, points[index - 2].timeToNeighbour, factor, 0, 0);
                }
            }
        }
        //Aas long as there's still time left keep on the same node, and interpolate further
        if ((timePassed < time && !interpolateTime) || (distancePassed < distanceToNeighbour && interpolateTime))
        {
            railClock = CamPointInterpolation(time, timePassed, distancePassed);
            StartCoroutine(railClock);
        }
        else //If arrived at the next node
        {
            timePassed = 0; //Reset the time passed to potentially start interpolating all over again with the next next node.
            distancePassed = 0;
            if ((((index + 1 < transform.childCount && direction) || (index - 1 > 0 && !direction)) && interpolationMode == InterPolationMode.Linear) ||
                (((index + 2 < transform.childCount && direction) || (index - 2 > 0 && !direction)) && interpolationMode == InterPolationMode.Hermite))
            {//As long as there are enough nodes left to interpolate with.
                if (direction)
                {
                    index++;
                }
                else
                {
                    index--;
                }
                distanceToNeighbour = points[index].distanceToNextNeighbour;
                time = points[index].timeToNeighbour;
                railClock = CamPointInterpolation(time, timePassed, distancePassed);
                StartCoroutine(railClock);
            }
            else //If there are not enough nodes left to interpolate with.
            {
                switch (railMode)
                {
                    case RailMode.BackAndForth: //Turn around
                        direction = !direction;
                        railClock = CamPointInterpolation(time, timePassed, distancePassed);
                        StartCoroutine(railClock);
                        break;
                    case RailMode.Loop: //Start back at the start.
                        if (interpolationMode == InterPolationMode.Linear)
                        {
                            index = 0;
                        }
                        else
                        {
                            index = 1;
                        }
                        railClock = CamPointInterpolation(time, timePassed, distancePassed);
                        StartCoroutine(railClock);
                        break;
                }
            }
        }
    }


    

    /// <summary>
    /// Interpolate smoothly
    /// </summary>
    /// <param name="y0">Coordinate 1</param>
    /// <param name="y1">Coordinate 2</param>
    /// <param name="y2">Coordinate 3</param>
    /// <param name="y3">Coordinate 4</param>
    /// <param name="mu">How far to interpolate</param>
    /// <param name="tension">1 is high, 0 normal, -1 is low</param>
    /// <param name="bias">0 is even, positive is towards first segment, negative towards the other</param>
    /// <returns>Interpolation betwen the middle two points</returns>
    float HermiteInterpolate(float y0, float y1, float y2, float y3, float mu, float tension, float bias)
    {
        mu = Mathf.Clamp(mu, 0, 1);
        float m0, m1, mu2, mu3;
        float a0, a1, a2, a3;
        tension = Mathf.Clamp(tension, -1, 1);

        mu2 = mu * mu;
        mu3 = mu2 * mu;
        m0 = (y1 - y0) * (1 + bias) * (1 - tension) / 2;
        m0 += (y2 - y1) * (1 - bias) * (1 - tension) / 2;
        m1 = (y2 - y1) * (1 + bias) * (1 - tension) / 2;
        m1 += (y3 - y2) * (1 - bias) * (1 - tension) / 2;
        a0 = 2 * mu3 - 3 * mu2 + 1;
        a1 = mu3 - 2 * mu2 + mu;
        a2 = mu3 - mu2;
        a3 = -2 * mu3 + 3 * mu2;

        return (a0 * y1 + a1 * m0 + a2 * m1 + a3 * y2);
    }


    /// <summary>
    /// Interpolate smoothly
    /// </summary>
    /// <param name="y0">Coordinate 1</param>
    /// <param name="y1">Coordinate 2</param>
    /// <param name="y2">Coordinate 3</param>
    /// <param name="y3">Coordinate 4</param>
    /// <param name="mu">How far to interpolate</param>
    /// <param name="tension">1 is high, 0 normal, -1 is low</param>
    /// <param name="bias">0 is even, positive is towards first segment, negative towards the other</param>
    /// <returns>Interpolation betwen the middle two points</returns>
    Vector3 HermiteInterpolate(Vector3 y0, Vector3 y1, Vector3 y2, Vector3 y3, float mu, float tension, float bias)
    {
        float x, y, z;
        x = HermiteInterpolate(y0.x, y1.x, y2.x, y3.x, mu, tension, bias);
        y = HermiteInterpolate(y0.y, y1.y, y2.y, y3.y, mu, tension, bias);
        z = HermiteInterpolate(y0.z, y1.z, y2.z, y3.z, mu, tension, bias);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Interpolate smoothly
    /// </summary>
    /// <param name="y0">Coordinate 1</param>
    /// <param name="y1">Coordinate 2</param>
    /// <param name="y2">Coordinate 3</param>
    /// <param name="y3">Coordinate 4</param>
    /// <param name="mu">How far to interpolate</param>
    /// <param name="tension">1 is high, 0 normal, -1 is low</param>
    /// <param name="bias">0 is even, positive is towards first segment, negative towards the other</param>
    /// <returns>Interpolation betwen the middle two points</returns>
    Quaternion HermiteInterpolate(Quaternion y0, Quaternion y1, Quaternion y2, Quaternion y3, float mu, float tension, float bias)
    {
        float x, y, z, w;
        x = HermiteInterpolate(y0.x, y1.x, y2.x, y3.x, mu, tension, bias);
        y = HermiteInterpolate(y0.y, y1.y, y2.y, y3.y, mu, tension, bias);
        z = HermiteInterpolate(y0.z, y1.z, y2.z, y3.z, mu, tension, bias);
        w = HermiteInterpolate(y0.w, y1.w, y2.w, y3.w, mu, tension, bias);
        return new Quaternion(x, y, z, w);
    }

}
