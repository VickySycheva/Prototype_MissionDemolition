using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public GameObject cloudSphere;
    public int numSphereMin = 6;
    public int numSphereMax = 10;
    public Vector3 sphereOffsetScale = new Vector3(5,2,1); //Максимальное расстояние CloudSphere 
                                                            // от центра Cloud вдоль каждой оси
    public Vector2 sphereScaleRangeX = new Vector2(4,8); //Диапазон масштаба для каждой оси
    public Vector2 sphereScaleRangeY = new Vector2(3,4);
    public Vector2 sphereScaleRangeZ = new Vector2(2,4);
    public float scaleYMin = 2f;
    private List<GameObject> spheres;

    // Start is called before the first frame update
    void Start()
    {
        spheres = new List<GameObject>();

        int num = Random.Range(numSphereMin, numSphereMax);
        for (int i = 0; i < num; i++)
        {
            GameObject sp = Instantiate<GameObject>(cloudSphere);
            spheres.Add(sp);
            Transform spTrans = sp.transform;
            spTrans.SetParent(this.transform);

            //Выбрать случайное местоположение
            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            spTrans.localPosition = offset;

            //Выбрвть случайный масштаб
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            //Скорректировать масштаб у по расстоянию х от центра
            scale.y *= 1 - (Mathf.Abs(offset.x)/sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);
            
            spTrans.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     Restart();
        // }
    }

    private void Restart()
    {
        foreach (GameObject sp in spheres)
        {
            Destroy(sp);
        }
        Start();
    }
}
