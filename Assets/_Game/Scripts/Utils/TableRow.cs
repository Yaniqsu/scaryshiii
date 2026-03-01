using System;

namespace YNQ.Utils
{
    [Serializable]
    public struct TableRow<T1,T2>
    {
        public T1 item1;
        public T2 item2;
    }
}