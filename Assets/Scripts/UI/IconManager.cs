using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites;
    [SerializeField]
    Sprite defaultImage;
    [SerializeField]
    Image profileImage;
    public static IconManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        string id=PlayerPrefs.GetString("profile_image");
        if(id != null && id!="" )
        {
            int _id=int.Parse(id);
            profileImage.sprite = sprites[_id];
        }
    
    }
    public void SetImage(int id)
    {
        PlayerPrefs.SetString("profile_image",id.ToString());
        profileImage.sprite = sprites[id];
    }

}
