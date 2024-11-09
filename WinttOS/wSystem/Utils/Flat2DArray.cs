using System.Collections.Generic;

namespace WinttOS.wSystem.Utils
{
    public class Flat2DArrayInt32
    {
        private List<int> _list;

        private int _maxX;
        private int _maxY;

        public Flat2DArrayInt32(int size1, int size2)
        {
            _list = new List<int>();
            _maxX = size1;
            _maxY = size2;

            for(int i = 0; i < _maxX * _maxY; i++)
            {
                _list.Add(0);
            }
        }

        private int TwoD2One(int x, int y)
        {
            return y * _maxX + x;
        }

        public int this[int idx, int idx1]
        {
            get
            {
                return _list[TwoD2One(idx, idx1)];
            }

            set
            {
                _list[TwoD2One(idx, idx1)] = value;
            }
        }
    }
}
