using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.View.Admin.TroubleManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagementProject.ViewModel.AdminVM.TroubleManagementVM
{
    public class TroubleManagementViewModel : BaseViewModel
    {
        private bool _isGettingSource;
        public bool IsGettingSource
        {
            get { return _isGettingSource; }
            set { _isGettingSource = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _selectedCbbItem;
        public ComboBoxItem SelectedCbbItem 
        { 
            get { return _selectedCbbItem; } 
            set { _selectedCbbItem = value; OnPropertyChanged(); }
        }
        private ObservableCollection<TroubleDTO> _troubleList;
        public ObservableCollection<TroubleDTO> TroubleList
        {
            get { return _troubleList; }
            set { _troubleList = value; OnPropertyChanged(); }
        }
        private TroubleDTO _selectedItem;
        public TroubleDTO SelectedItem
        {
            get { return _selectedItem; }   
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _selectedStatus;
        public ComboBoxItem SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; OnPropertyChanged(); }
        }

        private DateTime _SelectedDate;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value; OnPropertyChanged(); }
        }

        private DateTime _SelectedFinishDate;
        public DateTime SelectedFinishDate
        {
            get { return _SelectedFinishDate; }
            set { _SelectedFinishDate = value; OnPropertyChanged(); }
        }

        private float _RepairCost;
        public float RepairCost
        {
            get { return _RepairCost; }
            set { _RepairCost = value; OnPropertyChanged(); }
        }
        private bool _isSaving;
        public bool IsSaving
        {
            get { return _isSaving; }
            set { _isSaving = value; OnPropertyChanged(); }
        }

        public ICommand ReloadTroubleListCM { get; set; }
        public ICommand LoadDetailTroubleCM { get; set; }
        public ICommand UpdateTroubleListCM { get; set; }

        public TroubleManagementViewModel()
        {
            LoadDetailTroubleCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ChoseWindow();
            });
            ReloadTroubleListCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                TroubleList = new ObservableCollection<TroubleDTO>();
                IsGettingSource = true;
                
                await ReloadErrorList();

                IsGettingSource = false;
            });
            UpdateTroubleListCM = new RelayCommand<Window>((p) =>{ if (IsSaving) return false; return true;
        }, async(p) =>
            {
               
                if (SelectedStatus is null)
                {
                    CustomMessageBox.ShowOk("Không hợp lệ!", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                    return;
                }
                IsSaving = true;
                await UpdateErrorFunc(p);
                IsSaving = false;
            });
        }

        public async Task UpdateErrorFunc(Window p)
        {
            if(SelectedStatus.Content.ToString() == Utils.STATUS.WAITING)
            {
                TroubleDTO UpdTrouble = new TroubleDTO
                {
                    Id=SelectedItem.Id,
                    SubmittedAt=SelectedItem.SubmittedAt,
                    TroubleStatus= SelectedStatus.Content.ToString(),
                };
                (bool isSucess, string mess) = await TroubleService.Ins.UpdateStatusTrouble(UpdTrouble);
                if (isSucess)
                {
                 
                    CustomMessageBox.ShowOk(mess, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                    await ReloadErrorList();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(mess, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            }
            else if (SelectedStatus.Content.ToString() == Utils.STATUS.CANCLE)
            {
                TroubleDTO trouble = new TroubleDTO
                {
                    Id = SelectedItem.Id,
                    TroubleStatus = SelectedStatus.Content.ToString(),
                };

                (bool isS, string mess) = await TroubleService.Ins.UpdateStatusTrouble(trouble);

                if (isS)
                {
                    CustomMessageBox.ShowOk(mess, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                    await ReloadErrorList();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(mess, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            }
            else if (SelectedStatus.Content.ToString() == Utils.STATUS.IN_PROGRESS)
            {
                TroubleDTO trouble = new TroubleDTO
                {
                    Id = SelectedItem.Id,
                    StartDate = SelectedDate,
                    TroubleStatus = SelectedStatus.Content.ToString(),
                };

                (bool isS, string mess) = await TroubleService.Ins.UpdateStatusTrouble(trouble);

                if (isS)
                {
                    CustomMessageBox.ShowOk(mess, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                    await ReloadErrorList();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(mess, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            }
            else if (SelectedStatus.Content.ToString() == Utils.STATUS.DONE)
            {
                if (SelectedItem.StartDate > SelectedFinishDate)
                {
                    CustomMessageBox.ShowOk("Ngày không hợp lệ!","Lỗi","OK", Views.CustomMessageBoxImage.Error);
                    return;
                }
                TroubleDTO trouble = new TroubleDTO
                {
                    Id = SelectedItem.Id,
                    StartDate = SelectedDate,
                    FinishDate = SelectedFinishDate,
                    TroubleStatus = SelectedStatus.Content.ToString(),
                    RepairCost = RepairCost,
                };

                (bool isS, string mess) = await TroubleService.Ins.UpdateStatusTrouble(trouble);

                if (isS)
                {
                    CustomMessageBox.ShowOk(mess, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                    await ReloadErrorList();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(mess, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }

            }
        }

        public async Task ReloadErrorList()
        {
            try
            {
                if (SelectedCbbItem == null) return;
                try
                {
                    List<TroubleDTO> troubleDTOs = await TroubleService.Ins.GetAllTrouble();

                    TroubleList = new ObservableCollection<TroubleDTO>();

                    if ((string)SelectedCbbItem.Tag == "Toàn bộ")
                    {
                        TroubleList = new ObservableCollection<TroubleDTO>(troubleDTOs);
                    }
                    else
                    {
                        TroubleList = new ObservableCollection<TroubleDTO>(troubleDTOs.Where(tr => tr.TroubleStatus == SelectedCbbItem.Tag.ToString()));
                    }
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                throw;
            }
            catch (Exception e)
            {
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
        }

        private void ChoseWindow()
        {
            SelectedDate = DateTime.Today;
            SelectedFinishDate = DateTime.Today;
            RepairCost = 0;
            if (SelectedItem.TroubleStatus == Utils.STATUS.DONE)
            {
                TroubleInformation w = new TroubleInformation();
                w.ShowDialog();
            }
            else if (SelectedItem.TroubleStatus == Utils.STATUS.WAITING)
            {
                EditTroubleInformation w = new EditTroubleInformation();
                w.ShowDialog();
            }
            else if (SelectedItem.TroubleStatus == Utils.STATUS.IN_PROGRESS)
            {
                EditTroubleInformation_Inprocess w = new EditTroubleInformation_Inprocess();
                w.ShowDialog();
            }
        }
    }
}
