using GongSolutions.Wpf.DragDrop;
using System.Windows;

namespace Texter.ViewModels
{
    public partial class TextManagerViewModel : IDropTarget
    {
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (!DefaultDropHandler.CanAcceptData(dropInfo)) return;

            var sourceItem = dropInfo.Data as TextItem;
            var targetItem = dropInfo.TargetItem as TextItem;

            if (sourceItem != null && targetItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            if (dropInfo == null || dropInfo.DragInfo == null)
                return;

            string targetGroupName = dropInfo.TargetGroup.Name?.ToString();
            TextItem data = (TextItem)dropInfo.Data;
            data.GroupName = GetGroupByName(targetGroupName)?.Text;

            var insertIndex = dropInfo.InsertIndex != dropInfo.UnfilteredInsertIndex ? dropInfo.UnfilteredInsertIndex : dropInfo.InsertIndex;

            int actualIndex = TextItems.IndexOf(data);
            if (actualIndex != -1)
            {
                TextItems.RemoveAt(actualIndex);
                if (actualIndex < insertIndex)
                    insertIndex--;
            }

            TextItems.Insert(insertIndex, data);

        }
    }
}
