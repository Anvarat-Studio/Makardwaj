using System;
using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.UI
{
    public class NumberHandler : MonoBehaviour
    {
        public GenericDictionary<int, Sprite> m_numberList;

        public Stack<Sprite> GetNumberImages(int number)
        {
            Stack<Sprite> numberImages = new Stack<Sprite>();

            do
            {
                var digit = number % 10;
                number /= 10;
                numberImages.Push(m_numberList[digit]);
            } while (number > 0);

            return numberImages;
        }

        public Sprite GetNumberImage(int number)
        {
            return m_numberList[number];
        }
    }
}
