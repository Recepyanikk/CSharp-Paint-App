using System.Diagnostics.Eventing.Reader;
using System.Drawing.Imaging;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace proje
{
    public partial class Form1 : Form
    {
        bool cizim = false;
        Bitmap alan;
        Pen kalem1 = new Pen(Color.Gray);
        Pen pen = new Pen(Color.Black, 2);
        Point ax, ay;
        int Index = 0;
        Graphics g;
        ColorDialog colorDialog = new ColorDialog();
        int x, y, by, bx, cx, cy;
        int silgiboy = 20;
        int boyut = 20;
        int fýrcaboy;
        bool gridAcik = false;
        Stack<Bitmap> gecmis = new Stack<Bitmap>();
        public Form1()
        {
            InitializeComponent();
            this.Width = 1473;          //uygulama boyutu ayarlandý
            this.Height = 743;
            this.PerformLayout();
            int w = pic.Width > 0 ? pic.Width : 1100;
            int h = pic.Height > 0 ? pic.Height : 600;
            alan = new Bitmap(w, h);
            g = Graphics.FromImage(alan);
            g.Clear(Color.White);
            gecmis.Clear();
            GecmisiKaydet(); 
            renk.BackColor = pen.Color;
            trackBar1.Minimum = 1;
            trackBar1.Maximum = 15;
            trackBar1.Value = 2;
            trackBar2.Minimum = 2;          //kalem silgi kare fýrça nýn boyutlarýnýn ayarlanmasý için kullanýlaln barlarýn min max degerelri ayralandý
            trackBar2.Maximum = 200;
            trackBar2.Value = silgiboy;
            trackBar3.Minimum = 10;
            trackBar3.Maximum = 40;
            trackBar4.Maximum = 50;
            trackBar4.Minimum = 6;
            trackBar4.Value = 20;
            trackBar5.Maximum = 255;
            trackBar5.Minimum = 0;
            trackBar6.Maximum = 255;
            trackBar6.Minimum = 0;
            trackBar7.Maximum = 255;
            trackBar7.Minimum = 0;

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void kare_Click(object sender, EventArgs e)
        {
            Index = 3;                      //kare çizimi yapýlabilmesi için ýndex 3 e ayarlandý;
        }

        private void renk_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color secilen = colorDialog.Color;
                pen.Color = secilen;
                renk.BackColor = secilen;
                button1.BackColor = secilen;
                btn_boya.BackColor = secilen;
            }

        }

        private void doldur_Click(object sender, EventArgs e)
        {
            Index = 4;          //çizgi çizebilmek için index 4 eayarlandý
        }

        private void kalem_Click(object sender, EventArgs e)
        {
            Index = 1;          //çkalem ile çizim yapabilmek için index 1 e ayarlandý
        }

        private void silgi_Click(object sender, EventArgs e)
        {
            Index = 0;      //silgi kullana bilmek için index 0 a ayrlandý
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Index = 2;  //daire çizilebilmesi için index 2 ye atandý
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pic_Click(object sender, EventArgs e)
        {

        }

        private void pic_MouseDown(object sender, MouseEventArgs e)//fareye týklama
        {
            // 1. IZGARA KARELERÝNÝ DOLDURMA (Index 7)
            if (Index == 7)
            {
                // Týklanan koordinatýn hangi karenin sol üst köþesine denk geldiðini buluyoruz
                // Örn: Boyut 20 ise ve 35. piksele týkladýysan, (35 / 20) * 20 = 20. piksel (karenin baþlangýcý) olur.
                int kareX = (e.X / boyut) * boyut;
                int kareY = (e.Y / boyut) * boyut;

                // Bulduðumuz karenin içini seçili renkle boyuyoruz
                using (Graphics g_alan = Graphics.FromImage(alan))
                {
                    Brush firca = new SolidBrush(pen.Color);
                    g_alan.FillRectangle(firca, kareX, kareY, boyut, boyut);
                }

                pic.Invalidate(); // Ekraný yenile
                GecmisiKaydet();
                return;
            }

            // 2. NORMAL ÇÝZÝM BAÞLATMA
            cizim = true;
            ay = e.Location;
            cx = e.X;
            cy = e.Y;

        }

        private void pic_MouseMove(object sender, MouseEventArgs e)//fare hareket ederken
        {
            if (cizim)
            {


                if (Index == 1)//kalem seçildi
                {
                    ax = e.Location; //fare hareket ettikçe yeni konumu
                    g.DrawLine(pen, ax, ay); //grafik nesnesi kullanýlarak ilk konum son konum arasý çizgi
                    ay = ax; //bir sonraki harreketin degeri bitiþ degeri
                }

                else if (Index == 0)//silgi seçildi
                {
                    int boyut = silgiboy;
                    Brush fýrca = new SolidBrush(Color.White);
                    g.FillEllipse(fýrca, e.X - boyut / 2, e.Y - boyut / 2, boyut, boyut);//fare imlecinin bulunduðu konuma göre dairesel bir silgi oluþturulur

                }
                else if (Index == 6) // fýrça seçildi
                {

                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

                    // Kalem kalýnlýðýný fýrça boyutuna eþitleyelim
                    float eskiGeniþlik = pen.Width;
                    pen.Width = fýrcaboy;

                    ax = e.Location;
                    // Ýki nokta arasýna çizgi çekerek boþluklarý dolduruyoruz
                    g.DrawLine(pen, ay, ax);
                    ay = ax;

                    // Kalem kalýnlýðýný eski haline getirelim (diðer araçlarý bozmamak için)
                    pen.Width = eskiGeniþlik;

                    pic.Invalidate();
                }
                pic.Image = alan;//çizilen þekiller picturebox a atanýr ve çizilir
                pic.Refresh();//ekran güncellenir
                x = e.X;//imlecin son x kordinatý
                y = e.Y;//imlecin son y kordinatý


            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)//fare týklamassý bittiðinde
        {
            GecmisiKaydet();
            cizim = false;//çizim iþelemi biter

            bx = x - cx; //ilk ve son konuma göre geniþlik
            by = y - cy; //ilk ve son konuma göre yükseklik


            if (Index == 2)
            {
                g.DrawEllipse(pen, cx, cy, bx, by);//ilk son konumu ve boyutlarýna göre daire çizer
            }
            else if (Index == 3)
            {
                g.DrawRectangle(pen, cx, cy, bx, by);//ilk son konumu ve boyutlarýna göre dikdortgen
            }
            else if (Index == 4)
            {
                g.DrawLine(pen, new Point(cx, cy), new Point(x, y)); //baþlangýç noktasý ve bitiþ nokktasý arasýnda çizgi çizer
            }
            else if (Index == 5)
            {
                Point[] ucgen = { new Point(cx, cy + by), new Point(cx + bx, by + cy), new Point(cx + (bx / 2), cy) };
                //                alt sag köþe             alt sol köþe                 üst köþe
                g.DrawPolygon(pen, ucgen);//buna göre üçgen çizilir
            }
            pic.Image = alan;//alana aktarýlýr
            pic.Refresh();//picurebox yenilenir

        }
        private void RenkGuncelle()
        {
            Color c = Color.FromArgb(trackBar5.Value, trackBar6.Value, trackBar7.Value);
            pen.Color = c;
            renk.BackColor = c;
            button1.BackColor = c;
            btn_boya.BackColor = c; // Canlý olarak butonda rengi gör
        }


        private void temizle_Click(object sender, EventArgs e)
        {
            GecmisiKaydet();
            g.Clear(Color.White);//tüm alan temizlenir
            pic.Refresh();
        }

        private void kaydet_Click(object sender, EventArgs e)
        {
            SaveFileDialog dosya = new SaveFileDialog(); //dosya kaydetme penceresi oluþturulur
            dosya.Filter = "PNG Dosyalarý|*.png|JPEG Dosyalarý|*.jpg;*.jpeg|Tüm Dosyalar|*.*"; //dosya filtreleri eklenir
            dosya.DefaultExt = "png";       //dosyanýn default ddeðeri seçilir
            if (dosya.ShowDialog() == DialogResult.OK)//kaydet butonuna basýlýrsa if bloðu çalýþýr
            {
                try
                {
                    string a = dosya.FileName; //kullanýcýnýn seçtiði dosya adý ve yolu alýnýr
                    alan.Save(a, ImageFormat.Png);//kaydedileceði yer ve formatý yazýlýp kaydedilir
                    MessageBox.Show("resim kaydedildi");
                }
                catch
                {
                    MessageBox.Show("hata oluþtu");//sorun yaþanýrsa
                }
            }

        }

        private void yukle_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();       //dosya seçme penceresi oluþturuldu
            dosya.Filter = "PNG Dosyalarý|*.png|JPEG Dosyalarý|*.jpg;*.jpeg|Tüm Dosyalar|*.*";  //dosya filtereleri eklendi
            if (dosya.ShowDialog() == DialogResult.OK)//kullanýcý tamama týklayýp seçerse if bloðu çalýþýr
            {
                try
                {
                    Bitmap gorsel = new Bitmap(dosya.FileName);//bir görüntü nesnesi oluþturuldu
                    int yenigenis = 1140;           //resmin yeni boyutlarý ayarlandý
                    int yeniboy = 604;
                    Bitmap yenigorsel = new Bitmap(yenigenis, yeniboy);//belirtilen boyutlarda yeni görsel nesnesi oluþturuldu
                    using (Graphics grafik = Graphics.FromImage(yenigorsel))//yeni görseli çizebilmek için grafik nesnesi oluþur iþlem bittiðinde silinir
                    {
                        grafik.DrawImage(gorsel, 0, 0, yenigenis, yeniboy);//yeni resim çizilir
                    }
                    alan.Dispose();//bitmap bellekten temizleniyor
                    g.Dispose();//grafik bellekten temizleniyor //bellek sýzýntýsýný engellemek için öenemli

                    alan = yenigorsel;
                    g = Graphics.FromImage(alan); //yeni resimde çizim yapabilmek için grafik nesnesi tekrardan ayarlanýyor
                    pic.Image = alan;//görüntü atanýyor 
                    pic.Refresh();//görüntü yenileniyor
                }
                catch
                {
                    MessageBox.Show("resim yüklenirken hata oluþtu");//resim yüklenemezse hata mesajý veriyor
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pen.Width = trackBar1.Value;        //kalemein boyutunu varolan degere göre ayralr
            Index = 1;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            silgiboy = trackBar2.Value;     //silginin boyutunu varolan degere göre ayralr
            Index = 0;
        }

        private void ucciz_Click(object sender, EventArgs e)
        {
            Index = 5;     //ucgen çizmek için indexi 5 atar
        }
        private void kareciz(int boyut)
        {
            gridAcik = !gridAcik;
            pic.Invalidate();
        }

        private void kareler_Click_1(object sender, EventArgs e)
        {

            kareciz(boyut);         //picturebox'ý karelere bölmek için yazdýðýmýz kareçiz fonksiyonunu çaðýrýr

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            boyut = trackBar3.Value;    //piccturebox'ý bölecek karelerin boyutunu varolan degere atar  
            kareciz(boyut);
        }

        private void fýrca_Click(object sender, EventArgs e)
        {
            Index = 6;
            fýrcaboy = trackBar4.Value;     //fýrça ile çizim yapabilmk için indexe 6 atar ve varolan degeri boya atr

        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            fýrcaboy = trackBar4.Value;       //fýrca boyutu ayralnýr
            Index = 6;
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            RenkGuncelle();
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            RenkGuncelle();
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            RenkGuncelle();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult b = MessageBox.Show("Deðiþiklikleri kaydetmek istermisin", "Programdan çýkýlýyor", MessageBoxButtons.YesNoCancel);
            if (b == DialogResult.Yes)
            {
                SaveFileDialog dosya = new SaveFileDialog(); //dosya kaydetme projesi oluþturulur
                dosya.Filter = "PNG Dosyalarý|*.png|JPEG Dosyalarý|*.jpg;*.jpeg|Tüm Dosyalar|*.*"; //dosya filtreleri eklenir
                dosya.DefaultExt = "png";       //dosyanýn default ddeðeri seçilir
                if (dosya.ShowDialog() == DialogResult.OK)//kaydet butonuna basýlýrsa if bloðu çalýþýr
                {
                    try
                    {
                        string a = dosya.FileName; //kullanýcýnýn seçtiði dosya adý ve yolu alýnýr
                        alan.Save(a, ImageFormat.Png);//kaydedileceði yer ve formatý yazýlýp kaydedilir
                        MessageBox.Show("resim kaydedildi");
                    }
                    catch
                    {
                        MessageBox.Show("hata oluþtu");//sorun yaþanýrsa
                    }
                }
            }
            else if (b == DialogResult.Cancel)
            {
                e.Cancel = true;
            }

        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (alan != null)
            {
                e.Graphics.DrawImage(alan, 0, 0);
            }
            if (gridAcik)
            {
                for (int i = 0; i < pic.Width; i += boyut)
                {
                    e.Graphics.DrawLine(kalem1, i, 0, i, pic.Height);
                }
                for (int j = 0; j < pic.Height; j += boyut)
                {
                    e.Graphics.DrawLine(kalem1, 0, j, pic.Width, j);
                }
            }
        }
        private void Renk_Doldur(Bitmap bmp, Point pt, Color hedefRenk, Color yeniRenk)
        {

            if (hedefRenk.ToArgb() == yeniRenk.ToArgb()) return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point a = pixels.Pop();
                if (a.X < bmp.Width && a.X > 0 && a.Y < bmp.Height && a.Y > 0)
                {
                    if (bmp.GetPixel(a.X, a.Y).ToArgb() == hedefRenk.ToArgb())
                    {
                        bmp.SetPixel(a.X, a.Y, yeniRenk);
                        pixels.Push(new Point(a.X - 1, a.Y));
                        pixels.Push(new Point(a.X + 1, a.Y));
                        pixels.Push(new Point(a.X, a.Y - 1));
                        pixels.Push(new Point(a.X, a.Y + 1));
                    }
                }
            }
        }

        private void btn_boya_Click(object sender, EventArgs e)
        {
            Index = 7;
        }
        private void GecmisiKaydet()
        {
            gecmis.Push(new Bitmap(alan));

            if (gecmis.Count > 31)
            {
            }
        }
        private void geri_Click(object sender, EventArgs e)
        {
            
            if (gecmis.Count > 0)
            {
                Bitmap sonResim = gecmis.Pop();
                
                if (g != null) g.Dispose();

                if (alan != null) alan.Dispose();

                alan = sonResim;

                g = Graphics.FromImage(alan);

                pic.Image = alan;
                pic.Invalidate();
            }
            else
            {
                MessageBox.Show("Geri alýnacak daha fazla iþlem yok!");
            }
        }
    }
}
