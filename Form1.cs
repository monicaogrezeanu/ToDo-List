using System.Security.Policy;
using System.IO;
using static Tema5.Form1;
using System.Runtime.Serialization;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO.Pipes;

namespace Tema5
{
    public partial class Form1 : Form
    {
        int contor = 1, contor2 = 1;

        [Serializable]
        public class Sarcina
        {
            public string Nume { get; set; }
            public string Descriere { get; set; }
            public DateTime DataScadenta { get; set; }
            public int Prioritate { get; set; }

            override public string ToString()
            {
                return Nume;
            }
        }

        List<Sarcina> sarcini = new List<Sarcina>();
        XmlSerializer serializer = new XmlSerializer(typeof(List<Sarcina>));


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            dateTimePicker1.Enabled = false;
            numericUpDown1.Enabled = false;
        }

        private void nouToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();
            dateTimePicker1.Value = DateTime.Now;
            numericUpDown1.Value = 1;

            contor = 1;
            contor2 = 1;

            sarcini.Clear();

        }
       
        private void deschidereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (Stream stream = File.OpenRead(openFileDialog.FileName))
                    {
                        sarcini = (List<Sarcina>)serializer.Deserialize(stream);
                        listBox1.Items.Clear();
                        foreach (Sarcina sarcina in sarcini)
                        {
                            listBox1.Items.Add(sarcina);
                        }
                    }
                }
                catch (SerializationException ex)
                {
                    MessageBox.Show("Failed to deserialize. Reason: " + ex.Message);
                }
            }
        }

        private void salvareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sarcini.Count > 0)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "XML File|*.xml";
                saveFileDialog1.Title = "Salvare ca";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                    {
                        serializer.Serialize(fs, sarcini);
                    }
                }
            }
            else MessageBox.Show("Nu există nicio sarcină de salvat.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void ieșireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void adăugațiSarcinăToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add($"Sarcina{contor2}");
            Sarcina sarcina = new Sarcina();

            int index = listBox1.FindStringExact($"Sarcina{contor2}");

            if (index != ListBox.NoMatches)
            {
                listBox1.SelectedIndex = index;
            }
            contor2++;
        }

        private void editațiSarcinaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count >0)
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                dateTimePicker1.Enabled = true;
                numericUpDown1.Enabled = true;
            }
            else MessageBox.Show("Selectați o sarcină.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ștergețiSarcinaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void despreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("    Această aplicație reprezintă un ToDo list, care îi permite utilizatorului " +
                "să creeze, editeze și șteargă activități organizate în liste, pe care le poate salva ulterior " +
                "pe dispozitivul său, pentru a le putea accesa oricând.", "Despre");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            { 
                Sarcina sarcina = new Sarcina();
                sarcina.Nume = textBox1.Text;
                sarcina.Descriere = textBox2.Text;
                sarcina.DataScadenta = dateTimePicker1.Value;
                sarcina.Prioritate = (int)numericUpDown1.Value;
                listBox1.Items.Add(sarcina);
                sarcini.Add(sarcina);

                textBox1.Clear();
                textBox2.Clear();
                dateTimePicker1.Value = DateTime.Now;
                numericUpDown1.Value = 1;

                listBox1.Items.Remove(listBox1.SelectedItem);

                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                dateTimePicker1.Enabled = false;
                numericUpDown1.Enabled = false;
            }
            else MessageBox.Show("Introduceți un nume pentru sarcină.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            dateTimePicker1.Value = DateTime.Now;
            numericUpDown1.Value = 1;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sarcina sarcinas = listBox1.SelectedItem as Sarcina;
            if (sarcinas != null)
            {
                textBox1.Text = sarcinas.Nume;
                textBox2.Text = sarcinas.Descriere;
                dateTimePicker1.Value = sarcinas.DataScadenta;
                numericUpDown1.Value = sarcinas.Prioritate;
            }
        }

    }
}