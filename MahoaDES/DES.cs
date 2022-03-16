using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahoaDES
{
    class DES
    {

        public static List<string[]> listCn = new List<string[]>();
        public static List<string[]> listDn = new List<string[]>();

        public static List<string[]> listLn = new List<string[]>();
        public static List<string[]> listRn = new List<string[]>();
        public static List<string[]> listRnLn = new List<string[]>();

        public static List<string[]> key_List = new List<string[]>();
        public static List<string[]> listCnDn = new List<string[]>();
        public static List<string[]> listE_R = new List<string[]>();
        public static List<string[]> listERXorK = new List<string[]>();
        public static List<string[]> listSboxOut = new List<string[]>();
        public static List<string[]> listFRK = new List<string[]>();


        public static string[] xArray = new string[8];
        public static string[] yArray = new string[8];
        public static int[] X_IntArray = new int[8];
        public static int[] Y_IntArray = new int[8];
        public static string[] SnBnArray = new string[8];
        public static List<string[]> listSnBnArray = new List<string[]>();

        public static string[] MT_PC1 = {"57", "49", "41", "33", "25", "17", "9", "1", "58", "50", "42", "34", "26", "18", "10", "2", "59", "51",
            "43", "35", "27", "19", "11", "3", "60", "52", "44", "36", "63", "55", "47", "39", "31", "23", "15", "7", "62", "54", "46",
            "38", "30", "22", "14", "6", "61", "53", "45", "37", "29", "21", "13", "5", "28", "20", "12", "4" };

        public static string[] MT_PC2 = { "14", "17", "11", "24", "1", "5", "3", "28", "15", "6", "21", "10", "23", "19", "12", "4", "26", "8",
            "16", "7", "27", "20", "13", "2", "41", "52", "31", "37", "47", "55", "30", "40", "51", "45", "33", "48", "44", "49", "39",
            "56", "34", "53", "46", "42", "50", "36", "29", "32" };

        public static string[] MT_IP = {"58", "50", "42", "34", "26", "18", "10", "2", "60", "52", "44", "36", "28", "20", "12", "4", "62", "54",
            "46", "38", "30", "22", "14", "6", "64", "56", "48", "40", "32", "24", "16", "8", "57", "49", "41", "33", "25", "17", "9", "1",
            "59", "51", "43", "35", "27", "19", "11", "3", "61", "53", "45", "37", "29", "21", "13", "5", "63", "55", "47", "39", "31",
            "23", "15", "7" };

        public static string[] MT_P = { "16", "7", "20", "21", "29", "12", "28", "17", "1", "15", "23", "26", "5", "18", "31", "10", "2", "8",
            "24", "14", "32", "27", "3","9", "19", "13", "30", "6", "22", "11", "4", "25" };

        public static string[] E_Table = { "32", "1", "2", "3", "4", "5", "4", "5", "6", "7", "8", "9", "8", "9", "10", "11", "12", "13", "12", "13",
            "14", "15", "16", "17", "16", "17", "18", "19", "20", "21", "20", "21", "22", "23", "24", "25", "24", "25", "26", "27", "28",
            "29", "28", "29", "30", "31", "32", "1" };

        public static string[] Dich_CnDn = { "1", "1", "2", "2", "2", "2", "2", "2", "1", "2", "2", "2", "2", "2", "2", "1" };

        public static string[] MT_IP_negative1 = {"40" ,"8" ,"48" ,"16" ,"56" ,"24" ,"64" ,"32" ,"39" ,"7" ,"47" ,"15" ,"55" ,"23" ,"63" ,"31" ,"38" 
                ,"6" ,"46" ,"14" ,"54" ,"22" ,"62" ,"30" ,"37" ,"5" ,"45" ,"13" ,"53" ,"21" ,"61" ,"29" ,"36" ,"4" ,"44" ,"12" ,"52" ,"20" ,"60" ,"28" 
                ,"35" ,"3" ,"43" ,"11" ,"51" ,"19" ,"59" ,"27" ,"34" ,"2" ,"42" ,"10" ,"50" ,"18" ,"58" ,"26" ,"33" ,"1" ,"41" ,"9" ,"49" ,"17" ,"57" ,"25"};

        public static int[,] S1 =    { {14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7},
                                {0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8},
                                {4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0},
                                {15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13}
                              };

        public static int[,] S2 =    {
                                { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                                { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                                { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                                { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
                              };

        public static int[,] S3 =  {
                                { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
                                { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                                { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
                                { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
                                };

        public static int[,] S4 = {
                                { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                                { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                                { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
                                { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
                            };

        public static int[,] S5 = {
                                { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                                { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                                { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                                { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
                            };

        public static int[,] S6 = {
                                { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                                { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                                { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
                                { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }

                             };
        public static int[,] S7 = {
                                { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                                { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                                { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                                { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
                            };

        public static int[,] S8 = {
                                { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                                { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                                { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
                                { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
                            };
        public static List<int[,]> listSBox = new List<int[,]>() {S1,S2,S3,S4,S5,S6,S7,S8};

        //bien doi mang string - > mang int
        public static int[] stringArrayToIntArray(string[] str)
        {
            int[] intArray = new int[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                intArray[i] = int.Parse(str[i]);
            }

            return intArray;
        }

        public static void DisposeAll()
        {
            listCn = new List<string[]>();
            listDn = new List<string[]>();      
            listLn = new List<string[]>();
            listRn = new List<string[]>();
            listRnLn = new List<string[]>();
            key_List = new List<string[]>();
            listCnDn = new List<string[]>();
            listE_R = new List<string[]>();
            listERXorK = new List<string[]>();
            listSboxOut = new List<string[]>();
            listFRK = new List<string[]>();
            listSnBnArray = new List<string[]>();
        }
        //bien doi mang int - > mang string
        public static string[] intArrayToStringArray(int[] Int)
        {
            string[] strArray = new string[Int.Length];
            for (int i = 0; i < Int.Length; i++)
            {
                strArray[i] = Int[i].ToString();
            }

            return strArray;
        }

      
        public static string[] HexToBin4bit(string hexdec)
        {
           
            string[] binaryArr = new string[hexdec.Length];
            for (int i = 0; i <hexdec.Length; i++)
            {
                switch (hexdec[i])
                {
                    case '0':
                        binaryArr[i] = "0000";
                        break;
                    case '1':
                        binaryArr[i] = "0001";
                        break;
                    case '2':
                        binaryArr[i] = "0010";
                        break;
                    case '3':
                        binaryArr[i] = "0011";
                        break;
                    case '4':
                        binaryArr[i] = "0100";
                        break;
                    case '5':
                        binaryArr[i] = "0101";
                        break;
                    case '6':
                        binaryArr[i] = "0110";
                        break;
                    case '7':
                        binaryArr[i] = "0111";
                        break;
                    case '8':
                        binaryArr[i] = "1000";
                        break;
                    case '9':
                        binaryArr[i] = "1001";
                        break;
                    case 'A':
                    case 'a':
                        binaryArr[i] = "1010";
                        break;
                    case 'B':
                    case 'b':
                        binaryArr[i] = "1011";
                        break;
                    case 'C':
                    case 'c':
                        binaryArr[i] = "1100";
                        break;
                    case 'D':
                    case 'd':
                        binaryArr[i] = "1101";
                        break;
                    case 'E':
                    case 'e':
                        binaryArr[i] = "1110";
                        break;
                    case 'F':
                    case 'f':
                        binaryArr[i] = "1111";
                        break;
                   

                }
               
            }
            return binaryArr;
        }

        public static string[] DecimalToBin4bit(string[] hexdec)
        {

            string[] binaryArr = new string[hexdec.Length];
            for (int i = 0; i < hexdec.Length; i++)
            {
                switch (hexdec[i])
                {
                    case "0":
                        binaryArr[i] = "0000";
                        break;
                    case "1":
                        binaryArr[i] = "0001";
                        break;
                    case "2":
                        binaryArr[i] = "0010";
                        break;
                    case "3":
                        binaryArr[i] = "0011";
                        break;
                    case "4":
                        binaryArr[i] = "0100";
                        break;
                    case "5":
                        binaryArr[i] = "0101";
                        break;
                    case "6":
                        binaryArr[i] = "0110";
                        break;
                    case "7":
                        binaryArr[i] = "0111";
                        break;
                    case "8":
                        binaryArr[i] = "1000";
                        break;
                    case "9":
                        binaryArr[i] = "1001";
                        break;
                    
                    case "10":
                        binaryArr[i] = "1010";
                        break;
                    
                    case "11":
                        binaryArr[i] = "1011";
                        break;
                    case "12":
                    
                        binaryArr[i] = "1100";
                        break;
                    
                    case "13":
                        binaryArr[i] = "1101";
                        break;
                    
                    case "14":
                        binaryArr[i] = "1110";
                        break;
                    case "15":
                    
                        binaryArr[i] = "1111";
                        break;
                }

            }
            return binaryArr;
        }

        //mang 16pt nhi phan 4bit -> mang 64 pt nhi phan 1bit
        public static string[] Convert_16unit4bit_To_64unit1bit(string[] str)
        {
            string[] temp = new string[64];
            int index = 0;
            for (int i = 0; i < str.Length; i++)
            {
                for (int j = 0; j < str[i].Length; j++)
                {
                    temp[index] = str[i][j].ToString();
                    index++;
                }
            }
            return temp;
        }

        public static string[] Convert_8unit4bit_To_32unit1bit(string[] str)
        {
            string[] temp = new string[32];
            int index = 0;
            for (int i = 0; i < str.Length; i++)
            {
                for (int j = 0; j < str[i].Length; j++)
                {
                    temp[index] = str[i][j].ToString();
                    index++;
                }
            }
            return temp;
        }

        public static string[] hoanVi(string[] matranArr, string[] binaryStr, int soBit)
        {
            string[] hoanVi = new string[soBit];
            int index = 0;
            for (int i = 0; i < soBit; i++)
            {
                for (int j = 0; j < binaryStr.Length; j++)
                {
                    int temp = int.Parse(matranArr[i]) - 1;
                    if (temp.ToString().Equals((j).ToString()))
                    {
                        hoanVi[index] = binaryStr[j];
                        index++;
                    }
                }
            }
            return hoanVi;
        }

        public static string[] hoanViNguoc(string[] matranArr, string[] binaryStr)
        {
            string[] hoanVi = new string[binaryStr.Length];
           
            for (int i = 0; i < binaryStr.Length; i++)
            {
                int indexOfOutput = int.Parse(matranArr[i]) -1;
                hoanVi[indexOfOutput] = binaryStr[i];  
            }
            return hoanVi;
        }
        public static void CnDnTable(string[] CnDnArray, string[] hoanVi_K)
        {
            string[] Cn = new string[28];
            string[] Dn = new string[28];
           
            for (int j = 0; j < 28; j++)
            {
                Cn[j] = hoanVi_K[j];

            }
            int ind = 0;
            for (int j = 28; j < 56; j++)
            {
                Dn[ind] = hoanVi_K[j];
                ind++;
            }

            listCn.Add(Cn);
            listDn.Add(Dn);
            for (int i = 1; i <= 16; i++)
            {
                Cn = new string[28];
                Dn = new string[28];


                for (int j = 0; j < 28; j++)
                {
                    int indexSauKhiDichTrai = int.Parse(CnDnArray[i - 1]) + j;

                    if (27 - indexSauKhiDichTrai >= 0)
                    {
                        Cn[j] = listCn[i - 1][indexSauKhiDichTrai];
                        Dn[j] = listDn[i - 1][indexSauKhiDichTrai];
                    }

                    else
                    {
                        int index = indexSauKhiDichTrai - 27;
                        for (int z = 0; z < index; z++)
                        {
                            Cn[j] = listCn[i - 1][z];
                            Dn[j] = listDn[i - 1][z];
                        }
                    }
                }
                listCn.Add(Cn);
                listDn.Add(Dn);

                string[] temp = Cn.Concat(Dn).ToArray();
                listCnDn.Add(temp);

            }  

        }

        public static string XOR(string input1, string input2)
        {
            if (input1.Equals(input2))
                return "0";
            else return "1";
        }

        public static string[] Bn(string[] listFn)
        {
            string[] Bn = new string[8];
            string listFnStr = string.Join("", listFn);
            string temp = "";
            int index = 0;
            for (int i = 0; i < 48; i+= 6)
            {
                temp = listFnStr.Substring(i, 6);
                Bn[index] = temp;
                index++;
            }
            return Bn;
        }

        public static void timXY(string[] Bn_Array)
        {
           
            for (int i = 0; i < 8; i++)
            {
                xArray[i] = Bn_Array[i][0].ToString() + Bn_Array[i][5];
                yArray[i] = Bn_Array[i][1].ToString() + Bn_Array[i][2] + Bn_Array[i][3] + Bn_Array[i][4];

                //chuyen xy nhi phan sang thap phan
                X_IntArray[i] = Convert.ToInt32(xArray[i], 2);
                Y_IntArray[i] = Convert.ToInt32(yArray[i], 2);
            }
        }

        public static void hoanViFquaSBox(string[] Bn)
        {
            for (int i = 0; i < Bn.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 16; k++)
                    {
                        SnBnArray[i] = (listSBox[i][X_IntArray[i], Y_IntArray[i]]).ToString();
                    }
                }
            }
           
        }

        public static string[] KeyXorER(string[] key, string[] Rn)
        {
            string[] E_R = DES.hoanVi(DES.E_Table, Rn,48);
            string[] temp = new string[48];
            for (int j = 0; j < 48; j++)
            {
                temp[j] = XOR(key[j], E_R[j]);
            }
            listE_R.Add(E_R);
            listERXorK.Add(temp);
            return temp;
        }

        public static string[] L_Xor_F_RK(string[] Ln, string[] F_RK)
        {  
            string[] temp = new string[32];
            for (int j = 0; j < 32; j++)
            {
                temp[j] = XOR(Ln[j], F_RK[j]);
            }
            return temp;
        }

        public static void L0R0(string[] M_binary)
        {
            string[] Ln = new string[32];
            string[] Rn = new string[32];
            string[] M_binaryStr64 = Convert_16unit4bit_To_64unit1bit(M_binary);
            string[] hoanVi_M_array = hoanVi(MT_IP, M_binaryStr64, 64);

            for (int j = 0; j < 32; j++)
            {
                Ln[j] = hoanVi_M_array[j];

            }

            int index = 0;
            for (int j = 32; j < 64; j++)
            {
                Rn[index] = hoanVi_M_array[j];
                index++;
            }
            listLn.Add(Ln);
            listRn.Add(Rn);
        }

       public static string binary4bitToHexDecimal(string binary)
        {
            string decima = "";
            for (int i = 0; i < binary.Length; i += 4)
            {
                string temp = binary.Substring(i, 4);
                string temp2 = Convert.ToInt32(temp, 2).ToString();
                decima += int.Parse(temp2).ToString("X");
            }   
            return decima;
        }

    }
}
