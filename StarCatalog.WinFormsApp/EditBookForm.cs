using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StarCatalog.Core;



namespace StarCatalog.WinFormsApp
{
    public partial class EditBookForm : Form
    {
        /// <summary>
        /// Возвращает введенное пользователем название звезды.
        /// </summary>
        public string StarName { get; private set; }

        /// <summary>
        /// Возвращает введенное ФИО первооткрывателя.
        /// </summary>
        public string Discoverer { get; private set; }

        /// <summary>
        /// Возвращает выбранный из выпадающего списка тип звезды.
        /// </summary>
        public string StarType { get; private set; }

        /// <summary>
        /// Возвращает введенный радиус звезды.
        /// </summary>
        public double Radius { get; private set; }

        /// <summary>
        /// Конструктор окна редактирования. Если передан null — форма работает на создание.
        /// </summary>
        /// <param name="star">Объект для изменения или null.</param>
        public EditBookForm(Star star)
        {
            InitializeComponent();

            cmbStarType.Items.Clear();
            cmbStarType.Items.Add("Красный гигант");
            cmbStarType.Items.Add("Желтый карлик");
            cmbStarType.Items.Add("Белый карлик");
            cmbStarType.Items.Add("Нейтронная звезда");
            cmbStarType.SelectedIndex = 0;

            if (star != null)
            {
                txtName.Text = star.Name;
                txtDiscoverer.Text = star.Discoverer;
                cmbStarType.SelectedItem = star.StarType;
                txtRadius.Text = star.Radius.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Пожалуйста, введите название звезды.");
                return;
            }

            double radius;
            if (!double.TryParse(txtRadius.Text, out radius) || radius <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректный положительный радиус.");
                return;
            }

            StarName = txtName.Text.Trim();
            Discoverer = txtDiscoverer.Text.Trim();
            StarType = cmbStarType.SelectedItem.ToString();
            Radius = radius;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}