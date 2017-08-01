using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Texter.ViewModels;

namespace Texter.DragDrop
{
    public static class DragDropHelper
    {
        public static bool HasValidTarget(IDropInfo dropInfo)
        {
            bool result = false;
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
                    if (targetVisual.Tag != null && (Tags?)targetVisual.Tag == Tags.DropToGroupHeaderMarker)
                    {
                        if (targetVisual.DataContext is KeyValuePair<GroupItem, ObservableCollection<TextItem>>)
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

    }
}
