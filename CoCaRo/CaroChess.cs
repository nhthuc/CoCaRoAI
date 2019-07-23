using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoCaRo
{
    public enum KETTHUC
    {
        HoaCo,
        Player1,
        Player2,
        Player,
        Com
    }
    class CaroChess
    {
        public static Pen pen;
        public static SolidBrush sbWhite;
        public static SolidBrush sbBlack;
        public static SolidBrush sbMediumSlateBlue;
        private OCo[,] mangOCo;
        private BanCo _banCo;
        private Stack<OCo> stt_cacNuocDaDi;
        private Stack<OCo> stt_cacNuocUndo;
        private int _luotDi;
        private int _cheDoChoi;
        private bool _sanSang;
        private KETTHUC _ketThuc;

        public bool SanSang
        {
            get
            {
                return _sanSang;
            }

            set
            {
                _sanSang = value;
            }
        }

        public int CheDoChoi
        {
            get
            {
                return _cheDoChoi;
            }

            set
            {
                _cheDoChoi = value;
            }
        }

        public CaroChess()
        {
            pen = new Pen(Color.DarkBlue); // tạo đường kẻ 
            sbWhite = new SolidBrush(Color.White);
            sbBlack = new SolidBrush(Color.Black);
            sbMediumSlateBlue = new SolidBrush(Color.MediumSlateBlue);
            _banCo = new BanCo(20, 20);
            mangOCo = new OCo[_banCo.Sodong, _banCo.Socot];
            stt_cacNuocDaDi = new Stack<OCo>();
            stt_cacNuocUndo = new Stack<OCo>();
            _luotDi = 1;
        } 

        public void VeBanCo(Graphics g)
        {
            _banCo.VeBanCo(g);
        }

        public void KhoiTaoMangOCo()
        {
            for (int i = 0; i < _banCo.Sodong; i++)
            {
                for (int j = 0; j < _banCo.Socot; j++)
                {
                    mangOCo[i, j] = new OCo(i, j, new Point(j * OCo._chieuRong, i * OCo._chieuCao), 0);
                }
            }
        }
        public bool DanhCo(int mouseX, int mouseY, Graphics g)
        {
            if (mouseX % OCo._chieuRong == 0 || mouseY % OCo._chieuCao == 0)
            {
                return false;
            }
            int cot = mouseX / OCo._chieuRong;
            int dong = mouseY / OCo._chieuCao;

            //không cho đánh lại ô đã đánh
            if (mangOCo[dong, cot].Sohuu != 0)
            {
                return false;
            }
            //xét lượt đi 
            switch (_luotDi)
            {
                case 1:
                    mangOCo[dong, cot].Sohuu = 1;
                    _banCo.VeQuanCo(g, mangOCo[dong, cot].Vitri, sbBlack);
                    _luotDi = 2;
                    break;
                case 2:
                    mangOCo[dong, cot].Sohuu = 2;
                    _banCo.VeQuanCo(g, mangOCo[dong, cot].Vitri, sbWhite);
                    _luotDi = 1;
                    break;
                default:
                    MessageBox.Show("Lỗi chương trình");
                    break;
            }
            stt_cacNuocUndo = new Stack<OCo>(); // tạo mới vùng nhớ undo khi đánh vào ô đã undo
            //tạo mới vùng nhớ để khi tham chiếu k bị mấy giá trị
            OCo oco = new OCo(mangOCo[dong, cot].Dong, mangOCo[dong, cot].Cot, mangOCo[dong, cot].Vitri, mangOCo[dong, cot].Sohuu);
            stt_cacNuocDaDi.Push(oco);
            return true;
        }
        public void VeLaiQuanCo(Graphics g)
        {
            foreach (OCo oco in stt_cacNuocDaDi)
            {
                if (oco.Sohuu == 1)
                    _banCo.VeQuanCo(g, oco.Vitri, sbBlack);
                else if (oco.Sohuu == 2)
                    _banCo.VeQuanCo(g, oco.Vitri, sbWhite);
            }
        }
        public void StartPvsP(Graphics g)
        {
            _sanSang = true;
            stt_cacNuocDaDi = new Stack<OCo>();// xóa các ô cờ đã đánh
            stt_cacNuocUndo = new Stack<OCo>();
            _luotDi = 1;
            _cheDoChoi = 1;
            KhoiTaoMangOCo();
            VeBanCo(g);
        }
        public void StartPvsCom(Graphics g)
        {
            _sanSang = true;
            stt_cacNuocDaDi = new Stack<OCo>();// xóa các ô cờ đã đánh
            stt_cacNuocUndo = new Stack<OCo>();
            _luotDi = 1;
            _cheDoChoi = 2;
            KhoiTaoMangOCo();
            VeBanCo(g);
            KhoiDongComputer(g);
        }
        #region Undo Redo
        public void Undo(Graphics g)
        {
            if (stt_cacNuocDaDi.Count != 0)
            {
                OCo oco = stt_cacNuocDaDi.Pop();
                stt_cacNuocUndo.Push(new OCo(oco.Dong, oco.Cot, oco.Vitri, oco.Sohuu));
                mangOCo[oco.Dong, oco.Cot].Sohuu = 0;
                _banCo.XoaQuanCo(g, oco.Vitri, sbMediumSlateBlue);
                if (_luotDi == 1)
                {
                    _luotDi = 2;
                }
                else
                    _luotDi = 1;
            }
            
            //VeBanCo(g);
            //VeLaiQuanCo(g);
        }
        public void Redo(Graphics g)
        {
            if (stt_cacNuocUndo.Count != 0)
            {
                OCo oco = stt_cacNuocUndo.Pop();
                stt_cacNuocDaDi.Push(new OCo(oco.Dong, oco.Cot, oco.Vitri, oco.Sohuu));
                mangOCo[oco.Dong, oco.Cot].Sohuu = oco.Sohuu;
                _banCo.VeQuanCo(g, oco.Vitri, oco.Sohuu == 1 ? sbBlack : sbWhite);
                if (_luotDi == 1)
                {
                    _luotDi = 2;
                }
                else
                    _luotDi = 1;
            }
        }
        #endregion
        #region Duyet thang thua
        public void KetThucTroChoi()
        {
            switch (_ketThuc)
            {
                case KETTHUC.HoaCo:
                    MessageBox.Show("Hòa Cờ");
                    break;
                case KETTHUC.Player1:
                    MessageBox.Show("Người chơi 1 thắng !!!", "Chúc mừng", MessageBoxButtons.OK);
                    break;
                case KETTHUC.Player2:
                    MessageBox.Show("Người chơi 2 thắng !!!", "Chúc mừng", MessageBoxButtons.OK);
                    break;
                case KETTHUC.Player:
                    MessageBox.Show("Bạn là người chiến thắng !!!","Chúc mừng", MessageBoxButtons.OK);
                    break;
                case KETTHUC.Com:
                    MessageBox.Show("Computer đã chiến thắng !!!","Chúc mừng",MessageBoxButtons.OK);
                    break;
                default:
                    break;
            }
            _sanSang = false;
        }
        public bool KiemTraChienThang()
        {
            if (stt_cacNuocDaDi.Count == _banCo.Sodong * _banCo.Socot)
            {
                _ketThuc = KETTHUC.HoaCo;
                return true;
            }
            foreach (OCo oco in stt_cacNuocDaDi)
            {
                if (DuyetDoc(oco.Dong,oco.Cot,oco.Sohuu) || DuyetNgang(oco.Dong, oco.Cot, oco.Sohuu) || DuyetCheoXuoi(oco.Dong, oco.Cot, oco.Sohuu) || DuyetCheoNguoc(oco.Dong, oco.Cot, oco.Sohuu))
                {
                    if (_cheDoChoi == 1)
                    {
                        _ketThuc = oco.Sohuu == 1 ? KETTHUC.Player1 : KETTHUC.Player2;
                    }
                    else
                        _ketThuc = oco.Sohuu == 1 ? KETTHUC.Com : KETTHUC.Player;
                    return true;
                }
            }
            return false;
        }
        public bool DuyetDoc(int currDong, int currCot, int soHuu)
        {
            if (currDong > _banCo.Sodong - 5)
            {
                return false;
            }
            int dem;
            for (dem = 1; dem < 5; dem++)
            {
                if (mangOCo[currDong + dem, currCot].Sohuu != soHuu)
                    return false;
            }
            if (currDong == 0 || currDong + dem == _banCo.Sodong)
            {
                return true;
            }
            if (mangOCo[currDong - 1,currCot].Sohuu == 0 || mangOCo[currDong + dem, currCot].Sohuu == 0)
            {
                return true;
            }
            return false;
        }
        public bool DuyetNgang(int currDong, int currCot, int soHuu)
        {
            if (currCot > _banCo.Socot - 5)
            {
                return false;
            }
            int dem;
            for (dem = 1; dem < 5; dem++)
            {
                if (mangOCo[currDong, currCot + dem].Sohuu != soHuu)
                    return false;
            }
            if (currCot == 0 || currCot + dem == _banCo.Socot)
            {
                return true;
            }
            if (mangOCo[currDong, currCot - 1].Sohuu == 0 || mangOCo[currDong , currCot + dem].Sohuu == 0)
            {
                return true;
            }
            return false;
        }
        public bool DuyetCheoXuoi(int currDong, int currCot, int soHuu)
        {
            if (currDong > _banCo.Sodong - 5 || currCot > _banCo.Socot - 5)
            {
                return false;
            }
            int dem;
            for (dem = 1; dem < 5; dem++)
            {
                if (mangOCo[currDong + dem, currCot + dem].Sohuu != soHuu)
                    return false;
            }
            if (currDong == 0 || currDong + dem == _banCo.Sodong || currCot == 0 || currCot + dem == _banCo.Socot)
            {
                return true;
            }
            if (mangOCo[currDong - 1, currCot - 1].Sohuu == 0 || mangOCo[currDong + dem, currCot + dem].Sohuu == 0)
            {
                return true;
            }
            return false;
        }
        public bool DuyetCheoNguoc(int currDong, int currCot, int soHuu)
        {
            if (currDong < 4 || currCot > _banCo.Socot - 5)
            {
                return false;
            }
            int dem;
            for (dem = 1; dem < 5; dem++)
            {
                if (mangOCo[currDong - dem, currCot + dem].Sohuu != soHuu)
                    return false;
            }
            if (currDong == 4 || currDong == _banCo.Sodong - 1 || currCot == 0 || currCot + dem == _banCo.Socot)
            {
                return true;
            }
            if (mangOCo[currDong + 1, currCot - 1].Sohuu == 0 || mangOCo[currDong - dem, currCot + dem].Sohuu == 0)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region AI 
        private long[] mangDiemTanCong = new long[7] { 0, 3, 24, 192, 1536, 12288, 98304 };
        private long[] mangDiemPhongNgu = new long[7] { 0, 1, 9, 81, 729, 6561, 59049 };
        public void KhoiDongComputer(Graphics g)
        {
            if (stt_cacNuocDaDi.Count == 0)
            {
                DanhCo( _banCo.Socot / 2 * OCo._chieuRong + 1, _banCo.Sodong / 2 * OCo._chieuCao + 1, g);
            }
            else
            {
                OCo oco = TimKiemNuocDi();
                DanhCo(oco.Vitri.X + 1, oco.Vitri.Y + 1, g);
            }
        }
        private OCo TimKiemNuocDi()
        {
            OCo oCoResult = new OCo();
            long diemMax = 0;
            for (int i = 0; i < _banCo.Sodong; i++)
            {
                for (int j = 0; j < _banCo.Socot; j++)
                {
                    if (mangOCo[i,j].Sohuu == 0)
                    {
                        long diemTanCong = DiemTC_DuyetDoc(i, j) + DiemTC_DuyetNgang(i, j) + DiemTC_DuyetCheoXuoi(i, j) + DiemTC_DuyetCheoNguoc(i, j);
                        long diemPhongNgu = DiemPN_DuyetDoc(i, j) + DiemPN_DuyetNgang(i, j) + DiemPN_DuyetCheoXuoi(i, j) + DiemPN_DuyetCheoNguoc(i, j);
                        long diemTam = diemTanCong > diemPhongNgu ? diemTanCong : diemPhongNgu;
                        if (diemMax < diemTam)
                        {
                            diemMax = diemTam;
                            oCoResult = new OCo(mangOCo[i, j].Dong, mangOCo[i, j].Cot, mangOCo[i, j].Vitri, mangOCo[i, j].Sohuu);
                        }
                    }
                }
            }

            return oCoResult;
        }

        
        #region Tấn công
        private long DiemTC_DuyetDoc(int currDong, int currCot)
        {
            long diemTong = 0;
            int soQuanTa = 0;
            int soQuanDich = 0;
            //duyệt từ trên xuống
            for (int dem = 1; dem < 6 && currDong + dem < _banCo.Sodong; dem++)
            {
                if (mangOCo[currDong + dem, currCot].Sohuu == 1)
                {
                    soQuanTa++;
                }
                else if (mangOCo[currDong + dem, currCot].Sohuu == 2)
                {
                    soQuanDich++;
                    break;
                }
                else
                    break;
            }
            //Duyệt từ dưới lên
            for (int dem = 1; dem < 6 && currDong - dem >= 0; dem++)
            {
                if (mangOCo[currDong - dem, currCot].Sohuu == 1)
                {
                    soQuanTa++;
                }
                else if (mangOCo[currDong - dem, currCot].Sohuu == 2)
                {
                    soQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (soQuanDich ==2)
            {
                return 0;
            }
            diemTong -= mangDiemPhongNgu[soQuanDich + 1];
            diemTong += mangDiemTanCong[soQuanTa];
            return diemTong;
        }
        private long DiemTC_DuyetNgang(int currDong, int currCot)
        {
            long diemTong = 0;
            int soQuanTa = 0;
            int soQuanDich = 0;
            //duyệt trái qua phải
            for (int dem = 1; dem < 6 && currCot + dem < _banCo.Socot; dem++)
            {
                if (mangOCo[currDong, currCot + dem].Sohuu == 1)
                {
                    soQuanTa++;
                }
                else if (mangOCo[currDong, currCot + dem].Sohuu == 2)
                {
                    soQuanDich++;
                    break;
                }
                else
                    break;
            }
            //phải qua trái
            for (int dem = 1; dem < 6 && currCot - dem >= 0; dem++)
            {
                if (mangOCo[currDong, currCot - dem].Sohuu == 1)
                {
                    soQuanTa++;
                }
                else if (mangOCo[currDong, currCot - dem].Sohuu == 2)
                {
                    soQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (soQuanDich == 2)
            {
                return 0;
            }
            diemTong -= mangDiemPhongNgu[soQuanDich + 1];
            diemTong += mangDiemTanCong[soQuanTa];
            return diemTong;
        }
        private long DiemTC_DuyetCheoNguoc(int currDong, int currCot)
        {
            long diemTong = 0;
            int soQuanTa = 0;
            int soQuanDich = 0;
            //duyệt hướng trái dưới lên phải trên
            for (int dem = 1; dem < 6 && currCot + dem < _banCo.Socot && currDong - dem >= 0; dem++)
            {
                if (mangOCo[currDong - dem, currCot + dem].Sohuu == 1)
                {
                    soQuanTa++;
                }
                else if (mangOCo[currDong - dem, currCot + dem].Sohuu == 2)
                {
                    soQuanDich++;
                    break;
                }
                else
                    break;
            }
            //ngược lại
            for (int dem = 1; dem < 6 && currCot - dem >= 0 && currDong + dem < _banCo.Sodong; dem++)
            {
                if (mangOCo[currDong + dem, currCot - dem].Sohuu == 1)
                {
                    soQuanTa++;
                }
                else if (mangOCo[currDong + dem, currCot - dem].Sohuu == 2)
                {
                    soQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (soQuanDich == 2)
            {
                return 0;
            }
            diemTong -= mangDiemPhongNgu[soQuanDich + 1];
            diemTong += mangDiemTanCong[soQuanTa];
            return diemTong;
        }
        private long DiemTC_DuyetCheoXuoi(int currDong, int currCot)
        {
            long diemTong = 0;
            int soQuanTa = 0;
            int soQuanDich = 0;
            //duyệt hướng trái trên xuống phải dưới
            for (int dem = 1; dem < 6 && currCot + dem < _banCo.Socot && currDong + dem <_banCo.Sodong; dem++)
            {
                if (mangOCo[currDong + dem, currCot + dem].Sohuu == 1)
                {
                    soQuanTa++;
                }
                else if (mangOCo[currDong + dem, currCot + dem].Sohuu == 2)
                {
                    soQuanDich++;
                    break;
                }
                else
                    break;
            }
            //duyệt hướng ngược lại 
            for (int dem = 1; dem < 6 && currCot - dem >= 0 && currDong - dem >= 0; dem++)
            {
                if (mangOCo[currDong - dem, currCot - dem].Sohuu == 1)
                {
                    soQuanTa++;
                }
                else if (mangOCo[currDong - dem, currCot - dem].Sohuu == 2)
                {
                    soQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (soQuanDich == 2)
            {
                return 0;
            }
            diemTong -= mangDiemPhongNgu[soQuanDich + 1];
            diemTong += mangDiemTanCong[soQuanTa];
            return diemTong;
        }
        #endregion

        #region Phòng ngự
        private long DiemPN_DuyetDoc(int currDong, int currCot)
        {
            long diemTong = 0;
            int soQuanTa = 0;
            int soQuanDich = 0;
            //duyệt từ trên xuống
            for (int dem = 1; dem < 6 && currDong + dem < _banCo.Sodong; dem++)
            {
                if (mangOCo[currDong + dem, currCot].Sohuu == 1)
                {
                    soQuanTa++;
                    break;
                }
                else if (mangOCo[currDong + dem, currCot].Sohuu == 2)
                {
                    soQuanDich++;
                }
                else
                    break;
            }
            //Duyệt từ dưới lên
            for (int dem = 1; dem < 6 && currDong - dem >= 0; dem++)
            {
                if (mangOCo[currDong - dem, currCot].Sohuu == 1)
                {
                    soQuanTa++;
                    break;
                }
                else if (mangOCo[currDong - dem, currCot].Sohuu == 2)
                {
                    soQuanDich++;
                }
                else
                    break;
            }
            if (soQuanTa == 2)
            {
                return 0;
            }
            diemTong += mangDiemPhongNgu[soQuanDich];
            return diemTong;
        }
        private long DiemPN_DuyetNgang(int currDong, int currCot)
        {
            long diemTong = 0;
            int soQuanTa = 0;
            int soQuanDich = 0;
            //duyệt trái qua phải
            for (int dem = 1; dem < 6 && currCot + dem < _banCo.Socot; dem++)
            {
                if (mangOCo[currDong, currCot + dem].Sohuu == 1)
                {
                    soQuanTa++;
                    break;
                }
                else if (mangOCo[currDong, currCot + dem].Sohuu == 2)
                {
                    soQuanDich++;
                }
                else
                    break;
            }
            //phải qua trái
            for (int dem = 1; dem < 6 && currCot - dem >= 0; dem++)
            {
                if (mangOCo[currDong, currCot - dem].Sohuu == 1)
                {
                    soQuanTa++;
                    break;
                }
                else if (mangOCo[currDong, currCot - dem].Sohuu == 2)
                {
                    soQuanDich++;
                }
                else
                    break;
            }
            if (soQuanTa == 2)
            {
                return 0;
            }
            diemTong += mangDiemPhongNgu[soQuanDich];
            return diemTong;
        }
        private long DiemPN_DuyetCheoNguoc(int currDong, int currCot)
        {
            long diemTong = 0;
            int soQuanTa = 0;
            int soQuanDich = 0;
            //duyệt hướng trái dưới lên phải trên
            for (int dem = 1; dem < 6 && currCot + dem < _banCo.Socot && currDong - dem >= 0; dem++)
            {
                if (mangOCo[currDong - dem, currCot + dem].Sohuu == 1)
                {
                    soQuanTa++;
                    break;
                }
                else if (mangOCo[currDong - dem, currCot + dem].Sohuu == 2)
                {
                    soQuanDich++;
                }
                else
                    break;
            }
            //ngược lại
            for (int dem = 1; dem < 6 && currCot - dem >= 0 && currDong + dem < _banCo.Sodong; dem++)
            {
                if (mangOCo[currDong + dem, currCot - dem].Sohuu == 1)
                {
                    soQuanTa++;
                    break;
                }
                else if (mangOCo[currDong + dem, currCot - dem].Sohuu == 2)
                {
                    soQuanDich++;
                }
                else
                    break;
            }
            if (soQuanTa == 2)
            {
                return 0;
            }
            diemTong += mangDiemPhongNgu[soQuanDich];
            return diemTong;
        }
        private long DiemPN_DuyetCheoXuoi(int currDong, int currCot)
        {
            long diemTong = 0;
            int soQuanTa = 0;
            int soQuanDich = 0;
            //duyệt hướng trái trên xuống phải dưới
            for (int dem = 1; dem < 6 && currCot + dem < _banCo.Socot && currDong + dem < _banCo.Sodong; dem++)
            {
                if (mangOCo[currDong + dem, currCot + dem].Sohuu == 1)
                {
                    soQuanTa++;
                    break;
                }
                else if (mangOCo[currDong + dem, currCot + dem].Sohuu == 2)
                {
                    soQuanDich++;
                }
                else
                    break;
            }
            //duyệt hướng ngược lại 
            for (int dem = 1; dem < 6 && currCot - dem >= 0 && currDong - dem >= 0; dem++)
            {
                if (mangOCo[currDong - dem, currCot - dem].Sohuu == 1)
                {
                    soQuanTa++;
                    break;
                }
                else if (mangOCo[currDong - dem, currCot - dem].Sohuu == 2)
                {
                    soQuanDich++;
                }
                else
                    break;
            }
            if (soQuanTa == 2)
            {
                return 0;
            }
            diemTong += mangDiemPhongNgu[soQuanDich];
            return diemTong;
        }
        #endregion
        #endregion
    }
}
