using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class TrackImage : MonoBehaviour
{
    public ARTrackedImageManager manager;
    public List<GameObject> objList = new List<GameObject>();                   //list type = GameObject
    Dictionary<string, GameObject> prefDic = new Dictionary<string, GameObject>();  //키, 벨류 으로 구성된 딕셔너리

    private List<ARTrackedImage> _trackedImg = new List<ARTrackedImage>();
    private List<float> __tractedTimer = new List<float>();

    void Awake()
    {
        foreach (GameObject obj in objList)
        {
            string name = obj.name;                                         //키값 = 이름
            prefDic.Add(name, obj);                                         //이름 , 실제 오브젝트
        }
    }

    private void OnEnable()
    {
        manager.trackedImagesChanged += ImageChanged;
    }
  
    private void OnDisable()
    {
        manager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(ARTrackedImage img in args.added)
        {
            UpdateImage(img);
        }

        foreach(ARTrackedImage img in args.updated)
        {
            UpdateImage(img);
        }

    }
    
    private void UpdateImage(ARTrackedImage img)
    {
        string name = img.referenceImage.name;
        GameObject obj = prefDic[name];
        obj.transform.position = img.transform.position;            //프리펩의 위치/회전값을 인식한 이미지의 위치 회전값에 맞추기
        obj.transform.rotation = img.transform.rotation;
        obj.SetActive(true);                                        //프리펩을 활성화시킴.
        
    }

  public float rotationSpeed = 50f;

    void Update()
    {
        
        transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
    
    }
}
