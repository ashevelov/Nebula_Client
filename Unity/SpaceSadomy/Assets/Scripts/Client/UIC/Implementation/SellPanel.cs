using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UIC.Implementation
{
    public class SellPanel : MonoBehaviour, ISellPanel {

        public InputField priceText;
        public int GetPrice()
        {
            string p ="";
            foreach(char c in priceText.text)
            {
                if (char.IsNumber(c))
                {
                    p += c;
                }
            }
            int prc = int.Parse(p);
            priceText.text = prc.ToString();
            return prc;
        }
        public void SetStartPrice(int price)
        {
            priceText.text = price.ToString();
        }

        public Text description;
        public void SetDescrption(string desc)
        {
            description.text = desc;
        }


        public Image icon;
        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }
    }
}

