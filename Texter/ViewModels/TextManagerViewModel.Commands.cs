namespace Texter.ViewModels
{
    public partial class TextManagerViewModel
    {
        private RelayCommand _addGroupCommand;
        public RelayCommand AddGroupCommand => _addGroupCommand ?? (_addGroupCommand = new RelayCommand(AddGroup, () => !string.IsNullOrWhiteSpace(GroupInput)));

        private RelayCommand _addTextCommand;
        public RelayCommand AddTextCommand => _addTextCommand ?? (_addTextCommand = new RelayCommand(AddText, () => !string.IsNullOrWhiteSpace(TextInput) && SelectedGroupID.HasValue));

        private RelayCommand<GroupItem> _removeGroupItemCommand;
        public RelayCommand<GroupItem> RemoveGroupItemCommand => _removeGroupItemCommand ?? (_removeGroupItemCommand = new RelayCommand<GroupItem>(RemoveGroupItem, gi => gi != null));

        private RelayCommand<TextItem> _removeItemCommand;
        public RelayCommand<TextItem> RemoveItemCommand => _removeItemCommand ?? (_removeItemCommand = new RelayCommand<TextItem>(RemoveText, ti => ti != null));

        private RelayCommand<TextItem> _pasteFromClipboardCommand;
        public RelayCommand<TextItem> PasteFromClipboardCommand => _pasteFromClipboardCommand ?? (_pasteFromClipboardCommand = new RelayCommand<TextItem>(PasteFromClipboard, ti => ti != null));


        

    }
}
