   M S B A M L   `   `   `  ????A  =PQM-V2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:PQM_V2.ViewsPQM_V2.Views  5      ?$clr-namespace:PQM_V2.Views.HomeViewsPQM_V2.Views.HomeViews  5      5clr-namespace:PQM_V2.ConvertersPQM_V2.Converters  5      g8clr-namespace:PQM_V2.Views.HomeViews.AttributePanelViews*PQM_V2.Views.HomeViews.AttributePanelViews  5	          PQM_V2.Views.HomeView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:PQM_V2.Views     5      :	homeViews$clr-namespace:PQM_V2.Views.HomeViews     5      6
convertersclr-namespace:PQM_V2.Converters     5      XattributePanelViews8clr-namespace:PQM_V2.Views.HomeViews.AttributePanelViews     5	      -   5          ? 	Resources  5         navMenuItem ?? 
TargetType ?? Property ?? Value ?? Path ?? RelativeSource ?? AncestorType2   ,PQM_V2.Converters.BooleanVisibilityConverter  BooleanVisibilityConverter??%?   &	        &	 ?     ?? 5   
   " m?6$   .?5      ??  ? ?? 6'   ?? $ ActualHeight? ?? " o??? 5       ? ?? 6&   ?? $ ActualWidth? ?? " o?5        5   
   60   5       J? InputBindings 5      ?? 5   
   
 ?? Key$ E??5      	 ?? 	Modifiers$
	 Ctrlf?5      
 ?? Command
 5      ?? *navigateStartupCommand+ ?? CommandParameter 5      Z? 5      ?? 5   
   $ E??5      $	 
Ctrl+Shiftf?5      
 5      ?? *exitApplicationCommand+ 5      Z? 5      ?? 5   
   $ O??5      $
	 Ctrlf?5       
 5      ?? *openFilesCommand+ 5      Z? 5       ?? 5!   
   $ O??5#      $	 
Ctrl+Shiftf?5$      
 5!      ?? *openFolderCommand+ 5"      Z? 5$      ?? 5%   
   $
 left??5&      $ Right??5'      
 5%      ?? *changeStructureCommand+5'      ?? 5(   
   $
 left??5)      $
 Left??5*      
 5(      ?? *changeStructureCommand+5*      ?? 5+   
   $ right??5,      $
 Down??5-      
 5+      ?? *changeStructureCommand+5-      ?? 5.   
   $ right??5/      $ Right??50      
 5.      ?? *changeStructureCommand+50      ?? 51   
   $
 left??52      $ Up??53      
 51      ?? *changeStructureCommand+53      ?? 54   
   $ 1??55      $ D1??56      $
	 Ctrlf?57      
 54      ?? *$"updateSelectedStructureTypeCommand+57      ?? 58   
   $ 2??59      $ D2??5:      $
	 Ctrlf?5;      
 58      ?? *$"updateSelectedStructureTypeCommand+5;      ?? 5<   
   $ 3??5=      $ D3??5>      $
	 Ctrlf?5?      
 5<      ?? *$"updateSelectedStructureTypeCommand+5?      .??5A      ?  ? ColumnDefinitions 5B      ?? 5C      $??1* ?6   $??400px??6*   $??100px??6;   ?? 5D      $
??auto ?6   ?? 5E      $??2* ?6   $??200px??6*   ?? 5F      $
??auto ?6   ?? 5G      $??1* ?6   $??150px??6*    ? RowDefinitions 5I   
   ?? 5J      $
??30px ?6   ?? 5K      $??2* ?6   $??200px??6(   ?? 5L      $
??auto ?6   ?? 5M      $??1* ?6   $??200px??6(   .+?5O   
   ??  ?? Orientation$ 
Horizontal=?6   $??0??5P      $??0??6"   .+?5Q      o? .@?5R      m? $
??File??6   #??[  6)   .@?5S      m? $??
Open Files??6    m? InputGestureText$ Ctrl + O??63    m? Command 5T      ?? *openFilesCommand+m? 5U      $??Open Folder??6   $ Ctrl + Shift + O??64    5V      ?? *openFolderCommand+?? 5W      m? 5X      $??Close Graph??6   $ Ctrl + E??64    5Y      ?? *navigateStartupCommand+m? 5Z      $??Exit Aplication??6   $ Ctrl + Shift + E??68    5[      ?? *exitApplicationCommand+5\      5]      o? 5^      .@?5_      m? $
??View??6   #??[  6)   .@?5`      m? $??Graph Visible??6    m? IsCheckable .  m? StaysOpenOnClick .  m? 	IsChecked 5b      ?? $ graphVisible? ?? Mode$ TwoWay=? ?? UpdateSourceTrigger$ PropertyChanged=?5c      m? 5d      $??Table Visible??6    .  .  5f      ?? $ tableVisible?$ TwoWay=?$ PropertyChanged=?5g      m? 5h      $??Attribute Panel Visible??6    .  .  5j      ?? $ attributesVisible?$ TwoWay=?$ PropertyChanged=?5k      5l      5m      o? 5n      .@?5o      m? $??Export??6   #??[  6+   .@?5p      m? $??Graph as .png??6    5q      ?? *exportGraphCommand+m? 5r      $??Table as .csv??6    5s      ?? *exportTableCommand+5t      5u      5v      ?? 5x   
   $??Stretch=?5{      $??Stretch=?65   $??4??5|      ???#DEDEDE??5x      ?? *layout.rowSplit.row+??6@   ?? *layout.rowSplit.column+??5y      ?? *layout.rowSplit.rowSpan+??6H   ?? *layout.rowSplit.colSpan+y?5z      ?? $ layout.rowVisibility? ?? 	Converter# [ 5}      ?? 5~   
   ???#DEDEDE$??Center=?5?      $??Stretch=?64   $??4??5?      ??5~      ?? *layout.colSplit.row+??6@   ?? *layout.colSplit.column+??5      ?? *layout.colSplit.rowSpan+??6H   ?? *layout.colSplit.colSpan+y?5?      ?? $ layout.colVisibility?# [ 5?      ?? 5?   
   $??1??6   $??1??6$   $??3??5?      $??Center=?5?      $??Stretch=?64   ???#DEDEDE$??4??5?      1   +PQM_V2.Views.HomeViews.StructuresLegendView  5?   
   $??0??5?      $??1??6   $??3??5?      5?      ? 5?   
   -   5         ?Name$ gridTopLeft??5?      ??5?      ?? *layout.graph.row+??63   ?? *layout.graph.column+??5?      ?? *layout.graph.rowSpan+??6;   ?? *layout.graph.colSpan+y?5?      ?? $ graphVisible?# [ .+?5?      &    PQM_V2.Views.HomeViews.GraphView  5?      5?      ? 5?   
   -   5        $ GridBottomLeft??5?      ??5?      ?? *layout.table.row+??63   ?? *layout.table.column+??5?      ?? *layout.table.rowSpan+??6;   ?? *layout.table.colSpan+y?5?      ?? $ tableVisible?# [ .+?5?      &    PQM_V2.Views.HomeViews.TableView  5?      5?      ? 5?   
   ??5?      ?? *layout.attributes.row+??68   ?? *layout.attributes.column+??5?      ?? *layout.attributes.rowSpan+??6@   ?? *layout.attributes.colSpan+y?5?      ?? $ attributesVisible?# [ .+?5?      C   =PQM_V2.Views.HomeViews.AttributePanelViews.AttributePanelView  5?      5?      5?      5?      