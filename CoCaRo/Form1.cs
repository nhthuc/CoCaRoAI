using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoCaRo
{
    public partial class fCaro : Form
    {
        private CaroChess caroChess;
        private Graphics grs;
        public fCaro()
        {
            InitializeComponent();
            caroChess = new CaroChess();
            caroChess.KhoiTaoMangOCo();
            grs = pnlBanCo.CreateGraphics();
            pVsCToolStripMenuItem.Click += PVsCToolStripMenuItem_Click;
            btnPvC.Click += BtnPvC_Click;
        }

        private void BtnPvC_Click(object sender, EventArgs e)
        {
            PvsC(sender, e);
        }

        private void PVsCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PvsC(sender, e);
        }

        private void tmChuChay_Tick_1(object sender, EventArgs e)
        {
            lbText.Location = new Point(lbText.Location.X, lbText.Location.Y - 1);
            if(lbText.Location.Y + lbText.Height < 0)
            {
                lbText.Location = new Point(lbText.Location.X, plChuChay.Height);
            }
        }

        private void fCaro_Load(object sender, EventArgs e)
        {
            lbText.Text = "Người được đi trước  \n đánh vào vị trí \n  bất kỳ trên bàn cờ. \n  Người chơi phải \n tìm cách tích đủ 5 \n ô theo chiều dọc \nhoặc chiều ngang hoặc \n đường chéo mà không \n  bị chặn 2 đầu thì \n  sẽ thắng.";
            tmChuChay.Enabled = true;
            
        }

        private void pnlBanCo_Paint(object sender, PaintEventArgs e)
        {
            caroChess.VeBanCo(grs);
            caroChess.VeLaiQuanCo(grs);
        }

        private void pnlBanCo_MouseClick(object sender, MouseEventArgs e)
        {
            if (!caroChess.SanSang)
            {
                return;
            }
            if (caroChess.DanhCo(e.X, e.Y, grs))
            {
                if (caroChess.KiemTraChienThang())
                {
                    caroChess.KetThucTroChoi();
                }
                else
                if (caroChess.CheDoChoi == 2)
                {
                    caroChess.KhoiDongComputer(grs);
                    if (caroChess.KiemTraChienThang())
                    {
                        caroChess.KetThucTroChoi();
                    }
                }
            }
        }

        private void PvsP(object sender, EventArgs e)
        {
            label1.Visible = false;
            grs.Clear(pnlBanCo.BackColor); //xóa các quân cờ, giữ lại màu nền
            caroChess.StartPvsP(grs);
        }

        private void btnPvP_Click(object sender, EventArgs e)
        {
            PvsP(sender,e);
        }

        private void pVsPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PvsP(sender, e);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //grs.Clear(pnlBanCo.BackColor);
            caroChess.Undo(grs);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            caroChess.Redo(grs);
        }
        private void PvsC(object sender, EventArgs e)
        {
            label1.Visible = false;
            grs.Clear(pnlBanCo.BackColor); //xóa các quân cờ, giữ lại màu nền
            caroChess.StartPvsCom(grs);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            PvsP(sender, e);
            label1.Visible = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
                Application.Exit();
        }

        private void fCaro_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát trò chơi không vậy ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (caroChess.CheDoChoi == 2)
            {
                PvsC(sender, e);
            }
            else
                PvsP(sender, e);
        }
    }
}
