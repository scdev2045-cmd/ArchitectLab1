using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StarCatalog.Shared;

namespace StarCatalog.WinFormsApp
{
    /// <summary>
    /// Диалоговая форма для ввода характеристик при добавлении или редактировании звезды.
    /// Работает исключительно с объектом передачи данных StarDto.
    /// </summary>
    public partial class EditBookForm : Form
    {
        /// <summary>
        /// Идентификатор редактируемой звезды (0 при создании новой записи).
        /// </summary>
        private readonly int _starId;

        /// <summary>
        /// Возвращает собранный объект StarDto после нажатия кнопки сохранения.
        /// </summary>
        public StarDto ResultStar { get; private set; }

        /// <summary>
        /// Инициализирует форму редактирования звезды.
        /// Если передан null — форма работает в режиме добавления новой звезды.
        /// </summary>
        /// <param name="star">DTO редактируемой звезды или null для создания новой.</param>
        public EditBookForm(StarDto star)
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
                _starId = star.Id;
                this.Text = "Редактирование звезды";

                txtName.Text = star.Name;
                txtDiscoverer.Text = star.Discoverer;
                cmbStarType.SelectedItem = star.StarType;
                txtRadius.Text = star.Radius.ToString();
            }
            else
            {
                _starId = 0;
                this.Text = "Добавление звезды";
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки «СОХРАНИТЬ».
        /// Выполняет базовую проверку ввода полей формы и формирует StarDto.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название звезды.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiscoverer.Text))
            {
                MessageBox.Show("Введите ФИО первооткрывателя.");
                return;
            }

            if (cmbStarType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип звезды.");
                return;
            }

            double radius;
            if (!double.TryParse(txtRadius.Text, out radius))
            {
                MessageBox.Show("Радиус должен быть числом.");
                return;
            }

            if (radius <= 0)
            {
                MessageBox.Show("Радиус должен быть больше нуля.");
                return;
            }

            ResultStar = new StarDto(
                _starId,
                txtName.Text.Trim(),
                txtDiscoverer.Text.Trim(),
                cmbStarType.SelectedItem.ToString(),
                radius);

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки «ОТМЕНА».
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}