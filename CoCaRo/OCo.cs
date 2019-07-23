using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCaRo
{
    class OCo
    {
        public const int _chieuRong = 25;
        public const int _chieuCao = 25;

        private Point _vitri;
        private int _sohuu; // 0 khong so huu cua ai, 1 ng choi 1, 2 ng choi 2
        private int _dong;
        private int _cot;
        public int Dong
        {
            get
            {
                return _dong;
            }

            set
            {
                _dong = value;
            }
        }
        
        public int Cot
        {
            get
            {
                return _cot;
            }

            set
            {
                _cot = value;
            }
        }

        public Point Vitri
        {
            get
            {
                return _vitri;
            }

            set
            {
                _vitri = value;
            }
        }

        public int Sohuu
        {
            get
            {
                return _sohuu;
            }

            set
            {
                _sohuu = value;
            }
        }
        public OCo()
        {

        }
        public OCo(int dong, int cot, Point vitri, int sohuu)
        {
            _dong = dong;
            _cot = cot;
            _vitri = vitri;
            _sohuu = sohuu;
        }
    }
}
