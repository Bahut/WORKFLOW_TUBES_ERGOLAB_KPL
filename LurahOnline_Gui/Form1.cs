using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LurahOnline_Gui
{
    public partial class Form1 : Form
    {
        private readonly HttpClient client = new HttpClient();

        private const string BaseUrl = "https://localhost:7278";
        private const string ApiUrl = BaseUrl + "/api/complaints";

        private TextBox txtJudul;
        private ComboBox cmbKategori;
        private TextBox txtDeskripsi;
        private TextBox txtLokasi;
        private TextBox txtPelapor;
        private Button btnKirim;

        private TextBox txtCariId;
        private Button btnCekStatus;
        private Label lblHasilStatus;

        public Form1()
        {
            InitializeComponent();
            BuildGui();
        }

        private void BuildGui()
        {
            this.Text = "LurahOnline - Sistem Pengaduan";
            this.Size = new Size(900, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);

            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10);

            TabPage tabPengaduan = new TabPage("Buat Pengaduan");
            TabPage tabStatus = new TabPage("Cek Status");

            tabControl.TabPages.Add(tabPengaduan);
            tabControl.TabPages.Add(tabStatus);

            this.Controls.Add(tabControl);

            BuildTabPengaduan(tabPengaduan);
            BuildTabStatus(tabStatus);
        }

        private Label CreateLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = new Point(x, y);
            label.Size = new Size(160, 30);
            label.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            return label;
        }

        private TextBox CreateTextBox(int x, int y, int width)
        {
            TextBox textBox = new TextBox();
            textBox.Location = new Point(x, y);
            textBox.Size = new Size(width, 30);
            textBox.Font = new Font("Segoe UI", 10);
            return textBox;
        }

        private Button CreateButton(string text, int x, int y, int width)
        {
            Button button = new Button();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(width, 42);
            button.BackColor = Color.FromArgb(15, 118, 110);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            return button;
        }

        private void BuildTabPengaduan(TabPage tab)
        {
            Label title = new Label();
            title.Text = "Form Pengaduan Masyarakat";
            title.Location = new Point(30, 25);
            title.Size = new Size(500, 40);
            title.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            tab.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Isi data pengaduan sesuai keluhan yang ingin dilaporkan.";
            subtitle.Location = new Point(30, 65);
            subtitle.Size = new Size(700, 30);
            subtitle.Font = new Font("Segoe UI", 10);
            tab.Controls.Add(subtitle);

            tab.Controls.Add(CreateLabel("Judul", 30, 115));
            txtJudul = CreateTextBox(200, 115, 520);
            tab.Controls.Add(txtJudul);

            tab.Controls.Add(CreateLabel("Kategori", 30, 160));
            cmbKategori = new ComboBox();
            cmbKategori.Location = new Point(200, 160);
            cmbKategori.Size = new Size(520, 30);
            cmbKategori.Font = new Font("Segoe UI", 10);
            cmbKategori.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKategori.Items.AddRange(new string[]
            {
                "Infrastruktur",
                "Kebersihan",
                "Keamanan",
                "Administrasi"
            });
            cmbKategori.SelectedIndex = 0;
            tab.Controls.Add(cmbKategori);

            tab.Controls.Add(CreateLabel("Deskripsi", 30, 205));
            txtDeskripsi = new TextBox();
            txtDeskripsi.Location = new Point(200, 205);
            txtDeskripsi.Size = new Size(520, 120);
            txtDeskripsi.Multiline = true;
            txtDeskripsi.Font = new Font("Segoe UI", 10);
            tab.Controls.Add(txtDeskripsi);

            tab.Controls.Add(CreateLabel("Lokasi", 30, 345));
            txtLokasi = CreateTextBox(200, 345, 520);
            tab.Controls.Add(txtLokasi);

            tab.Controls.Add(CreateLabel("Pelapor", 30, 390));
            txtPelapor = CreateTextBox(200, 390, 520);
            tab.Controls.Add(txtPelapor);

            btnKirim = CreateButton("Kirim Pengaduan", 200, 450, 220);
            btnKirim.Click += async (sender, e) => await KirimPengaduan();
            tab.Controls.Add(btnKirim);
        }

        private void BuildTabStatus(TabPage tab)
        {
            Label title = new Label();
            title.Text = "Cek Status Pengaduan";
            title.Location = new Point(30, 25);
            title.Size = new Size(500, 40);
            title.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            tab.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Masukkan ID pengaduan untuk melihat status laporan.";
            subtitle.Location = new Point(30, 65);
            subtitle.Size = new Size(700, 30);
            subtitle.Font = new Font("Segoe UI", 10);
            tab.Controls.Add(subtitle);

            tab.Controls.Add(CreateLabel("ID Pengaduan", 30, 120));
            txtCariId = CreateTextBox(200, 120, 250);
            tab.Controls.Add(txtCariId);

            btnCekStatus = CreateButton("Cek Status", 200, 170, 180);
            btnCekStatus.Click += async (sender, e) => await CekStatus();
            tab.Controls.Add(btnCekStatus);

            lblHasilStatus = new Label();
            lblHasilStatus.Location = new Point(30, 250);
            lblHasilStatus.Size = new Size(800, 230);
            lblHasilStatus.Font = new Font("Segoe UI", 11);
            lblHasilStatus.BackColor = Color.White;
            lblHasilStatus.BorderStyle = BorderStyle.FixedSingle;
            lblHasilStatus.Padding = new Padding(15);
            lblHasilStatus.Text = "Status pengaduan akan tampil di sini.";
            tab.Controls.Add(lblHasilStatus);
        }

        private async Task KirimPengaduan()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtJudul.Text))
                {
                    MessageBox.Show("Judul wajib diisi.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDeskripsi.Text))
                {
                    MessageBox.Show("Deskripsi wajib diisi.");
                    return;
                }

                var complaint = new
                {
                    title = txtJudul.Text,
                    category = cmbKategori.Text,
                    description = txtDeskripsi.Text,
                    location = txtLokasi.Text,
                    reporter = txtPelapor.Text
                };

                string json = JsonSerializer.Serialize(complaint);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(ApiUrl, content);
                string responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Pengaduan berhasil dikirim.\n\n" + responseText);

                    txtJudul.Clear();
                    txtDeskripsi.Clear();
                    txtLokasi.Clear();
                    txtPelapor.Clear();
                    cmbKategori.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Gagal mengirim pengaduan.\n\n" + responseText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async Task CekStatus()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCariId.Text))
                {
                    MessageBox.Show("Masukkan ID pengaduan.");
                    return;
                }

                HttpResponseMessage response = await client.GetAsync(ApiUrl + "/" + txtCariId.Text);
                string responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    lblHasilStatus.Text = "Data tidak ditemukan.\n\n" + responseText;
                    return;
                }

                ComplaintDto data = JsonSerializer.Deserialize<ComplaintDto>(
                    responseText,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                lblHasilStatus.Text =
                    "ID          : " + data.Id + Environment.NewLine +
                    "Judul       : " + data.Title + Environment.NewLine +
                    "Kategori    : " + data.Category + Environment.NewLine +
                    "Deskripsi   : " + data.Description + Environment.NewLine +
                    "Lokasi      : " + data.Location + Environment.NewLine +
                    "Pelapor     : " + data.Reporter + Environment.NewLine +
                    "Status      : " + data.Status;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }

    public class ComplaintDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Reporter { get; set; }
        public string Status { get; set; }
    }
}