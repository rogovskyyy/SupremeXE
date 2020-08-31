using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SupremeXE_UI
{
    public partial class SupremeXE_UI_Main : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public Browser browser = null;
        public Config cfg = Config.GetInstance();
        private List<Products> items = null;
        private List<DynamicControls> dyncontrol = new List<DynamicControls>();
        private List<string> sizes = new List<string>() {"Any","Small","Medium","Large","XLarge"};
        private const int spaceBetween = 60;

        public SupremeXE_UI_Main()
        {
            InitializeComponent();
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                items = JsonConvert.DeserializeObject<List<Products>>(File.ReadAllText(@"products.json"));
            }
            catch(IOException e)
            {
                Debugger.Log($"{e}");
            }

            dyncontrol.Add(new DynamicControls
            {
                image = pictureBox1,
                name = comboBox1,
                color = comboBox2,
                size = comboBox3,
                remove = button4
            });

            foreach (Products item in items)
            {
                comboBox1.Items.Add(item.Name);
            }

            foreach(var item in sizes)
            {
                comboBox3.Items.Add(item);
            }
        }

        private void FillColorComboBox(DynamicControls cmbbx, List<string> items)
        {
            cmbbx.color.Items.Clear();
            foreach (var item in items)
            {
                cmbbx.color.Items.Add(item);
            }
        }
    }
}