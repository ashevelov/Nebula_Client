using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorPanel : MonoBehaviour {

	public Sprite[] tutorSpritesRu;
	public Sprite[] tutorSpritesEn;
	public Image image;

	void Start()
	{
		NextImage();
	}

	private int index =-1;
	public void NextImage()
	{
		Sprite[] sprites = (Application.systemLanguage == SystemLanguage.Russian) ? tutorSpritesRu : tutorSpritesEn;
		index = (index+1 < sprites.Length) ? index+1 : 0;
		image.sprite = sprites[index];
	}
}
