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
using System.Collections.Specialized;

namespace ktmtDA
{
    public partial class ImageShow : Form
    {
        string Dirpath;//bien lay duong dan thu muc
        StringCollection stringCol = new StringCollection();//luu lich su xem

        int ii= Properties.Settings.Default.History.Count;//bien index trong listbox
       
        int i=0;//bien dem trong listbox va listview
        int iold, inew;//dem so luong anh trong thu muc
        public ImageShow()
        {
            InitializeComponent();
            btnLoadImage.Text = Resource1.LoadImage;// Dat ten cho nut tai hinh anh
            Text = Resource1.ImageShow;//Dat ten cho chuong trinh
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            DialogResult mess = folderBrowserDialog1.ShowDialog();
            iold = i;

            if (mess == DialogResult.OK)
            {
                listView1.Items.Clear();//Lam moi listview
                Dirpath = folderBrowserDialog1.SelectedPath;
                label2.Text = Dirpath;//Hien thi duong dan thu muc
 
                pictureBox1.Image = null;
                string[] Files = Directory.GetFiles(Dirpath);                            
                foreach (String fn in Files)//lay cac duong dan anh
                {                  
                    if (fn.ToLower().EndsWith(".jpg") || fn.ToLower().EndsWith(".gif") ||
                    fn.ToLower().EndsWith(".png") || fn.ToLower().EndsWith(".bmp") ||
                    fn.ToLower().EndsWith(".jpeg"))
                    {
                        ListViewItem item = new ListViewItem();//them cac item anh vao listview
                        Image pic;
                        pic = Image.FromFile(fn);
                        imageList1.Images.Add(pic);
                        item.Name = fn;
                        item.ImageIndex = i;
                        listView1.Items.Add(item);                        
                        i++;
                    }
                }
            }
            inew = i;

        }
        

        private void listView1_Click(object sender, EventArgs e)
        {
            label1.Text = Resource1.History;
            string NameImage = listView1.SelectedItems[0].Name;
            string NameAndTime;//lay ngay gio he thong

            int day = DateTime.Now.Day;
            string sDay;
            if (day < 10)
                sDay = "0" + day.ToString();
            else sDay = day.ToString();

            int month = DateTime.Now.Month;
            string sMonth;
            if (month < 10)
                sMonth = "0" + month.ToString();
            else sMonth = month.ToString();

            int year = DateTime.Now.Year;

            int hour = DateTime.Now.Hour;
            string sHour;
            if (hour < 10)
                sHour = "0" + hour.ToString();
            else sHour = hour.ToString();

            int minute = DateTime.Now.Minute;
            string sMinute;
            if (minute < 10)
                sMinute = "0" + minute.ToString();
            else sMinute = minute.ToString();

            int second = DateTime.Now.Second;
            string sSecond;
            if (second < 10)
                sSecond = "0" + second.ToString();
            else sSecond = second.ToString();

            NameAndTime = sHour + ":" + sMinute + ":" + sSecond + "_" + sDay +"/"+ sMonth +"/"+ year.ToString()+" "+NameImage;
            //ten item gom thoi gian he thong va duong dan anh
            listBox1.Items.Add(NameAndTime);//them item vao listbox

            stringCol.Add(NameAndTime);//luu lich su
            
            listBox1.SelectedIndex =ii;//tro den item moi dc them vao
            ii++;

            pictureBox1.Image = Image.FromFile(NameImage);//hien thi anh vao picturebox
            textBox1.Text = NameImage;//dua duong dan ra textbox
            textBox1.Enabled = true;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            string ListBoxItemName = listBox1.SelectedItem.ToString();
            int strindex = 0;//loai bo ngay gio trong cac item cua listbox
            char temp = '.';
            while (temp != ' ')
            {
                temp = ListBoxItemName[strindex];
                strindex++;
            }
            string lbin = ListBoxItemName.Remove(0, strindex - 1);
            textBox1.Text = lbin;//dua duong dan ra textbox
            textBox1.Enabled = true;
            pictureBox1.ImageLocation = lbin;//hien thi anh khi click vao listbox
            
        }

