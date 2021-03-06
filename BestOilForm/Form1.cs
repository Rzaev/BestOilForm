using Guna.UI2.WinForms;
using iTextSharp.text;
using iTextSharp.text.pdf;
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

namespace BestOilForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void PriceTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LitrChoiceTxt.Enabled = false;
            MoneyChoiceTxt.Enabled = false;
            PayButton.Enabled = false;
            CalculatePetrol.Enabled = false;
            LitrRadio.Enabled = false;
            MoneyRadio.Enabled = false;
        }

        private void PetrolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PetrolList.Text == "A-95")
            {
                PriceTxt.Text = "1.50";
                Double.Parse(PriceTxt.Text);
            }
            else if (PetrolList.Text == "A-92")
            {
                PriceTxt.Text = "1.25";
                Double.Parse(PriceTxt.Text);
            }
            else
            {
                PriceTxt.Text = "0.80";
                Double.Parse(PriceTxt.Text);
            }

            LitrRadio.Enabled = true;
            MoneyRadio.Enabled = true;

        }

        private void LitrRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (LitrRadio.Checked)
            {
                MoneyChoiceTxt.Clear();
                LitrChoiceTxt.Enabled = true;
            }
            else
                LitrChoiceTxt.Enabled = false;

        }

        private void MoneyRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (MoneyRadio.Checked)
            {
                LitrChoiceTxt.Clear();
                MoneyChoiceTxt.Enabled = true;
            }
            else
                MoneyChoiceTxt.Enabled = false;
        }

        private void CalculatePetrol_Click(object sender, EventArgs e)
        {
            if (LitrChoiceTxt.Enabled == true)
            {
                double price = Double.Parse(PriceTxt.Text);
                int litr = int.Parse(LitrChoiceTxt.Text);
                PetrolMoneyLbl.Text = (price * litr).ToString();
            }
            else
            {
                //double price = Double.Parse(PriceTxt.Text);
                //int money = int.Parse(MoneyChoiceTxt.Text);
                //PetrolMoneyLbl.Text = (money / price).ToString();
                PetrolMoneyLbl.Text = MoneyChoiceTxt.Text;
            }
        }

        private void LitrChoiceTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void LitrChoiceTxt_TextChanged(object sender, EventArgs e)
        {
            if (LitrChoiceTxt.Text != string.Empty)
            {
                CalculatePetrol.Enabled = true;
               // PayButton.Enabled = true;
            }
            else
            {
                CalculatePetrol.Enabled = false;
                //PayButton.Enabled = false;
            }

            TotalMoneyEnable();


        }

       

        private void MoneyChoiceTxt_TextChanged(object sender, EventArgs e)
        {
            if (MoneyChoiceTxt.Text != string.Empty)
            {
                CalculatePetrol.Enabled = true;
            }
            else
            {
                CalculatePetrol.Enabled = false;
            }

            TotalMoneyEnable();
        }

        private void HotDogCh_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox(HotDogCh, HotDogCount);
        }

        private void HamburgerCh_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox(HamburgerCh, HamburgerCount);
        }

        private void FrenchFries_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox(FrenchFries, FrenchFriesCount);
        }

        private void ColaCh_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox(ColaCh, ColaCount);
        }

        private void CalculateCafe_Click(object sender, EventArgs e)
        {
            double total = 0;
            if (IsChecked(HotDogCh))
                total += (Double.Parse(HotDogPrice.Text) * (int)HotDogCount.Value);
            if(IsChecked(HamburgerCh))
            {
                total += (Double.Parse(HamburgerPrice.Text) * (int)HamburgerCount.Value);
            }
            if (IsChecked(FrenchFries))
                total += (Double.Parse(FrenchFriesPrice.Text) * (int)FrenchFriesCount.Value);
            if (IsChecked(ColaCh))
                total += (Double.Parse(ColaPrice.Text) * (int)ColaCount.Value);

            CafeMoney.Text = total.ToString();

        }

        private void PayButton_Click(object sender, EventArgs e)
        {
            Double.Parse(PetrolMoneyLbl.Text);
            Double.Parse(CafeMoney.Text);
            TotalMoney.Text = (Double.Parse(PetrolMoneyLbl.Text) + Double.Parse(CafeMoney.Text)).ToString();
            WritePdf();
        }

        #region Functions

        private void CheckBox(CheckBox ch, Guna2NumericUpDown num)
        {
            if (ch.Checked)
                num.Enabled = true;
            else
                num.Enabled = false;

            MoneyEnable();
            TotalMoneyEnable();
        }

        private void MoneyEnable()
        {
            if (HotDogCh.Checked || HamburgerCh.Checked || FrenchFries.Checked || ColaCh.Checked)
            {
                CalculateCafe.Enabled = true;
            }
            else
            {
                CalculateCafe.Enabled = false;
                CafeMoney.Text = "0";
            }
        }

        private bool IsChecked(CheckBox ch)
        {
            if (ch.Checked)
                return true;
            else
                return false;
        }


        

        private void TotalMoneyEnable()
        {
            if (CalculatePetrol.Enabled == true || CalculateCafe.Enabled == true)
                PayButton.Enabled = true;
            else
                PayButton.Enabled = false;
        }

        public Paragraph GetElements(CheckBox ch,Guna2NumericUpDown count, iTextSharp.text.Font font)
        {
            Paragraph element = new Paragraph(ch.Text +" "+ int.Parse( count.Text), font);
            return element;
        }

        public void WritePdf()
        {
            Document pdoc = new Document(PageSize.A4, 20f, 20f, 30f, 30f);
            PdfWriter pWriter = PdfWriter.GetInstance(pdoc, new FileStream("D:\\Myfstpdf.pdf", FileMode.Create));
            pdoc.Open();



            System.Drawing.Image pImage = System.Drawing.Image.FromFile("D:\\azpetrol.JFIF");
            iTextSharp.text.Image ItextImage = iTextSharp.text.Image.GetInstance(pImage, System.Drawing.Imaging.ImageFormat.Jpeg);
            ItextImage.Alignment = Element.ALIGN_CENTER;
            pdoc.Add(ItextImage);

            iTextSharp.text.Font pfont1 = FontFactory.GetFont(iTextSharp.text.Font.FontFamily.TIMES_ROMAN.ToString(), 20,
                iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);


            Paragraph pgraph1 = new Paragraph(PetrolList.SelectedItem.ToString(),pfont1);
            pgraph1.Alignment = Element.ALIGN_CENTER;
            pdoc.Add(pgraph1);


            Paragraph pgraph2 = new Paragraph(LitrChoiceTxt.Text+ guna2HtmlLabel3.Text, pfont1);
            pgraph2.Alignment = Element.ALIGN_CENTER;
            Paragraph pgraph3 = new Paragraph(MoneyChoiceTxt.Text+ guna2HtmlLabel4.Text, pfont1);
            pgraph3.Alignment = Element.ALIGN_CENTER;
            if (LitrChoiceTxt.Enabled == true)
            {
                pdoc.Add(pgraph2);
            }
            else
                pdoc.Add(pgraph3);

            Paragraph element1 = new Paragraph(int.Parse(HotDogCount.Text)+" "+HotDogCh.Text,pfont1);
            element1.Alignment = Element.ALIGN_CENTER;
            Paragraph element2 = new Paragraph(int.Parse(HamburgerCount.Text) + " " + HamburgerCh.Text,pfont1);
            element2.Alignment = Element.ALIGN_CENTER;
            Paragraph element3 = new Paragraph(int.Parse(FrenchFriesCount.Text) + " " + FrenchFries.Text, pfont1);
            element3.Alignment = Element.ALIGN_CENTER;
            Paragraph element4 = new Paragraph(int.Parse(ColaCount.Text) + " " + ColaCh.Text, pfont1);
            element4.Alignment = Element.ALIGN_CENTER;
            if (IsChecked(HotDogCh) && int.Parse(HotDogCount.Text)!=0)
                pdoc.Add(element1);
            if (IsChecked(HamburgerCh)&& int.Parse(HamburgerCount.Text) != 0)
            {
                pdoc.Add(element2);
            }
            if (IsChecked(FrenchFries)&& int.Parse(FrenchFriesCount.Text) != 0)
                pdoc.Add(element3);
            if (IsChecked(ColaCh)&& int.Parse(ColaCount.Text) != 0)
                pdoc.Add(element4);
            //element = GetElements(ColaCh, ColaCount, pfont1);


            iTextSharp.text.Font pfont2 = FontFactory.GetFont(iTextSharp.text.Font.FontFamily.TIMES_ROMAN.ToString(), 20,
               iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.MAGENTA);

            Paragraph pgraph4 = new Paragraph("Total:" + TotalMoney.Text,pfont2);
            pgraph4.Alignment = Element.ALIGN_CENTER;
            pdoc.Add(pgraph4);

            pdoc.Close();
        }



        #endregion

       
    }
}
