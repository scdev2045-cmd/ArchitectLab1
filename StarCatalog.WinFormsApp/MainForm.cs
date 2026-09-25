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
    public partial class MainForm : Form
    {
        private Logic logic = new Logic();

        public MainForm()
        {
            InitializeComponent();

            cmbFilterType.Items.Clear();
            cmbFilterType.Items.Add("Красный гигант");
            cmbFilterType.Items.Add("Желтый карлик");
            cmbFilterType.Items.Add("Белый карлик");
            cmbFilterType.Items.Add("Нейтронная звезда");
            cmbFilterType.SelectedIndex = 0;

            RefreshGrid(logic.GetAllStars());
        }

        /// <summary>
        /// Обновляет источник данных таблицы.
        /// </summary>
        /// <param name="list">Список отображаемых звезд.</param>
        private void RefreshGrid(List<Star> list)
        {
            dataGridViewStars.DataSource = null;
            dataGridViewStars.DataSource = list;
        }

        /// <summary>
        /// Возвращает звезду, выбранную в строке таблицы.
        /// </summary>
        private Star GetSelectedStar()
        {
            if (dataGridViewStars.CurrentRow != null)
            {
                return dataGridViewStars.CurrentRow.DataBoundItem as Star;
            }
            return null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EditBookForm form = new EditBookForm(null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                logic.AddStar(form.StarName, form.Discoverer, form.StarType, form.Radius);
                RefreshGrid(logic.GetAllStars());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Star selected = GetSelectedStar();
            if (selected == null)
            {
                MessageBox.Show("Выберите звезду в таблице для редактирования.");
                return;
            }

            EditBookForm form = new EditBookForm(selected);
            if (form.ShowDialog() == DialogResult.OK)
            {
                logic.UpdateStar(selected.Id, form.StarName, form.Discoverer, form.StarType, form.Radius);
                RefreshGrid(logic.GetAllStars());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Star selected = GetSelectedStar();
            if (selected == null)
            {
                MessageBox.Show("Выберите звезду для удаления.");
                return;
            }

            logic.DeleteStar(selected.Id);
            RefreshGrid(logic.GetAllStars());
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (cmbFilterType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип звезды из списка.");
                return;
            }

            string selectedType = cmbFilterType.SelectedItem.ToString();
            List<Star> filtered = logic.GetStarsByType(selectedType);
            RefreshGrid(filtered);
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            RefreshGrid(logic.GetAllStars());
        }

        private void btnCalculateAvg_Click(object sender, EventArgs e)
        {
            double avg = logic.CalculateAverageRadius();
            lblAvgRadius.Text = "Средний радиус звезд: " + avg.ToString("F2") + " R⊙";
        }
    }
}