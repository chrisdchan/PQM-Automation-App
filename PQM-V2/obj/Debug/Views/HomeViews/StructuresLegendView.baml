   M S B A M L   `   `   `  ????A  =PQM-V2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:PQM_V2.Views.HomeViewsPQM_V2.Views.HomeViews  5      5clr-namespace:PQM_V2.ConvertersPQM_V2.Converters  5      1    +PQM_V2.Views.HomeViews.StructuresLegendView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:PQM_V2.Views.HomeViews     5      Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  6A   0
convertersclr-namespace:PQM_V2.Converters  5      +muxc using:Microsoft.UI.Xaml.Controls  5      ???#707070  ? 	Resources  5      /   )PQM_V2.Converters.InverseBooleanConverter   inverseBooleanConverter2   ,PQM_V2.Converters.BooleanVisibilityConverter  booleanVisibilityConverter*   $PQM_V2.Converters.CombiningConverter ! notBooleanVisibilityConverter   
Converter1   
Converter2  faButton ?? 
TargetType ?? Property ?? Value ?? 
TargetType  
stackPanel ?? Triggers P? Property	 P? Value??%N  &	        &	      &	 &     7   7  &	 G     &	 Z      5   
   6-     5   
   60     5   
   8   8  5   (   ?? 5   
   " ??6!   .?5      ??  ? ??$ 20??6&   ?? 5       ? ??$ 20??6'   ?? 5       ? ?? 5      ?? 5      " ??6&   .R?5      ?? 
?????? ._?5      ?? $??Center=?6/   $??Center=?6J   5      5      5      5      ?? 5   
   " ??6#   .?5       ??  ? ?? ?#7070706+    5!      P? 5"       ?@J?IsMouseOver	 . . ?5#      ??  ? ?? ?#80808063   5$      5&      5'      .??5(      ? .+?5)      ?? -   5        
 ??Name$
 structureListItemsControl??5*       ?? AlternationCount$ 4??6:   ??5)      ?? *structureViewList+??5+      ?? 5,      dataTemplate??.R?5-      ??  ??Name$ 
stackPanel??5/   !   -   5         ?? Orientation$ 
Horizontal=?5-   !   $??0q?6:   #??[ 6E   
 ? Tag 5/   5   ?? *structure.index+??50   !   ??  ?? Path$ ActualWidth? ?? RelativeSource ??  ?? AncestorType$ ItemsControlL?.+?51      ?? ???#ffffff$??15(?6Y   $	??150??6g   $??
5, 0, 5, 0q?6s   ??6$   ?? *structure.name+6s   ?? 52      $??10??6E   $??30??6Q   $??
5, 0, 5, 0q?6\   ??6$   ?? *structure.color+6\   ?? 53      #??[ 6!   ??54   !   ??  ?? ElementName$ structureListItemsControl??$) #DataContext.changeVisibilityCommand???55   !   ?? *structure.index+y?56   !   ?? *structure.visible+ ?? 	Converter# [ .??57      "  FontAwesome.WPF.ImageAwesome     
Foreground
 ?????   Icon$	 Eye=?6A   58      ?? 59      #??[ 6!   ??5:   !   ?? $ structureListItemsControl??$) #DataContext.changeVisibilityCommand???5;   !   ?? *structure.index+y?5<   !   ?? *structure.visible+# [ .??5=        
 ?????$ EyeSlash=?6A   5>        5?      
 ??  ?$ Check=?6<   $??15??6I   $??15??6T   $??10, 0, 5, 0q?6`   y?5@   *   ?? *structure.selected+# [  ? ContextMenu 5A      ?? 5B       ?? 	Placement$ 
MousePoint=?6*   .@?5C   "   m? $
??Show??6+    m? Command 5D   +   ?? *%#DataContext.changeVisibilityCommand+ ?? Source %  System.Windows.Markup.Reference  *structureListItemsControl+{?5E   +   ?? *structure.visible+# [  ??5F   +   ?? *showMenuColor+m? 5G   "   $
??Hide??6+    5H   +   ?? *%#DataContext.changeVisibilityCommand+   *structureListItemsControl+{?5I   +   ?? *structure.visible+??5J   +   ?? *hideMenuColor+m? 5K   "   $??Isolate??6+    5L   +   ?? *%#DataContext.isolateStructureCommand+   *structureListItemsControl+m? 5N   "   $??Select??6+    5O   +   ?? *$"DataContext.selectStructureCommand+   *structureListItemsControl+5Q       J? InputBindings 5S      d? 5T       d? MouseAction$ 	LeftClicke?5V   +    ?? Command 5T   +   ?? $ structureListItemsControl??$( "DataContext.selectStructureCommand? ?? CommandParameter 5U   +   ?? $ structure.index?5W      5Y      5Z      5\      5^      5_      