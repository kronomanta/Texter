using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Texter.ViewModels
{
    public partial class TextManagerViewModel : IDropTarget
    {
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            //if (!DefaultDropHandler.CanAcceptData(dropInfo)) return;

            //drag item to item
            var sourceItem = dropInfo.Data as TextItem;

            if (sourceItem != null)
            {
                var targetItem = dropInfo.TargetItem as TextItem;
                if (targetItem != null)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Move;
                }
                else
                {
                    //drag item to something

                    if (dropInfo.TargetItem != null)
                    {

                    }
                    else if (dropInfo.TargetCollection != null)
                    {

                    }
                    else
                    {
                        var targetVisual = dropInfo.VisualTargetItem as FrameworkElement;
                        if (targetVisual != null)
                        {
                            if (targetVisual.Tag != null && targetVisual.Tag.ToString() == "DropMarker")
                            {
                                if (targetVisual.DataContext is KeyValuePair<GroupItem, ObservableCollection<TextItem>>)
                                {
                                    dropInfo.Effects = DragDropEffects.Move;
                                }
                            }
                        }
                    }


                }
            }
            else
            {

            }



        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            if (dropInfo == null || dropInfo.DragInfo == null)
                return;

            TextItem dataItem = dropInfo.Data as TextItem;
            if (dataItem != null)
            {
                var sourceItems = (ObservableCollection<TextItem>)dropInfo.DragInfo.SourceCollection;

                KeyValuePair<GroupItem, ObservableCollection<TextItem>>? targetGroup = null;

                var targetItems = dropInfo.TargetCollection as ObservableCollection<TextItem>;
                if (targetItems != null)
                {
                    targetGroup = Items.Single(x => x.Value == targetItems);
                }
                else
                {
                    var targetVisual = dropInfo.VisualTargetItem as FrameworkElement;
                    if (targetVisual != null)
                    {
                        if (targetVisual.Tag != null && targetVisual.Tag.ToString() == "DropMarker")
                        {
                            if (targetVisual.DataContext is KeyValuePair<GroupItem, ObservableCollection<TextItem>>)
                            {
                                targetGroup = (KeyValuePair<GroupItem, ObservableCollection<TextItem>>)targetVisual.DataContext;
                            }
                        }
                    }
                }


                if (targetGroup == null) return;

                dataItem.GroupName = targetGroup.Value.Key?.Text;

                var insertIndex = dropInfo.InsertIndex != dropInfo.UnfilteredInsertIndex ? dropInfo.UnfilteredInsertIndex : dropInfo.InsertIndex;

                if (sourceItems == targetGroup.Value.Value)
                {
                    int actualIndex = sourceItems.IndexOf(dataItem);
                    if (actualIndex > -1 && actualIndex < insertIndex)
                        insertIndex--;
                }

                sourceItems.Remove(dataItem);
                targetGroup.Value.Value.Insert(insertIndex, dataItem);
            }
        }
    }
}
