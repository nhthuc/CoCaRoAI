using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCaRo
{
    class BanCo
    {
        private int _sodong;
        private int _socot;

        public int Socot
        {
            get
            {
                return _socot;
            }

            set
            {
                _socot = value;
            }
        }

        public int Sodong
        {
            get
            {
                return _sodong;
            }

            set
            {
                _sodong = value;
            }
        }

        public BanCo()
        {
            Sodong = 0;
            Socot = 0;
        }
        public BanCo(int soDong, int soCot)
        {
            this.Sodong = soDong;
            this.Socot = soCot; 
        }
        public void VeBanCo(Graphics g)
        {
            for (int i = 0; i <= Socot; i++)
            {
                g.DrawLine(CaroChess.pen, i * OCo._chieuRong, 0, i * OCo._chieuRong, Sodong * OCo._chieuCao);
            }
            for (int i = 0; i <= Sodong; i++)
            {
                g.DrawLine(CaroChess.pen, 0, i * OCo._chieuCao, Socot * OCo._chieuRong, i * OCo._chieuCao);  
            }
        }

        public void VeQuanCo(Graphics g, Point point, SolidBrush sb)
        {
            g.FillEllipse(sb, point.X + 2, point.Y + 2, OCo._chieuRong - 4, OCo._chieuCao - 4);
        }
        public void XoaQuanCo(Graphics g, Point point, SolidBrush sb)
        {
            g.FillRectangle(sb, point.X + 2, point.Y + 2, OCo._chieuRong - 4, OCo._chieuCao - 4);
        }
    }
}
