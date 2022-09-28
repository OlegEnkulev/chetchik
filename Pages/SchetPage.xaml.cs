using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using chetchik.Resources;

namespace chetchik.Pages
{
    public partial class SchetPage : Page
    {
        int HotWaterCost;
        int ColdWaterCost;

        bool predohran = false;

        bool pokazHotLastExists = true;
        bool pokazColdLastExists = true;

        public SchetPage()
        {
            InitializeComponent();

            if(Core.DB.ВидСчетчика.Where(w => w.Наименование == "Горячая Вода").FirstOrDefault() == null)
            {
                MessageBox.Show("В базе данных отсутствует тариф на горячую воду!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                PayBTN.IsEnabled = false;
            }
            HotWaterTarifLabel.Content = Core.DB.ВидСчетчика.Where(w => w.Наименование == "Горячая Вода").FirstOrDefault().ТарифЗаКубическийМетр;
            if (Core.DB.ВидСчетчика.Where(w => w.Наименование == "Холодная Вода").FirstOrDefault() == null)
            {
                MessageBox.Show("В базе данных отсутствует тариф на холодную воду!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                PayBTN.IsEnabled = false;
            }
            ColdWaterTarifLabel.Content = Core.DB.ВидСчетчика.Where(w => w.Наименование == "Холодная Вода").FirstOrDefault().ТарифЗаКубическийМетр;

            ColdWaterBox.Text = "0";
            HotWaterBox.Text = "0";
        }

        private void PayBTN_Click(object sender, RoutedEventArgs e)
        {
            if(predohran == false)
            {
                MessageBox.Show("Проверьте введенную информацию!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Счета schetHot = new Счета { };
                Показания pokazHot = new Показания { АйдиСчета = schetHot.Айди, КубическиеМетры = Convert.ToInt32(HotWaterBox.Text), Дата = DateTime.Now, АйдиПользователя = Core.currentUser.Айди, АйдиВидаСчетчика = 3 };
                Оплаты oplataHot = new Оплаты { АйдиСтатуса = 1, Цена = HotWaterCost, АйдиСчета = schetHot.Айди };
                Core.DB.Показания.Add(pokazHot);
                Core.DB.Счета.Add(schetHot);
                Core.DB.Оплаты.Add(oplataHot);
                Core.DB.SaveChanges();

                Счета schetCold = new Счета { };
                Показания pokazCold = new Показания { АйдиСчета = schetCold.Айди, КубическиеМетры = Convert.ToInt32(ColdWaterBox.Text), Дата = DateTime.Now, АйдиПользователя = Core.currentUser.Айди, АйдиВидаСчетчика = 4 };
                Оплаты oplataCold = new Оплаты { АйдиСтатуса = 1, Цена = ColdWaterCost, АйдиСчета = schetCold.Айди };
                Core.DB.Показания.Add(pokazCold);
                Core.DB.Счета.Add(schetCold);
                Core.DB.Оплаты.Add(oplataCold);

                Core.DB.SaveChanges();
                MessageBox.Show("Готово");
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            Core.currentUser = null;
            Core.mainWindow.MainFrame.Navigate(new Pages.MainPage());
        }

        private void WaterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                List<Показания> pokazHotList = Core.DB.Показания.Where(p => p.АйдиПользователя == Core.currentUser.Айди && p.ВидСчетчика.Наименование == "Горячая вода").ToList();
                Показания pokazHotLast = new Показания();

                if (pokazHotList.Count != 0)
                {
                    pokazHotLast = pokazHotList[0];

                    for (int i = 0; i < pokazHotList.Count(); i++)
                    {
                        if (pokazHotList[i].Дата > pokazHotLast.Дата)
                        {
                            pokazHotLast = pokazHotList[i];
                        }
                    }
                }
                else
                {
                    pokazHotLastExists = false;
                }

                

                List<Показания> pokazColdList = Core.DB.Показания.Where(p => p.АйдиПользователя == Core.currentUser.Айди && p.ВидСчетчика.Наименование == "Холодная вода").ToList();
                Показания pokazColdLast = new Показания();

                if (pokazColdList.Count != 0)
                {
                    pokazColdLast = pokazColdList[0];

                    for (int i = 0; i < pokazColdList.Count(); i++)
                    {
                        if (pokazColdList[i].Дата > pokazColdLast.Дата)
                        {
                            pokazColdLast = pokazColdList[i];
                        }
                    }
                }
                else
                {
                    pokazColdLastExists = false;
                }

                int ColdWater;
                int HotWater;

                if (pokazColdLastExists)
                {
                    ColdWaterLastLabel.Content = pokazColdLast.КубическиеМетры;
                    ColdWater = (Convert.ToInt32(ColdWaterBox.Text) - pokazColdLast.КубическиеМетры) * Core.DB.ВидСчетчика.Where(w => w.Наименование == "Холодная Вода").FirstOrDefault().ТарифЗаКубическийМетр;
                    ColdWaterLabel.Content = ColdWater.ToString();
                }
                else
                {
                    ColdWaterLastLabel.Content = "-";
                    ColdWater = Convert.ToInt32(ColdWaterBox.Text) * Core.DB.ВидСчетчика.Where(w => w.Наименование == "Холодная Вода").FirstOrDefault().ТарифЗаКубическийМетр;
                    ColdWaterLabel.Content = ColdWater.ToString();
                }

                if (pokazHotLastExists)
                {
                    HotWaterLastLabel.Content = pokazHotLast.КубическиеМетры;
                    HotWater = (Convert.ToInt32(HotWaterBox.Text) - pokazHotLast.КубическиеМетры) * Core.DB.ВидСчетчика.Where(w => w.Наименование == "Горячая Вода").FirstOrDefault().ТарифЗаКубическийМетр;
                    HotWaterLabel.Content = HotWater.ToString();
                }
                else
                {
                    HotWaterLastLabel.Content = "-";
                    HotWater = Convert.ToInt32(HotWaterBox.Text) * Core.DB.ВидСчетчика.Where(w => w.Наименование == "Горячая Вода").FirstOrDefault().ТарифЗаКубическийМетр;
                    HotWaterLabel.Content = HotWater.ToString();
                }

                HotWaterCost = HotWater;
                ColdWaterCost = ColdWater;

                AllLabel.Content = Convert.ToInt32(ColdWaterLabel.Content) + Convert.ToInt32(HotWaterLabel.Content);

                predohran = true;
            }
            catch
            {
                predohran = false;
            }
        }
    }
}
