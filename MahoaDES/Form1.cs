using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Diagnostics;

namespace MahoaDES
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public string[] stringArr_NhiPhan(string key)
        {
           
            string[] keyBinaryArray = DES.HexToBin4bit(key);
            return keyBinaryArray;
        }

        public void Key_Binary(String bienKhoa)
        { 
            string temp = "";
            foreach (var item in stringArr_NhiPhan(bienKhoa))
            {
                temp += (item + " ");
            }
        }

        public void tim_K_table(string[] matranPC02_Array)
        {    
            for (int i = 0; i < 16; i++)
            {
                string[] key_Array = new string[56];
                key_Array = DES.hoanVi(matranPC02_Array, DES.listCnDn[i], 48);
                DES.key_List.Add(key_Array);
            }
        }

        public void HoanViKey(String bienKhoa)
        {
            string[] binaryStr64 = DES.Convert_16unit4bit_To_64unit1bit(stringArr_NhiPhan(bienKhoa));
            string[] strArray = DES.hoanVi(DES.MT_PC1, binaryStr64, 56);
            string temp = "";
          
            for (int i = 0; i < strArray.Length; i++)
            {
                temp += strArray[i];
                if ((i+1) % 4 == 0) temp += " ";
            }
        }

        public void CnDnTable(String bienKhoa)
        {
            string[] binaryStr64 = DES.Convert_16unit4bit_To_64unit1bit(stringArr_NhiPhan(bienKhoa));
            string[] keyHoanVi = DES.hoanVi(DES.MT_PC1, binaryStr64, 56);
            DES.CnDnTable(DES.Dich_CnDn, keyHoanVi);
        }
        public void TimLnRn_MaHoa(string plainText)
        {
            tim_K_table(DES.MT_PC2);
            for (int i = 0; i < 16; i++)
            {
                //moi vong co SiBi

                //tinh R tiep theo de gan cho L
                //muon tim dc R1 thi phai lat L0 XOR f(ER0,K1). L0 co, R0 co, K1 co. 
                //b1.Phai tim f(ER0,K1). 
                //b2: f chia thanh 8 nhom 6 bit,
                //b3:moix nhom 6bit hoan vi qua bang sbox tuong ung
                //b3.1: tim x,y;
                //b3.2: tim x,y tuong ung trong Sbox => hoan thanh 1 Si(Bi), lap lai 8 lan;
                //Thuc hien b1:tim f(ER0,K1)

                //listLn[1] = listRn[0];

                DES.L0R0(stringArr_NhiPhan(plainText));
                string[] f = DES.KeyXorER(DES.key_List[i], DES.listRn[i]);//xong b1;
                                                          
                string[] Bn_array = DES.Bn(f);//xong b2;
                DES.listSboxOut.Add(Bn_array);
                //thuc hien buoc 3;
                //ket qua ra S1(B1) -> S8(B8)
                DES.timXY(Bn_array);
                DES.hoanViFquaSBox(Bn_array);
                string[] tempSnBn = DES.DecimalToBin4bit(DES.SnBnArray);
                DES.listSnBnArray.Add(tempSnBn);
                //hoan vi cua f(R0,K1) ;
                string[] binaryStr = DES.Convert_8unit4bit_To_32unit1bit(tempSnBn);
                string[] F_RK = DES.hoanVi(DES.MT_P, binaryStr, 32);
                DES.listFRK.Add(F_RK);
                string[] temp = DES.listRn[i];
                DES.listLn.Insert(i+1,temp);
             
                string[] temp2 = DES.L_Xor_F_RK(DES.listLn[i],F_RK);
                DES.listRn.Insert(i+1,temp2);

            }

        }

        public void TimLR_GiaiMa(string cypher, String bienKhoa)
        {
            
            Key_Binary(bienKhoa);

            //hoan vi khoa 56 bit
            HoanViKey(bienKhoa);

            //bang dich theo key theo bang CnDn
            CnDnTable(bienKhoa);
            tim_K_table(DES.MT_PC2);
            string cypherText = cypher;
            string[] ipNegative1 = DES.HexToBin4bit(cypherText);
            ipNegative1 = DES.Convert_16unit4bit_To_64unit1bit(ipNegative1);
            string[] L16R16 = DES.hoanViNguoc(DES.MT_IP_negative1, ipNegative1);
            string[] temp = new string[32];
          
            for (int j = 0; j < 32; j++)
            {
                temp[j] = L16R16[j];
            }
            DES.listLn.Add(temp);
            temp = new string[32];
            int index = 0;
            for (int j = 32; j < 64; j++)
            {
                temp[index] = L16R16[j];
                index++;
            }
            DES.listRn.Add(temp);
            int ind = 0;
            for (int i = 15; i >= 0; i--)
            {

              
                string[] f = DES.KeyXorER(DES.key_List[i], DES.listRn[ind]);//xong b1;

                string[] Bn_array = DES.Bn(f);//xong b2;
                DES.listSboxOut.Add(Bn_array);
                //thuc hien buoc 3;
                //ket qua ra S1(B1) -> S8(B8)
                DES.timXY(Bn_array);
                DES.hoanViFquaSBox(Bn_array);
                string[] tempSnBn = DES.DecimalToBin4bit(DES.SnBnArray);
                DES.listSnBnArray.Add(tempSnBn);
                //hoan vi cua f(R0,K1) ;
                string[] binaryStr = DES.Convert_8unit4bit_To_32unit1bit(tempSnBn);
                string[] F_RK = DES.hoanVi(DES.MT_P, binaryStr, 32);
                DES.listFRK.Add(F_RK);
                string[] temp1 = DES.listRn[ind];
                
                DES.listLn.Insert(ind + 1, temp1);

                string[] temp2 = DES.L_Xor_F_RK(DES.listLn[ind], F_RK);
                DES.listRn.Insert(ind + 1, temp2);
                ind++;
            }
        }

        public static string ConvertStringToHex(String input, System.Text.Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        public static string ConvertHexToString(String hexInput, System.Text.Encoding encoding)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }


        private void btnMaHoa_Click(object sender, EventArgs e)
        {
            if (txtKhoa.Text.Length != 16)
            {
                MessageBox.Show("  Độ dài K phải = 16!", "Thông báo");
                return;
            }

            if (txtBanRo.Text == "")
            {
                MessageBox.Show("Mời bạn nhập dữ liệu cần mã hóa!", "Thông báo");
                return;
            }
            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();
            string cypherText = "";
            string plainText = ConvertStringToHex(txtBanRo.Text, System.Text.Encoding.Unicode);
            while (plainText.Length % 16 != 0)
            {
                plainText += "F";
            }
            //txtBanRo.Text = plainText;

            string[] plainTextArray = new string[plainText.Length / 16];
            int index = 0;
            for (int i = 0; i < plainTextArray.Length; i++)
            {
                plainTextArray[i] = plainText.Substring(index,16);
                index += 16;
            }
            for (int k = 0; k < plainTextArray.Length; k++)
            {
                plainText = plainTextArray[k];
                //bien doi key - > nhi phan
                Key_Binary(txtKhoa.Text);

                //hoan vi khoa 56 bit
                HoanViKey(txtKhoa.Text);

                //bang dich theo key theo bang CnDn
                CnDnTable(txtKhoa.Text);

                //DES.LnRn(stringArr_NhiPhan(txtBanRo.Text));

                TimLnRn_MaHoa(plainText);
                string[] R16L16 = DES.listRn[16].Concat(DES.listLn[16]).ToArray();
                string[] hoanviIpNegative1 = DES.hoanVi(DES.MT_IP_negative1, R16L16, 64);

                string hoanviIpNegative1Str = string.Join("", hoanviIpNegative1);
                cypherText += DES.binary4bitToHexDecimal(hoanviIpNegative1Str);
                DES.DisposeAll();
            }
            //txtBanMaHoa.Text = ConvertHexToString(cypherText, System.Text.Encoding.Unicode);
            txtBanMaHoa.Text = cypherText; 
            // Stop timing
            stopwatch.Stop();
            MessageBox.Show($"Thời gian mã hóa thuật toán Des là: {stopwatch.Elapsed.TotalSeconds} (s)");
        }

        private void rtbK1_16_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtBanMaHoa.Clear();
            txtBanGiaiMa.Clear();
            txtBanRo.Clear();
            txtKhoa.Clear();
            txtKhoa2.Clear();
            txtKhoa3.Clear();
            txtBanRo.Focus();
        }
       
        private void btnGiaiMa_Click(object sender, EventArgs e)
        {


            txtBanGiaiMa.Clear();
            if (txtKhoa.Text.Length != 16)
            {
                MessageBox.Show("  Độ dài K phải = 16!", "Thông báo");
                return;
            }
            if (txtBanMaHoa.Text == "")
            {
                MessageBox.Show("Mời bạn nhập dữ liệu cần giải mã!", "Thông báo");
                return;
            }
            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();
            string cypher = "";
            //string cypherText1 = ConvertStringToHex(txtBanMaHoa.Text, System.Text.Encoding.Unicode);
            string cypherText1 = txtBanMaHoa.Text;
            while (cypherText1.Length % 16 != 0)
            {
                cypherText1 += "F";
            }
            //txtBanMaHoa.Text = cypherText1;
            string[] cypherTextArray = new string[cypherText1.Length / 16];
            int index1 = 0;
            for (int i = 0; i < cypherTextArray.Length; i++)
            {
                cypherTextArray[i] = cypherText1.Substring(index1, 16);
                index1 += 16;
            }
            for (int k = 0; k < cypherTextArray.Length; k++)
            {

                cypher = cypherTextArray[k];

                TimLR_GiaiMa(cypher, txtKhoa.Text);
                string[] R0L0 = DES.listRn[16].Concat(DES.listLn[16]).ToArray();
                string[] cypherText = DES.hoanViNguoc(DES.MT_IP, R0L0);

                string banRoCuaDoan = DES.binary4bitToHexDecimal(string.Join("", cypherText));
                txtBanGiaiMa.Text += banRoCuaDoan;

                DES.DisposeAll();
            }
            txtBanGiaiMa.Text = ConvertHexToString(txtBanGiaiMa.Text, System.Text.Encoding.Unicode);
            // Stop timing
            stopwatch.Stop();
            MessageBox.Show($"Thời gian giải mã thuật toán Des là: {stopwatch.Elapsed.TotalSeconds} (s)");
        }

        private void txtKetQua_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnMaHoa3_Click(object sender, EventArgs e)
        {
            if (txtBanRo.Text == "")
            {
                MessageBox.Show("Mời bạn nhập dữ liệu cần mã hóa!", "Thông báo");
                return;
            }
            if (txtKhoa.Text.Length != 16)
            {
                MessageBox.Show("  Độ dài K phải = 16!", "Thông báo");
                return;
            }
            if (txtKhoa2.Text.Length != 16)
            {
                MessageBox.Show("  Độ dài K2 phải = 16!", "Thông báo");
                return;
            }
            if (txtKhoa3.Text.Length != 16)
            {
                MessageBox.Show("  Độ dài K3 phải = 16!", "Thông báo");
                return;
            }
            if (txtKhoa2.Text == txtKhoa.Text)
            {
                MessageBox.Show("Khóa K2 không được giống Khóa K", "Thông báo");
                return;
            }
            if (txtKhoa3.Text == txtKhoa.Text)
            {
                MessageBox.Show("Khóa K3 không được giống Khóa K", "Thông báo");
                return;
            }
            if (txtKhoa3.Text == txtKhoa2.Text)
            {
                MessageBox.Show("Khóa K3 không được giống Khóa K2", "Thông báo");
                return;
            }
            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();

            //MÃ HÓA KHÓA VỚI KHÓA 1
            string cypherText = "";
            string plainText = ConvertStringToHex(txtBanRo.Text, System.Text.Encoding.Unicode);
            while (plainText.Length % 16 != 0)
            {
                plainText += "F";
            }
            //txtBanRo.Text = plainText;
            string[] plainTextArray = new string[plainText.Length / 16];
            int index = 0;
            for (int i = 0; i < plainTextArray.Length; i++)
            {
                plainTextArray[i] = plainText.Substring(index, 16);
                index += 16;
            }
            for (int k = 0; k < plainTextArray.Length; k++)
            {

                plainText = plainTextArray[k];
                //bien doi key - > nhi phan
                Key_Binary(txtKhoa.Text);

                //hoan vi khoa 56 bit
                HoanViKey(txtKhoa.Text);

                //bang dich theo key theo bang CnDn
                CnDnTable(txtKhoa.Text);

                //DES.LnRn(stringArr_NhiPhan(txtBanRo.Text));

                TimLnRn_MaHoa(plainText);
                string[] R16L16 = DES.listRn[16].Concat(DES.listLn[16]).ToArray();
                string[] hoanviIpNegative1 = DES.hoanVi(DES.MT_IP_negative1, R16L16, 64);
                
                string hoanviIpNegative1Str = string.Join("", hoanviIpNegative1);
                cypherText += DES.binary4bitToHexDecimal(hoanviIpNegative1Str);
                DES.DisposeAll();
            }

            //GIẢI MÃ VỚI KHÓA 2
            string cypher = "";
            string banRoCuaDoan = "";
            string cypherText1 = cypherText;
            string plainText2 = "";
            while (cypherText1.Length % 16 != 0)
            {
                cypherText1 += "F";
            }
            //txtBanMaHoa.Text = cypherText1;
            string[] cypherTextArray = new string[cypherText1.Length / 16];
            int index1 = 0;
            for (int i = 0; i < cypherTextArray.Length; i++)
            {
                cypherTextArray[i] = cypherText1.Substring(index1, 16);
                index1 += 16;
            }
            for (int k = 0; k < cypherTextArray.Length; k++)
            {
                cypher = cypherTextArray[k];

                TimLR_GiaiMa(cypher, txtKhoa2.Text);
                string[] R0L0 = DES.listRn[16].Concat(DES.listLn[16]).ToArray();
                string[] cypherText2 = DES.hoanViNguoc(DES.MT_IP, R0L0);

                banRoCuaDoan = DES.binary4bitToHexDecimal(string.Join("", cypherText2));
                plainText2 += banRoCuaDoan;
                DES.DisposeAll();
            }

            //MÃ HÓA VỚI KHÓA K3
            string cypherText3 = "";
            string plainText3 = plainText2;
            string[] plainTextArray3 = new string[plainText3.Length / 16];
            int index3 = 0;
            for (int i = 0; i < plainTextArray3.Length; i++)
            {
                plainTextArray3[i] = plainText3.Substring(index3, 16);
                index3 += 16;
            }
            for (int k = 0; k < plainTextArray3.Length; k++)
            {

                plainText3 = plainTextArray3[k];
                //bien doi key - > nhi phan
                Key_Binary(txtKhoa3.Text);

                //hoan vi khoa 56 bit
                HoanViKey(txtKhoa3.Text);

                //bang dich theo key theo bang CnDn
                CnDnTable(txtKhoa3.Text);

                //DES.LnRn(stringArr_NhiPhan(txtBanRo.Text));

                TimLnRn_MaHoa(plainText3);
                string[] R16L16 = DES.listRn[16].Concat(DES.listLn[16]).ToArray();
                string[] hoanviIpNegative3 = DES.hoanVi(DES.MT_IP_negative1, R16L16, 64);
                string hoanviIpNegative1Str3 = string.Join("", hoanviIpNegative3);
                cypherText3 += DES.binary4bitToHexDecimal(hoanviIpNegative1Str3);
                DES.DisposeAll();
            }
            //txtBanMaHoa.Text = ConvertHexToString(cypherText3, System.Text.Encoding.Unicode);
            txtBanMaHoa.Text = cypherText3;
            // Stop timing
            stopwatch.Stop();
            MessageBox.Show($"Thời gian mã hóa thuật toán 3DES là: {stopwatch.Elapsed.TotalSeconds} (s)");
        }

        private void btnGiaiMa3_Click(object sender, EventArgs e)
        {
            if (txtKhoa.Text.Length != 16)
            {
                MessageBox.Show("  Độ dài K phải = 16!", "Thông báo");
                return;
            }
            if (txtKhoa2.Text.Length != 16)
            {
                MessageBox.Show("  Độ dài K2 phải = 16!", "Thông báo");
                return;
            }
            if (txtKhoa3.Text.Length != 16)
            {
                MessageBox.Show("  Độ dài K3 phải = 16!", "Thông báo");
                return;
            }
            if (txtBanMaHoa.Text == "")
            {
                MessageBox.Show("Mời bạn nhập dữ liệu cần giải mã!", "Thông báo");
                return;
            }
            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();

            //GIẢI MÃ VỚI KHÓA K3
            string cypher3 = "";
            string cypherText3 = txtBanMaHoa.Text;
            String plainText3 = "";
            while (cypherText3.Length % 16 != 0)
            {
                cypherText3 += "F";
            }
            //txtBanMaHoa.Text = cypherText3;
            string[] cypherTextArray3 = new string[cypherText3.Length / 16];
            int index3 = 0;
            for (int i = 0; i < cypherTextArray3.Length; i++)
            {
                cypherTextArray3[i] = cypherText3.Substring(index3, 16);
                index3 += 16;
            }
            for (int k = 0; k < cypherTextArray3.Length; k++)
            {
                cypher3 = cypherTextArray3[k];

                TimLR_GiaiMa(cypher3, txtKhoa3.Text);
                string[] R0L0 = DES.listRn[16].Concat(DES.listLn[16]).ToArray();
                string[] cypherText3b = DES.hoanViNguoc(DES.MT_IP, R0L0);

                string banRoCuaDoan3 = DES.binary4bitToHexDecimal(string.Join("", cypherText3b));
                plainText3 += banRoCuaDoan3;
                DES.DisposeAll();
            }

            //MÃ HÓA VỚI KHÓA K2
            string cypherText2 = "";
            string plainText2 = plainText3;
            string[] plainTextArray2 = new string[plainText2.Length / 16];
            int index2 = 0;
            for (int i = 0; i < plainTextArray2.Length; i++)
            {
                plainTextArray2[i] = plainText2.Substring(index2, 16);
                index2 += 16;
            }
            for (int k = 0; k < plainTextArray2.Length; k++)
            {

                plainText2 = plainTextArray2[k];
                //bien doi key - > nhi phan
                Key_Binary(txtKhoa2.Text);

                //hoan vi khoa 56 bit
                HoanViKey(txtKhoa2.Text);

                //bang dich theo key theo bang CnDn
                CnDnTable(txtKhoa2.Text);

                //DES.LnRn(stringArr_NhiPhan(txtBanRo.Text));

                TimLnRn_MaHoa(plainText2);
                string[] R16L16 = DES.listRn[16].Concat(DES.listLn[16]).ToArray();
                string[] hoanviIpNegative2 = DES.hoanVi(DES.MT_IP_negative1, R16L16, 64);
                string hoanviIpNegative1Str2 = string.Join("", hoanviIpNegative2);
                cypherText2 += DES.binary4bitToHexDecimal(hoanviIpNegative1Str2);
                DES.DisposeAll();
            }

            //GIẢI MÃ VỚI KHÓA K
            string cypher = "";
            string cypherText1 = cypherText2;
            String plainText = "";
            while (cypherText1.Length % 16 != 0)
            {
                cypherText1 += "F";
            }
            //txtBanMaHoa.Text = cypherText1;
            string[] cypherTextArray = new string[cypherText1.Length / 16];
            int index1 = 0;
            for (int i = 0; i < cypherTextArray.Length; i++)
            {
                cypherTextArray[i] = cypherText1.Substring(index1, 16);
                index1 += 16;
            }
            for (int k = 0; k < cypherTextArray.Length; k++)
            {
                cypher = cypherTextArray[k];

                TimLR_GiaiMa(cypher, txtKhoa.Text);
                string[] R0L0 = DES.listRn[16].Concat(DES.listLn[16]).ToArray();
                string[] cypherText = DES.hoanViNguoc(DES.MT_IP, R0L0);

                string banRoCuaDoan = DES.binary4bitToHexDecimal(string.Join("", cypherText));
                plainText += banRoCuaDoan;
                DES.DisposeAll();
            }
            txtBanGiaiMa.Text = ConvertHexToString(plainText, System.Text.Encoding.Unicode);
            // Stop timing
            stopwatch.Stop();
            MessageBox.Show($"Thời gian giải mã thuật toán 3DES là: {stopwatch.Elapsed.TotalSeconds} (s)");
        }
    }

   
}
