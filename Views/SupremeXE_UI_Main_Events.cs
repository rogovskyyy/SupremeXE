using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SupremeXE_UI
{
    public partial class SupremeXE_UI_Main
    {
        public void SupremeXE_UI_Main_Load(object sender, EventArgs e) { }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void SupremeXE_UI_Main_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private async void Close_Button_Click(object sender, EventArgs e)
        {
            var instance = Browser.GetInstance();
            await instance.DestroyBrowser();
            Application.Exit();
        }

        private void Close_Button_MouseEnter(object sender, EventArgs e)
        {
            this.Close_Button.Image = Properties.Resources.Close_Active;
        }

        private void Close_Button_MouseLeave(object sender, EventArgs e)
        {
            this.Close_Button.Image = Properties.Resources.Close;
        }

        private void Minimize_Button_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Minimize_Button_MouseEnter(object sender, EventArgs e)
        {
            this.Minimize_Button.Image = Properties.Resources.Minimize_Active;
        }

        private void Minimize_Button_MouseLeave(object sender, EventArgs e)
        {
            this.Minimize_Button.Image = Properties.Resources.Minimize;
        }
        private async void LaunchButton_Click(object sender, EventArgs e)
        {

            if (browser == null)
            {
                browser = Browser.GetInstance();
                await browser.OpenBrowser(false);

                if (radioButton1.Checked)
                {             
                    browser.Automatic(GetAllControlsValues());
                }
            }
        }

        private List<ToBuy> GetAllControlsValues()
        {
            List<ToBuy> items = new List<ToBuy>();
            for(int i = 0; i <= dyncontrol.Count - 1; i++)
            {
                var toBuy = new ToBuy();

                toBuy.SelectedName = dyncontrol[i].name.Text;
                toBuy.SelectedColor = dyncontrol[i].color.Text;
                toBuy.SelectedSize = dyncontrol[i].size.Text;

                items.Add(toBuy);
            }
            return items;
        }

        private async void StopButton_Click(object sender, EventArgs e)
        {
            if (browser != null)
            {
                await browser.DestroyBrowser();
                browser = null;
            }
        }

        private async void Download_Data_Click(object sender, System.EventArgs e)
        {
            ProductChecker pdc = new ProductChecker();
            await Task.Run(() => pdc.Process());
        }

        private void event_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb == null)
                return;

            for (int i = 0; i <= dyncontrol.Count - 1; i++)
            {
                if (dyncontrol[i].name.Name == cmb.Name)
                {
                    var item = items.Find(x => x.Name == dyncontrol[i].name.Text);
                    dyncontrol[i].image.Image = Image.FromFile($@"{Directory.GetCurrentDirectory()}\\temp\\{item.Index}_temp.png");
                    dyncontrol[i].image.SizeMode = PictureBoxSizeMode.StretchImage;
                    FillColorComboBox(dyncontrol[i], item.Colors);
                }
            }
        }

        private void remove_Click(object sender, System.EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            for (int i = 0; i <= dyncontrol.Count - 1; i++)
            {
                if (dyncontrol[i].remove.Name == btn.Name)
                {
                    Controls.Remove(dyncontrol[i].image);
                    Controls.Remove(dyncontrol[i].name);
                    Controls.Remove(dyncontrol[i].color);
                    Controls.Remove(dyncontrol[i].size);
                    Controls.Remove(dyncontrol[i].remove);

                    // 3 5
                    dyncontrol.Remove(dyncontrol[i]);

                    // 3 4
                    for (int j = i; j <= dyncontrol.Count - 1; j++)
                    {
                        dyncontrol[j].image.Name = $"pctbx_{j}";
                        dyncontrol[j].name.Name = $"cmbbx_name_{j}";
                        dyncontrol[j].color.Name = $"cmbbx_color_{j}";
                        dyncontrol[j].size.Name = $"cmbbx_size_{j}";
                        dyncontrol[j].remove.Name = $"btn_{j}";

                        dyncontrol[j].image.Location = new Point(dyncontrol[j].image.Location.X, dyncontrol[j].image.Location.Y - spaceBetween);
                        dyncontrol[j].name.Location = new Point(dyncontrol[j].name.Location.X, dyncontrol[j].name.Location.Y - spaceBetween);
                        dyncontrol[j].color.Location = new Point(dyncontrol[j].color.Location.X, dyncontrol[j].color.Location.Y - spaceBetween);
                        dyncontrol[j].size.Location = new Point(dyncontrol[j].size.Location.X, dyncontrol[j].size.Location.Y - spaceBetween);
                        dyncontrol[j].remove.Location = new Point(dyncontrol[j].remove.Location.X, dyncontrol[j].remove.Location.Y - spaceBetween);
                    }
                }
            }
            button3.Location = new Point(button3.Location.X, button3.Location.Y - spaceBetween);
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            Debugger.Log($"{dyncontrol.Count}");
            Debugger.Log($"{dyncontrol[0].name.Text}");

            if (dyncontrol.Count <= 9)
            {
                DynamicControls lastItem = dyncontrol[dyncontrol.Count - 1];
                int index = dyncontrol.Count;

                // Dynamic PictureBox
                PictureBox pctbx = new PictureBox();
                pctbx.Name = $"pctbx_{index}";
                pctbx.Size = new Size(50, 50);
                pctbx.Location = new Point(lastItem.image.Location.X, lastItem.image.Location.Y + spaceBetween);
                pctbx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(66)))), ((int)(((byte)(117)))));
                Controls.Add(pctbx);

                // Dynamic ComboBox for Name
                ComboBox cmbbx_name = new ComboBox();
                cmbbx_name.Name = $"cmbbx_name_{index}";
                cmbbx_name.Size = new Size(420, 30);
                cmbbx_name.Location = new Point(lastItem.name.Location.X, lastItem.name.Location.Y + spaceBetween);
                cmbbx_name.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbbx_name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(66)))), ((int)(((byte)(117)))));
                cmbbx_name.ForeColor = Color.White;
                cmbbx_name.Font = new Font("Microsoft Sans Serif", 12F);
                cmbbx_name.SelectedIndexChanged += new System.EventHandler(event_SelectedIndexChanged);
                Controls.Add(cmbbx_name);

                foreach (var item in items)
                {
                    cmbbx_name.Items.Add(item.Name);
                }

                // Dynamic ComboBox for Color
                ComboBox cmbbx_color = new ComboBox();
                cmbbx_color.Name = $"cmbbx_color_{index}";
                cmbbx_color.Size = new Size(125, 30);
                cmbbx_color.Location = new Point(lastItem.color.Location.X, lastItem.color.Location.Y + spaceBetween);
                cmbbx_color.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbbx_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(66)))), ((int)(((byte)(117)))));
                cmbbx_color.ForeColor = Color.White;
                cmbbx_color.Font = new Font("Microsoft Sans Serif", 12F);
                Controls.Add(cmbbx_color);

                // Dynamic ComboBox for Size
                ComboBox cmbbx_size = new ComboBox();
                cmbbx_size.Name = $"cmbbx_size_{index}";
                cmbbx_size.Size = new Size(125, 30);
                cmbbx_size.Location = new Point(lastItem.size.Location.X, lastItem.size.Location.Y + spaceBetween);
                cmbbx_size.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbbx_size.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(66)))), ((int)(((byte)(117)))));
                cmbbx_size.ForeColor = Color.White;
                cmbbx_size.Font = new Font("Microsoft Sans Serif", 12F);
                Controls.Add(cmbbx_size);

                foreach (var item in sizes)
                {
                    cmbbx_size.Items.Add(item);
                }

                cmbbx_size.SelectedIndex = 0;

                // Dynamic Button for Remove
                Button btn = new Button();
                btn.Name = $"btn_{index}";
                btn.Size = new Size(button4.Size.Width, button4.Size.Height);
                btn.Location = new Point(lastItem.remove.Location.X, lastItem.remove.Location.Y + spaceBetween);
                btn.BackColor = System.Drawing.Color.FromArgb(35, 113, 181);
                btn.Font = new Font("Microsoft Sans Serif", 12F);
                btn.ForeColor = Color.White;
                btn.Text = button4.Text;
                btn.FlatStyle = FlatStyle.Popup;
                btn.Click += new System.EventHandler(remove_Click);
                Controls.Add(btn);

                button3.Location = new Point(button3.Location.X, button3.Location.Y + spaceBetween);

                DynamicControls dynprod = new DynamicControls
                {
                    image = pctbx,
                    name = cmbbx_name,
                    color = cmbbx_color,
                    size = cmbbx_size,
                    remove = btn
                };

                dyncontrol.Add(dynprod);
            }
            else
            {
                MessageBox.Show("Nie mozna dodać więcej niż 10 przedmiotów", "Komunikat", MessageBoxButtons.OK);
            }
        }
    }
}