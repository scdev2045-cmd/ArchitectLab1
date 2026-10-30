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
using Ninject;

namespace StarCatalog.WinFormsApp
{
    /// <summary>
    /// Главная форма пользовательского интерфейса каталога звезд.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Поле бизнес-логики, инициализируемое через DI-контейнер.
        /// </summary>
        private readonly ILogic logic;

        /// <summary>
        /// Конструктор по умолчанию для визуального дизайнера Windows Forms.
        /// </summary>
        public MainForm() : this(new StandardKernel(new SimpleConfigModule()).Get<ILogic>())
        {
        }

        /// <summary>
        /// Инициализирует компоненты формы и внедряет зависимость бизнес-логики через интерфейс (DIP).
        /// </summary>
        /// <param name="logic">Экземпляр бизнес-логики, реализующий ILogic.</param>
        public MainForm(ILogic logic)
        {
            InitializeComponent();

            this.logic = logic ?? throw new ArgumentNullException(nameof(logic));

            cmbFilterType.Items.Clear();
            cmbFilterType.Items.Add("Красный гигант");
            cmbFilterType.Items.Add("Желтый карлик");
            cmbFilterType.Items.Add("Белый карлик");
            cmbFilterType.Items.Add("Нейтронная звезда");
            cmbFilterType.SelectedIndex = 0;

            RefreshGrid(this.logic.GetAllStars());
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
        /// <returns>Экземпляр выбранной звезды или null.</returns>
        private Star GetSelectedStar()
        {
            if (dataGridViewStars.CurrentRow != null)
            {
                return dataGridViewStars.CurrentRow.DataBoundItem as Star;
            }
            return null;
        }

        /// <summary>
        /// Обработчик события добавления новой звезды.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            EditBookForm form = new EditBookForm(null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                bool added = logic.AddStar(form.StarName, form.Discoverer, form.StarType, form.Radius);

                if (added)
                {
                    MessageBox.Show("Звезда добавлена.");
                    RefreshGrid(logic.GetAllStars());
                }
                else
                {
                    MessageBox.Show("Данные звезды заполнены неправильно.");
                }
            }
        }

        /// <summary>
        /// Обработчик события редактирования выбранной звезды.
        /// </summary>
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
                bool updated = logic.UpdateStar(selected.Id, form.StarName, form.Discoverer, form.StarType, form.Radius);

                if (updated)
                {
                    MessageBox.Show("Данные звезды изменены.");
                    RefreshGrid(logic.GetAllStars());
                }
                else
                {
                    MessageBox.Show("Не удалось изменить данные звезды.");
                }
            }
        }

        /// <summary>
        /// Обработчик события удаления выбранной звезды.
        /// </summary>
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

        /// <summary>
        /// Обработчик фильтрации звезд по выбранному типу.
        /// </summary>
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

        /// <summary>
        /// Сбрасывает текущий фильтр и отображает полный каталог.
        /// </summary>
        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            RefreshGrid(logic.GetAllStars());
        }

        /// <summary>
        /// Рассчитывает и отображает средний радиус звезд каталога.
        /// </summary>
        private void btnCalculateAvg_Click(object sender, EventArgs e)
        {
            double avg = logic.CalculateAverageRadius();
            lblAvgRadius.Text = "Средний радиус звезд: " + avg.ToString("F2") + " R⊙";
        }
    }
}