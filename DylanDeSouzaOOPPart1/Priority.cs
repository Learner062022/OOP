using System;

namespace DylanDeSouzaOOPPart1
{
    public struct Priority
    {
        int value;

        public int Value
        {
            get
            {
                return value;
            }
        }

        public Priority(int v)
        {
            value = Math.Clamp(v, -1, 1);
        }

        public static Priority operator ++(Priority p)
        {
            if (p.value < 1)
            {
                p.value++;
            }
                
            return p;
        }

        public static Priority operator --(Priority p)
        {
            if (p.value > -1)
            {
                p.value--;
            }
                
            return p;
        }
    }
}