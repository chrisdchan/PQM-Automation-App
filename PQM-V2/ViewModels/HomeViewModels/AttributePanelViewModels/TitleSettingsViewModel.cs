using PQM_V2.Commands;
using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2.ViewModels.HomeViewModels.AttributePanelViewModels
{
    public class TitleSettingsViewModel : BaseViewModel
    {
        private GraphCustomizeStore _graphCustomizeStore;
        private TitleType _type;
        private int _size;
        private int _leftOffset;
        private int _topOffset;
        private bool _bold;
        private bool _italic;
        private TitleSettings _titleSettings;
        public int size { get => _size; set { _size = value; onPropertyChanged(nameof(size)); } }
        public int leftOffset { get => _leftOffset; set { _leftOffset = value; onPropertyChanged(nameof(leftOffset)); } }
        public int topOffset { get => _topOffset; set { _topOffset = value; onPropertyChanged(nameof(topOffset)); } }
        public bool bold { get => _bold; set { _bold = value; onPropertyChanged(nameof(bold)); } }
        public bool italic { get => _italic; set { _italic = value; onPropertyChanged(nameof(italic)); } }

        public RelayCommand updateCommad { get; private set; }
        public TitleSettingsViewModel(TitleSettings titleSettings)
        {
            _graphCustomizeStore = (Application.Current as App).graphCustomizeStore;
            titleSettings.type = _type;
            titleSettings.size = _size;
            titleSettings.leftOffset = _leftOffset;
            titleSettings.topOffset = _topOffset;
            titleSettings.bold = _bold;
            titleSettings.italic = _italic;
            _titleSettings = titleSettings;

            updateCommad = new RelayCommand(update);
        }

        private void update(object msg)
        {
            _titleSettings.size = _size;
            _titleSettings.leftOffset = _leftOffset;
            _titleSettings.topOffset = _topOffset;
            _titleSettings.bold = _bold;
            _titleSettings.italic = _italic;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }

    }
    }
}
