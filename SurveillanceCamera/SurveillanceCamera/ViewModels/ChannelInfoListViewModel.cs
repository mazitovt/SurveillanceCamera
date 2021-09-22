using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using SurveillanceCamera.Models;
using SurveillanceCamera.Services;
using SurveillanceCamera.Services.Serialization;
using Xamarin.Forms;

namespace SurveillanceCamera.ViewModels
{
    public class ChannelInfoListViewModel : BaseViewModel
    {
        
        public delegate void Handler(ObservableCollection<object> channels, NotifyCollectionChangedEventArgs args);
        public event Handler SelectionChanged;
        
        
        private ObservableCollection<ChannelInfo> _channelList;


        public ObservableCollection<object> SelectedChannels { get; set; } = new ObservableCollection<object>();

        public ObservableCollection<ChannelInfo> ChannelList
        {
            get => _channelList;
            set
            {
                _channelList = value;
                OnPropertyChanged();
            }
        }

        public ChannelInfoListViewModel()
        {
            LoadChannelInfoList();

            SelectedChannels.CollectionChanged += (sender, args) =>
            {
                SelectionChanged?.Invoke((ObservableCollection<object>)sender, args);
            };

        }

        private void LoadChannelInfoList()
        {
            Task.Run(async () =>
            {
                var xmlResult = await new CamerasService().GetChannelInfo();
                ChannelList = new CustomSerializationService().Deserialize(xmlResult);
            });
        }
        
    }
}