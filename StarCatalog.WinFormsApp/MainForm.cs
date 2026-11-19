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
    /// Главная форма пользовательского интерфейса Windows Forms.
    /// Реализует интерфейс IStarView: генерирует события действий пользователя
    /// и отвечает только за отображение переданных данных.
    /// </summary>
    public partial class MainForm : Form, IStarView
    {
        /// <summary>
        /// Событие запроса загрузки полного списка звезд.
        /// </summary>
        public event Action LoadStarsRequested;

        /// <summary>
        /// Событие запроса добавления новой звезды.
        /// </summary>
        public event Action<StarDto> AddStarRequested;

        /// <summary>
        /// Событие запроса поиска звезды по идентификатору для редактирования.
        /// </summary>
        public event Action<int> FindStarForEditRequested;

        /// <summary>
        /// Событие запроса сохранения изменений существующей звезды.
        /// </summary>
        public event Action<StarDto> UpdateStarRequested;

        /// <summary>
        /// Событие запроса удаления звезды по идентификатору.
        /// </summary>
        public event Action<int> DeleteStarRequested;

        /// <summary>
        /// Событие запроса фильтрации звезд по спектральному типу.
        /// </summary>
        public event Action<string> FilterByTypeRequested;

        /// <summary>
        /// Событие запроса сброса фильтра.
        /// </summary>
        public event Action ResetFilterRequested;

        /// <summary>
        /// Событие запроса расчета среднего радиуса звезд.
        /// </summary>
        public event Action CalculateAverageRadiusRequested;

        /// <summary>
        /// Инициализирует компоненты главной формы и элементы фильтрации.
        /// Не зависит от бизнес-логики и DI-контейнера.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            this.Text = "Каталог астрономических звезд";

            cmbFilterType.Items.Clear();
            cmbFilterType.Items.Add("Красный гигант");
            cmbFilterType.Items.Add("Желтый карлик");
            cmbFilterType.Items.Add("Белый карлик");
            cmbFilterType.Items.Add("Нейтронная звезда");
            cmbFilterType.SelectedIndex = 0;
        }

        /// <summary>
        /// Вызывается автоматически при первой загрузке формы (уже после подписки презентера).
        /// Генерирует событие LoadStarsRequested для первичного заполнения таблицы.
        /// </summary>
        /// <param name="e">Аргументы события загрузки формы.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Action handler = LoadStarsRequested;
            if (handler != null)
            {
                handler();
            }
        }



        /// <summary>
        /// Отображает переданный список DTO звезд в таблице DataGridView.
        /// </summary>
        /// <param name="stars">Список DTO звезд для отображения.</param>
        public void ShowStars(List<StarDto> stars)
        {
            dataGridViewStars.DataSource = null;
            dataGridViewStars.DataSource = stars;
        }

        /// <summary>
        /// Открывает окно редактирования для переданной звезды и, при подтверждении,
        /// генерирует событие обновления данных.
        /// </summary>
        /// <param name="star">DTO редактируемой звезды.</param>
        public void ShowStarForEdit(StarDto star)
        {
            if (star == null)
            {
                ShowError("Не удалось загрузить данные выбранной звезды.");
                return;
            }

            using (EditBookForm form = new EditBookForm(star))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Action<StarDto> handler = UpdateStarRequested;
                    if (handler != null)
                    {
                        handler(form.ResultStar);
                    }
                }
            }
        }

        /// <summary>
        /// Отображает рассчитанный средний радиус в текстовой метке формы.
        /// </summary>
        /// <param name="averageRadius">Средний радиус в радиусах Солнца.</param>
        public void ShowAverageRadius(double averageRadius)
        {
            lblAvgRadius.Text = "Средний радиус звезд: " + averageRadius.ToString("F2") + " R⊙";
        }

        /// <summary>
        /// Отображает информационное окно с сообщением об успешной операции.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Отображает диалоговое окно с сообщением об ошибке.
        /// </summary>
        /// <param name="errorMessage">Текст сообщения об ошибке.</param>
        public void ShowError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }



        /// <summary>
        /// Возвращает DTO звезды, выбранной в текущей строке таблицы.
        /// </summary>
        /// <returns>Выбранный объект StarDto или null, если строка не выбрана.</returns>
        private StarDto GetSelectedStar()
        {
            if (dataGridViewStars.CurrentRow != null)
            {
                return dataGridViewStars.CurrentRow.DataBoundItem as StarDto;
            }

            return null;
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Добавить».
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (EditBookForm form = new EditBookForm(null))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Action<StarDto> handler = AddStarRequested;
                    if (handler != null)
                    {
                        handler(form.ResultStar);
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Изменить».
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            StarDto selected = GetSelectedStar();
            if (selected == null)
            {
                ShowError("Выберите звезду в таблице для редактирования.");
                return;
            }

            Action<int> handler = FindStarForEditRequested;
            if (handler != null)
            {
                handler(selected.Id);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Удалить».
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            StarDto selected = GetSelectedStar();
            if (selected == null)
            {
                ShowError("Выберите звезду для удаления.");
                return;
            }

            Action<int> handler = DeleteStarRequested;
            if (handler != null)
            {
                handler(selected.Id);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Фильтр».
        /// </summary>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (cmbFilterType.SelectedItem == null)
            {
                ShowError("Выберите тип звезды из списка.");
                return;
            }

            string selectedType = cmbFilterType.SelectedItem.ToString();

            Action<string> handler = FilterByTypeRequested;
            if (handler != null)
            {
                handler(selectedType);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Сбросить фильтр».
        /// </summary>
        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            Action handler = ResetFilterRequested;
            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Средний радиус».
        /// </summary>
        private void btnCalculateAvg_Click(object sender, EventArgs e)
        {
            Action handler = CalculateAverageRadiusRequested;
            if (handler != null)
            {
                handler();
            }
        }
    }
}