        private void ImageShow_Load(object sender, EventArgs e)
        {
            Dirpath = Properties.Settings.Default.RecentFolder;
            label2.Text = Dirpath;
            string[] Files = Directory.GetFiles(Dirpath);
            
            foreach (String fn in Files)//lay cac duong dan anh
            {
                if (fn.ToLower().EndsWith(".jpg") || fn.ToLower().EndsWith(".gif") ||
                fn.ToLower().EndsWith(".png") || fn.ToLower().EndsWith(".bmp") ||
                fn.ToLower().EndsWith(".jpeg"))
                {
                    ListViewItem item = new ListViewItem();//them cac item anh vao listview
                    Image pic;
                    pic = Image.FromFile(fn);
                    imageList1.Images.Add(pic);
                    item.Name = fn;
                    item.ImageIndex = i;
                    listView1.Items.Add(item);
                    i++;
                }
            }
            inew = i; iold = 0;
            for (int j=0;j<Properties.Settings.Default.History.Count;j++)//tai lai lich su xem luc truoc
            {
                listBox1.Items.Add(Properties.Settings.Default.History[j]);
            }
            listBox1.SelectedIndex = Properties.Settings.Default.History.Count - 1;//chi den hinh anh dc xem gan nhat

            if(listBox1.Items.Count>0)
            {
                label1.Text = Resource1.History;   
                string ListBoxItemName = listBox1.SelectedItem.ToString();
                int strindex = 0;//loai bo ngay gio trong cac item cua listbox
                char temp = '.';
                while (temp != ' ')
                {
                    temp = ListBoxItemName[strindex];
                    strindex++;
                }
                string lbin = ListBoxItemName.Remove(0, strindex - 1);
                textBox1.Text = lbin;//dua duong dan ra textbox
                textBox1.Enabled = true;
                pictureBox1.ImageLocation = lbin;//hien thi anh
            }
            
        }


        private void ImageShow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.RecentFolder = Dirpath;//luu lai duong dan thu muc truoc do
            Properties.Settings.Default.History = stringCol;//luu lai lich su xem
            Properties.Settings.Default.Save();
        }



        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            string Info = "";//thong tin tooltip cua anh
            if (pictureBox1.Image != null)
            {
                FileInfo FI = new FileInfo(textBox1.Text.ToString());
                Info =  Resource1.Name_Image + ": " + FI.Name;
                Info = Info + "\n" + Resource1.Dimensions_Image+": " + pictureBox1.Image.Width.ToString()+ "x" +pictureBox1.Image.Height.ToString();
                float lenImage = (float)FI.Length / 1024;
                Info = Info + "\n" + Resource1.Size_Image + ": " + lenImage + "Kb";
                Info = Info + "\n" + Resource1.CreationTime_Image + ": " + FI.CreationTime;
                toolTip2.SetToolTip(pictureBox1, Info);
            }
            else
            {
                toolTip2.SetToolTip(pictureBox1, Resource1.PicBoxToolTip);
            }
        }

        

        private void label2_MouseHover(object sender, EventArgs e)
        {
            if(label2.Text!=null)
            {
                toolTip1.ToolTipTitle=Dirpath;
                if ((inew-iold) == 0)
                {
                    toolTip1.SetToolTip(label2, "0 image");
                }
                else if((inew - iold)== 1)
                {
                    toolTip1.SetToolTip(label2, "1 "+Resource1.Image);
                }
                else
                {
                    toolTip1.SetToolTip(label2,inew - iold + " " + Resource1.Images);
                }
            }
        }

        private void btnLoadImage_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = Resource1.LoadImage;//Title cua toolTip
            toolTip1.SetToolTip(btnLoadImage, Resource1.LoadImageToolTip);//Hien thi toolTip cho nut LoadImage  
        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = textBox1.Text;
            toolTip1.SetToolTip(textBox1, textBox1.Text);
        }

        private void listBox1_MouseHover(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index != -1 && index < listBox1.Items.Count)
            {
                toolTip3.SetToolTip(listBox1, listBox1.Items[index].ToString());
            }
        }

       
    }
}
