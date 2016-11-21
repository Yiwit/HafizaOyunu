using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Hafıza_Oyunu_Uygulama
{
    public partial class Form1 : Form
    {
        public List<Button> arraybtn = new List<Button>();
        Form2 oyunForm = new Form2();
        Button btn;
        int falseClick = 0;
        int trueClick = 0;
        XmlDocument doc = new XmlDocument();
        string dosyaYolu = Application.StartupPath + "\\HafızaOyunu.xml";
        bool available = false;
        
        public Form1()
        {
            InitializeComponent();
            skortablosu();
        }

          public void skortablosu()
        {
              
            listView1.Items.Clear(); 
            doc.Load(dosyaYolu);

            XElement root = XElement.Load(dosyaYolu);
            var orderedtabs = root.Elements("Oyuncular").OrderByDescending(a => (int)a.Element("Puan")).ToList().Take(10);

            foreach (var tab in orderedtabs)
            {
                var ID = tab.Attribute("ID").Value;
                var Nick = tab.Element("KullanıcıAdı").Value;
                var Score = tab.Element("Puan").Value;
                ListViewItem listViewItem = new ListViewItem();

                listViewItem.Text = Nick;
                listViewItem.SubItems.Add(Score);
                listViewItem.SubItems.Add(ID);
                listView1.Items.Add(listViewItem);
            }
        }

        void btnOlustur()
        {

            int btnKutu = 10;
            for (int i = 0; i < btnKutu; i++)
            {
                for (int a = 0; a < btnKutu; a++)
                {
                    btn = new Button();
                    btn.Top = i * 55 + 40;
                    btn.Left = a * 55 + 40;
                    btn.Size = new Size(55, 55);
                    btn.Name = "btn" + i + a;
                    btn.BackColor = Color.Gray;
                    btn.Click += btn_Click;
                    arraybtn.Add(btn);
                    oyunForm.Controls.Add(btn);
                }

            }
            oyunForm.Show();
            this.Hide();
        }

        int[] dizi = new int[5];
        void randombtn()
        {
            int s = 0;
            Random rastgele = new Random();
            for (int i = 0; i < 5; )
            {
                s = rastgele.Next(0, 100);
                if (i == 0)
                {
                    dizi[i] = s;
                    i++;
                }
                else if (dizi.Contains(s))
                {
                    continue;
                }
                else
                {
                    dizi[i] = s;
                    i++;
                }
            }
        }



        private void btn_Click(object sender, EventArgs e)
        {
            Button click = (Button)sender;

            if (arraybtn[dizi[0]] == click)
            {
                if (arraybtn[dizi[0]].BackColor == Color.Gray)
                {
                    trueClick += 1;
                    arraybtn[dizi[0]].BackColor = Color.Red;
                }
            }
            else if (arraybtn[dizi[1]] == click)
            {
                if (arraybtn[dizi[1]].BackColor == Color.Gray)
                {
                    trueClick += 1;
                    arraybtn[dizi[1]].BackColor = Color.Red;
                }
            }
            else if (arraybtn[dizi[2]] == click)
            {
                if (arraybtn[dizi[2]].BackColor == Color.Gray)
                {
                    trueClick += 1;
                    arraybtn[dizi[2]].BackColor = Color.Red;

                }
            }
            else if (arraybtn[dizi[3]] == click)
            {
                if (arraybtn[dizi[3]].BackColor == Color.Gray)
                {
                    trueClick += 1;
                    arraybtn[dizi[3]].BackColor = Color.Red;
                }
            }
            else if (arraybtn[dizi[4]] == click)
            {
                if (arraybtn[dizi[4]].BackColor == Color.Gray)
                {
                    trueClick += 1;
                    arraybtn[dizi[4]].BackColor = Color.Red;
                }
            }

            else
            {
                falseClick += 1;
                click.BackColor = Color.Black;

                if (falseClick == 545)
                {
                    MessageBox.Show("Hakkınız dolmuştur");
                    oyunForm.Close();
                    this.Show();
                }
                }
               
            if (trueClick == 5)
            {


                doc.Load(dosyaYolu);

                XmlNode RootElement1 = doc.SelectSingleNode("Oyuncular");

                foreach (XmlNode element in RootElement1)
                {
                    string kullanıcı = element.SelectSingleNode("KullanıcıAdı").InnerText;
                    string puanxml = element.SelectSingleNode("Puan").InnerText;
                    string id = element.Attributes["ID"].Value;

                    if (kullanıcı == txtbxKullaniciAdi.Text)
                    {
                        if (Convert.ToInt32(puanxml.ToString()) < Convert.ToInt32(oyunForm.lblPuan.Text.ToString()))
                        {
                            XmlNode edit = doc.SelectSingleNode("Oyuncular/Oyuncu[@ID='" + id + "']");
                            edit.SelectSingleNode("KullanıcıAdı").InnerText = txtbxKullaniciAdi.Text;
                            element.SelectSingleNode("Puan").InnerText = oyunForm.lblPuan.Text;
                        }
                        available = true;
                    }

                }
                if (!available)
                {
                    XmlElement RootElement = doc.CreateElement("Oyuncular");

                    XmlAttribute ID = doc.CreateAttribute("ID");
                    XmlNode kullanıcı = doc.CreateNode(XmlNodeType.Element, "KullanıcıAdı", null);
                    XmlNode puan = doc.CreateNode(XmlNodeType.Element, "Puan", null);

                    XmlNode sonElement = doc.SelectSingleNode("Oyuncular").LastChild;
                    int sonID;
                    if (sonElement != null)
                    {
                        sonID = int.Parse(sonElement.Attributes["ID"].Value);
                    }
                    else
                        sonID = 0;

                    ID.Value = (sonID + 1).ToString();
                    kullanıcı.InnerText = txtbxKullaniciAdi.Text;
                    puan.InnerText = oyunForm.lblPuan.Text;

                    RootElement.Attributes.Append(ID);
                    RootElement.AppendChild(kullanıcı);
                    RootElement.AppendChild(puan);

                    doc.DocumentElement.AppendChild(RootElement);
                }

                doc.Save(dosyaYolu);



                DialogResult result = MessageBox.Show("Tekrar oynamak istermisiniZ?", "Hafıza Oyunu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Application.Restart();

                }
                else if (result == DialogResult.No)
                {
                    Application.Exit();
                }


            }
        }
        private void btnbasla_Click(object sender, EventArgs e)
        {


            if (cmbxZorlukSeviye.SelectedIndex == 0)
            {

                timer1.Interval = 600;
                timer1.Start();
                randombtn();
                btnOlustur();
                for (int i = 0; i < 5; i++)
                {
                    arraybtn[dizi[i]].BackColor = Color.Red;
                }


            }
            if (cmbxZorlukSeviye.SelectedIndex == 1)
            {

                timer1.Interval = 450;
                timer1.Start();
                randombtn();
                btnOlustur();

            }
            if (cmbxZorlukSeviye.SelectedIndex == 2)
            {

                timer1.Interval = 100;
                timer1.Start();
                randombtn();
                btnOlustur();

            }
            if (cmbxZorlukSeviye.SelectedIndex == 3)
            {
                oyunForm.lblkalanSure.Hide();
                oyunForm.lblPuan.Hide();
                oyunForm.label1.Hide();
                oyunForm.label2.Hide();
                timer1.Interval = 1000;
                timer1.Start();
                randombtn();
                btnOlustur();

            }




        }
        int sayi = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sayi % 2 == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    arraybtn[dizi[i]].BackColor = Color.Blue;
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    arraybtn[dizi[i]].BackColor = Color.Gray;
                }

            }
            if (sayi == 5)
            {
                timer1.Stop();
            }
            sayi++;
        }
    }
}

