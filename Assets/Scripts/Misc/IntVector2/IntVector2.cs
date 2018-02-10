using System;

namespace Extension.ExtraTypes
{
    [Serializable]
    public struct IntVector2
    {
        public int x;
        public int y;

        public IntVector2(int xCoordinate, int yCoordinate)
        {
            x = xCoordinate;
            y = yCoordinate;
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(this, obj))
            {
                return true;
            }
            else if(obj is IntVector2)
            {
                IntVector2 target = (IntVector2)obj;
                return this.x == target.x && this.y == target.y;
            }
            else
            {
                return false;
            }
        }

        public static bool operator == (IntVector2 intVector2A, IntVector2 intVector2B)
        {
            if (IsNull(intVector2A) && !IsNull(intVector2B))
                return false;

            if (!IsNull(intVector2A) && IsNull(intVector2B))
                return false;

            if (IsNull(intVector2A) && IsNull(intVector2B))
                return true;

            return intVector2A.x.Equals(intVector2B.x) && intVector2A.y.Equals(intVector2B.y);
        }

        public static bool operator != (IntVector2 intVector2A, IntVector2 intVector2B)
        {
            return !(intVector2A == intVector2B);
        }

        public override int GetHashCode()
        {
            int SEED = 26;
            return (x.GetHashCode() + y.GetHashCode()) * SEED;
        }

        public override string ToString()
        {
            return string.Format("[{0} : {1}]", x, y);
        }

        private static bool IsNull(object obj)
        {
            return ReferenceEquals(obj, null);
        }
    }
}